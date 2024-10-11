using System.IO.MemoryMappedFiles;
using System.Runtime.Versioning;

namespace main.memory;

public class SharedMemory
{
    /// <summary>
    /// Read data from the specified shared memory by name and range.
    /// </summary>
    /// <param name="offset">Read start position (in bytes)</param>
    /// <param name="length">Length of data to read (in bytes)</param>
    /// <returns>Byte array containing the read data</returns>
    [SupportedOSPlatform("windows")]
    public static byte[] ReadFromSharedMemory(long offset, int length)
    {
        byte[] buffer = new byte[length];
        try
        {
            MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(Params.SharedMemoryName);
            // arguments (position in shared memory, length, access type)
            MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor(offset, length, MemoryMappedFileAccess.Read);
            // arguments (position in accessor, buffer, position in buffer, length)
            accessor.ReadArray(0, buffer, 0, length);
            accessor.Dispose();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Not found the specified shared memory.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Permission denied to access the shared memory.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
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
    [SupportedOSPlatform("windows")]
    public static bool WriteToSharedMemory(long offset, byte[] data)
    {
        try
        {
            MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(Params.SharedMemoryName);
            // arguments (position in shared memory, length, access type)
            MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor(offset, data.Length, MemoryMappedFileAccess.Write);
            // arguments (position in accessor, buffer, position in buffer, length)
            accessor.WriteArray(0, data, 0, data.Length);
            accessor.Dispose();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Not found the specified shared memory.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Permission denied to access the shared memory.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
        }
        return true;
    }
}
