// See https://aka.ms/new-console-template for more information
using TreeDb;
using System.Text.Json;
using TreeDb.Classes;
using TreeDb.Classes.MemoryDataStructures;
using TreeDb.Classes.Persistence;
using TreeDb.Classes.Services;
using System;

namespace TestDb
{
    internal class Program
    {
        private static DatabaseInitConfig config;
        private static string savePath = "C:\\DbTest\\Database";
        private static string testPath = "C:\\DbTest\\Test";

        public static void Main1(String[] args)
        {
            Console.WriteLine("Welcome, enter new database name.");

            config = DatabaseInitConfig.LoadFromFile("Database");
            config.SaveLocation = savePath;
            config.RootDirectories = new List<(string, string)> { };
            config.DatabaseName = Console.ReadLine();

            Database db = new Database(config);
            db.SaveDatabaseToDisk("Database");
            db.SaveDatabaseToDisk("C:\\DbTest\\Database.json");
            var table = db.GetView(config.SaveLocation);

            var command = Console.ReadLine();
            while (!command.Equals("exit"))
            {
                command = Console.ReadLine();
                switch (command)
                {
                    case "exit":
                        break;
                    case "print":
                        PrintTable(table, 1);
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
                        table.InsertViewRecord(nodename, true);
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
                        // Run query
                        var _query = Console.ReadLine();
                        Console.WriteLine(table[_query]);
                        break;

                    case "savedb":
                        // make attribute
                        Console.WriteLine("Saving database");
                        db.SaveDatabaseToDisk(savePath);
                        break;
                    default:
                        break;
                }
            }
        }

        public static void Main(String[] args)
        {
            Database db = new Database(new DatabaseInitConfig()
            {
                SaveLocation = savePath,
                DatabaseName = "TestDb",
                RootDirectories = new List<(string, string)> { },
                WatchLocations = new List<string> { }
            });
            CreateDataDirectory();
            CreateDatabaseTableForDirectory(db);
            RunQueries(db);
            DeleteDirectories();
        }

        private static void DeleteDirectories()
        {
            Directory.Delete(savePath);
            Directory.Delete(testPath);
        }

        private static void RunQueries(Database db)
        {
            var view1 = db.GetView("C:");
            PrintTable(view1, 0);
            Console.ReadKey();
            db.SaveDatabaseToDisk(Path.Combine(savePath, "Database.json"));
        }

        private static void CreateDatabaseTableForDirectory(Database db)
        {
            var view1 = db.CreateOrInitView(savePath, true);
            var view2 = db.CreateOrInitView(Path.Combine(testPath, "Child1"), true);
            var view3 = db.CreateOrInitView(Path.Combine(testPath, "Child2"), true);
            var view4 = db.CreateOrInitView(Path.Combine(testPath, "Child1", "GrandChild1"), true);

            view1.InsertViewRecord("File1", true);
            var file1 = view1.GetOrCreateRecord("File1", true, true);
            var file2 = view1.GetOrCreateRecord("File2", true, true);
            file1.SetAttribute("name", "string>");
            file2.SetTag("Tag1");
        }

        private static void CreateDataDirectory()
        {
            Directory.CreateDirectory(savePath);
            System.Diagnostics.Process.Start("explorer.exe", savePath);
        }

        private static void PrintTable(FullNodeView table, int tabs = 0)
        {
            string tab = new string(' ', tabs);
            Console.WriteLine(tab + table.Attributes[QueryService.NAME]);
            foreach (var attribute in table.Attributes.Keys)
            {
                Console.WriteLine($"{tab} - {attribute} = {table.Attributes[attribute]}");
            }
            foreach (var tag in table.Tags)
            {
                Console.WriteLine($"{tab} - {tag}");
            }
            foreach (var childtable in table.RecordTables)
            {
                PrintTable(childtable.Value, tabs + 4);
            }
        }
    }
}
