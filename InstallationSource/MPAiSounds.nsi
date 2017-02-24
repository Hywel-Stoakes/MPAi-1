
!include "MUI.nsh"

!define APP_NAME "MPAiSounds"
!define COMP_NAME "MPAi"
!define VERSION "4.2.0.0"
!define COPYRIGHT "MPAi  © 2017"
!define DESCRIPTION "Maori Pronunciation Aid for Sounds"
!define INSTALLER_NAME "C:\Users\joshb\Desktop\Nsisqssg\Output\MPAi\MPAISounds_Setup.exe"
!define MAIN_APP_EXE "MPAiSounds.exe"
!define INSTALL_TYPE "SetShellVarContext all"
!define REG_ROOT "HKLM"
!define REG_APP_PATH "Software\Microsoft\Windows\CurrentVersion\App Paths\${MAIN_APP_EXE}"
!define UNINSTALL_PATH "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}"
!define MUI_ICON "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\Resources\MPAi.ico"
!define MUI_UNICON "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\Resources\MPAi.ico"
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "C:\Users\joshb\Desktop\Nsisqssg\Output\MPAi\header.bmp"
!define MUI_HEADERIMAGE_BITMAP_NOSTRETCH
!define MUI_WELCOMEFINISHPAGE_BITMAP "C:\Users\joshb\Desktop\Nsisqssg\Output\MPAi\SplashScreen.bmp"
!define MUI_WELCOMEFINISHPAGE_BITMAP_NOSTRETCH
!define MUI_UNHEADERIMAGE_BITMAP "C:\Users\joshb\Desktop\Nsisqssg\Output\MPAi\header.bmp"
!define MUI_UNHEADERIMAGE_BITMAP_NOSTRETCH
!define MUI_UNWELCOMEFINISHPAGE_BITMAP "C:\Users\joshb\Desktop\Nsisqssg\Output\MPAi\SplashScreen.bmp"
!define MUI_UNWELCOMEFINISHPAGE_BITMAP_NOSTRETCH
######################################################################

VIProductVersion  "${VERSION}"
VIAddVersionKey "ProductName"  "${APP_NAME}"
VIAddVersionKey "CompanyName"  "${COMP_NAME}"
VIAddVersionKey "LegalCopyright"  "${COPYRIGHT}"
VIAddVersionKey "FileDescription"  "${DESCRIPTION}"
VIAddVersionKey "FileVersion"  "${VERSION}"

######################################################################

SetCompressor ZLIB
Name "${APP_NAME}"
Caption "${APP_NAME}"
OutFile "${INSTALLER_NAME}"
BrandingText "${APP_NAME}"
XPStyle on
InstallDirRegKey "${REG_ROOT}" "${REG_APP_PATH}" ""
InstallDir "$APPDATA\MPAi\MPAiSounds"
######################################################################

!define MUI_ABORTWARNING
!define MUI_ABORTWARNING_TEXT "Are you sure you wish to cancel the installation of MPAi?"
!define MUI_UNABORTWARNING
!define MUI_UNABORTWARNING_TEXT "Are you sure you wish to cancel the uinstallation of MPAi?          Yes, will prevent MPAi from being removed, and data deleted."

!ifdef REG_START_MENU
!define MUI_STARTMENUPAGE_NODISABLE
!define MUI_STARTMENUPAGE_DEFAULTFOLDER "MPAiSounds"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "${REG_ROOT}"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "${UNINSTALL_PATH}"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "${REG_START_MENU}"
!insertmacro MUI_PAGE_STARTMENU Application $SM_Folder
!endif

!insertmacro MUI_PAGE_WELCOME
!define MUI_WELCOMEPAGE_TITLE "Install MPAi Sounds"

Function .onInit
 
  ReadRegStr $R0 HKLM \
  "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" \
  "UninstallString"
  StrCmp $R0 "" done
 
  MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION \
  "${APP_NAME} is already installed. $\n$\nClick `OK` to remove the \
  previous version or `Cancel` to cancel this upgrade." \
  IDOK uninst
  Abort
 
uninst:
  ClearErrors
  ExecWait '$R0 _?=$INSTDIR' 
 
  IfErrors no_remove_uninstaller done
  no_remove_uninstaller:
 
done:
 
FunctionEnd
######################################################################
#This code checks if .net framework is installed else it will install it.
!define NETVersion "3.5"
!define NETInstaller "dotNetFx35setup.exe"
Section "-hidden section" SecFramework 
  IfFileExists "$WINDIR\Microsoft.NET\Framework\v${NETVersion}" NETFrameworkInstalled 0
  File /oname=$TEMP\${NETInstaller} ${NETInstaller}
 
  DetailPrint "Starting Microsoft .NET Framework v${NETVersion} Setup..."
  ExecWait "$TEMP\${NETInstaller}"
  Return
 
  NETFrameworkInstalled:
  DetailPrint "Microsoft .NET Framework is already installed!"
 
SectionEnd


######################################################################

!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM

!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "English"



######################################################################

Section MPAI_Sound
${INSTALL_TYPE}
SetOverwrite ifnewer
SetOutPath "$INSTDIR"
File /r "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\Video"
File /r "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\Resources"
File /r "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\Formant"
File /r "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\VlcLibs"

File "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\bin\Debug\EntityFramework.dll"
File "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\bin\Debug\EntityFramework.SqlServer.dll"
File "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\bin\Debug\EntityFramework.SqlServer.xml"
File "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\bin\Debug\EntityFramework.xml"
File "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\bin\Debug\MPAiSounds.exe"
File "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\bin\Debug\NAudio.dll"
File "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\bin\Debug\NAudio.xml"
File "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\bin\Debug\Vlc.DotNet.Core.dll"
File "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\bin\Debug\Vlc.DotNet.Core.Interops.dll"
File "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\bin\Debug\Vlc.DotNet.Forms.dll"
File "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\bin\Debug\Vlc.DotNet.Wpf.dll"
File "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\bin\Debug\MPAiSounds.exe.config"
File "C:\Users\joshb\Desktop\MPAiSafe\MPAi\MPAi\MPAiSounds\MPAi\bin\Debug\MPAiSounds.pdb"

SectionEnd

######################################################################


Section -Icons_Reg
SetOutPath "$INSTDIR"
WriteUninstaller "$INSTDIR\uninstallSounds.exe"
!ifdef REG_START_MENU
!insertmacro MUI_STARTMENU_WRITE_BEGIN Application
CreateDirectory "$SMPROGRAMS\$SM_Folder"
CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
CreateShortCut "$DESKTOP\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}" "" "$INSTDIR\Resources\MPAi.ico"
CreateShortCut "$SMPROGRAMS\$SM_Folder\Uninstall ${APP_NAME}.lnk" "$INSTDIR\uninstallSounds.exe"

!insertmacro MUI_STARTMENU_WRITE_END
!endif

!ifndef REG_START_MENU
CreateDirectory "$SMPROGRAMS\MPAiSounds"
CreateShortCut "$SMPROGRAMS\MPAiSounds\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}" "" "$INSTDIR\Resources\MPAi.ico"
CreateShortCut "$DESKTOP\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}" "" "$INSTDIR\Resources\MPAi.ico"
CreateShortCut "$SMPROGRAMS\MPAi\Uninstall ${APP_NAME}.lnk" "$INSTDIR\uninstallSounds.exe"

!endif

WriteRegStr ${REG_ROOT} "${REG_APP_PATH}" "" "$INSTDIR\${MAIN_APP_EXE}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayName" "${APP_NAME}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "UninstallString" "$INSTDIR\uninstallSounds.exe"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayIcon" "$INSTDIR\Resources\MPAi.ico"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayVersion" "${VERSION}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "Publisher" "${COMP_NAME}"


SectionEnd

######################################################################

Section "Uninstall"
${INSTALL_TYPE}

Delete "$INSTDIR\AppSettings.dat"
Delete "$INSTDIR\SystemPaths.dat"

Delete "$INSTDIR\MPAiSounds.exe"

Delete "$INSTDIR\EntityFramework.dll"
Delete "$INSTDIR\EntityFramework.SqlServer.dll"
Delete "$INSTDIR\EntityFramework.SqlServer.xml"
Delete "$INSTDIR\EntityFramework.xml"
Delete "$INSTDIR\MPAiSounds.exe"
Delete "$INSTDIR\NAudio.dll"
Delete "$INSTDIR\NAudio.xml"
Delete "$INSTDIR\Vlc.DotNet.Core.dll"
Delete "$INSTDIR\Vlc.DotNet.Core.Interops.dll"
Delete "$INSTDIR\Vlc.DotNet.Forms.dll"
Delete "$INSTDIR\Vlc.DotNet.Wpf.dll"
Delete "$INSTDIR\MPAiSounds.exe.config"
Delete "$INSTDIR\MPAiSounds.pdb"

RMDir /r "$INSTDIR\Database"
RMDir /r "$INSTDIR\Formant"
RMDir /r "$INSTDIR\Resources"
RMDir /r "$INSTDIR\ScoreboardReports"
RMDir /r "$INSTDIR\Video"
RMDir /r "$INSTDIR\VlcLibs"
RMDir /r "$INSTDIR\Recording"


Delete "$INSTDIR\uninstallSounds.exe"

RMDir "$INSTDIR"

!ifdef REG_START_MENU
!insertmacro MUI_STARTMENU_GETFOLDER "Application" $SM_Folder
Delete "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk"
Delete "$SMPROGRAMS\$SM_Folder\Uninstall ${APP_NAME}.lnk"
Delete "$DESKTOP\${APP_NAME}.lnk"

RMDir "$SMPROGRAMS\$SM_Folder"
!endif

!ifndef REG_START_MENU
Delete "$SMPROGRAMS\MPAi\${APP_NAME}.lnk"
Delete "$SMPROGRAMS\MPAi\Uninstall ${APP_NAME}.lnk"

Delete "$DESKTOP\${APP_NAME}.lnk"

RMDir "$SMPROGRAMS\MPAiSounds"
!endif

DeleteRegKey ${REG_ROOT} "${REG_APP_PATH}"
DeleteRegKey ${REG_ROOT} "${UNINSTALL_PATH}"
SectionEnd

######################################################################

