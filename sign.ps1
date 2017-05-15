param($dllPath, $dllName, $keyPath)

$fullPath = Join-Path $dllPath $dllName

& "c:\Program Files (x86)\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools\ildasm.exe" /all /typelist /out="$fullPath.il" "$fullPath.dll"

& "c:\Windows\Microsoft.NET\Framework\v4.0.30319\ilasm.exe" /dll /optimize /key=$keyPath "$fullPath.il" /output="$fullPath.dll"