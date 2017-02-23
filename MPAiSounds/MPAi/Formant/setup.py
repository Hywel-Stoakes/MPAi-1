from distutils.core import setup
import py2exe

setup(
    console=['MPAiPlotRunner.py', 'MPAiVowelRunner.py'],
    options = {'py2exe': {"dll_excludes": ["msvcr90.dll"]}}
)
