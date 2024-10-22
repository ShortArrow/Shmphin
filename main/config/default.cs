namespace main.config;
class Default : IConfig
{
  public string? SharedMemoryName => "SHMSHM";
  public uint? ColumnsLength => 8;
  public uint? CellLength => 1;
  public uint? SharedMemorySize => 128;
  public uint? SharedMemoryOffset => 0;
}
