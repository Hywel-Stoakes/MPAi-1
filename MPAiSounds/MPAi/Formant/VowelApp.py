"""
original code from the FormantPlot of which some of the code here is taken from was written by Byron Hui.

Author: Joshua Brudan
upi: jbru239

VowelApp acts as the outer Frame in which the target plot, and vowel buttons are accessed.
"""

from Tkinter import *

import os

from PIL import Image, ImageTk, ImageSequence #For Gif Playing.
import glob
from FormantPlot import FormantPlot
from VowelPlot import VowelPlot
from VowelScorer import VowelScorer
import sys
import thread



DEFAULTHEIGHT = 900 #Height of the MainFrame

FRAMEHEIGHT = 900
FRAMEWIDTH = 900

"""
VowelApp is the application which shows the VowelPlot target system. This is created for a particular voiceType and languageType
Which is defined at the Instantiation of the object. Allows the user to practice the pronouciation of Maori vowel sounds.
"""

class VowelApp:

    def __init__(self):

        #Makes sure parameters are correct and acceptable.
        if len(sys.argv) == 3:
            if sys.argv[1].lower() == 'masculine' or sys.argv[1].lower() == 'feminine':
                voiceType = sys.argv[1].lower()
            else:
                exceptString = 'Invalid VoiceType: '+sys.argv[1]+', Must be masculine or feminine'
                raise Exception(exceptString)
                self.quitPlot()
            if sys.argv[2].lower() == 'native' or sys.argv[2].lower() == 'modern':
                langType = sys.argv[2].lower()
            else:
                exceptString = 'Invalid LanguageType: '+sys.argv[2]+', Must be native or modern.'
                raise Exception(exceptString)
                self.quitPlot()
        else:
            exceptString = 'Invalid number of arguments , requires 3, ' + (str)(len(sys.argv)) + ' given.'
            try:
                raise Exception(exceptString)
            except Exception as e:
                print e
                # self.quitPlot()
                print 'for Testing, setting you to masculine and modern'
                voiceType = 'masculine'
                langType = 'native'


        self.vowelScorer = VowelScorer()
        self.id = self.getID(voiceType, langType)

        print 'starting vowel plot for ', voiceType, langType

        #set the default vowel
        self.vowel = 'a:'
        self.previousVowel = 'e:'

        self.initialiseRoot()

        #self.initiatePron()
        self.setUpMenus()
        self.initialiseFrame()

        self.loadVowelPlot(self.id, self.vowel, self.plotWidth, self.plotHeight)


    """
    Creates thr root for the vowel Plot application. Sets it default behaviour, such as resizable and what to do on exit.
    """
    def initialiseRoot(self):
        self.root = Tk()
        self.root.title("MPAi Vowel Plot") #Dont change the title without changing the c# code to end the process.
        self.root.protocol("WM_DELETE_WINDOW", self.quitApp) #Defines the default close operation.
        self.resizeCount=0

        try:
            maxWidth = self.root.winfo_screenwidth()
            maxHeight = self.root.winfo_screenheight()
            print maxWidth, maxHeight
            if maxHeight > 950:
                self.plotHeight = 900
                self.plotWidth = 900
            elif maxHeight > 650:
                self.plotHeight = 600
                self.plotWidth = 600
            else:
                self.plotHeight = 500
                self.plotWidth = 500
        except(Exception):
            self.plotWidth = FRAMEWIDTH
            self.plotHeight = FRAMEHEIGHT

        self.root.resizable(False, False) #Allows the window to be resized both verically and horizonally.
        self.positionWindowInCentre(self.root, self.plotWidth, self.plotHeight) #Determines where to place the Frame on the screen.

    def preventResizing(self):
        self.root.resizable(False, False)

    def allowResizing(self):
        self.root.resizable(False, False)

    def bindResizing(self):
        self.bindedFunctionID = self.frame.bind("<Configure>", self.resizeRequest)

    def resizeRequest(self, event):
        self.vowelPlot.onResize(event.width, event.height)

    def unbindResizing(self):
        print "unbinding resizable"
        self.root.unbind("<Configure>", self.bindedFunctionID)


    """
    Set the overall geometry of the application, its position of the screen as well as its size.
    """
    def positionWindowInCentre(self, widget, widgetWidth, widgetHeight):
        widthScreen = self.root.winfo_screenwidth() # gets the width of the screen
        heightScreen = self.root.winfo_screenheight() # gets the height of the screen
        x = (widthScreen/2) - (widgetWidth/2)
        y = (heightScreen/2) - (widgetHeight/2)
        widget.geometry('%dx%d+%d+%d' % (widgetWidth, widgetHeight, x, y)) #Sizes and Centres the application on the screen.

    """
    Sets the icon which apears at the topright of the application.
    """
    def setIcon(self):
        #TODO need an Icon.
        #self.root.iconbitmap(default='fPIcon.ico')
        pass

    """
    Gets the path of the current working directory.
    """
    def getAppPathName(self):
        return os.path.dirname(os.getcwd())


    """
    Converts a voiceType and landType into an idea which simplifies the process of determining which data to be loaded.
    """
    def getID(self, voiceType, langType):
        count = 0
        if(voiceType == 'feminine'):
            count+= 2
        if(langType == 'modern'):
            count+= 1
        return count

    """
    Sets up the menu for the vowel plot application.
    """
    def setUpMenus(self):
        #TODO Currently used as a proof of concept to show the gif player alone side the target.
        #The gif player seems to make the target unstable.

        menubar = Menu(self.root)
        #self.showingPron = False
        #menubar.add_command(label="Toggle: Maori Vowel Pronociation Help", command=self.togglePronHelp)

        self.root.config(menu=menubar)

    """
    Sets up the layout of the Frame in which the target is located.
    """
    def initialiseFrame(self):
        # Grid.rowconfigure(self.root, 0, weight=0)
        # Grid.columnconfigure(self.root, 0, weight=1)
        self.frame = Frame(self.root, bg='white')
        self.frame.pack(fill=BOTH, expand = YES)
        # self.frame.grid(row=0, column=0, sticky=N+S+E+W)

    """
    Loads in the vowel plot for the particular id and vowel. This should only be done while not Recording.
    The buttons which call this method are hidden durinig the recording process.
    """
    def loadVowelPlot(self, id, vowel, width, height):
        self.killFramesChildren()
        self.vowelPlot = VowelPlot(self.frame, self.getAppPathName(), self.root, id, self, self.vowelScorer, vowel, width, height )
        self.vowelPlot.onResize(width,height)
        self.bindResizing()

    """
    Removes the children of the frame which in this case is the Vowel Plot canvas, of which the target and buttons are children of.
    """
    def killFramesChildren(self):
        for widget in self.frame.winfo_children():
            widget.destroy()


    def mainloop(self):
        self.root.mainloop()

    def quitApp(self):
        print("Exiting")
        self.vowelScorer.connectAndSendText()
        self.root.destroy()
        sys.exit(0)
        quit()
