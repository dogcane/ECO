#DA FARE - Cambiare porta, rigenerare guid progetti nella solution

function Copy-File($originalFile, $destinationFile)
{
	Copy-Item $originalFile $destinationFile
}

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
$rootPath = (get-item $scriptPath).FullName
$solutionPath = $rootPath + "\ECO.sln"
$MsBuild = $env:systemroot + "\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe";
$BuildPath = $rootPath + "\Deploy"
$BuildLog = $BuildPath + "\Deploy.log"

Clear-Host;
write-host "********************************" -foreground Green
write-host "ECO DLL GENERATOR V1.0"  -foreground Green
write-host "********************************" -foreground Green
write-host "Root path:" $rootPath
write-host
write-host "Solution path:" $solutionPath
write-host
write-host "Creating output path:" $BuildPath
write-host
$buildFolder = New-Item -ItemType Directory -Force -Path $BuildPath
#$buildFolder.attributes="Hidden"

write-host
write-host "Launch Build Process...."
write-host
$BuildArgs = @{            
    FilePath = $MsBuild            
    ArgumentList = $solutionPath, "/p:Configuration=Release /p:Platform=""Any CPU""", ("/p:OutputPath=" + $BuildPath), "/v:minimal"
    RedirectStandardOutput = $BuildLog            
    Wait = $true            
    #WindowStyle = "Hidden"
}            
            
# Start the build            
Start-Process @BuildArgs #| Out-String -stream -width 1024 > $DebugBuildLogFile

Get-ChildItem -Path $BuildPath -Recurse -Exclude ECO*.dll,Deploy.Log | Remove-Item -force -recurse
Get-ChildItem -Path $BuildPath -Recurse -Include ECO.Sample.*.dll | Remove-Item -force -recurse
Get-ChildItem -Path $BuildPath -Recurse -Include ECO*Test*.dll | Remove-Item -force -recurse

write-host
write-host "********************************" -foreground Green
write-host "Deploy completed"  -foreground Green
write-host "********************************" -foreground Green