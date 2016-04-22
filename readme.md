If you get:

"This project references NuGet package(s) that are missing on this computer. Use NuGet Package Restore to download them"

1. Open \BusProviders\Microsoft.IoT.Lightning.Providers\Microsoft.Iot.Lightning.Providers.sln
2. Compile or restore packages so you get dependencies in \BusProviders\Microsoft.IoT.Lightning.Providers\packages
3. Close

This is because \BusProviders is a submodule, and normally you would want \Providers\Microsoft.Iot.Lightning.Providers.vcxproj at a sibling level