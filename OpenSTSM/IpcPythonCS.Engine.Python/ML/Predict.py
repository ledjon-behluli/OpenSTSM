from IpcPythonCS.RPC.RPCWrapper import RPCWrapper
from IpcPythonCS.Communication.ICommunicator import ICommunicator

import sys
import cv2
import os
import shutil
import numpy as np
import ctypes.wintypes
import heapq
import json
import re
import itertools
import math
import natsort 

from keras.models import load_model
from keras.preprocessing import image
from keras.applications.inception_v3 import preprocess_input

class Prediction:
	def __init__(self, Class, Probability, decimalPoint_Probability):
		self.Class = Class
		self.Probability = round(Probability, decimalPoint_Probability)

class Element:
	Id = itertools.count()
	def __init__(self, Coordinates):
		self.Id = next(Element.Id)
		self.Coordinates = Coordinates		

class Coordinates:
	def __init__(self, x1, y1, x2, y2):
		self.x1 = x1
		self.y1 = y1
		self.x2 = x2
		self.y2 = y2

class Proposal:	
	Id = itertools.count()
	def __init__(self, filename, Coordinates):
		self.Id = next(Proposal.Id)
		self.Filename = filename
		self.MiddlePoint = self.CalculateMiddlePoint(Coordinates.x1, Coordinates.y1, Coordinates.x2, Coordinates.y2)
		self.Coordinates = Coordinates
		self.FileShortName = int(filename[filename.find("_") + len("_") : filename.rfind(".")])		
		self.CrossDiagonalLength = self.CalculateCrossDiagonalLength(Coordinates)

	def CalculateMiddlePoint(self, x1, y1, x2, y2):
		horizontal_middle_length = int(math.ceil((x2 - x1) / 2))
		vertical_middle_length = int(math.ceil((y2 - y1) / 2))
		
		x_middle_point = int(math.ceil(x1 + horizontal_middle_length))
		y_middle_point = int(math.ceil(y1 + vertical_middle_length))

		return x_middle_point, y_middle_point

	def CalculateCrossDiagonalLength(self, Coordinates):
		'''
			* P1(x1, y1)			* P4(x2, y1)




			* P3(x1, y2)			* P2(x2, y2)
		'''

		p1 = [Coordinates.x1, Coordinates.y1]
		p2 = [Coordinates.x2, Coordinates.y2]
		p3 = [Coordinates.x1, Coordinates.y2]
		p4 = [Coordinates.x2, Coordinates.y1]

		p1_p2_diagonal = int(math.sqrt(((p1[0]-p2[0])**2)+((p1[1]-p2[1])**2)))
		p3_p4_diagonal = int(math.sqrt(((p3[0]-p4[0])**2)+((p3[1]-p4[1])**2)))

		return p1_p2_diagonal + p3_p4_diagonal


class Predict(RPCWrapper):
    _communicator = ICommunicator
    
    inputImgPath = ''
    outputImgPath = ''
    coordinatesPath = ''
    numRegionProposals = 80

    CATEGORICAL = ["arrow_L_D", "arrow_L_U", "arrow_R_D", "arrow_R_U", "arrow_straight_down", "arrow_straight_left", "arrow_straight_right", 
                   "arrow_straight_up", "comparator", "controller", "feedback", "input", "output", "process"]          

    # Variabil Parameters
    MiddlePointDistance_Threshold = 5
    OuterSelection_Threshold = 3
    DecimalPoint_Probability = 5 	# 99.7683421 => 99.76
    RegionProposals_Multiplicity = 1
    SpatialDistanceOfCoordinatePoints_Threshold = 8		# Spatial distance between two point which is considered to be the "same" point in space
    NumOfResultsPerElement = 2      # Number of results per identified element (Probability 1, 2 ... numOfResults)
    ImageResizeFactor = 1.0
    img_width, img_height = 299, 299
    model = ''

    def __init__(self, communicator):
        self._communicator = communicator
        self.outputImgPath = '{}\\OpenSTSM\\Temp\\SelectiveOutput\\'.format(self.GetMyDocumentsPath())
        self.coordinatesPath = '{}Coordinates\\'.format(self.outputImgPath)     

        return

    def GetMyDocumentsPath(self):
        CSIDL_PERSONAL = 5       # My Documents
        SHGFP_TYPE_CURRENT = 0   # Get current, not default value

        buf= ctypes.create_unicode_buffer(ctypes.wintypes.MAX_PATH)
        ctypes.windll.shell32.SHGetFolderPathW(None, CSIDL_PERSONAL, None, SHGFP_TYPE_CURRENT, buf)

        return buf.value

    def DeletePreviousData(self):
        for file in os.listdir(self.outputImgPath):
            file_path = os.path.join(self.outputImgPath, file)
            if os.path.isfile(file_path):
                os.unlink(file_path)

        for file in os.listdir(self.coordinatesPath):
            file_path = os.path.join(self.coordinatesPath, file)       
            if os.path.isfile(file_path):
                os.unlink(file_path)    

    def predict(self, model, img, numOfResults, decimalPoint_Probability):
        x = image.img_to_array(img)
        x = np.expand_dims(x, axis=0)
        x = preprocess_input(x)
        preds = model.predict(x, batch_size=32)
        
        Predictions = []
        indexes = heapq.nlargest(numOfResults, range(len(preds[0])), (preds[0]).take)
        
        for idx in indexes:
            Predictions.append(Prediction(self.CATEGORICAL[idx], preds[0][idx] * 100, decimalPoint_Probability))
        
        return Predictions

    def GetDistanceBetweenProposalsMiddlePoint(self, Proposal_1, Proposal_2):
        p1 = Proposal_1.MiddlePoint		# p1 = [x_middle_point_1, y_middle_point_1]
        p2 = Proposal_2.MiddlePoint		# p2 = [x_middle_point_2, y_middle_point_2]
        
        return int(math.sqrt(((p1[0]-p2[0])**2)+((p1[1]-p2[1])**2)))

    def CheckForSpatialCoordinateMatches(self, Element_1, Element_2):
        '''
    	        * E_1_P1(x1, y1), E_2_P1(x1, y1) 	* E_1_P4(x2, y1), E_2_P4(x2, y1)
    	
    
    	        * E_2_P3(x1, y2) 					* E_2_P2(x2, y2)
    
    
    
    
    	        * E_1_P3(x1, y2)					* E_1_P2(x2, y2)
        '''    
        E_1_P1 = [Element_1.Coordinates.x1, Element_1.Coordinates.y1]
        E_1_P2 = [Element_1.Coordinates.x2, Element_1.Coordinates.y2]
        E_1_P3 = [Element_1.Coordinates.x1, Element_1.Coordinates.y2]
        E_1_P4 = [Element_1.Coordinates.x2, Element_1.Coordinates.y1]
    
        E_2_P1 = [Element_2.Coordinates.x1, Element_2.Coordinates.y1]
        E_2_P2 = [Element_2.Coordinates.x2, Element_2.Coordinates.y2]
        E_2_P3 = [Element_2.Coordinates.x1, Element_2.Coordinates.y2]
        E_2_P4 = [Element_2.Coordinates.x2, Element_2.Coordinates.y1]
    
        Spatial_P1 = int(math.sqrt(((E_1_P1[0]-E_2_P1[0])**2)+((E_1_P1[1]-E_2_P1[1])**2)))
        Spatial_P2 = int(math.sqrt(((E_1_P2[0]-E_2_P2[0])**2)+((E_1_P2[1]-E_2_P2[1])**2)))
        Spatial_P3 = int(math.sqrt(((E_1_P3[0]-E_2_P3[0])**2)+((E_1_P3[1]-E_2_P3[1])**2)))
        Spatial_P4 = int(math.sqrt(((E_1_P4[0]-E_2_P4[0])**2)+((E_1_P4[1]-E_2_P4[1])**2)))
    
    
        if Spatial_P1 < self.SpatialDistanceOfCoordinatePoints_Threshold:
            return True
    
        if Spatial_P2 < self.SpatialDistanceOfCoordinatePoints_Threshold:
            return True
    
        if Spatial_P2 < self.SpatialDistanceOfCoordinatePoints_Threshold:
            return True
    
        if Spatial_P2 < self.SpatialDistanceOfCoordinatePoints_Threshold:
            return True
    
        return False

    def CalculateCrossDiagonalLength(self, Coordinates):
        '''
            * P1(x1, y1)			* P4(x2, y1)
        
        
        
        
            * P3(x1, y2)			* P2(x2, y2)
        '''        
        p1 = [Coordinates.x1, Coordinates.y1]
        p2 = [Coordinates.x2, Coordinates.y2]
        p3 = [Coordinates.x1, Coordinates.y2]
        p4 = [Coordinates.x2, Coordinates.y1]
        
        p1_p2_diagonal = int(math.sqrt(((p1[0]-p2[0])**2)+((p1[1]-p2[1])**2)))
        p3_p4_diagonal = int(math.sqrt(((p3[0]-p4[0])**2)+((p3[1]-p4[1])**2)))
        
        return p1_p2_diagonal + p3_p4_diagonal

    def CalculateMiddlePoint(self, x1, y1, x2, y2):
        horizontal_middle_length = int(math.ceil((x2 - x1) / 2))
        vertical_middle_length = int(math.ceil((y2 - y1) / 2))
        
        x_middle_point = int(math.ceil(x1 + horizontal_middle_length))
        y_middle_point = int(math.ceil(y1 + vertical_middle_length))
        
        return x_middle_point, y_middle_point

    ###################################################################

    def LoadModel(self, modelPath):
        try:
            self.model = load_model(modelPath)
            return True
        except Exception as e:
            return str('Error:{}'.format(e))

    def ConvertToGrayscale(self, inputImgPath):
        try:
            newPath = inputImgPath.split('.')[0]
            img = cv2.imread(inputImgPath, 0)  
            (thresh, img_bw) = cv2.threshold(img, 128, 255, cv2.THRESH_BINARY | cv2.THRESH_OTSU)    
            newPath = '{}{}'.format(newPath, '_bw.png')
            cv2.imwrite(newPath, img_bw)
            self.inputImgPath = newPath
        except Exception as e:
            return str('Error:{}'.format(e))

    def RunSelectiveSearch(self, numRegionProposals, imgResizeFactor):
        try:            
            self.numRegionProposals = numRegionProposals
            self.ImageResizeFactor = imgResizeFactor

            self.DeletePreviousData()

            ss_type = 'q'

            # speed-up using multithreads
            cv2.setUseOptimized(True)
            cv2.setNumThreads(4)
 
            # read image
            im = cv2.imread(self.inputImgPath)
  
            # resize image
            newHeight = int(imgResizeFactor * im.shape[1])
            newWidth = int(im.shape[1]*newHeight/im.shape[0])
            im = cv2.resize(im, (newWidth, newHeight))    
 
            # create Selective Search Segmentation Object using default parameters
            ss = cv2.ximgproc.segmentation.createSelectiveSearchSegmentation()
 
            # set input image on which we will run segmentation
            ss.setBaseImage(im)
 
            # Switch to fast but low recall Selective Search method
            if (ss_type == 'f'):
                ss.switchToSelectiveSearchFast()
 
            # Switch to high recall but slow Selective Search method
            elif (ss_type == 'q'):
                ss.switchToSelectiveSearchQuality()
            # if argument is neither f nor q exit
            else:
                sys.exit(1)
 
            # run selective search segmentation on input image
            rects = ss.process()
            #print('Total Number of Region Proposals: {}'.format(len(rects)))
     
            # number of region proposals to show
            numShowRects = self.numRegionProposals
 
            # create a copy of original image
            imOut = im.copy()

            # itereate over all the region proposals
            for i, rect in enumerate(rects):
                # draw rectangle for region proposal till numShowRects
                if (i < numShowRects):
                    x, y, w, h = rect
                    cv2.rectangle(imOut, (x, y), (x+w, y+h), (0, 255, 0), 1, cv2.LINE_AA)
                    roi = im[y:y+h, x:x+w]     
                    cv2.imwrite("{}out_{}.png".format(self.outputImgPath, i), roi)                
                    with open("{}out_{}.txt".format(self.coordinatesPath, i), "w") as file:
                        file.write("x1:{}, y1:{}, x2:{}, y2:{}".format(x, y, x+w, y+h))   
        
            return True
        except Exception as e:
            return str('Error:{}'.format(e))

    def RunPrediction(self, middlePointDistance_Threshold, outerSelection_Threshold, decimalPoint_Probability, regionProposals_Multiplicity, spatialDistanceOfCoordinatePoints_Threshold, numOfResultsPerElement, useGpuAcceleration):  
        try:
            self.MiddlePointDistance_Threshold = middlePointDistance_Threshold
            self.OuterSelection_Threshold = outerSelection_Threshold
            self.DecimalPoint_Probability = decimalPoint_Probability 	
            self.RegionProposals_Multiplicity = regionProposals_Multiplicity
            self.SpatialDistanceOfCoordinatePoints_Threshold = spatialDistanceOfCoordinatePoints_Threshold	
            self.NumOfResultsPerElement = numOfResultsPerElement

            if(useGpuAcceleration == False):
                os.environ["CUDA_VISIBLE_DEVICES"]="1"     # Run on CPU

            os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'    # Ignore warnings

            #############################################################

            Proposals = []

            for filename in natsort.natsorted(os.listdir(self.coordinatesPath), reverse=False):
                f = open(self.coordinatesPath + filename, "r")	
                coords = [x.strip().split(':')[1] for x in f.read().split(',')]
            
                x1 = int(coords[0])
                y1 = int(coords[1])
                x2 = int(coords[2])
                y2 = int(coords[3])
            
                Proposals.append(Proposal(filename, Coordinates(x1, y1, x2, y2)))

            OpenList = []
            ClosedList = []
            SortedOpenList = []
            StopCondition = False

            Proposals = sorted(Proposals, key = lambda x: x.FileShortName)

            if(Proposals.__len__() > 0):
                ClosedList.append(Proposals[0])
                CurrentProposal = Proposals[0]
                for proposal in [x for x in Proposals if x.FileShortName != CurrentProposal.FileShortName]:
                    OpenList.append(proposal)

            SortedOpenList = sorted(OpenList, key = lambda x: x.FileShortName)

            while StopCondition == False:
                for proposal in SortedOpenList:
                    distance = self.GetDistanceBetweenProposalsMiddlePoint(CurrentProposal, proposal)
        
                    if(distance <= self.MiddlePointDistance_Threshold):
                        proposal.Id = CurrentProposal.Id
                        ClosedList.append(proposal)
                        OpenList.remove(proposal)
                        SortedOpenList = sorted(OpenList, key = lambda x: x.FileShortName)

                if(OpenList.__len__() > 0):
                    CurrentProposal = min(OpenList, key = lambda x: x.Id)
                    ClosedList.append(CurrentProposal)
                    OpenList.remove(CurrentProposal)
                    SortedOpenList = sorted(OpenList, key = lambda x: x.FileShortName)
                else:
                    StopCondition = True

            MultiOccurrenceProposals = []		# This list will contain proposals which have at least 2 region proposals with the same "relative" MiddlePoint

            for key, group in itertools.groupby(ClosedList, key=lambda x: x.Id):
                RegionProposals = list(group)
                if(len(RegionProposals) > self.RegionProposals_Multiplicity):
                    for proposal in RegionProposals:
                        MultiOccurrenceProposals.append(proposal)


            Elements = []
            i = 0

            # Create an element which takes the biggest coordinates of all memebers of that group

            for key, group in itertools.groupby(MultiOccurrenceProposals, key=lambda x: x.Id):	
                Regions = list(group)

                min_x1 = min(proposal.Coordinates.x1 for proposal in Regions) - self.OuterSelection_Threshold
                min_y1 = min(proposal.Coordinates.y1 for proposal in Regions) - self.OuterSelection_Threshold
                max_x2 = max(proposal.Coordinates.x2 for proposal in Regions) + self.OuterSelection_Threshold
                max_y2 = max(proposal.Coordinates.y2 for proposal in Regions) + self.OuterSelection_Threshold
            
                Elements.append(Element(Coordinates(min_x1, min_y1, max_x2, max_y2)))
                i = i + 1


            # In case region proposals have been found which are not similar in 4 coordinates but maybe have 1, 2 or 3 similar coordinates 

            OpenListOfElements = []
            ClosedListOfElements = []
            SortedOpenListOfElements = []
            StopCondition = False

            if(Elements.__len__() > 0):
                ClosedListOfElements.append(Elements[0])
                CurrentElement = Elements[0]
                for element in [x for x in Elements if x.Id != CurrentElement.Id]:
                    OpenListOfElements.append(element)

            SortedOpenListOfElements = sorted(OpenListOfElements, key = lambda x: x.Id)

            while StopCondition == False:
                for element in SortedOpenListOfElements:
                    spatial_coordinate_match = self.CheckForSpatialCoordinateMatches(CurrentElement, element)

                    if(spatial_coordinate_match):			
                        element.Id = CurrentElement.Id
                        ClosedListOfElements.append(element)
                        OpenListOfElements.remove(element)
                        SortedOpenListOfElements = sorted(OpenListOfElements, key = lambda x: x.Id)

                if(OpenListOfElements.__len__() > 0):
                    CurrentElement = min(OpenListOfElements, key = lambda x: x.Id)
                    ClosedListOfElements.append(CurrentElement)
                    OpenListOfElements.remove(CurrentElement)
                    SortedOpenListOfElements = sorted(OpenListOfElements, key = lambda x: x.Id)
                else:
                    StopCondition = True


            # Create an element which takes the biggest CDL (Cross Diagonal Length) of all memebers of that group

            Elements = []

            for key, group in itertools.groupby(ClosedListOfElements, key=lambda x: x.Id):	
                __elements = list(group)
                highest_CDL = 0

                for _element in __elements:
                    CDL = self.CalculateCrossDiagonalLength(_element.Coordinates)
                    if CDL > highest_CDL:
                        highest_CDL = CDL
                        _FinalElementOfGroup = _element			

                Elements.append(_FinalElementOfGroup)

            im = cv2.imread(self.inputImgPath)
            newHeight = int(self.ImageResizeFactor * im.shape[1])
            newWidth = int(im.shape[1]*newHeight/im.shape[0])
            im = cv2.resize(im, (newWidth, newHeight))    

            data = []

            # Print out results
            for element in Elements:
                x1 = element.Coordinates.x1
                y1 = element.Coordinates.y1
                x2 = element.Coordinates.x2
                y2 = element.Coordinates.y2
            
                roi = im[y1:y2, x1:x2]     		
                resize_roi = cv2.resize(roi, (self.img_width, self.img_height))	
            
                _predictions = self.predict(self.model, resize_roi, self.NumOfResultsPerElement, self.DecimalPoint_Probability)
            
                for _predict in _predictions:  
                    point = self.CalculateMiddlePoint(element.Coordinates.x1, element.Coordinates.y1, element.Coordinates.x2, element.Coordinates.y2)
                    data.append({"Id": element.Id, "Class": _predict.Class, "Probability": _predict.Probability, "X": point[0], "Y": point[1]})

            return json.dumps(data)
        except Exception as e:
            return str('Error:{}'.format(e))