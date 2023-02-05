using System;
using System.IO;
using System.Collections.Generic;
using System.Timers;




namespace WriteTextFile {

    class Program {

        public static int h = 1;
     
    
        static void Main(string[] args){  
        
            bool finished = false;



            while(!finished){
                //executa amb h
            //mutacio
            string strCmdText = "./ff -O -o domain.pddl -f problem.pddl -h 4 > output.txt";
            //System.Diagnostics.Process.Start(strCmdText, "").WaitForExit();
            finished = System.Diagnostics.Process.Start(strCmdText, "").WaitForExit(6000);
            ++h;
            }

        }
    }
}

