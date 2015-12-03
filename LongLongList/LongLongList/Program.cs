﻿using System;
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
        public static List<int> numbers = new List<int>();
        public static object locker = new object();

        
        static void Main(string[] args)
        {
            //bool prost = true;
            //int k = 0;
            const int limit_max = 10000000;
            const int threads_max = 100;

            int limit = 0;
            string selection;

            Console.WriteLine("Программа формирует список сумм простых чисел (из натурального ряда до {0}) и случайных значений \nи ищет в получившемся списке четные числа", limit_max);

            do
            {
                Console.WriteLine("Введите предел для формирования списка простых чисел от 2 до {0}", limit_max);
                selection = Console.ReadLine();
                //} while (limit < 2 || limit > 10000000);

            } while (!(Int32.TryParse(selection, out limit) && limit > 2 && limit <= limit_max));

                Random rnd = new Random();

            //Thread t = new Thread(CheckDivision);
            //t.Start("test");


            //генерируем список простых чисел от 2 до limit
            for (int n = 2; n <= limit; n++)
            {
                if (isPrime(n))
                {
                    //Console.WriteLine("Число " + n.ToString() + " простое");
                    Program.numbers.Add(n + rnd.Next(1, 11));
                    //k++;
                    //Console.WriteLine("Список готов на " + (n * 100 / limit).ToString() + "%");
                    Console.Write("\rСписок готов на {0}%   ", n * 100 / limit);
                }
                /*else
                {
                    Console.WriteLine("Число " + n.ToString() + " не простое");
                }*/
            }

            Console.WriteLine("\nПростых чисел в диапазоне от 1 до {0}: {1}", limit.ToString(), numbers.Count());
            int threads;
            do
            {
                Console.WriteLine("Введите количество потоков для обработки списка от 1 до {0}", threads_max);
                selection = Console.ReadLine();
            } while (!(Int32.TryParse(selection, out threads) && threads > 0 && threads <= threads_max)) ;

            
            //int threads = int.Parse(Console.ReadLine());
            int j = 1;
            int rangestart = 0;
            int rangeend = 0;

            //List<ThreadWithState> liststates = new List<ThreadWithState>();
            List<Thread> listthreads = new List<Thread>();

            for (j = 1; j <= threads; j++)
            {
                do
                {
                    Console.WriteLine("Введите конец диапазона для потока {0} в пределах от {1} до {2}", j, rangestart + 1, numbers.Count());
                    selection = Console.ReadLine();
                    //} while (rangeend <= rangestart || rangeend > numbers.Count() - 1);

                } while (!(Int32.TryParse(selection, out rangeend) && rangeend - 1> rangestart && rangeend -1 <= numbers.Count() - 1)) ;
                rangeend -= 1;

                    //ThreadWithState tws = new ThreadWithState(rangestart, rangeend, j);
                Thread t = new Thread(new ThreadWithState(rangestart, rangeend, j).CheckDivision);
                //t.Start();
                listthreads.Add(t);

                rangestart = rangeend + 1;
                //rangeend = (rangeend + 1) * 2 - 1;

            }

            Stopwatch stop = new Stopwatch();
            stop.Start();

            foreach (Thread item in listthreads)
            {
                item.Start();
            }


            /*
            ThreadWithState tws1 = new ThreadWithState(0, numbers.Count() / 2 - 1, 2);
            ThreadWithState tws2 = new ThreadWithState(numbers.Count() / 2, numbers.Count() - 1, 2);

            Thread t1 = new Thread(new ThreadStart(tws1.CheckDivision));
            t1.Start();

            Thread t2 = new Thread(new ThreadStart(tws2.CheckDivision));
            t2.Start();
            */

            foreach (Thread item in listthreads)
            {
                item.Join();
            }


            stop.Stop();
            TimeSpan time = stop.Elapsed;
            //t.Join(20000);//костыль


            //t1.Join();
            //t2.Join();

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                time.Hours, time.Minutes, time.Seconds,
                time.Milliseconds / 10);
            Console.WriteLine("Время обработки и вывода на экран: " + elapsedTime);

            Console.WriteLine("Нажмите любую клавишу для выхода из программы");
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
