!include "MUI2.nsh"

; ── Compression ──────────────────────────────────────────────────────────────
SetCompressor /SOLID lzma
SetCompressorDictSize 64

; ── App Info ──────────────────────────────────────────────────────────────────
Name "ProtoBot Rebuilt"
OutFile "ProtoBotRebuilt-Installer.exe"
InstallDir "$PROGRAMFILES64\ProtoBot Rebuilt"
InstallDirRegKey HKLM "Software\ProtoBot Rebuilt" "InstallDir"
RequestExecutionLevel admin

; ── UI ──────────────────────────────────────────────────────────────────────
!define MUI_ABORTWARNING
!define MUI_ICON "Assets/Sprites/Logos/AppIcon.ico"
!define MUI_UNICON "Assets/Sprites/Logos/AppIcon.ico"

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_LANGUAGE "English"

; ── Install ──────────────────────────────────────────────────────────────────
Section "Install"
  SetOutPath "$INSTDIR"
  File /r "build\StandaloneWindows64\*"

  ; Models folder for saved builds; grant Users write access so the app
  ; can save there without running as admin (S-1-5-32-545 = BUILTIN\Users)
  CreateDirectory "$INSTDIR\Models"
  nsExec::ExecToLog 'icacls "$INSTDIR\Models" /grant *S-1-5-32-545:(OI)(CI)M'

  ; Start menu shortcut
  CreateDirectory "$SMPROGRAMS\ProtoBot Rebuilt"
  CreateShortcut "$SMPROGRAMS\ProtoBot Rebuilt\ProtoBot Rebuilt.lnk" "$INSTDIR\ProtoBot Rebuilt.exe"

  ; Desktop shortcut
  CreateShortcut "$DESKTOP\ProtoBot Rebuilt.lnk" "$INSTDIR\ProtoBot Rebuilt.exe"

  ; Write uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"

  ; Add to Windows Add/Remove Programs
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ProtoBot Rebuilt" "DisplayName" "ProtoBot Rebuilt"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ProtoBot Rebuilt" "UninstallString" "$INSTDIR\Uninstall.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ProtoBot Rebuilt" "InstallLocation" "$INSTDIR"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ProtoBot Rebuilt" "DisplayIcon" "$INSTDIR\ProtoBot Rebuilt.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ProtoBot Rebuilt" "Publisher" "ProtoBot Rebuilt"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ProtoBot Rebuilt" "DisplayVersion" "1.3.7"
SectionEnd

; ── Uninstall ────────────────────────────────────────────────────────────────
Section "Uninstall"
  RMDir /r "$INSTDIR"

  Delete "$DESKTOP\ProtoBot Rebuilt.lnk"
  RMDir /r "$SMPROGRAMS\ProtoBot Rebuilt"

  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\ProtoBot Rebuilt"
  DeleteRegKey HKLM "Software\ProtoBot Rebuilt"
SectionEnd