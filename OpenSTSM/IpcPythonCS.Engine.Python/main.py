from ML.SelectiveSearch import SelectiveSearch
from IpcPythonCS.Communication.Pipe.PipeServer import PipeServer

server = PipeServer()
server.WaitForConnection("openSTSM")
ss = SelectiveSearch(server)

## Infinite execution ##
while (True):
    try:
        ss.ProcessFunctionCall()
    except:
        server.Close()
        server.WaitForConnection("openSTSM")
        ss = SelectiveSearch(server)

'''
## One time execution ##
try:
    while(True):
        calc.ProcessFunctionCall()

except:
    print("Connection ended.")
'''