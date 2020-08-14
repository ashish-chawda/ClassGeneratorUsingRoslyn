using System;

namespace ClassGeneratorUsingRoslyn
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string response = RosylnClassCreator.GenerateClass();
                Console.WriteLine(response);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error generating class");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
