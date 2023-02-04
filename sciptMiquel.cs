using System;
using System.IO;
using System.Collections.Generic;

namespace WriteTextFile
{
    class Program
    {
        public struct Location {
            public string name;
            public int sharksNeeded;
            public int sharksAvailable;
        };

        public struct Path {
            public Location one;
            public Location two;
            public int fuel; 
        };

        public struct Van {
            public Location start;
            public int maxLoad;
        }

        static HashSet<Location> locations = new HashSet<Location>();
        static HashSet<Path> paths = new HashSet<Path>();
        static HashSet<Van> vans = new HashSet<Van>();

        static Location toLocation(string name, int need, int available) 
        {
            Location loc;
            loc.name = name;
            loc.sharksNeeded = need;
            loc.sharksAvailable = available;
            return loc;
        }

        static Van toVan(Location loc, int maxLoad) 
        {
            Van van;
            van.start = loc;
            van.maxLoad = maxLoad;
            return van;
        }

        static void addLocation(string name, int need, int available) 
        {
            Location loc = toLocation(name, need, available);
            locations.Add(loc);
        }

        static void addVan(Location loc, int maxLoad) {
            Van van = toVan(loc, maxLoad);
            vans.Add(van);
        }

        static void Main(string[] args)
        {            
            addLocation("Egham", 1, 0);
            addLocation("London", 1, 0);
            addLocation("Oxford", 1, 4);

            addVan(toLocation("Egham", 1, 0), 3);

            string text = "(define (problem one)\n" +
                            "\t(:domain plushistics)\n\n" + 
                            "\t(:objects\n" ;

            
            text += "\t\t";
            foreach (Location loc in locations) {text += loc.name + " ";}
            text += "- city\n";

            // Write the string to a file
            File.WriteAllText("./example.txt", text);

            Console.WriteLine("Text written to file successfully.");
        }
    }
}