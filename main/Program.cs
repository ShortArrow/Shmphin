namespace main;

class Program {
  static void Main(string[] args) {
    if (args.Length > 0) {
      Console.WriteLine($"引数: {string.Join(", ", args)}");
    } else {
      Console.WriteLine("引数がありません。");
    }
  }
}
