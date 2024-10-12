namespace test;

public class UnitTest1
{
  [Fact]
  public void ReadShmNameFromConfigFile()
  {
    main.config.Config.UpdateConfig(
      Path.GetFullPath(
        "../../../testdata/config.toml"
      )
    );
    var actual = main.config.Config.SharedMemoryName;
    var expected = "SHMSHM";
    Assert.Equal(expected, actual);
  }
}