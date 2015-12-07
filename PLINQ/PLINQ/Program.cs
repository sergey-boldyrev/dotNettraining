using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace PLINQ
{
    class Program
    {
        public static MyList numbers = new MyList();
        public static MyList numbers1 = new MyList();

        static void Main(string[] args)
        {
            #region list creation

            const int limit_max = 1000000;
            const int threads_max = 10;

            int limit = limit_max;
            string selection;
            Random rnd = new Random();
            Stopwatch stop1 = new Stopwatch();
            Stopwatch stop2 = new Stopwatch();
            Stopwatch stop3 = new Stopwatch();
            Stopwatch stop = new Stopwatch();

            Console.WriteLine("Программа формирует список сумм простых чисел (из натурального ряда до {0}) и случайных значений \nи ищет в получившемся списке простые числа", limit_max);

            String uri_path = @AppDomain.CurrentDomain.BaseDirectory; //System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string localPath = new Uri(uri_path).LocalPath;
            string[] presentLists = Directory.GetFiles(localPath, "*.*").Where(s => s.EndsWith(".dat")).ToArray();

            if (presentLists.Length > 0)
            {
                numbers = DeserializeBinaryFormat(presentLists[0]);
                Console.WriteLine("Список из {0} элементов успешно загружен. Для повторного формирования списка удалите файл default.dat из директории программы при следующем запуске", numbers.values.Count());
            }
            else
            {
                do
                {
                    Console.WriteLine("Введите предел для формирования списка простых чисел от 2 до {0}", limit_max);
                    selection = Console.ReadLine();
                } while (!(Int32.TryParse(selection, out limit) && limit > 2 && limit <= limit_max));

                

                //генерируем список простых чисел от 2 до limit
                
                stop1.Start();
                for (int n = 2; n <= limit; n++)
                {
                    if (isPrime(n))
                    {
                        numbers.values.Add(n + rnd.Next(1, 11));

                    }
                    Console.Write("\rСписок готов на {0}%   ", n * 100 / limit);
                }
                stop1.Stop();
                SerializeBinaryFormat(numbers, "default.dat");
                Console.WriteLine("\nПростых чисел в диапазоне от 1 до {0}: {1}", limit.ToString(), numbers.values.Count());
                
                TimeSpan time1 = stop1.Elapsed;
                string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    time1.Hours, time1.Minutes, time1.Seconds,
                    time1.Milliseconds / 10);
                Console.WriteLine("Время подсчета в 1 поток: {0}", elapsedTime1);
            }

            Console.ReadKey();

            IEnumerable<int> nums = ParallelEnumerable.Range(2, limit);

            //Stopwatch stop1 = new Stopwatch();
            stop2.Start();

            nums.AsParallel().ForAll(c =>
            {
                if (isPrime(c))
                    {
                        numbers1.values.Add(c + rnd.Next(1, 11));

                    }
                    //Console.Write("\rСписок готов на {0}%   ", c * 100 / limit);
            });
            stop2.Stop();
            SerializeBinaryFormat(numbers1, "default1.dat");
            Console.WriteLine("\nПростых чисел в диапазоне от 1 до {0}: {1}", limit.ToString(), numbers1.values.Count());
            TimeSpan time2 = stop2.Elapsed;
            string elapsedTime2 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                time2.Hours, time2.Minutes, time2.Seconds,
                time2.Milliseconds / 10);
                Console.WriteLine("Время подсчета c PLINQ: {0}", elapsedTime2);

            #endregion

            #region threads

            int threads;
            do
            {
                Console.WriteLine("Введите количество потоков для обработки списка от 1 до {0}", threads_max);
                selection = Console.ReadLine();
            } while (!(Int32.TryParse(selection, out threads) && threads > 0 && threads <= threads_max));


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
                    Console.WriteLine("Введите конец диапазона для потока {0} в пределах от {1} до {2}", j, rangestart + 1, numbers.values.Count());
                    selection = Console.ReadLine();
                    //} while (rangeend <= rangestart || rangeend > numbers.Count() - 1);

                } while (!(Int32.TryParse(selection, out rangeend) && rangeend - 1 > rangestart && rangeend - 1 <= numbers.values.Count() - 1));
                rangeend -= 1;

                //ThreadWithState tws = new ThreadWithState(rangestart, rangeend, j);
                Thread t = new Thread(new ThreadWithState(rangestart, rangeend, j).CheckDivision);
                //t.Start();
                listthreads.Add(t);

                rangestart = rangeend + 1;
                //rangeend = (rangeend + 1) * 2 - 1;

            }

            //Stopwatch stop = new Stopwatch();
            stop.Start();

            foreach (Thread item in listthreads)
            {
                item.Start();
            }

            foreach (Thread item in listthreads)
            {
                item.Join();
            }


            stop.Stop();
            TimeSpan time = stop.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                time.Hours, time.Minutes, time.Seconds,
                time.Milliseconds / 10);
            Console.WriteLine("Время обработки и вывода на экран (кол-во потоков = {0}): {1}", threads, elapsedTime);

            #endregion

            Console.ReadKey();


            stop3.Start();
            numbers.values.AsParallel().ForAll(c =>
            {
                //Console.WriteLine(c);
                if (isPrime(c))
                {
                    Console.WriteLine("Значение: " + c.ToString());// + ", Номер: " + item + ", Поток: " + id);
                    //j++;
                }
                /*
                if (c.Length == 5)
                    System.Threading.Interlocked.Increment(ref count);
                */
            });

            stop3.Stop();
            TimeSpan time3 = stop3.Elapsed;

            string elapsedTime3 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                time3.Hours, time3.Minutes, time3.Seconds,
                time3.Milliseconds / 10);
            Console.WriteLine("Время обработки и вывода на экран c помощью PLINQ: " + elapsedTime3);

            Console.WriteLine("Нажмите любую клавишу для выхода из программы");
            Console.ReadKey();

        }


        public static bool isPrime(int n)
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


        private static void SerializeBinaryFormat(Object objectGraph, string fileName)
        {
            using (Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                BinaryFormatter binformatter = new BinaryFormatter();
                binformatter.Serialize(fStream, objectGraph);
                Console.WriteLine("=> Список сохранен в двоичном формате");
            }

        }

        private static MyList DeserializeBinaryFormat(string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream fStream = File.OpenRead(fileName))
            {
                return (MyList)formatter.Deserialize(fStream);
            }
        }
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

            int j = 0;//счетчик срабатываний
            int n = 0;//
            int item = n;


            for (item = start; item <= end; item++)
            {
                //if (Program.div2(Program.numbers.values[item]))
                if (Program.isPrime(Program.numbers.values[item]))
                {
                    Console.WriteLine("Значение: " + Program.numbers.values[item].ToString() + ", Номер: " + item + ", Поток: " + id);
                    j++;
                }
            }

        }
    }

    [Serializable]

    public class MyList
    {
        public List<int> values;

        public MyList()
        {
            this.values = new List<int>();
        }
    }



}
