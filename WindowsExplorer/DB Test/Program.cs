// See https://aka.ms/new-console-template for more information
using TreeDb;
using System.Text.Json;

namespace TestDb
{
    internal class Program
    {
        public static void Main(String[] args)
        {
            Console.WriteLine("Hello, World!");
            var command = Console.ReadLine();

            Database db = new Database();
            var table = db.RootTable("NewTable", true);
            while (!command.Equals("exit"))
            {
                command = Console.ReadLine();
                switch (command)
                {
                    case "exit":
                        break;
                    case "print":
                        Console.WriteLine(table);
                        break;
                    case "nav":
                        Console.WriteLine("Enter query");
                        var query = Console.ReadLine();
                        table = table.Query(query).First();
                        Console.WriteLine(table);
                        break;
                    case "make":
                        // Make new foldertable as child
                        Console.WriteLine("Make NodeTableName");
                        var nodename = Console.ReadLine();
                        table.Put(command, new NodeTable(nodename, table));
                        Console.WriteLine(table);
                        break;
                    case "tag":
                        // Add tag
                        Console.WriteLine("Enter Tag string");
                        var tag = Console.ReadLine();
                        table.PutTag(tag);
                        Console.WriteLine(table);
                        break;
                    case "attr":
                        // make attribute
                        Console.WriteLine("Enter Attribute name");
                        var attrName = Console.ReadLine();
                        Console.WriteLine("Enter Attribute value");
                        var attrValue = Console.ReadLine();
                        table[attrName] = attrValue;
                        Console.WriteLine(table);
                        break;
                    case "query":
                        // make attribute
                        var _query = Console.ReadLine();
                        Console.WriteLine(table[_query]);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
