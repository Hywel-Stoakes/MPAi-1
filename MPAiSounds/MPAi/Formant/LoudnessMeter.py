'''
Created on 29/11/2010

@author: Byron
'''

from Tkinter import *
from tkSnack import *




#The maximum root mean square (loudness) that the meter will record up to
MAXRMS = 14000
#The desired loudness when using the formant aid
RECOMMENDEDMAXLOUDNESS = 14000

'''
A meter that tracks how loud a particular live recording is. The loudness is measured in watts and the calculations are doen by dividing the watts with
the watts/pixel number.
'''
class LoudnessMeter():

    '''
    Initialises the height, width, and watts/pixel parameters needed for the loudness meter to function
    '''
    def __init__(self, canvas, yOffset):

        self.canvas = canvas
        self.canvasWidth = self.canvas.winfo_reqwidth()
        self.height = self.canvas.winfo_reqheight()
        self.wattsPerPixel = self.calculateWattsPerPixel()
        self.width= 5


    '''
    Performs the calculations to get the loudness of the recording.
    Arguments: sound - the sound that is being recorded.
    Returns the loudness measured in watts.
    '''
    def calculateLoudness(self, sound):
        latestPitchValues = sound.pitch(method='ESPS')[len(sound.pitch(method='ESPS')) - 1]
        loudness = latestPitchValues[2]
        return loudness

    '''
    Returns watts/pixel number.
    '''
    def calculateWattsPerPixel(self):
        return MAXRMS/self.height

    '''
    Updates the loudness meter by drawing a new rectangle. The rectangles height being determined by the loudness level.
    Arguments: sound - the sound that is being recorded.
    '''
    def updateMeter(self, sound):
        self.canvas.delete("loudnessLevel")
        loudness = self.calculateLoudness(sound)
        loudnessLevelInPixels = loudness/self.wattsPerPixel

        color = self.getColor(loudnessLevelInPixels)
        # x1 = self.canvasWidth-self.width     #LEFT
        # y1 = self.height-loudnessLevelInPixels      #TOP
        # x2 = self.canvasWidth               #RIGHT
        # y2 = self.height                 #BOTTOM
        #
        # self.canvas.create_rectangle(x1, y1, x2, y2, fill=color, tags='loudnessLevel')



    '''
    Gets HardCoded Colour scheme, This could be upgrades to be dynamic, but for such a insignificant element,
    '''
    def getColor(self, pixel):
        perc = (pixel/self.height) * 100
        if perc > 60 :
            self.canvas.itemconfig('toLoud', state = 'normal')
            self.canvas.itemconfig('toQuiet', state = 'hidden')
            self.canvas.itemconfig('Loudness', state='normal')
            return "#E1002D"
        elif perc > 15:
            self.canvas.itemconfig('toLoud', state = 'hidden')
            self.canvas.itemconfig('toQuiet', state = 'hidden')
            self.canvas.itemconfig('Loudness', state='hidden')
            return "#5BBF00"
        elif perc > 0:
            self.canvas.itemconfig('toLoud', state = 'hidden')
            self.canvas.itemconfig('toQuiet', state = 'normal')
            self.canvas.itemconfig('Loudness', state='normal')
            return "#0015D2"
        else:
            return "#E1002D"



    '''
    Clears the meter
    '''
    def clearMeter(self):
        self.canvas.delete('loudnessLevel')
