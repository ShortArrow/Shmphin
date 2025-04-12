$testTarget = ".\main\bin\Debug\net8.0\Shmphin.exe"

function RunCommand {
  param (
    [array]$ArgumentList,
    [array] $InputStrings = @()
  )
  $processStartInfo = New-Object System.Diagnostics.ProcessStartInfo
  $processStartInfo.FileName = $testTarget
  $processStartInfo.Arguments = $ArgumentList
  $processStartInfo.RedirectStandardOutput = $true
  $processStartInfo.RedirectStandardInput = $true
  $processStartInfo.UseShellExecute = $false

  $process = New-Object System.Diagnostics.Process
  $process.StartInfo = $processStartInfo
  $process.Start() | Out-Null
  
  foreach($inputString in $InputStrings) {
    $process.StandardInput.WriteLine($inputString)
  }
  
  $process.StandardInput.WriteLine("q")

  $output = $process.StandardOutput.ReadToEnd()
  $process.WaitForExit()

  return $output
}