using main.config;

namespace test;

public class TomlTests
{
  [Fact]
  public void ReadShmNameFromConfigFile()
  {
    var config = new CurrentConfig(new("SHMSHM", 8, 1));
    config.UpdateConfig(
      Path.GetFullPath(
        "../../../testdata/config.toml"
      )
    );
    config.Sync();
    var actual = config.SharedMemoryName;
    var expected = "SHMSHM";
    Assert.Equal(expected, actual);
  }
}
