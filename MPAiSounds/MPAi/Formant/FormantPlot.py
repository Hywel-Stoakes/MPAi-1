from Tkinter import *
from tkSnack import *
import tkSnack
import ttk #unsure of purpose currently
from LoudnessMeter import *
import tkFileDialog
#TEST
import os
import SoundProcessing
from time import sleep
import thread

SIZERATIO = 1.6180339887499 #GoldenRatio

DEFAULTHEIGHT = 580 #Height of the MainFrame

PLOTHEIGHT = DEFAULTHEIGHT
PLOTWIDTH = round(DEFAULTHEIGHT * SIZERATIO) #Calculates the correct width from height.

XSHIFT = 75
YSHIFT = 100

XOFFSET =100
YOFFSET=500

SCALEDOWN = 3
class FormantPlot:

    def __init__(self, parent, path, root, plotID, formApp):

        self.plotHeight = PLOTHEIGHT
        self.plotWidth = PLOTWIDTH

        self.root = root
        self.formApp = formApp
        self.vowelType = formApp.vowelType

        self.parent = parent
        self.path = path
        self.id = plotID
        self.buttonCounter = 0

        if plotID == 0 or plotID == 1:
            self.gender = "male"
        else:
            self.gender = "female"

        self.hasFormants = False

        #plot Setup:
        self.updateRate = 30
        self.setupPlot()

        self.drawGoldStandardMonophthongs(True)

        self.createKey()
        self.createAxis()

        self.prevX = 0
        self.prevY = 0

        # sound Setup:
        self.initialiseSounds()
        self.formantRadius = 2
        self.formantColour = 'black'
        self.isRecording = False
        self.lineHiddens = False

        self.loadedPlots= False
        self.hasloadedPlots = False

        self.backgroundNoiseDistance = 50
        self.count  =0

        self.notStopped = False




    def setupPlot(self):
        #Plot
        self.formantPlotFrame = self.parent
        # self.formantPlotCanvas = Canvas(self.formantPlotFrame, height=PLOTHEIGHT-60, width=PLOTWIDTH, bg='white')
        self.formantPlotCanvas = Canvas(self.formantPlotFrame,height=PLOTHEIGHT,width=PLOTWIDTH ,bg='white')

        #self.formantPlotCanvas.grid(row=0 ,column=0, sticky=N+S+E+W)
        self.formantPlotCanvas.pack(fill=BOTH, expand =YES)
        self.formantPlotCanvas.bind("<Button-1>", self.requestRecord)

        #Creates the loudnessMeter on the formant plot canvas.
        self.loudnessMeter = LoudnessMeter(self.formantPlotCanvas,YSHIFT)
        self.createBoxs()

    def createButtons(self):
        self.buttonCounter += 1
        self.formantPlotCanvas.delete('buttons')
        height = (int)((float)(2)*(self.plotHeight/PLOTHEIGHT))
        font = ('Arial','12')

        self.formantPlotCanvas.create_rectangle(0,self.plotHeight-height*30-8,self.plotWidth+10,self.plotHeight+10, tags='buttons', fill='#d3d3d3')

        text = "  Back to Menu  "
        helpButton = Button(self.parent, text=text,command=self.goBackToMenu, font=font )
        helpButton.configure(height=height,activebackground='green', anchor=W, relief = GROOVE)
        helpButtonWindow = self.formantPlotCanvas.create_window(2, self.plotHeight-4*(self.plotHeight/PLOTHEIGHT), anchor=SW,tags=('backButton','buttons'),window=helpButton)


        text = text=14*" "+"Record"+14*" "
        self.recordButton = Button(self.parent, text=text, font=font, command=self.requestRecord)
        self.recordButton.configure(height=height,activebackground='green', anchor=W, relief = GROOVE, bd=3)
        self.formantPlotCanvas.create_window((self.plotWidth/2), self.plotHeight-4*(self.plotHeight/PLOTHEIGHT)-25, anchor=CENTER,tags=('recordButton','buttons'),window=self.recordButton)

        if self.buttonCounter >2 and self.formantPlotCanvas.winfo_height() <= 580:
            self.formantPlotCanvas.delete("recordButton")
            self.formantPlotCanvas.create_window((self.plotWidth/2), self.plotHeight-4*(self.plotHeight/PLOTHEIGHT)-15, anchor=CENTER, tags=('recordButton','buttons'),window=self.recordButton)
        if self.vowelType == 'short':
            text=" Short Vowels "
        else:

            text=' Long Vowels '

        self.vowelButton = Button(self.parent, text=text, font=font, command=self.toggleVowelType)
        self.vowelButton.configure(height=height,activebackground='green', anchor=W, relief = GROOVE,bd='3')
        self.formantPlotCanvas.create_window(self.plotWidth, self.plotHeight-4*(self.plotHeight/PLOTHEIGHT), anchor=SE,tags=('vowelButton','buttons'),window=self.vowelButton)


        self.formantPlotCanvas.itemconfig('buttons', state='normal')

    def goBackToMenu(self):
         self.formApp.backToMenu()

    def toggleVowelType(self,*event):
        if self.isRecording == False:


            self.formApp.toggleVowelType()

    def loadHelp(self):
        pass
    def createBoxs(self):
        self.formantPlotCanvas.delete('boxes')
        boxWidth= 150
        font = ('Arial','15')

        x1 = (PLOTWIDTH/2)*(self.plotWidth/PLOTWIDTH) - (boxWidth)
        y1 = 2
        x2 = (PLOTWIDTH/2)*(self.plotWidth/PLOTWIDTH)
        y2 = 32

        self.recordingBox = self.formantPlotCanvas.create_rectangle(x1,y1,x2,y2, tag=('boxes','Recording'), fill='orange', outline='black')
        self.recordingBoxText = self.formantPlotCanvas.create_text((x1+x2)/2,(y1+y2)/2, tag=('boxes','recordingText'), text='Loading...',font=font, fill='black')
        self.formantPlotCanvas.itemconfig('Recording', state='hidden')
        self.formantPlotCanvas.itemconfig('recordingText', state='hidden')

        x1 = (PLOTWIDTH/2)*(self.plotWidth/PLOTWIDTH)
        y1 = 2
        x2 = (PLOTWIDTH/2)*(self.plotWidth/PLOTWIDTH) + boxWidth
        y2 = 32
        self.recordingBox = self.formantPlotCanvas.create_rectangle(x1,y1,x2,y2, tag=('boxes','Loudness'), outline='black')
        self.recordingBoxText = self.formantPlotCanvas.create_text((x1+x2)/2,(y1+y2)/2, tag=('boxes','toLoud'), text='Too Loud.',font=font, fill='black')
        self.recordingBoxText = self.formantPlotCanvas.create_text((x1+x2)/2,(y1+y2)/2, tag=('boxes','toQuiet'), text='Too Quiet.',font=font, fill='black')
        self.formantPlotCanvas.itemconfig('toLoud', state = 'hidden')
        self.formantPlotCanvas.itemconfig('toQuiet', state = 'hidden')
        self.formantPlotCanvas.itemconfig('Loudness', state='hidden')

    def resize(self, width, height):
        print self.formantPlotCanvas.winfo_height()
        print self.formantPlotCanvas.winfo_width()

        self.plotWidth = (float)(width)
        self.plotHeight = (float)(height)
        self.drawGoldStandardMonophthongs(True)
        self.redrawPlotInfo()
        self.createBoxs()
        self.createButtons()

    def createKey(self,):
        self.formantPlotCanvas.delete("legend")
        font = ('Arial','13')
        radius = 4
        x=self.plotWidth-160
        y=15
        self.formantPlotCanvas.create_text(x+80, y, text="Key:", tag='legend', font=font)
        self.formantPlotCanvas.create_oval(x-radius,y+20-radius,x+radius,y+20+radius,tag='legend',fill='black')
        self.formantPlotCanvas.create_text(x+80,y+20,text='Recorded Vowels',tag='legend',font=font)
        self.formantPlotCanvas.create_oval(x-radius,y+40-radius,x+radius,y+40+radius,tag='legend',fill='yellow')
        self.formantPlotCanvas.create_text(x+80,y+40,text='Loaded Vowels',tag='legend',font=font)

        self.formantPlotCanvas.create_line(x-15, 0, x-15, y+50, tag='legend')
        self.formantPlotCanvas.create_line(x-15, y+50, self.plotWidth+50, y+50, tag='legend')

    def createAxis(self):
        self.formantPlotCanvas.delete("formantAxes")
        xFrameScale = (self.plotWidth/PLOTWIDTH)
        yFrameScale = (self.plotHeight/PLOTHEIGHT)


        self.formantPlotCanvas.create_line(XSHIFT*xFrameScale, (self.plotHeight-YSHIFT*yFrameScale), self.plotWidth, (self.plotHeight-YSHIFT*yFrameScale), tags="formantAxes")
        yAxisCount = 15
        yIncrement = ((self.plotHeight-YSHIFT)/yAxisCount)
        for yIndex in range(yAxisCount):
            y = yIndex*yIncrement
            self.formantPlotCanvas.create_line(XSHIFT*xFrameScale-8,y,XSHIFT*xFrameScale,y, tags="formantAxes")

        self.formantPlotCanvas.create_line(XSHIFT*xFrameScale,0,XSHIFT*xFrameScale,self.plotHeight-YSHIFT*yFrameScale, tags="formantAxes")
        xAxisCount = 20
        xIncrement = (self.plotWidth-XSHIFT*xFrameScale)/xAxisCount
        for xIndex in range(xAxisCount):
            x = xIndex*xIncrement
            self.formantPlotCanvas.create_line(XSHIFT*xFrameScale+x,self.plotHeight-YSHIFT*yFrameScale+8,XSHIFT*xFrameScale+x,self.plotHeight-YSHIFT*yFrameScale, tags="formantAxes")

        self.formantPlotCanvas.create_text((XSHIFT*xFrameScale+self.plotWidth)/2, self.plotHeight-YSHIFT*yFrameScale+20,text = "Tongue Position",font = ('Arial','13'), tags="formantAxes")
        self.formantPlotCanvas.create_text(XSHIFT*xFrameScale+22, self.plotHeight-YSHIFT*yFrameScale+15,text = "Front",font = ('Arial','13'), tags="formantAxes")
        self.formantPlotCanvas.create_text(self.plotWidth-30, self.plotHeight-YSHIFT*yFrameScale+15,text = "Back",font = ('Arial','13'), tags="formantAxes")
        self.formantPlotCanvas.create_text(XSHIFT*xFrameScale/2, (self.plotHeight-YSHIFT*yFrameScale)/2,text = "Mouth",font = ('Arial','13'), tags="formantAxes")
        self.formantPlotCanvas.create_text(XSHIFT*xFrameScale/2, ((self.plotHeight-YSHIFT*yFrameScale)/2)+15,text = "Openness",font = ('Arial','13'), tags="formantAxes")
        self.formantPlotCanvas.create_text(XSHIFT*xFrameScale/2, self.plotHeight-YSHIFT*yFrameScale-15,text = "Open",font = ('Arial','13'), tags="formantAxes")
        self.formantPlotCanvas.create_text(XSHIFT*xFrameScale/2, 20,text = "Closed",font = ('Arial','13'), tags="formantAxes")

    def drawGoldStandardMonophthongs(self, toFill):
        self.formantPlotCanvas.delete("ellipses")
        vowelType = self.vowelType
        (xScale, xShift, yScale, yShift) = self.getPlotScaleInfo(vowelType)
        data = self.goldStandardMonophthongs(self.id,vowelType)

        xFrameScale = (self.plotWidth/PLOTWIDTH)
        yFrameScale = (self.plotHeight/PLOTHEIGHT)

        std = 1
        for f1mean, f1sd, f2mean, f2sd, vowel in data:
            font = ('Arial','20','bold')
            x1 = ((f2mean-std*f2sd)*xScale - xShift) * xFrameScale
            y1 = ((f1mean-std*f1sd)*yScale - yShift) * yFrameScale
            x2 = ((f2mean+std*f2sd)*xScale - xShift) * xFrameScale
            y2 = ((f1mean+std*f1sd)*yScale - yShift) * yFrameScale
            if toFill:
                currentOval = self.formantPlotCanvas.create_oval(x1,y1,x2,y2, outline='black', tag='ellipses', fill='#ffffff', activefill='#e3c5d6')
            else:
                currentOval = self.formantPlotCanvas.create_oval(x1,y1,x2,y2, outline='black', tag='ellipses',activefill='#e3c5d6')

            ovalText= self.formantPlotCanvas.create_text((f2mean*xScale-xShift)*xFrameScale,(f1mean*yScale-yShift)*yFrameScale,fill='red',font=font,tag='ellipses', text=vowel)

        #creating the overlap
        for f1mean, f1sd, f2mean, f2sd, vowel in data:
            x1 = ((f2mean-std*f2sd)*xScale-xShift) * xFrameScale
            y1 = ((f1mean-std*f1sd)*yScale-yShift) * yFrameScale
            x2 = ((f2mean+std*f2sd)*xScale-xShift) * xFrameScale
            y2 = ((f1mean+std*f1sd)*yScale-yShift) * yFrameScale
            currentOval = self.formantPlotCanvas.create_oval(x1,y1,x2,y2, outline='black', tag='ellipses')

    def redrawPlotInfo(self):
        self.drawGoldStandardMonophthongs(False)
        self.createAxis()
        self.createKey()
        self.replotFormants()
        self.replotloadedPlots()

    def getPlotScaleInfo(self, vowelType):
        plotID = self.id

        if plotID == 0:
            if vowelType == 'short':
                xScale=1.5
                xShift=400
                yScale=2
                yShift=100
            else:
                xScale=1.5
                xShift=400
                yScale=2
                yShift=100
        elif plotID == 1:
            if vowelType == 'short':
                xScale=1.5
                xShift=400
                yScale=3
                yShift=200
            else:
                xScale=1.5
                xShift=400
                yScale=3
                yShift=200
        elif plotID == 2:
            if vowelType == 'short':
                xScale=1.2
                xShift=130
                yScale=1.8
                yShift=130
            else:
                xScale=1.2
                xShift=130
                yScale=1.8
                yShift=130
        elif plotID == 3:
            if vowelType == 'short':
                xScale=1.2
                xShift=150
                yScale=2.3
                yShift=175
            else:
                xScale=1.2
                xShift=150
                yScale=2.3
                yShift=175
        else:
            print "ERROR ID INVALID"
            xScale = 0
            xShift = 0
            yScale = 0
            yShift = 0
        return (xScale, xShift, yScale, yShift)

    def goldStandardMonophthongs(self, plotID, vowelType):
        path = self.path
        if plotID == 0:
            if vowelType == 'short':
                data = self.appendData(path,'data\maori\monDataNativeMale.txt')
            else:
                data = self.appendData(path,'data\maori\longVowelNativeMale.txt')
        elif plotID == 1:
            if vowelType == 'short':
                data = self.appendData(path,'data\maori\monDataModernMale.txt')
            else:
                data = self.appendData(path,'data\maori\longVowelModernMale.txt')
        elif plotID == 2:
            if vowelType == 'short':
                data = self.appendData(path,'data\maori\monDataNativeFemale.txt')
            else:
                data = self.appendData(path,'data\maori\longVowelNativeFemale.txt')
        elif plotID == 3:
            if vowelType == 'short':
                data = self.appendData(path,'data\maori\monDataModernFemale.txt')
            else:
                data = self.appendData(path,'data\maori\longVowelModernFemale.txt')
        return data

    def appendData(self, pat, str):
        MonophthongDataPath = os.path.join(pat, str)
        MonophthongFile = open(MonophthongDataPath,'r')
        MonophthongData = []
        #Code from ywan478 2010 SoftEng206 Project
        #Load in the monophthong data for males
        for monophthongDataLine in MonophthongFile:
            monophthongDataLine = monophthongDataLine.split()
            #F1 mean and standard deviation
            monophthongDataLine[0]= (int(float(monophthongDataLine[0]))/SCALEDOWN)
            monophthongDataLine[1]= (int(float(monophthongDataLine[1]))/SCALEDOWN)
            #F2 mean and standard deviation
            monophthongDataLine[2]= PLOTWIDTH - (int(float(monophthongDataLine[2]))/SCALEDOWN) + XSHIFT + XOFFSET
            monophthongDataLine[3]= (int(float(monophthongDataLine[3]))/SCALEDOWN)
            MonophthongData.append(monophthongDataLine)
            #End code from ywan478
        return MonophthongData

#***********************************************************************************************************
# Sound

    '''
    Creates the Snack Sound objects used in the formant plot.
    '''
    def initialiseSounds(self):
        tkSnack.initializeSnack(self.root)
        self.recordedAudio = Sound()
        self.soundCopy = Sound()
        self.loadedAudio = Sound()

    def replotFormants(self):
        if(self.hasFormants == True):
            self.formantPlotCanvas.delete("userformants")
            radius = self.formantRadius
            color = self.formantColour

            for index in range(len(self.xPlotList)):

                x = self.xPlotList[index]
                y = self.yPlotList[index]
                self.formantPlotCanvas.create_oval( x * (self.plotWidth/PLOTWIDTH) - radius, y * (self.plotHeight/PLOTHEIGHT) - radius, x * (self.plotWidth/PLOTWIDTH) + radius, y * (self.plotHeight/PLOTHEIGHT) + radius, fill=color, tags="userformants")

    def replotloadedPlots(self):
        if(self.hasloadedPlots == True):
            self.formantPlotCanvas.delete("loadedPlots")
            self.formantPlotCanvas.delete("loadedLines")
            radius = self.formantRadius
            color = 'yellow'
            prevX = self.xLoadedPlotList[0]
            prevY = self.yLoadedPlotList[0]

            for index in range(len(self.xLoadedPlotList)):
                x = self.xLoadedPlotList[index]
                y = self.yLoadedPlotList[index]

                xScaled = x * (self.plotWidth/PLOTWIDTH)
                yScaled = y * (self.plotHeight/PLOTHEIGHT)
                prevXScaled = prevX * (self.plotWidth/PLOTWIDTH)
                prevYScaled = prevY * (self.plotHeight/PLOTHEIGHT)
                self.formantPlotCanvas.create_oval(x * (self.plotWidth/PLOTWIDTH) - radius, y * (self.plotHeight/PLOTHEIGHT) - radius, x * (self.plotWidth/PLOTWIDTH) + radius, y * (self.plotHeight/PLOTHEIGHT) + radius, fill=color, tags="loadedPlots")


                if ( (((x-prevX) ** 2 + (y-prevY) ** 2) ** (0.5)) < self.backgroundNoiseDistance):
                    self.formantPlotCanvas.create_line(prevXScaled,prevYScaled,xScaled,yScaled, tags='loadedLines')

                prevX = x
                prevY = y

    """
    Plot Fomrants takes a sound file and plots the last formant in the file.
    """
    def plotFormants(self, sound):
        #Gets the probablity of sound being a voice for the last formant in the sound file. (false means last formant, true means all formants)
        probabilityOfVoicing = SoundProcessing.getProbabilityOfVoicing(sound,False)

        if probabilityOfVoicing == 1.0:

            formant = SoundProcessing.getFormants(sound,self.id,False)

            #Only plot the latest formants of the sound if there's a good chance that it is the user speaking instead of background noise.
            if formant != None:
                (xScale, xShift, yScale, yShift) = self.getPlotScaleInfo(self.vowelType)
                #GetUser default or user defined colour and radius for the individual formants
                radius = self.formantRadius
                color = self.formantColour
                yPreScale = formant[0]/SCALEDOWN
                xPreScale = PLOTWIDTH - (formant[1]/SCALEDOWN) + XSHIFT + XOFFSET


                x = (xPreScale * xScale - xShift)
                y = (yPreScale * yScale - yShift)


                self.count += 1
                #Only plot of the Points are on the Grid. They can go over the axis though.
                if (x > 0 ) and ( y < PLOTHEIGHT):
                    distance = (((x-self.prevX) ** 2 + (y-self.prevY) ** 2) ** (0.5))

                    if( distance < self.backgroundNoiseDistance) and distance > 0:
                        self.hasFormants = True
                        self.formantPlotCanvas.create_oval( x * (self.plotWidth/PLOTWIDTH) - radius, y * (self.plotHeight/PLOTHEIGHT) - radius, x * (self.plotWidth/PLOTWIDTH) + radius, y * (self.plotHeight/PLOTHEIGHT) + radius, fill=color, tags="userformants")
                        self.xPlotList.append(x)
                        self.yPlotList.append(y)
                    self.prevX = x
                    self.prevY = y

                if not self.isRecording:
                    if formant != None:
                        pass

    """
    Determines if to record or stop.
    """
    def requestRecord(self,*event):
        if self.isRecording:
            if self.isLoading:
                pass
            else:
                self.stop()
        else:
            self.record()
    """
    record is called when the record button is pressed it starts recording the users sounds
    and makes the formant plot react accordingly.
    """
    def record(self):
        self.isLoading = True
        text=15*" "+"Stop"+15*" "
        self.recordButton.config(text=text, bg='orange', state='disabled')
        self.vowelButton.config(state='disabled')
        self.hasFormants = True
        self.xPlotList = []
        self.yPlotList = []
        self.root.resizable(False, False) #DISALLOWS the window to be resized both verically and horizonally.

        self.notStopped = True
        self.recordedAudio.record()
        self.isRecording = True

        #self.stopButton.config(state='normal')
        #self.clearScreenButton.config(state='disabled')
        self.formantPlotCanvas.itemconfig('Recording', state='normal', fill='orange')
        self.formantPlotCanvas.itemconfig('recordingText', state ='normal', text='Loading...')
        thread.start_new_thread(self.multiThreadUpdateCanvas, ("Thread-1", self.notStopped))

        self.count2 = 0

    def multiThreadUpdateCanvas(self, threadName, notStopped):
        self.clear()
        sleep(1.2)
        self.isLoading = False
        text=15*" "+"Stop"+15*" "
        self.recordButton.config(text=text, bg='#CE2029', state='normal')

        self.formantPlotCanvas.itemconfig('Recording', fill='red')
        self.formantPlotCanvas.itemconfig('recordingText', text = 'Recording')

        try:
            while self.notStopped:
                self.count2+=1
                self.soundCopy.copy(self.recordedAudio)
                SoundProcessing.crop(self.soundCopy)
                self.plotFormants(self.soundCopy)
                if self.count2 % 10 == 0:
                    self.updateLoudnessMeter(self.soundCopy)

        except Exception:
            import traceback
            print traceback.format_exc()

    def stop(self):
        self.isRecording = False


        self.recordButton.config(text=" Record ", bg = 'white')
        self.notStopped = False
        self.formantPlotCanvas.itemconfig('boxes', state='hidden')

        self.loudnessMeter.clearMeter
        self.recordedAudio.stop()

        self.root.after(200 ,self.removeLoudness)
        self.count = 0
        self.root.resizable(True, True) #Allows the window to be resized both verically and horizonally.
        self.vowelButton.config(state='normal')

    def removeLoudness(self):
        self.loudnessMeter.clearMeter()
        self.formantPlotCanvas.itemconfig('toLoud', state = 'hidden')
        self.formantPlotCanvas.itemconfig('toQuiet', state = 'hidden')
        self.formantPlotCanvas.itemconfig('Loudness', state='hidden')

    """
    Clears the screen of all plots, lines and loaded plots.
    """
    def clear(self):
        self.hasFormants = False
        self.hasloadedPlots = False
        self.formantPlotCanvas.delete('userformants')
        self.formantPlotCanvas.delete('loadedPlots')
        self.formantPlotCanvas.delete('loadedLines')

    def changeColor(self):
        self.formantColour = 'pink'

    '''
        Update the formant plot and loudness meter if the user is still recording.
    '''
    def updateAllCanvases(self):
        self.soundCopy.copy(self.recordedAudio)
        SoundProcessing.crop(self.soundCopy)
        self.plotFormants(self.soundCopy)

        self.updateLoudnessMeter(self.soundCopy)
        if self.isRecording:

            self.updateAllCanvases()

    def updateLoudnessMeter(self, sound):
        try:
            self.loudnessMeter.updateMeter(sound)

        except IndexError:
            pass

    def playAudioFile(self):
        self.loadedAudio.play()

    def loadAudioFile(self):
        self.formantPlotCanvas.delete("loadedLines")

        os.chdir("../..")
        self.xLoadedPlotList = []
        self.yLoadedPlotList = []

        self.loadedPlots = True
        print os.getcwd()
        filename = tkFileDialog.askopenfilename(initialdir="Recording")
        radius = self.formantRadius
        self.formantPlotCanvas.delete('loadedPlots')

        self.loadedAudio.config(load=filename)
        os.chdir("Formant/dist")
        import wave
        import contextlib
        fname = filename
        with contextlib.closing(wave.open(fname,'r')) as f:
            frames = f.getnframes()

            rate = f.getframerate()
            duration = float(frames) / float(rate)

        loadedPlots = SoundProcessing.getFormants(self.loadedAudio,self.gender,True)
        probabilityOfVoicingList = SoundProcessing.getProbabilityOfVoicing(self.loadedAudio,True)
        self.formantPlotCanvas.delete('loadedPlots')

        sleeptime =  float(duration)/len(probabilityOfVoicingList)

        thread.start_new_thread(self.multiThreadLoader, ("Thread-2", sleeptime, probabilityOfVoicingList, loadedPlots ))

    def multiThreadPlayer(self, threadName, sound):
        sound.play()

    '''
    multiThreadLoader plots the formant plots onto the formant plot using an additional thread to allow the application not to freeze
    especially if the audio file is very large.
    '''
    def multiThreadLoader(self, threadName, delay, probabilityOfVoicingList, loadedPlots) :
        self.hasloadedPlots = True
        self.count = 0
        try:
            radius = self.formantRadius
            for i in range(0,len(loadedPlots)):


                probabilityOfVoicing = probabilityOfVoicingList[i]
                if probabilityOfVoicing == 1.0:
                    formant = loadedPlots[i]
                    latestF1 = formant[0]/SCALEDOWN
                    latestF2 = PLOTWIDTH - (formant[1]/SCALEDOWN) + XSHIFT + XOFFSET

                    (xScale, xShift, yScale, yShift) = self.getPlotScaleInfo(self.vowelType)

                    if latestF2 > XSHIFT and latestF1 < PLOTHEIGHT-YSHIFT:


                        self.prevplotted = True
                        self.prevX = latestF2*xScale - xShift
                        self.prevY = latestF1*yScale - yShift
                        break
            for i in range(0,len(loadedPlots)):


                probabilityOfVoicing = probabilityOfVoicingList[i]
                if probabilityOfVoicing == 1.0:
                    formant = loadedPlots[i]
                    latestF1 = formant[0]/SCALEDOWN
                    latestF2 = PLOTWIDTH - (formant[1]/SCALEDOWN) + XSHIFT + XOFFSET

                    (xScale, xShift, yScale, yShift) = self.getPlotScaleInfo(self.vowelType)

                    if latestF2 > XSHIFT and latestF1 < PLOTHEIGHT-YSHIFT:

                        x = latestF2*xScale - xShift
                        y = latestF1*yScale - yShift

                        if ( (((x-self.prevX) ** 2 + (y-self.prevY) ** 2) ** (0.5)) < self.backgroundNoiseDistance):
                            self.xLoadedPlotList.append(x)
                            self.yLoadedPlotList.append(y)

                            x1 = x * (self.plotWidth/PLOTWIDTH) - radius
                            y1 = y * (self.plotHeight/PLOTHEIGHT) - radius
                            x2 = x * (self.plotWidth/PLOTWIDTH) + radius
                            y2 = y * (self.plotHeight/PLOTHEIGHT) + radius

                            if(self.prevplotted == True):
                                self.formantPlotCanvas.create_line(self.prevX * (self.plotWidth/PLOTWIDTH), self.prevY * (self.plotHeight/PLOTHEIGHT), x * (self.plotWidth/PLOTWIDTH), y * (self.plotHeight/PLOTHEIGHT), fill='black', tags='loadedLines')
                                pass

                            xCoord = x1

                            self.formantPlotCanvas.create_oval(x1,y1,x2,y2, fill='yellow', tags="loadedPlots")
                            self.prevplotted = True

                            self.count += 1
                        else:
                            self.prevplotter = False

                        self.prevX = x
                        self.prevY = y

        except Exception:
            import traceback
            print traceback.format_exc()
        self.formantPlotCanvas.itemconfig("loadedLines", state='hidden')
        self.lineHiddens = True

        self.redrawPlotInfo()
        self.count = 0

    def toggleLoadedPlots(self):
        if self.loadedPlots:
            self.formantPlotCanvas.itemconfig("loadedPlots", state="hidden")
            self.loadedPlots= False
        else:
            self.formantPlotCanvas.itemconfig("loadedPlots", state="normal")
            self.loadedPlots = True


    def toggleLines(self):
        if self.lineHiddens:
            self.formantPlotCanvas.itemconfig("loadedLines", state="normal")
            self.lineHiddens = False
        else:
            self.formantPlotCanvas.itemconfig("loadedLines", state="hidden")
            self.lineHiddens = True

    def toggleBackgroundNoise(self, level):
        if level == 0:
            self.backgroundNoiseDistance = 10000
        elif level == 1:
            self.backgroundNoiseDistance = 100
        elif level == 2:
            self.backgroundNoiseDistance = 50

        elif level == 3:
            self.backgroundNoiseDistance = 30
        else:
            self.backgroundNoiseDistance = 10

    def callback(self, event):
        print "canvas clicked"
