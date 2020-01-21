from IpcPythonCS.RPC.RPCWrapper import RPCWrapper
from IpcPythonCS.Communication.ICommunicator import ICommunicator

import sys
import cv2
import os
import shutil
import numpy as np

base_dir = os.getcwd()
InputImgPath = 'E:\\Libraries\\Desktop\\Visa Docs\\test_selective_1.png'
outputImgPath = '{}\\TempFiles\\SelectiveOutput'.format(base_dir)
coordinatesPath = "{}\\Coordinates".format(outputImgPath)
#NumRegionProposals = 80

class SelectiveSearch(RPCWrapper):
    _communicator = ICommunicator

    def __init__(self, communicator):
        self._communicator = communicator
        return

    def DeletePreviousData(self):
        for file in os.listdir(outputImgPath):
            file_path = os.path.join(outputImgPath, file)
            if os.path.isfile(file_path):
                os.unlink(file_path)

        for file in os.listdir(coordinatesPath):
            file_path = os.path.join(coordinatesPath, file)       
            if os.path.isfile(file_path):
                os.unlink(file_path)

    def Test(self, a):
        return a

    def Run(self, inputImgPath, numRegionProposals):
        InputImgPath = inputImgPath
        NumRegionProposals = numRegionProposals

        self.DeletePreviousData()

        ss_type = 'q'

        # speed-up using multithreads
        cv2.setUseOptimized(True)
        cv2.setNumThreads(4)
 
        # read image
        im = cv2.imread(imputImgPath)
  
        # resize image
        newHeight = 200
        newWidth = int(im.shape[1]*200/im.shape[0])
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
        # if argument is neither f nor q print help message
        else:
            print(__doc__)
            sys.exit(1)
 
        # run selective search segmentation on input image
        rects = ss.process()
        print('Total Number of Region Proposals: {}'.format(len(rects)))
     
        # number of region proposals to show
        numShowRects = NumRegionProposals
 
        # create a copy of original image
        imOut = im.copy()

        # itereate over all the region proposals
        for i, rect in enumerate(rects):
            # draw rectangle for region proposal till numShowRects
            if (i < numShowRects):
                x, y, w, h = rect
                cv2.rectangle(imOut, (x, y), (x+w, y+h), (0, 255, 0), 1, cv2.LINE_AA)
                roi = im[y:y+h, x:x+w]     
                cv2.imwrite("{}\\out_{}.png".format(outputImgPath, i), roi)                
                with open("{}\\coordinates\\out_{}.txt".format(outputImgPath, i), "w") as file:
                    file.write("x1:{}, y1:{}, x2:{}, y2:{}".format(x, y, x+w, y+h))   

        # show output
        cv2.imshow("Output", imOut)

        # record key press
        k = cv2.waitKey(0) & 0xFF

        cv2.destroyAllWindows()    
        return True
