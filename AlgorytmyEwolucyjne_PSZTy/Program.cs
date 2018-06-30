using System;

namespace Knapsack_Problem
{
    class Program
    {
        static void Main(string[] args)
        {
                //RandomInitializer randomInitializer = new RandomInitializer();
                var firstMatch = new FirstMatch();
                var secondMatch = new SecondMatch();

                Scheduler.GenerateInputData();
                firstMatch.StartAlgorithmLoop();
                secondMatch.StartAlgorithmLoop();
            try
            {
                Scheduler.WriteRealizationsToFile();
                Scheduler.WriteScheduleToFile();
                Scheduler.WriteParticipantsToFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
