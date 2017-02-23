
#Default Values these values will be used during first run, or if the file is deleted.
CENTREOVAL = 0.5 #SD from the mean
MIDDLEOVAL = 1.5 #SD from the mean
OUTEROVAL = 3 #SD from the mean

TARGETSIZERATIO = 0.67 #Ratio of target diameter to application width

def getDataFromFile():

    def readFile(fo):
        configValues = [CENTREOVAL, MIDDLEOVAL, OUTEROVAL, TARGETSIZERATIO]

        try:
            line = fo.readline()
            configValues[0] =  float(line[(line.index("=")+1):].strip())
        except Exception:
            pass
        try:
            line = fo.readline()
            configValues[1] =  float(line[(line.index("=")+1):].strip())
        except Exception:
            pass
        try:
            line = fo.readline()
            configValues[2] =  float(line[(line.index("=")+1):].strip())
        except Exception:
            pass
        try:
            line = fo.readline()
            configValues[3] =  float(line[(line.index("=")+1):].strip())
        except Exception:
            pass

        return configValues

    try:
        configFile = open("vowelPlotConfig.txt", 'r')
        configValues = readFile(configFile)
        configFile.close()
        return configValues

    except IOError:
        import os
        print os.getcwd()
        #If the file doesnt exist, then this will create it.
        configFile = open("vowelPlotConfig.txt", 'w')
        configFile.write("CENTREOVAL = " + str(CENTREOVAL)+"\n")
        configFile.write("MIDDLEOVAL = " + str(MIDDLEOVAL) + "\n")
        configFile.write("OUTEROVAL = "  + str(OUTEROVAL) +  "\n")
        configFile.write("TARGETSIZERATIO = " + str(TARGETSIZERATIO) + "\n")
        configFile.write("\n")
        configFile.write("To change the values simply replace the numbers above.\n")
        configFile.write("NOTE: This effects all users on this computer.\n")
        configFile.write("\n")
        configFile.write("Please note the defaults are C=0.45, M=1.5, O=3, T:0.67\n")
        configFile.write("These are constant and will be returned upon a fresh install.")
        configFile.close()

        configFile = open("vowelPlotConfig.txt", 'r')
        configValues = readFile(configFile)
        configFile.close()
        return configValues
