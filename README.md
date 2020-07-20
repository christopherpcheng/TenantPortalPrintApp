# TenantPortalPrintApp


## Info.plist before the last closing </dict>

```
    <key>CFBundleIdentifier</key>
    <string>com.mobilegroupinc.printapp</string>
    <key>CFBundleURLTypes</key>
    <array>
      <dict>
        <key>CFBundleURLName</key>
        <string>RTenantPortal</string>
        <key>CFBundleURLSchemes</key>
        <array>
          <string>rtenant-portal</string>
        </array>
      </dict>
    </array>	
```

## OSX Build commands from the project folder 
(Requires DotNet.Bundle)

```
dotnet restore -r osx-x64
dotnet msbuild -t:BundleApp -p:RuntimeIdentifier=osx-x64 -property:Configuration=Release 
```

## OSX commands

### Chmod for execution 
```
chmod +x Print\ App.app/Contents/MacOS/PrintApp
```
### Chmod for packaging
```
chmod -R 755 <folder containing the apps>
```
### AppleScript
Use Script Editor on macOS and save as application
```
on open location this_URL
	set this_URL to quoted form of this_URL
	do shell script "open -a /Applications/RTPPrintApp.app --args " & this_URL
end open location
```
### Manual Package building
```
/usr/bin/pkgbuild \
  --root "$TARGET_DIR" \
  --install-location "/Applications" \
  --identifier "$IDENTIFIER" \
  --version "$VERSION" \
  "$PKG_PATH"
```
### Removal of registered URI
```
/System/Library/Frameworks/CoreServices.framework/Frameworks/LaunchServices.framework/Support/lsregister -dump | grep "rtenant"
/System/Library/Frameworks/CoreServices.framework/Frameworks/LaunchServices.framework/Support/lsregister -u /Users/mobilegroupinc/Desktop/prt/Print\ App.app
```



