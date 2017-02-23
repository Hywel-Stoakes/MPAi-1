from VowelScorer import *
import sys

VowelScorer = VowelScorer()
VowelScorer.updateScore('a:', 700, [7,4,0,0,3])
VowelScorer.updateScore('a:', 400, [4,1,1,2,0])
VowelScorer.updateScore('e:', 1100, [11,5,1,2,3])
VowelScorer.updateScore('i:', 600, [6,2,1,1,2])
VowelScorer.updateScore('u:', 1100, [11,5,1,2,3])
VowelScorer.updateScore('o:', 1100, [11,5,1,2,3])
VowelScorer.updateScore('i:', 500, [5,3,0,1,1])

VowelScorer.connectAndSendText()
raw_input()
