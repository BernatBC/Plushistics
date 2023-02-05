// THIS IS A C SHARP FUNCTION

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

     public struct Van {// THIS IS A C SHARP FUNCTION

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
   };
   
     public struct Action {
     	    public string action;
     	    public Van van;
 	    public Location city1;
 	    public Location city2;
    };
     	    
    
    
    static void Main(string[] args)
        {  
        string text = System.IO.File.ReadAllText(@"./pddl.out");
	string[] t = text.Split("step");
	
	string[] t2 = t[1].Split("time");
	
	string[] s = t2[0].Split("\n"); //cos

  Queue qt = new Queue();

	for (int i = 0; i < s.Length; ++i) {
		string[] p = s[i].Split(" "); //per num,action,parametres per separat + espais :(
      			Action act = new Action();
      int state = 0;
      int van_num = 0;
      int city_num = 0;
      int city_num2 = 0;
		for (int j = 0; j < p.Length; ++j) {
			for (int x = 0; x < p[j].Length; ++x) {
        //per char
        if (p[j][x] == ' ') continue;
        else if (state == 0 && Char.IsDigit(p[j][x])) continue;
        else if (state == 0 && p[j][x] == ':') state = 1;
        else if (state == 1 && p[j][x] == 'L') {
          state = 2;
          act.action = "LOAD";
          break;
        }
        else if (state == 1 && p[j][x] == 'M') {
          state = 2;
          act.action = "MOVE";
          break;
        }
        else if (state == 1 && p[j][x] == 'U') {
          state = 2;
          act.action = "UNLOAD";
          break;
        }
        else if (state == 2 && p[j][x] == 'V') continue;
        else if (state == 2) {
          if (Char.IsDigit(p[j][x])) van_num = van_num * 10 + (p[j][x] - '0');
          else state = 3;
        }
        else if (state == 3) {
          if (Char.IsDigit(p[j][x])) city_num = city_num * 10 + (p[j][x] - '0');
          else state = 4;
        }

        else if (state == 4) {
          if (Char.IsDigit(p[j][x])) city_num2 = city_num2 * 10 + (p[j][x] - '0');
        }

        else Console.WriteLine(p[j][x]);
			}
      //Console.WriteLine("hola");
		}
    
    act.van = hashmap["V"+van_num.toString()];
    act.city1 = hashmap["C"+city_num.toString()];
    act.city2 = hashmap["C"+city_num2.toString()];
    qt.push(act);
  }
	

    }
    }
}


            public Location start;
            public int maxLoad;
   };
   
     public struct Action {
     	    public string action;
     	    public Van van;
 	    public Location city1;
 	    public Location city2;
    };
     	    
    
    
    static void Main(string[] args)
        {  
        string text = System.IO.File.ReadAllText(@"./pddl.out");
	string[] t = text.Split("step");
	
	string[] t2 = t[1].Split("time");
	
	string[] s = t2[0].Split("\n"); //cos

  Queue qt = new Queue();

	for (int i = 0; i < s.Length; ++i) {
		string[] p = s[i].Split(" "); //per num,action,parametres per separat + espais :(
      			Action act = new Action();
      int state = 0;
      int van_num = 0;
      int city_num = 0;
      int city_num2 = 0;
		for (int j = 0; j < p.Length; ++j) {
			for (int x = 0; x < p[j].Length; ++x) {
        //per char
        if (p[j][x] == ' ') continue;
        else if (state == 0 && Char.IsDigit(p[j][x])) continue;
        else if (state == 0 && p[j][x] == ':') state = 1;
        else if (state == 1 && p[j][x] == 'L') {
          state = 2;
          act.action = "LOAD";
          break;
        }
        else if (state == 1 && p[j][x] == 'M') {
          state = 2;
          act.action = "MOVE";
          break;
        }
        else if (state == 1 && p[j][x] == 'U') {
          state = 2;
          act.action = "UNLOAD";
          break;
        }
        else if (state == 2 && p[j][x] == 'V') continue;
        else if (state == 2) {
          if (Char.IsDigit(p[j][x])) van_num = van_num * 10 + (p[j][x] - '0');
          else state = 3;
        }
        else if (state == 3) {
          if (Char.IsDigit(p[j][x])) city_num = city_num * 10 + (p[j][x] - '0');
          else state = 4;
        }

        else if (state == 4) {
          if (Char.IsDigit(p[j][x])) city_num2 = city_num2 * 10 + (p[j][x] - '0');
        }

        else Console.WriteLine(p[j][x]);
			}
      //Console.WriteLine("hola");
		}
    
    act.van = hashmap["V"+van_num.toString()];
    act.city1 = hashmap["C"+city_num.toString()];
    act.city2 = hashmap["C"+city_num2.toString()];
    qt.push(act);
  }
	

    }
    }
}

