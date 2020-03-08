from ML.Predict import Predict
from IpcPythonCS.Communication.Pipe.PipeServer import PipeServer

import time

server = PipeServer()
server.WaitForConnection("openstsm")
predict = Predict(server)

## Infinite execution ##
while (True):
    try:
        predict.ProcessFunctionCall()
    except:
        server.Close()
        server.WaitForConnection("openstsm")
        predict = Predict(server)

'''
## One time execution ##
try:
    while(True):
        predict.ProcessFunctionCall()

except:
    print("Connection ended.")
'''