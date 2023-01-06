# Windows GitLab Runner Tray Status
This is a Windows application that lets you control `gitlab-runner` service from system tray icon.

![demo animation](./docs/images/demo-animation.gif)

## Features
* Start and stop the service from system tray icon context menu.
* Show color or b/w icon depending on the status of the service.

## Build & Install
PowerShell commands below build and add a startup shortcut (run as administrator to start/stop the service).

```PowerShell
dotnet build src
```
```PowerShell
$WshShell = New-Object -comObject WScript.Shell
$ShortcutFile = "$env:APPDATA\Microsoft\Windows\Start Menu\Programs\Startup\GitLabRunnerStatus.lnk";
$Shortcut = $WshShell.CreateShortcut($ShortcutFile)
$Shortcut.TargetPath = "$((Get-Item .).FullName)\src\TrayStatus.Bootstrapper\bin\Debug\net7.0-windows\TrayStatus.Bootstrapper.exe"
$Shortcut.Save()

$bytes = [System.IO.File]::ReadAllBytes($ShortcutFile)
$bytes[0x15] = $bytes[0x15] -bor 0x20 #set byte 21 (0x15) bit 6 (0x20) ON
[System.IO.File]::WriteAllBytes($ShortcutFile, $bytes)
```
