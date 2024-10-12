using Tommy;

namespace main.config;

class Toml
{
  private static readonly string configPath = "config.toml";
  public static string ConfigPath
  {
    get => configPath;
  }
  public static void GetToml()
  {
    // Parse into a node
    using StreamReader reader = File.OpenText(configPath);
    // Parse the table
    TomlTable table = TOML.Parse(reader);

    Console.WriteLine(table["title"].ToString());  // Prints "TOML Example"

    // You can check the type of the node via a property and access the exact type via As*-property
    Console.WriteLine(table["owner"]["dob"].IsDateTime); // Prints "True"

    // You can also do both with C# 7 syntax
    if (table["owner"]["dob"] is TomlDateTime dateTime)
      Console.WriteLine(dateTime.AsDateTime.ToString()); // Some types contain additional properties related to formatting

    // You can also iterate through all nodes inside an array or a table
    foreach (TomlNode node in table["database"]["ports"])
      Console.WriteLine(node.ToString());
  }
}
