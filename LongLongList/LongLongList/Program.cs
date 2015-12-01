using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace LongLongList
{
    class Program
    {
        static List<int> numbers = new List<int>(300);
        static object locker = new object();

        
        static void Main(string[] args)
        {
            //bool prost = true;
            int k = 0;
            
            Console.WriteLine("Введите число\n");
            //int n = int.Parse(Console.ReadLine());
            int limit = 1000;

            Stopwatch stop = new Stopwatch();
            stop.Start();

            Thread t = new Thread(CheckDivision);
            t.Start();            

            for (int n = 2; n <= limit; n++)
            {
                if (isPrime(n))
                {
                    Console.WriteLine("Число " + n.ToString() + " простое");
                    Program.numbers.Add(n+3);
                    k++;
                }
                /*else
                {
                    Console.WriteLine("Число " + n.ToString() + " не простое");
                }*/
            }

            stop.Stop();
            TimeSpan time = stop.Elapsed;
            t.Join();
            Console.WriteLine("Простых чисел в диапазоне от 1 до " + limit.ToString() + ": " + k.ToString());
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                time.Hours, time.Minutes, time.Seconds,
                time.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            Console.ReadKey();

        }

        static bool isPrime(int n)
        {
            bool prime = true;
            
            for (int i = 2; i <= n / 2; i++)
            {
                if (n % i == 0)
                {
                    prime = false;
                    break;
                }
            }
            return prime;
        }


        static bool div2(int n)
        {
            bool div2 = false;
            if (n % 2 == 0)
                {
                    div2 = true;
                }
            return div2;
        }

        static void CheckDivision() 
        {
            Boolean lockTaken = false;
            object obj = (System.Object)locker;
            int j = 0;
            try
            {

                Monitor.Enter(obj, ref lockTaken);

                //foreach (int item in numbers)
                for (int item = 0; item < numbers.Count(); item++)
                {
                    if (div2(numbers[item]))
                    {
                        Console.WriteLine(numbers[item].ToString());
                        j++;
                    }
                }
                Console.WriteLine(j.ToString());


            }
            finally
            {
                if (lockTaken) Monitor.Exit(obj);
            }

        }



    }

    [Serializable]
    public class MyList
    { 
        


    
    }
}
