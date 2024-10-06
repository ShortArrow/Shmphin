using System.IO.MemoryMappedFiles;
using System.Runtime.Versioning;

namespace main.memory;

public class SharedMemoryHelper
{
    /// <summary>
    /// 指定した共有メモリの名称と範囲からデータを読み出します。
    /// </summary>
    /// <param name="memoryName">共有メモリの名称</param>
    /// <param name="offset">読み出し開始位置（バイト単位）</param>
    /// <param name="length">読み出すデータの長さ（バイト単位）</param>
    /// <returns>読み出したデータを含むバイト配列</returns>
    [SupportedOSPlatform("windows")]
    public static byte[] ReadFromSharedMemory(string memoryName, long offset, int length)
    {
        byte[] buffer = new byte[length];

        try
        {
            // 既存のメモリマップトファイルを開く
            using MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(memoryName);
            // 指定した範囲のビューを作成
            using MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor(offset, length, MemoryMappedFileAccess.Read);
            // バッファにデータを読み込む
            accessor.ReadArray(0, buffer, 0, length);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("指定された共有メモリが見つかりません。");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("共有メモリへのアクセスが拒否されました。");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"エラーが発生しました: {ex.Message}");
        }

        return buffer;
    }
}
