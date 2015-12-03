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
        public static List<int> numbers = new List<int>(300);
        public static object locker = new object();

        
        static void Main(string[] args)
        {
            //bool prost = true;
            int k = 0;
            
            Console.WriteLine("Введите число\n");
            //int n = int.Parse(Console.ReadLine());
            int limit = 100000;

            Stopwatch stop = new Stopwatch();
            stop.Start();

            //Thread t = new Thread(CheckDivision);
            //t.Start("test");

            for (int n = 2; n <= limit; n++)
            {
                if (isPrime(n))
                {
                    //Console.WriteLine("Число " + n.ToString() + " простое");
                    Program.numbers.Add(n + 3);
                    k++;
                    Console.WriteLine("Список готов на " + (n * 100 / limit).ToString() + "%");
                }
                /*else
                {
                    Console.WriteLine("Число " + n.ToString() + " не простое");
                }*/
            }

            ThreadWithState tws1 = new ThreadWithState(0, numbers.Count() / 2, 1);
            ThreadWithState tws2 = new ThreadWithState(numbers.Count() / 2 - 1, numbers.Count() - 1, 2);

            Thread t1 = new Thread(new ThreadStart(tws1.CheckDivision));
            t1.Start();

            Thread t2 = new Thread(new ThreadStart(tws2.CheckDivision));
            t2.Start();

            stop.Stop();
            TimeSpan time = stop.Elapsed;
            //t.Join(20000);//костыль
            t1.Join();
            t2.Join();
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


        public static bool div2(int n)
        {
            bool div2 = false;
            if (n % 2 == 0)
                {
                    div2 = true;
                }
            return div2;
        }

        /*
        static void CheckDivision(object id) 
        {
            Boolean lockTaken = false;
            object obj = (System.Object)locker;
            int j = 0;//счетчик срабатываний
            int n = 0;//
            int item = n;

            Console.WriteLine(id.ToString());

            
            try
            {

                Monitor.Enter(obj, ref lockTaken);

                //foreach (int item in numbers)
                while (n <= numbers.Count())
                {

                    for (item = n; item < numbers.Count(); item++)
                    {
                        if (div2(numbers[item]))
                        {
                            Console.WriteLine(numbers[item].ToString() + ", " + id);
                            j++;
                        }
                    }
                    n = item;
                    if (n == numbers.Count())
                        break;
                }
                Console.WriteLine("Четных чисел в списке: " + j.ToString());


            }
            finally
            {
                if (lockTaken) Monitor.Exit(obj);
            }

        }*/



    }

    public class ThreadWithState
    {
        // State information used in the task.
        private int start;
        private int end;
        private int id;

        // The constructor obtains the state information.
        public ThreadWithState(int start, int end, int id)
        {
            this.start = start;
            this.end = end;
            this.id = id;
        }


     public void CheckDivision()
        {
            //Boolean lockTaken = false;
            object obj = (System.Object)Program.locker;
            int j = 0;//счетчик срабатываний
            int n = 0;//
            int item = n;

            //Console.WriteLine(id.ToString());

            
            /*try
            {
                Monitor.Enter(obj, ref lockTaken);

                //foreach (int item in numbers)
                //while (end <= Program.numbers.Count() && start >= 0  && start < end)
                //{*/

                    for (item = start; item <= end; item++)
                    {
                        if (Program.div2(Program.numbers[item]))
                        {
                            Console.WriteLine("Значение: " + Program.numbers[item].ToString() + ", Номер: " + item + ", Поток: " + id);
                            j++;
                        }
                    }
                    //n = item;
                    //if (n == Program.numbers.Count() || n == end)
                    //    break;
                //}
                Console.WriteLine("Четных чисел в списке: " + j.ToString() + ", " + id);


            /*}
            finally
            {
                if (lockTaken) Monitor.Exit(obj);
            }*/

        }
    }
}
