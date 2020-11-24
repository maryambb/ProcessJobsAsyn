using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcessJobsAsync
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Please enter the time range in second:");
            Console.Write("From: ");
            string timeString1 = Console.ReadLine();
            double time1 = StrToDouble(timeString1);

            Console.Write("To: ");
            string timeString2 = Console.ReadLine();
            double time2 = StrToDouble(timeString2);

            if (time2 <= time1)
            {
                throw new System.ArgumentException("End time cannot be greater that start time!");
            }

            List<Char> jobList = new List<char>();

            //Getting the job names from input
            while (true)
            {
                Console.Write("Enter job name: ");
                Console.WriteLine();
                ConsoleKeyInfo ck = Console.ReadKey();
                Console.WriteLine();
                Char ch = ck.KeyChar;

                if (ck.Key == ConsoleKey.Enter)
                {
                    break;
                }

                if (Char.IsLetter(ch))
                {
                    jobList.Add(ch);
                }
                else
                {
                    Console.WriteLine("Invalid data input. Please Enter only letter as job name! ");
                }
            }

            Console.WriteLine("------------------------------------");
            Console.WriteLine("------------- Result ---------------");

            MainAsync(jobList, time1, time2).Wait();


            Console.WriteLine("Enter Key...");
            Console.ReadLine();

        }

        static async Task MainAsync(List<char> jobList, double time1, double time2)
        {
            List<Task> listOfTasks = new List<Task>();
            int Seed = (int)DateTime.Now.Ticks;
            var random = new Random(Seed);

            foreach (var job in jobList)
            {
                listOfTasks.Add(DoAsync(job, time1, time2, random));
            }

            await Task.WhenAll(listOfTasks);
        }


        private static async Task DoAsync(Char job, double time1, double time2, Random randomTest)
        {
            if (Char.IsLower(job))
            {
                Console.WriteLine(" Job '{0}' is failed!", job);
            }
            else
            {
                int rndTime = GetRandomTime(time1, time2, randomTest);
                await Task.Delay((int)TimeSpan.FromSeconds(rndTime).TotalMilliseconds);
                Console.WriteLine(" Job '{0}' is completed successfully in '{1}' Seconds!", job, rndTime);
            }
        }

        private static int GetRandomTime(double time1, double time2, Random randomTest)
        {
            int randomTime = randomTest.Next((int)time1, (int)time2);

            return randomTime;
        }

        private static double StrToDouble(string inputTime)
        {
            double time;
            try
            {
                time = Convert.ToDouble(inputTime);
            }
            catch (Exception)
            {
                Console.WriteLine("OOPS! time is not in correct format!");
                throw;
            }
            return time;
        }
    }
}

