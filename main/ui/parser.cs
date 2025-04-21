namespace main.ui;
public class BytesToGrid
{
  public static bool KeyCheck(ConsoleKeyInfo keyInfo, string name)
  {
    return keyInfo
      .KeyChar
      .ToString()
      .Equals(name, StringComparison.CurrentCultureIgnoreCase);
  }
  public static uint CellSize(string inputString)
  {
    if (string.IsNullOrEmpty(inputString))
    {
      throw new Exception($"Invalid input {inputString} (input should be a non-empty string)");
    }
    else if (uint.TryParse(inputString, out uint value))
    {
      return value switch
      {
        1 => 1,
        2 => 2,
        4 => 4,
        8 => 8,
        _ => throw new Exception($"Invalid input {inputString} (input should be 1, 2, 4, or 8)")
      };
    }
    else
    {
      throw new Exception($"Invalid input {inputString} (input should be a number)");
    }
  }

  public static uint ColumnsLength(string inputString)
  {
    if (string.IsNullOrEmpty(inputString))
    {
      throw new Exception($"Invalid input {inputString} (input should be a non-empty string)");
    }
    else if (uint.TryParse(inputString, out uint value))
    {
      return value switch
      {
        8 => 8,
        16 => 16,
        32 => 32,
        64 => 64,
        _ => throw new Exception($"Invalid input {inputString} (input should be 8, 16, 32, or 64)")
      };
    }
    else
    {
      throw new Exception($"Invalid input {inputString} (input should be a number)");
    }

  }
  public static string SharedMemoryName(string inputString)
  {
    if (string.IsNullOrEmpty(inputString))
    {
      throw new Exception($"Invalid input {inputString} (input should be a non-empty string)");
    }
    else if (inputString.Length <= 32)
    {
      return inputString;
    }
    else
    {
      throw new Exception($"Invalid input {inputString} (input should be less than or equal to 32 characters)");
    }
  }
  public static byte[] NewValue(string inputString)
  {
    if (string.IsNullOrEmpty(inputString))
    {
      throw new Exception($"Invalid input {inputString} (input should be a non-empty string)");
    }

    // 文字列の長さが奇数の場合、先頭に '0' を追加
    if (inputString.Length % 2 != 0)
    {
      inputString = "0" + inputString;
    }

    List<byte> byteList = [];

    for (int i = 0; i < inputString.Length; i += 2)
    {
      string hexByte = inputString.Substring(i, 2);
      if (byte.TryParse(hexByte, System.Globalization.NumberStyles.HexNumber, null, out byte value))
      {
        byteList.Add(value);
      }
      else
      {
        throw new Exception($"Invalid input {inputString} (contains non-hex characters)");
      }
    }

    return [.. byteList];
  }
}
