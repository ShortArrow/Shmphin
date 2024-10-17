BeforeAll {
  . "$PSScriptRoot/Constants.ps1"
}

Describe 'Quit is working' {
  It 'simple test' {
    $expected = ""
    $res = $(RunCommand(@("test", "--name", "QQQQ"))) | Out-String
    Write-Host $res
    $res | Should -Be $expected
  }
}