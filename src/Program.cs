using System;

namespace Pazaak
{
    class Program
    {
        static void Main(string[] args)
        {
            var numberOfRuns = 1000;
            var prog = new PazaakGame();
            for (int i=0; i<numberOfRuns;i++)
            {
                prog.Run();
            }
            Console.WriteLine(prog.GetFinalGameWinState());
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
