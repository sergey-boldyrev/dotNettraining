using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BookCardIndex
{
    class Program
    {
        static void Main(string[] args)
        {
            #region App menu
            Console.WriteLine("1. List all books");
            Console.WriteLine("2. Add new book");
            //Console.WriteLine("3. List books");
            //Console.WriteLine("1. List books");
            string choice = Console.ReadLine();
            int x = 0;
            String uri_path = @AppDomain.CurrentDomain.BaseDirectory; //System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string localPath = new Uri(uri_path).LocalPath;
            string[] presentBooks = Directory.GetFiles(localPath, "*.dat");

            if (Int32.TryParse(choice, out x))
            {
                switch (x)
                {
                    case 1:
                        Console.WriteLine("List all books");
                        ListAllBooks(presentBooks);
                        string selection = Console.ReadLine();
                        MyBook selected = SelectBook(selection, presentBooks);
                        if (selected.name != "")
                        {
                            DisplayBook(selected);
                        }
                        else
                        {
                            Console.WriteLine("Wrong argument");
                        }
                        break;
                    case 2:
                        Console.WriteLine("Add new book");
                        MyBook new_one = AddBook();
                        DisplayBook(new_one);

                        if (presentBooks.Contains(localPath + new_one.name + ".dat") != true)
                        {
                            SerializeBinaryFormat(new_one, new_one.name + ".dat");
                        }
                        else
                        {
                            Console.WriteLine("This book is already present");
                        }
                        break;
                    default:
                        Console.WriteLine("Wrong argument");
                        break;
                }
            }
            else 
            {
                Console.WriteLine("Wrong argument");
            }

            #endregion
            Console.ReadLine();

        }
        private static MyBook AddBook()
        { 
            string name = "";
            do
            {
                Console.WriteLine("Please add name of the book");
                name = Console.ReadLine();
            } while (!MyBook.VerifyName(name));

            string authors_str = "";

            do
            {
                Console.WriteLine("Please add authors of the book separated with commas or pipes");
                authors_str = Console.ReadLine();
            } while (!MyBook.VerifyAuthors(authors_str));

            string[] authors_arr = authors_str.Split(',' , '|');
            List<string> authors = new List<string>();
            foreach (string item in authors_arr)
            {
                authors.Add(item);
            }

            string x = "";
            do
            {
                Console.WriteLine("Please add year the book was published");
                x = Console.ReadLine();
            } while (!MyBook.VerifyYear(x));

            int published = 0;
            int y = 0;
            if (Int32.TryParse(x, out y))
            {
                published = y;
            }
            MyBook temp = new MyBook(name, authors, published);
            return temp;
        }
        
        private static void DisplayBook(MyBook book)
        {
            Console.WriteLine(book.name + ", " + book.published.ToString() + ".");
            Console.WriteLine(string.Join<string>(", ", book.authors));
        }

        private static MyBook SelectBook(string id, string[] Books_arr)
        {
            MyBook tmp_book = default(MyBook);
                   
          int j = 0;
          if (Int32.TryParse(id, out j) && j >= 0 && j <= Books_arr.Length)
          {
              tmp_book = DeserializeBinaryFormat(Books_arr[j - 1]);
          }
          return tmp_book;
        }


        private static void ListAllBooks(string[] Books_arr)
        {
            int i = 1;
            foreach (string item in Books_arr)
            {
                MyBook tmp_book = DeserializeBinaryFormat(item);
                Console.WriteLine( i + " " + tmp_book.name + ".");
                i++;
            }        
        }

        private static void SerializeBinaryFormat(Object objectGraph, string fileName)
        {
            using (Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fStream, objectGraph);
            }
        }
        private static MyBook DeserializeBinaryFormat(string fileName)
        {
            // Задание форматирования при сериализации
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream fStream = File.OpenRead(fileName))
            {
                // Заставляем модуль форматирования десериализовать 
                // объекты из потока ввода-вывода
                return (MyBook)formatter.Deserialize(fStream);
            }
        }
    }

    [Serializable]

    class MyBook 
    {
        public string name;
        public List<string> authors;
        public int published;

        public MyBook(string name, List<string> authors, int published)
        {
            // TODO: Complete member initialization
            this.name = name;
            this.authors = authors;
            this.published = published;
        }
        
        public static bool VerifyName(string name)
        {
            //
            if(name != "")
                return true;

            return false;
        }

        public static bool VerifyAuthors(string authors)
        {
            //
            if (authors != "")
                return true;

            return false;
        }
        public static bool VerifyYear(string year)
        {
            //
            if (year != "")
                return true;

            return false;
        }
    }
}
