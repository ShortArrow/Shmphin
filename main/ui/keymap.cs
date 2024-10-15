class KeyMap
{
  static readonly Dictionary<string, string> keyMap = new(
    [
      new("u", "update display with read from memory"),
      new(":", "command mode"),
      new("q", "quit"),
      new("h", "help"),
      new("j", "move cursor down"),
      new("k", "move cursor up"),
      new("l", "move cursor right"),
      new("h", "move cursor left"),
      new("c", "change memory with input")
    ]
  );
}