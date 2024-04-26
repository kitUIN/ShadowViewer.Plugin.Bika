$OutDir = $args[0]
$ProjectDir = $args[1]
$AssemblyName = $args[2]
$Configuration = $args[3]
$DebugDir = Join-Path -Path $ProjectDir -ChildPath $OutDir
$RemoveFiles = @("ZstdSharp.dll","ShadowViewer.Analyzer.dll","Microsoft.Windows.Widgets.Providers.Projection.dll","kitUIN.FluentIcon.WinUI.Filled.dll","kitUIN.FluentIcon.WinUI.Regular.dll","PicaComic.xml","FluentIcon\Assets\FluentSystemIcons-Resizable.ttf", "Microsoft.Xaml.Interactivity.dll","Microsoft.Xaml.Interactions.dll","DryIoc.dll","CommunityToolkit.WinUI.UI.Media.dll","CommunityToolkit.WinUI.UI.Animations.dll","CommunityToolkit.WinUI.Extensions.dll","CommunityToolkit.WinUI.Controls.Primitives.dll","CommunityToolkit.WinUI.Behaviors.dll","CommunityToolkit.WinUI.Animations.dll","ColorCode.Core.dll","ColorCode.WinUI.dll","CommunityToolkit.Common.dll","CommunityToolkit.Labs.WinUI.SettingsControls.dllCommunityToolkit.Labs.WinUI.TokenView.dll","CommunityToolkit.Labs.WinUI.TransitionHelper.dll","CommunityToolkit.Mvvm.dll","CommunityToolkit.WinUI.dll","CommunityToolkit.Labs.WinUI.SettingsControls.dll","CommunityToolkit.Labs.WinUI.TokenView.dll","CommunityToolkit.WinUI.UI.Controls.Core.dll","CommunityToolkit.WinUI.UI.Controls.DataGrid.dll","CommunityToolkit.WinUI.UI.Controls.Input.dll","CommunityToolkit.WinUI.UI.Controls.Layout.dll","CommunityToolkit.WinUI.UI.Controls.Markdown.dll","CommunityToolkit.WinUI.UI.Controls.Media.dll","CommunityToolkit.WinUI.UI.Controls.Primitives.dll","CommunityToolkit.WinUI.UI.dll","CustomExtensions.WinUI.dll","DmProvider.dll","Kdbndp.dll","Microsoft.Data.SqlClient.dll","Microsoft.Data.Sqlite.dll","Microsoft.Extensions.DependencyInjection.Abstractions.dll","Microsoft.Extensions.DependencyInjection.dll","Microsoft.Graphics.Canvas.Interop.dll","Microsoft.Identity.Client.dll","Microsoft.IdentityModel.JsonWebTokens.dll","Microsoft.IdentityModel.Logging.dll","Microsoft.IdentityModel.Protocols.dll","Microsoft.IdentityModel.Protocols.OpenIdConnect.dll","Microsoft.IdentityModel.Tokens.dll","Microsoft.InteractiveExperiences.Projection.dll","Microsoft.Win32.SystemEvents.dll","Microsoft.Windows.ApplicationModel.DynamicDependency.Projection.dll","Microsoft.Windows.ApplicationModel.Resources.Projection.dll","Microsoft.Windows.ApplicationModel.WindowsAppRuntime.Projection.dll","Microsoft.Windows.AppLifecycle.Projection.dll","Microsoft.Windows.AppNotifications.Builder.Projection.dll","Microsoft.Windows.AppNotifications.Projection.dll","Microsoft.Windows.PushNotifications.Projection.dll","Microsoft.Windows.SDK.NET.dll","Microsoft.Windows.Security.AccessControl.Projection.dll","Microsoft.Windows.System.Power.Projection.dll","Microsoft.Windows.System.Projection.dll","Microsoft.Windows.Widgets.Projection.dll","Microsoft.WindowsAppRuntime.Bootstrap.Net.dll","Microsoft.WindowsAppRuntime.Release.Net.dll","Microsoft.WinUI.dll","MySqlConnector.dll","Newtonsoft.Json.dll","Npgsql.dll","Oracle.ManagedDataAccess.dll","Serilog.dll","Serilog.Sinks.File.dll","ShadowViewer.Core.dll","ShadowViewer.Core.pdb","ShadowViewer.Core.pri","SharpCompress.dll","SQLitePCLRaw.batteries_v2.dll","SQLitePCLRaw.core.dll","SQLitePCLRaw.provider.e_sqlite3.dll","SqlSugar.dll","System.Configuration.ConfigurationManager.dll","System.Diagnostics.PerformanceCounter.dll","System.DirectoryServices.dll","System.DirectoryServices.Protocols.dll","System.Drawing.Common.dll","System.IdentityModel.Tokens.Jwt.dll","System.Net.Http.WinHttpHandler.dll","System.Runtime.Caching.dll","System.Security.Cryptography.ProtectedData.dll","System.Security.Permissions.dll","System.Windows.Extensions.dll","WinRT.Runtime.dll","runtimes\alpine-arm\native\libe_sqlite3.so","runtimes\alpine-arm64\native\libe_sqlite3.so","runtimes\alpine-x64\native\libe_sqlite3.so","runtimes\browser-wasm\nativeassets\net6.0\e_sqlite3.a","runtimes\linux\lib\net6.0\System.DirectoryServices.Protocols.dll","runtimes\linux-arm\native\libe_sqlite3.so","runtimes\linux-arm64\native\libe_sqlite3.so","runtimes\linux-armel\native\libe_sqlite3.so","runtimes\linux-mips64\native\libe_sqlite3.so","runtimes\linux-musl-arm\native\libe_sqlite3.so","runtimes\linux-musl-arm64\native\libe_sqlite3.so","runtimes\linux-musl-x64\native\libe_sqlite3.so","runtimes\linux-ppc64le\native\libe_sqlite3.so","runtimes\linux-s390x\native\libe_sqlite3.so","runtimes\linux-x64\native\libe_sqlite3.so","runtimes\linux-x86\native\libe_sqlite3.so","runtimes\maccatalyst-arm64\native\libe_sqlite3.dylib","runtimes\maccatalyst-x64\native\libe_sqlite3.dylib","runtimes\osx\lib\net6.0\System.DirectoryServices.Protocols.dll","runtimes\osx-arm64\native\libe_sqlite3.dylib","runtimes\osx-x64\native\libe_sqlite3.dylib","runtimes\unix\lib\net6.0\System.Drawing.Common.dll","runtimes\unix\lib\netcoreapp3.1\Microsoft.Data.SqlClient.dll","runtimes\win\lib\net6.0\Microsoft.Win32.SystemEvents.dll","runtimes\win\lib\net6.0\System.Diagnostics.PerformanceCounter.dll","runtimes\win\lib\net6.0\System.DirectoryServices.dll","runtimes\win\lib\net6.0\System.DirectoryServices.Protocols.dll","runtimes\win\lib\net6.0\System.Drawing.Common.dll","runtimes\win\lib\net6.0\System.Net.Http.WinHttpHandler.dll","runtimes\win\lib\net6.0\System.Security.Cryptography.ProtectedData.dll","runtimes\win\lib\net6.0\System.Windows.Extensions.dll","runtimes\win\lib\netcoreapp3.1\Microsoft.Data.SqlClient.dll","runtimes\win\lib\netstandard2.0\System.Runtime.Caching.dll","runtimes\win-arm\native\e_sqlite3.dll","runtimes\win-arm\native\Microsoft.Data.SqlClient.SNI.dll","runtimes\win-arm64\native\e_sqlite3.dll","runtimes\win-arm64\native\Microsoft.Data.SqlClient.SNI.dll","runtimes\win-x64\native\e_sqlite3.dll","runtimes\win-x64\native\Microsoft.Data.SqlClient.SNI.dll","runtimes\win-x86\native\e_sqlite3.dll","runtimes\win-x86\native\Microsoft.Data.SqlClient.SNI.dll","runtimes\win10-arm64\native\Microsoft.Graphics.Canvas.dll","runtimes\win10-arm64\native\Microsoft.WindowsAppRuntime.Bootstrap.dll","runtimes\win10-x64\native\Microsoft.Graphics.Canvas.dll","runtimes\win10-x64\native\Microsoft.WindowsAppRuntime.Bootstrap.dll","runtimes\win10-x86\native\Microsoft.Graphics.Canvas.dll","runtimes\win10-x86\native\Microsoft.WindowsAppRuntime.Bootstrap.dll","ShadowViewer.Core\Controls\StatusBlock.xaml","ShadowViewer.Core\Controls\StatusBlock.xbf","ShadowViewer.Core\Themes\Generic.xaml","ShadowViewer.Core\Themes\Generic.xbf","CommunityToolkit.Labs.WinUI.SegmentedControl.dll")
foreach ($Element in $RemoveFiles) {
    $File = Join-Path -Path $DebugDir -ChildPath $Element
    if(Test-Path -Path $File)
    {
        Remove-Item -Path $File
        Write-Host "Remove Existing Dependent Files:" $File
    }
    
}
Get-ChildItem $DebugDir -force -recurse | Where-Object{$_.psiscontainer -and (Get-ChildItem $_.FullName -force -recurse|Where-Object{!$_.psiscontainer}).count -eq 0}| Remove-Item -Recurse -force
$PackagesPath = Join-Path -Path $ProjectDir -ChildPath "Packages"
if (!(Test-Path -Path $PackagesPath)) {
    New-Item -ItemType Directory -Path $PackagesPath
}
$PluginFile = $AssemblyName + '.*'

Get-ChildItem $DebugDir | ForEach-Object -Process {
    if($_ -is [System.IO.DirectoryInfo])
    {
        if($_.name -ne $AssemblyName){
            Write-Host $_.name
            if($_.name -like 'ShadowViewer.Plugin.*')
            {
                Remove-Item -Path $_.FullName -Recurse
            }
            if($_.name -eq "ShadowViewer.Core")
            {
                Remove-Item -Path $_.FullName -Recurse
            }
        }
       
    }else
    {
        if($_.name -notlike $PluginFile){
            if($_.name -like 'ShadowViewer.Plugin.*'){
                Remove-Item -Path $_.FullName
            } 
        }
    }
}

$csproj = $OutDir + $AssemblyName + ".csproj" 
$outjson = $OutDir + "plugin.json"
$xmldata = [xml](Get-Content $csproj)
$id = $xmldata.Project.PropertyGroup.PackageId.Replace("ShadowViewer.Plugin.","")
$name = $xmldata.Project.PropertyGroup.PluginName
$description = $xmldata.Project.PropertyGroup.Description
$logo = $xmldata.Project.PropertyGroup.PluginLogo
$version = $xmldata.Project.PropertyGroup.Version
$author = $xmldata.Project.PropertyGroup.Authors
$webUri = $xmldata.Project.PropertyGroup.RepositoryUrl
$lang = $xmldata.Project.PropertyGroup.PluginLang.Split(";")
$require = @()
$coreVersion = "0.0.0.0"
foreach ($itemGroup in $xmldata.Project.ItemGroup)
{
  foreach ($item in $itemGroup.ChildNodes)
  {
    if($item.Name -eq "PackageReference")
    {
        if($item.GetAttribute("Include").StartsWith("ShadowViewer.Plugin."))
        {
            $key = $item.GetAttribute("Include").Replace("ShadowViewer.Plugin.","")
            $require+= $key+">="+$item.GetAttribute("Version")
        }
        if($item.GetAttribute("Include") -eq "ShadowViewer.Core" )
        {
            $coreVersion = $item.GetAttribute("Version")
        }
    }
  }
}
$j=@{Id=$id;Name=$name;Description=$description;Logo=$logo;Version=$version;Author=$author;WebUri=$webUri;Lang=$lang;Require=$require;MinVersion=$coreVersion}
$j | ConvertTo-Json | Out-File $outjson
if(Test-Path -Path $csproj)
{
    Remove-Item -Path $csproj
}

if($Configuration -eq "Debug"){
    $file = $AssemblyName + "-Debug-"+$version+".zip"
}
else{
    $file = $AssemblyName + "-"+$version+".zip"
}
$ZipFile = Join-Path -Path $PackagesPath -ChildPath $file
if(Test-Path -Path $ZipFile)
{
    Remove-Item -Path $ZipFile
}

$file_list = Get-ChildItem -Path $DebugDir -File | Select-Object -ExpandProperty FullName
$directory_list = Get-ChildItem -Path $DebugDir -Directory | Select-Object -ExpandProperty FullName
$DebugDirFiles = $file_list + $directory_list
Write-Host $DebugDirFiles
Compress-Archive -Path $DebugDirFiles -DestinationPath $ZipFile
Write-Host "Create Zip:" $ZipFile