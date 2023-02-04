// THIS IS A C SHARP FUNCTION

using System;
using System.IO;
using System.Collections.Generic;

namespace WriteTextFile
{
    class Program
    {
    static void Main(string[] args)
        {  
        string text = System.IO.File.ReadAllText(@"./pddl.out");
	string[] t = text.Split("step");
	
	string[] t2 = t[1].Split("time");
	Console.WriteLine(t2[0]);

        }
        }
        }

