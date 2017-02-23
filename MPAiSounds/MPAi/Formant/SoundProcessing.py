'''
A set of methods used by the formant aid to perform its task.
'''

from tkSnack import *
import SoundProcessing

PREEMPFACTOR = 0.95
NUMFORMANTS = 3
FRAMELENGTHFORSINGLE =0.02


'''
Retrieves the last formant of the currently recording sound.
Arguments: sound - sound file being processed.
           gender - boolean, True if male. False if female.
           getAll - boolean, True if the application requires all the formants of the audio file. False if it only wants the last one.
Errors - No sound is currently being recorded.
Returns - formant/s of recording sound.
'''
def getFormants(sound,id,getAll):
    try:
        if id == 0 or id == 1:
            gender = "male"
            nomF1Freq = 400
        else:
            gender = "female"
            nomF1Freq = 500

        # getAll determines if the formant plot requires the last formant or all of the formants.
        if getAll == True:

            #calculate all the formants in the sound
            #default WindowLength is 0.049
            #default frameLength is 0.01
            #default lpcorder is 12
            #default windowing function is cos^4
            formants = sound.formant(numformants= NUMFORMANTS, preemphasisfactor = PREEMPFACTOR, nom_f1_freq = nomF1Freq)


            #formants = sound.formant(numformants = NUMFORMANTS, preemphasisfactor = PREEMPFACTOR, framelength = FRAMELENGTHFORALL, nom_f1_freq = nomF1Freq, lpcorder = LPCORDER)

        else:
            #calculate all the formant vectors in the sound. This sound if meant to be cropped to be as small as possible.
            #default WindowLength is 0.049
            #default frameLength is FRAMELENGTHFORSINGLE as 0.01 gives an error. bad lcp_poles.
            #default lpcorder is 12
            #default windowing function is cos^4
            formants = sound.formant(numformants = NUMFORMANTS, framelength =FRAMELENGTHFORSINGLE  ,preemphasisfactor = PREEMPFACTOR, nom_f1_freq = nomF1Freq)
            #Get the last formant vector.
            formants = formants[len(formants)-1]


            #formants = sound.formant(numformants = NUMFORMANTS, preemphasisfactor = PREEMPFACTOR, framelength = FRAMELENGTHFORSINGLE, nom_f1_freq = nomF1Freq, lpcorder = LPCORDER)[len(sound.formant())-1]

        return formants
    except IndexError:
        print "No formants found\n"
        return None

'''
Arguments: sound - sound file being processed.
           getAll - boolean, True if the application requires all the formants of the audio file. False if it only wants the last one.
Errors - No sound is currently being recorded.
Returns - 1.0 if the latest sound is from a person speaking. 0.0 if the latest sound is background noise.
'''
def getProbabilityOfVoicing(sound,getAll):
    try:
        if getAll == True:
            pitchValues = sound.pitch(method='ESPS')
            probabilityOfVoicing = []
            for pitchValue in pitchValues:
                probabilityOfVoicing.append(pitchValue[1])

        else:
            latestPitchValues = sound.pitch(method='ESPS')[len(sound.pitch(method='ESPS')) - 1]
            probabilityOfVoicing = latestPitchValues[1]

        return probabilityOfVoicing

    except IndexError:
        return 0


'''
Deletes the entire sound except for the last second of the sound
Returns - the last second of the sound
'''
def crop(sound):
    cropLength = 0.3 #The length of the crop to be used in real time formant creation. The smaller the less computation must be done before each formant can be loaded onto the plot. But the smaller the worse for signal processing.

    soundLength = sound.length(unit="SECONDS")

    soundInfo = sound.info()
    soundSampleRate = soundInfo[1]
    if soundLength > cropLength:
        sound.crop(start=(int)((soundLength - cropLength)*soundSampleRate),end=sound.length())
    return sound
