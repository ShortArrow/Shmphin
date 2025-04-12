BeforeAll {
  . "$PSScriptRoot/Constants.ps1"
}

Describe 'Help is working' {
  It '--help flag' {
    $expected = "Shmphin `\`[command`\] `\`[options`\`]"
    $res = $(RunCommand(@("--help"))) | Out-String
    $res | Should -Match $expected
  }
  It '-h flag' {
    $expected = "Shmphin `\`[command`\] `\`[options`\`]"
    $res = $(RunCommand(@("-h"))) | Out-String
    $res | Should -Match $expected
  }
  It '-? flag' {
    $expected = "Shmphin `\`[command`\] `\`[options`\`]"
    $res = $(RunCommand(@("-?"))) | Out-String
    $res | Should -Match $expected
  }
}
