from IpcPythonCS.RPC.RPCWrapper import RPCWrapper
from IpcPythonCS.Communication.ICommunicator import ICommunicator

import sys
import cv2
import os
import shutil
import numpy as np
import ctypes.wintypes


class SelectiveSearch(RPCWrapper):
    _communicator = ICommunicator
    
    inputImgPath = ''
    outputImgPath = ''
    coordinatesPath = ''
    numRegionProposals = 0

    def __init__(self, communicator):
        self._communicator = communicator
        self.outputImgPath = '{}\\OpenSTSM\\Temp\\SelectiveOutput'.format(self.GetMyDocumentsPath())
        self.coordinatesPath = '{}\\Coordinates'.format(self.outputImgPath)
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

    def Run(self, inputImgPath, numRegionProposals):
        try:
            self.inputImgPath = inputImgPath
            self.numRegionProposals = numRegionProposals

            self.DeletePreviousData()

            ss_type = 'q'

            # speed-up using multithreads
            cv2.setUseOptimized(True)
            cv2.setNumThreads(4)
 
            # read image
            im = cv2.imread(self.inputImgPath)
  
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
                    cv2.imwrite("{}\\out_{}.png".format(self.outputImgPath, i), roi)                
                    with open("{}\\out_{}.txt".format(self.coordinatesPath, i), "w") as file:
                        file.write("x1:{}, y1:{}, x2:{}, y2:{}".format(x, y, x+w, y+h))   
        
            return True
        except:
            return False
    