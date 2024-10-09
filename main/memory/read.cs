using System.IO.MemoryMappedFiles;
using System.Runtime.Versioning;

namespace main.memory;

public class SharedMemoryHelper
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
            // open the existing memory-mapped file
            using MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(Params.SharedMemoryName);
            // create a view accessor for the specified range
            using MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor(offset, length, MemoryMappedFileAccess.Read);
            // read data into the buffer
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
}
