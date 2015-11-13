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

            if (Int32.TryParse(choice, out x))
            {
                switch (x)
                {
                    case 1:
                        Console.WriteLine("List all books");
                        break;
                    case 2:
                        Console.WriteLine("Add new book");
                        MyBook new_one = AddBook();
                        DisplayBook(new_one);
                        SerializeBinaryFormat(new_one, new_one.name + ".dat");
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
            Console.WriteLine("Please add name of the book");
            string name = Console.ReadLine();
            Console.WriteLine("Please add authors of the book separated with commas or pipes");
            string authors_str = Console.ReadLine();
            string[] authors_arr = authors_str.Split(',' , '|');
            List<string> authors = new List<string>();
            foreach (string item in authors_arr)
            {
                authors.Add(item);
            }
            Console.WriteLine("Please add year the book was published");
            string x = Console.ReadLine();
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
        private static void SerializeBinaryFormat(Object objectGraph, string fileName)
        {
            using (Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fStream, objectGraph);
            }
        }
        private static Object DeserializeBinaryFormat(string fileName)
        {
            // Задание форматирования при сериализации
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream fStream = File.OpenRead(fileName))
            {
                // Заставляем модуль форматирования десериализовать 
                // объекты из потока ввода-вывода
                return formatter.Deserialize(fStream);
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
    }
}
