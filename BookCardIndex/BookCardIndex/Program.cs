﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BookCardIndex
{
    class Program
    {

        enum Ser_type { BIN, SOAP, XML };
        
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
            string[] presentBooks = Directory.GetFiles(localPath, "*.*").Where(s => s.EndsWith(".dat") || s.EndsWith(".xml") || s.EndsWith(".soap")).ToArray();

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

                        if (presentBooks.Contains(localPath + new_one.name) != true)
                        {
                            Console.WriteLine("Please enter type of serialization");
                            foreach (Ser_type iType in Enum.GetValues(typeof(Ser_type)))
                            {
                                Console.WriteLine(iType.ToString());
                            }
                            string type = "";
                            Ser_type test;
                            do
                            {
                                type = Console.ReadLine();
                            } while (!Enum.TryParse<Ser_type>(type, true, out test));


                            switch (test)
                            {
                                case Ser_type.BIN:
                                    Serialize(new_one, new_one.name + ".dat", Ser_type.BIN);
                                    break;
                                case Ser_type.SOAP:
                                    Serialize(new_one, new_one.name + ".soap", Ser_type.SOAP);
                                    break;
                                case Ser_type.XML:
                                    Serialize(new_one, new_one.name + ".xml", Ser_type.XML);
                                    break;
                                default:
                                    Console.WriteLine("Wrong argument");
                                    break;
                            }
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
            Console.WriteLine(book.name + ", " + book.published.ToString());
            Console.WriteLine(string.Join<string>(", ", book.authors));
        }

        private static MyBook SelectBook(string id, string[] Books_arr)
        {
            MyBook tmp_book = default(MyBook);
                   
          int j = 0;
          if (Int32.TryParse(id, out j) && j >= 0 && j <= Books_arr.Length)
          {
                switch (Path.GetExtension(Books_arr[j - 1]))
                {
                    case ".dat":
                        tmp_book = Deserialize(Books_arr[j - 1], Ser_type.BIN);
                        break;
                    case ".soap":
                        tmp_book = Deserialize(Books_arr[j - 1], Ser_type.SOAP);
                        break;
                    case ".xml":
                        tmp_book = Deserialize(Books_arr[j - 1], Ser_type.XML);
                        break;
                }
          }
          return tmp_book;
        }


        private static void ListAllBooks(string[] Books_arr)
        {
            int i = 1;
            foreach (string item in Books_arr)
            {
                MyBook tmp_book = new MyBook();
                switch (Path.GetExtension(item))
                {
                    case ".dat":
                        tmp_book = Deserialize(item, Ser_type.BIN);
                        break;
                    case ".soap":
                        tmp_book = Deserialize(item, Ser_type.SOAP);
                        break;
                    case ".xml":
                        tmp_book = Deserialize(item, Ser_type.XML);
                        break;
                }
                Console.WriteLine( i + " " + tmp_book.name);
                i++;
            }        
        }

        private static void Serialize(Object objectGraph, string fileName, Ser_type type)
        {
            using (Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            { 
                switch (type)
                {
                    case Ser_type.BIN:
                        BinaryFormatter binformatter = new BinaryFormatter();
                        binformatter.Serialize(fStream, objectGraph);
                        Console.WriteLine("=> Saved book in binary format!");
                        break;
                    case Ser_type.SOAP:
                        SoapFormatter soapformatter = new SoapFormatter();
                        soapformatter.Serialize(fStream, objectGraph);
                        Console.WriteLine("=> Saved book in SOAP format!");
                        break;
                    case Ser_type.XML:
                        XmlSerializer xmlFormat = new XmlSerializer(typeof(MyBook));
                        xmlFormat.Serialize(fStream, objectGraph);
                        Console.WriteLine("=> Saved book in XML format!");
                        break;
                    default:
                        BinaryFormatter defformatter = new BinaryFormatter();
                        defformatter.Serialize(fStream, objectGraph);
                        Console.WriteLine("=> Saved book in binary format!");
                        break;
                }
            }
            
        }

        private static MyBook Deserialize(string fileName, Ser_type type)
        {
            //using (Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            using (Stream fStream = File.OpenRead(fileName))
            {
                switch (type)
                {
                    case Ser_type.BIN:
                        BinaryFormatter binformatter = new BinaryFormatter();
                        return (MyBook)binformatter.Deserialize(fStream);
                        //break;
                    case Ser_type.SOAP:
                        SoapFormatter soapformatter = new SoapFormatter();
                        return (MyBook)soapformatter.Deserialize(fStream);
                        //break;
                    case Ser_type.XML:
                        XmlSerializer xmlFormat = new XmlSerializer(typeof(MyBook));
                        return (MyBook)xmlFormat.Deserialize(fStream);
                        //break;
                    default:
                        Console.WriteLine("Wrong argument");
                        return null;
                        //break;*/
                }
            }
        }

        /*
         * 
        private static void SerializeBinaryFormat(Object objectGraph, string fileName, Ser_type type)
        {
            using (Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            { 
                BinaryFormatter binformatter = new BinaryFormatter();
                binformatter.Serialize(fStream, objectGraph);
                Console.WriteLine("=> Saved book in binary format!");
            }
            
        }
         * 
        private static MyBook DeserializeBinaryFormat(string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream fStream = File.OpenRead(fileName))
            {
                return (MyBook)formatter.Deserialize(fStream);
            }
        }*/


        /*
        private static void SerializeXMLFormat(Object objectGraph, string fileName)
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(MyBook));
            using (Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xmlFormat.Serialize(fStream, objectGraph);
                Console.WriteLine("=> Saved book in XML format!");
            }

        }
        private static MyBook DeserializeXMLFormat(string fileName)
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(MyBook));
            using (Stream fStream = new FileStream(fileName,
            FileMode.Open, FileAccess.Read, FileShare.None))
            {
                return (MyBook)xmlFormat.Deserialize(fStream);
            }
        }

        private static void SerializeSOAPFormat(Object objectGraph, string fileName)
        {
            using (Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                SoapFormatter formatter = new SoapFormatter();
                formatter.Serialize(fStream, objectGraph);
                Console.WriteLine("=> Saved book in SOAP format!");
            }
        }
        private static MyBook DeserializeSOAPFormat(string fileName)
        {
            SoapFormatter formatter = new SoapFormatter();
            using (Stream fStream = File.OpenRead(fileName))
            {
                return (MyBook)formatter.Deserialize(fStream);
            }
        }*/

    }

    [Serializable]

    public class MyBook 
    {
        public string name;
        public List<string> authors;
        public int published;

        public MyBook()
        {
            // TODO: Complete member initialization
            this.name = default(string);
            this.authors = default(List<string>);
            this.published = default(int);
        }

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
            if (name != "" && name != " " && name.Trim() != "")
                return true;

            return false;
        }

        public static bool VerifyAuthors(string authors)
        {
            //
            if (authors != "" && authors != " " && authors.Trim() != "")
                return true;

            return false;
        }
        public static bool VerifyYear(string year)
        {
            //
            if (year != "" && year != " " && year.Trim() != "")
                return true;

            return false;
        }
    }
}
