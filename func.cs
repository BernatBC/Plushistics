// THIS IS A C SHARP FUNCTION

Queue<Action> parse_pddl ()
{
	string text = System.IO.File.ReadAllText(@"./pddl.out");
	string[] t = text.Split("[0-9]*:");
	Console.WriteLine(t[1]);
	Console.WriteLine(t[2]);
	Console.WriteLine(t[3]);
		 
}
