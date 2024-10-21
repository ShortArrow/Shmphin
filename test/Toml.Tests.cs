namespace test;

public class TomlTests
{
  [Fact]
  public void ReadShmNameFromConfigFile()
  {
    var config = new main.config.CurrentConfig();
    config.UpdateConfig(
      Path.GetFullPath(
        "../../../testdata/config.toml"
      )
    );
    var actual = config.SharedMemoryName;
    var expected = "SHMSHM";
    Assert.Equal(expected, actual);
  }
}
