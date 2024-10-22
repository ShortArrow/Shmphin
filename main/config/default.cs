namespace main.config;
class Default : IConfig
{
  public string? SharedMemoryName => "SHMSHM";
  public uint? ColumnsLength => 8;
  public uint? CellLength => 1;
}
