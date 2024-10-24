BeforeAll {
  . "$PSScriptRoot/Constants.ps1"
}

Describe 'Arguments are working' {
  It 'simple test' {
    $expected = "QQQQ"
    $res = $(RunCommand(@("test", "--name", "QQQQ"))) | Out-String
    Write-Host $res
    $res | Should -Match $expected
  }
}
