using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Runtime.Versioning;

using main.config;

namespace main.memory;

public class SharedMemory(IConfig config)
{
  private MemoryMappedFile? mmf = NewMemoryMappedFile(config);
  public void UpdateSharedMemory()
  {
    mmf?.Dispose();
    mmf = NewMemoryMappedFile(config);
  }
  public static MemoryMappedFile NewMemoryMappedFile(IConfig config)
  {
    if (OperatingSystem.IsWindows()) // check windows
    {
      return MemoryMappedFile.CreateOrOpen(config.SharedMemoryName ?? "NONAME", config.SharedMemorySize ?? 0);
    }
    if (OperatingSystem.IsLinux()) // check linux
    {
      var filePath = $"/dev/shm/{config.SharedMemoryName ?? "NONAME"}";
      return MemoryMappedFile.CreateFromFile(filePath, FileMode.OpenOrCreate, "", config.SharedMemorySize ?? 0);
    }
    else
    {
      throw new PlatformNotSupportedException("The current platform is not supported.");
    }
  }
  /// <summary>
  /// Read data from the specified shared memory by name and range.
  /// </summary>
  /// <param name="offset">Read start position (in bytes)</param>
  /// <param name="length">Length of data to read (in bytes)</param>
  /// <returns>Byte array containing the read data</returns>
  public byte[] ReadFromSharedMemory(long offset, int length)
  {
    byte[] buffer = new byte[length];
    try
    {
      UpdateSharedMemory();
      if (mmf == null) throw new NullReferenceException("MemoryMappedFile is null.");
      // arguments (position in shared memory, length, access type)
      MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor(offset, length, MemoryMappedFileAccess.Read);
      // arguments (position in accessor, buffer, position in buffer, length)
      accessor.ReadArray(0, buffer, 0, length);
      accessor.Dispose();
    }
    catch (FileNotFoundException)
    {
      Console.WriteLine("Not found the specified shared memory.");
      throw;
    }
    catch (UnauthorizedAccessException)
    {
      Console.WriteLine("Permission denied to access the shared memory.");
      throw;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error occurred: {ex.Message}");
      throw;
    }
    return buffer;
  }
  /// <summary>
  /// Write data from the specified shared memory by name and range.
  /// </summary>
  /// <param name="offset">Read start position (in bytes)</param>
  /// <param name="length">Length of data to read (in bytes)</param>
  /// <param name="data">Data to write</param>
  /// <return>True if the write operation is successful, false otherwise</return>
  public bool WriteToSharedMemory(ulong offset, byte[] data)
  {
    try
    {
      UpdateSharedMemory();
      if (mmf == null) throw new NullReferenceException("MemoryMappedFile is null.");
      // arguments (position in shared memory, length, access type)
      MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor((long)offset, data.Length, MemoryMappedFileAccess.Write);
      // arguments (position in accessor, buffer, position in buffer, length)
      accessor.WriteArray(0, data, 0, data.Length);
      accessor.Dispose();
    }
    catch (FileNotFoundException)
    {
      Debug.WriteLine("Not found the specified shared memory.");
      throw new Exception("Not found the specified shared memory.");
    }
    catch (UnauthorizedAccessException)
    {
      Debug.WriteLine("Permission denied to access the shared memory.");
      throw new Exception("Permission denied to access the shared memory.");
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"Error occurred: {ex.Message}");
      throw new Exception($"Error occurred: {ex.Message}");
    }
    return true;
  }

  internal void Update(ulong index, byte[] newValue)
  {
    Debug.WriteLine($"Update shared memory: index: {index}, newValue: {newValue}");
    WriteToSharedMemory(index, newValue);
    Debug.WriteLine("Update shared memory: done");
  }
}
