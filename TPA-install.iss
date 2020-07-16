; -- Example1.iss --
; Demonstrates copying 3 files and creating an icon.

; SEE THE DOCUMENTATION FOR DETAILS ON CREATING .ISS SCRIPT FILES!

[Setup]
AppName=Tenant Portal Print App
AppVersion=0.2.0
WizardStyle=modern
DefaultDirName={autopf}\Tenant Portal Print App
UninstallDisplayIcon={app}\RTPPrintApp.exe
Compression=lzma2
;Compression=none
;SolidCompression=yes
DisableProgramGroupPage=yes
OutputBaseFilename=SetupRTPPrintApp

[Files]
Source: "PrintApp\bin\Release\netcoreapp3.0\publish\win-x86\RTPPrintApp.exe"; DestDir: "{app}"; Flags: ignoreversion

[Registry]
Root: HKCR; Subkey: "rtenant-portal"; ValueType: "string"; ValueData: "URL:rtenant-portal Protocol" ; Flags: uninsdeletekey; Check: IsAdminInstallMode
;Root: HKCR; Subkey: "ctp"; ValueType: "string"; ValueData: "URL:ptapp Protocol"; Flags: uninsdeletekey

Root: HKCR; Subkey: "rtenant-portal"; ValueType: "string"; ValueName: "URL Protocol"; ValueData: "" ; Flags: uninsdeletevalue; Check: IsAdminInstallMode
Root: HKCR; Subkey: "rtenant-portal\shell\open\command";  ValueType: "string"; ValueData: """{app}\RTPPrintApp.exe"" ""%1""" ; Flags: uninsdeletevalue; Check: IsAdminInstallMode


;[Registry]
;Root: HKCR; Subkey: "ctp"; ValueType: "string"; ValueData: "URL:ptapp Protocol"; Flags: uninsdeletekey
;Root: HKCR; Subkey: "ctp"; ValueType: "string"; ValueName: "URL Protocol"; ValueData: ""
;Root: HKCR; Subkey: "ctp\DefaultIcon"; ValueType: "string"; ValueData: "{app}\YourApp.exe,0"
;Root: HKCR; Subkey: "ctp\shell\open\command"; ValueType: "string"; ValueData: """{app}\YourApp.exe"" ""%1"""

;Root: HKA; Subkey: "Software\My Company\My Program"; Flags: uninsdeletekey
;Root: HKA; Subkey: "Software\My Company\My Program\Settings"; ValueType: string; ValueName: "Language"; ValueData: "{language}"
; Associate .myp files with My Program (requires ChangesAssociations=yes)
;Root: HKA; Subkey: "Software\Classes\.myp"; ValueType: string; ValueName: ""; ValueData: "MyProgramFile.myp"; Flags: uninsdeletevalue
;Root: HKA; Subkey: "Software\Classes\.myp\OpenWithProgids"; ValueType: string; ValueName: "MyProgramFile.myp"; ValueData: ""; Flags: uninsdeletevalue
;Root: HKA; Subkey: "Software\Classes\MyProgramFile.myp"; ValueType: string; ValueName: ""; ValueData: "My Program File"; Flags: uninsdeletekey
;Root: HKA; Subkey: "Software\Classes\MyProgramFile.myp\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\MyProg.exe,0"
;Root: HKA; Subkey: "Software\Classes\MyProgramFile.myp\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\MyProg.exe"" ""%1"""
; HKA (and HKCU) should only be used for settings which are compatible with
; roaming profiles so settings like paths should be written to HKLM, which
; is only possible in administrative install mode.
;Root: HKLM; Subkey: "Software\My Company"; Flags: uninsdeletekeyifempty; Check: IsAdminInstallMode
;Root: HKLM; Subkey: "Software\My Company\My Program"; Flags: uninsdeletekey; Check: IsAdminInstallMode
;Root: HKLM; Subkey: "Software\My Company\My Program\Settings"; ValueType: string; ValueName: "InstallPath"; ValueData: "{app}"; Check: IsAdminInstallMode
; User specific settings should always be written to HKCU, which should only
; be done in non administrative install mode.
;Root: HKCU; Subkey: "Software\My Company\My Program\Settings"; ValueType: string; ValueName: "UserName"; ValueData: "{userinfoname}"; Check: not IsAdminInstallMode
;Root: HKCU; Subkey: "Software\My Company\My Program\Settings"; ValueType: string; ValueName: "UserOrganization"; ValueData: "{userinfoorg}"; Check: not IsAdminInstallMode

