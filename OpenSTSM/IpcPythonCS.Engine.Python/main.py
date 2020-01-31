from ML.SelectiveSearch import SelectiveSearch
from IpcPythonCS.Communication.Pipe.PipeServer import PipeServer

server = PipeServer()
server.WaitForConnection("openstsm")
ss = SelectiveSearch(server)

## Infinite execution ##
while (True):
    try:
        ss.ProcessFunctionCall()
    except:
        server.Close()
        server.WaitForConnection("openstsm")
        ss = SelectiveSearch(server)

'''
## One time execution ##
try:
    while(True):
        ss.ProcessFunctionCall()

except:
    print("Connection ended.")
'''