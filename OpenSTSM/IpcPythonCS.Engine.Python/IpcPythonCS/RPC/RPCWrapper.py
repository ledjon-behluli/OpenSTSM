import xml.etree.ElementTree as ET

class RPCWrapper:
    _communicator = None

    def __init__(self, communicator):
        self._communicator = communicator

    # Wait for client to request function call and process it
    def ProcessFunctionCall(self):
        funcDesc = self._communicator.Read()
        funcDesc = funcDesc.decode("utf-8")                
        funcRtn = self.__callByXMLFuncDesc(funcDesc)

        self._communicator.Write(self.__generateXMLReturnValue("int", funcRtn))     # Why always return int ?! Check this maybe it should return string, if so adjust RunSS

    # ElementTree XML Parser reference:
    # https://docs.python.org/2/library/xml.etree.elementtree.html
    def __callByXMLFuncDesc(self, xmlDesc):
        root = ET.fromstring(xmlDesc)
        funcName = None
        args = []
        children = root.getchildren()

        # parse function name from <function name="funcName">###</function>
        funcName = root.attrib["name"]

        # loop for xml tree
        # <function name="funcName"><arg type="type">int</arg></function>
        for child in children:
            if (child.tag == "arg"): # parse argument
                if (child.attrib["type"] == "int" or child.attrib["type"] == "System.Int32"): # if type is int
                    args.append(int(child.text))
                elif (child.attrib["type"] == "string" or child.attrib["type"] == "System.String"): # if type is int
                    args.append(str(child.text))
                elif (child.attrib["type"] == "float" or child.attrib["type"] == "System.Single"): # if type is float
                    args.append(float(child.text))
                elif (child.attrib["type"] == "bool" or child.attrib["type"] == "System.Boolean"): # if type is boolean
                    args.append(child.text == "True")

        return self.__callByFuncName(funcName, *args)

    # Generate XML description of return value. Temporary implementation; should be modified later
    def __generateXMLReturnValue(self, type, value):
        return "<return type=\"{0}\">{1}</return>".format(type, value)

    def __callByFuncName(self, functionName, *args):
        func = getattr(self, functionName)
        return func(*args)

    #def ConvertValueToXML(self, type, value):
    #    return "<return type=\"{0}\">{1}</return>".format(type, value);