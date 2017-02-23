import sys
import time
import struct
"""
vowelScorer maintains the scores from the vowel plot in order to send them
to the VowelPlotController.cs. Which creates the processs on which VowelPlot runs on.

vowelScorer Handles sending and recieving messages from the controller.
mainly the sending off the finally results to the analysis form.
F
Author: Joshua Brundan
"""
class VowelScorer:
    def __init__(self):
        self.isSafeToRecord = True
        self.plotRegions = ['Total','Centre', 'Second', 'Third', 'Not on Target']
        self.longVowels=['a:','e:','i:','o:','u:']
        self.lastScore = 0
        #VowelRegionsCounts
        self.vowelPlotRegionsCounts = []
        for i in range(len(self.longVowels)):
            vowelPlotRegionCount = [0,0,0,0]
            self.vowelPlotRegionsCounts.append(vowelPlotRegionCount)

        self.totalPlotRegionsCounts = [0,0,0,0]

        #VowelPercentages
        self.vowelPlotCounts = [0,0,0,0,0]
        self.vowelScores = [0,0,0,0,0]
        self.vowelPlotPercentages = [100,100,100,100,100]


        #TotalPercentage
        self.totalPlotCount = 0
        self.totalScore = 0
        self.overallPercentage = 100


    """
    Called at the end of a round of the vowelPlot. Used to update all the data for the particular vowel.
    As well as the overall data.
    """
    def updateScore(self, vowel, rawScore, plottedInfo):
        if plottedInfo == 0 :
            pass
        else:
            self.lastScore = (int)((float)(rawScore)/(float)(plottedInfo[0]))
        index = self.getIndex(vowel, self.longVowels)

        #updateVowelPlotRegionsCount for vowel
        vowelPlotRegionCount = self.vowelPlotRegionsCounts[index]
        for i in range(4):
            vowelPlotRegionCount[i] += plottedInfo[i+1]
            self.totalPlotRegionsCounts[i] += plottedInfo[i+1]

        #update vowelPlotCounts and vowelScores
        self.vowelPlotCounts[index] += plottedInfo[0]
        self.vowelScores[index] += rawScore
        if self.vowelPlotCounts[index] == 0:
            #Leave the percentage at its current value.
            pass
        else:
            percentage = (float)(self.vowelScores[index])/(float)(self.vowelPlotCounts[index])

        #update vowelPlotPercentages
        self.vowelPlotPercentages[index] = percentage

        #update totalPlotCount and totalScore
        self.totalPlotCount += plottedInfo[0]
        self.totalScore += rawScore

        #calculate overallPercentage
        if self.totalPlotCount == 0:
            #Leave the percentage at its current value.
            pass
        else:
            self.overallPercentage = (float)(self.totalScore)/(float)(self.totalPlotCount)

    """
    Gets the index of a element in a list.
    """
    def getIndex(self, vowel, vowelList):
        for i in range(len(vowelList)):
            if vowel == vowelList[i]:
                return i
        print "Error Vowel not valid."
        return -1
    def getLastScore(self):
        print self.lastScore

        return self.lastScore

    def createLineList(self):

        lineTotal = 'TotaTotal: '+ "{:.2f}".format(self.overallPercentage)
        sentLine = lineTotal
        print sentLine

        #a:
        index = 0
        if self.vowelPlotCounts[index] != 0:
            lineA = 'a: ' + "{:.2f}".format(self.vowelPlotPercentages[index])
            sentLine = sentLine + "\n" + lineA
            print sentLine
        #e:
        index = 1
        if self.vowelPlotCounts[index] != 0:
            lineE = 'e: '+ "{:.2f}".format(self.vowelPlotPercentages[index])
            sentLine = sentLine + "\n" + lineE

        #i:
        index = 2
        if self.vowelPlotCounts[index] != 0:
            lineI = 'i: '+ "{:.2f}".format(self.vowelPlotPercentages[index])
            sentLine = sentLine + "\n" + lineI
            print sentLine

        #o:
        index = 3
        if self.vowelPlotCounts[index] != 0:
            lineO = 'o: '+ "{:.2f}".format(self.vowelPlotPercentages[index])
            sentLine = sentLine + "\n" + lineO
            print sentLine

        #u:
        index = 4
        if self.vowelPlotCounts[index] != 0:
            lineU = 'u: '+ "{:.2f}".format(self.vowelPlotPercentages[index])
            sentLine = sentLine + "\n" + lineU
            print sentLine

        print sentLine
        return sentLine

    def safeToRecord(self):
        if(self.isSafeToRecord):
            return True
        else:
            return False

    def connectAndSendText(self):
        try:
            self.isSafeToRecord = False
            lineList = self.createLineList()

            f = open(r'\\.\pipe\NPSSVowelPlot', 'r+b', 0)
            i = 1

            f.write(struct.pack('I', len(lineList)) + lineList)   # Write str length and str
            f.seek(0)                               # EDIT: This is also necessary
            print 'Wrote:', lineList
        except IOError:
            print "Pipe File does not exist."
        # #time.sleep(2)
        self.isSafeToRecord = True
