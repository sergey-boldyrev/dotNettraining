using System;
//using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using System.Xml.Serialization;

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
            Console.WriteLine("Enter index from menu above");
            string choice = Console.ReadLine();
            int choice_int = 0;
            /*
            String uri_path = @AppDomain.CurrentDomain.BaseDirectory; //System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string localPath = new Uri(uri_path).LocalPath;
            string[] presentBooks = Directory.GetFiles(localPath, "*.*").Where(s => s.EndsWith(".dat") || s.EndsWith(".xml") || s.EndsWith(".soap")).ToArray();

             */ 
            string cnStr = ConfigurationManager.ConnectionStrings["my_db"].ConnectionString;


            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = cnStr;
                try
                {
                    cn.Open();
                    string strSQL = "Select * From books";
                    SqlCommand myCommand = new SqlCommand(strSQL, cn);
                    SqlDataReader dr = myCommand.ExecuteReader();

                    string strSQL1 = "SELECT COUNT(ID) FROM books";
                    SqlCommand myCommand1 = new SqlCommand(strSQL1, cn);

                    int num_books = (int)myCommand1.ExecuteScalar();

                    if (Int32.TryParse(choice, out choice_int))
                    {
                        switch (choice_int)
                        {
                            case 1:
                                if (dr.HasRows)
                                {
                                    Console.WriteLine("List all books");
                                    //ListAllBooks(presentBooks);
                            
                                    
                                    //SqlCommand myCommand = new SqlCommand(strSQL, cn);
                                    //SqlDataReader dr = myCommand.ExecuteReader();
                                    while (dr.Read())
                                    {
                                        Console.WriteLine("ID: {0} NAME: {1}", dr[0], dr[5]);
                                    }

                                    Console.WriteLine("Enter book index to view details");
                                    string selection = "";
                                    int y = 0;
                                    do
                                    {
                                        selection = Console.ReadLine();
                                    } while (!(Int32.TryParse(selection, out y) && y > 0 && y <= num_books));
                                    //MyBook selected = SelectBook(selection, presentBooks);

                                    string strSQL2 = "SELECT * FROM books WHERE ID="+y;
                                    SqlCommand myCommand2 = new SqlCommand(strSQL2, cn);
                                    /*
                                    if (selected.name != "")
                                    {
                                        DisplayBook(selected);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Wrong book index");
                                    }*/
                                }
                                else
                                {
                                    Console.WriteLine("No books in library");
                                }
                                break;
                            case 2:
                                Console.WriteLine("Add new book");
                                MyBook new_one = AddBook();
                                //DisplayBook(new_one);
                                string strSQL3 = string.Format("SELECT COUNT(NAME) FROM books WHERE NAME={0}", new_one.name);
                                SqlCommand myCommand3 = new SqlCommand(strSQL3, cn);
                                int book_present = (int)myCommand3.ExecuteScalar();

                                if (book_present < 1)//(presentBooks.Contains(localPath + new_one.name) != true)
                                {
                                    string strSQL4 = string.Format("INSERT INTO books (NAME,AUTHOR1,AUTHOR2,AUTHOR3,PUBLISHED) VALUES ({0},{1},{2},{3},{4})", 
                                        new_one.name, 
                                        new_one.authors[0], 
                                        new_one.authors[1], 
                                        new_one.authors[2],
                                        new_one.published);//INSERT INTO table_name (column1,column2,column3,...)
                                    SqlCommand myCommand4 = new SqlCommand(strSQL4, cn);
                                    int added = myCommand4.ExecuteNonQuery();
                                    Console.WriteLine("New book has been added");
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


            }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }

            
            finally
                {
                    cn.Close();
                }
            }

            #endregion
            Console.WriteLine("Press enter to exit app");
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

            string[] authors_arr = authors_str.Split(',', '|');
            //List<string> authors = new List<string>();
            /*foreach (string item in authors_arr)
            {
                authors.Add(item);
            }*/

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
            //MyBook temp = new MyBook(name, authors, published);
            MyBook temp = new MyBook(name, authors_arr, published);
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
                Console.WriteLine(i + " " + tmp_book.name);
                i++;
            }
            //return i;
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
    }

    [Serializable]

    public class MyBook
    {
        public string name;
        //public List<string> authors;
        public string[] authors;
        public int published;

        public MyBook()
        {
            // TODO: Complete member initialization
            this.name = default(string);
            //this.authors = default(List<string>);
            this.authors = default(string[]);
            this.published = default(int);
        }

        //public MyBook(string name, List<string> authors, int published)
        public MyBook(string name, string[] authors, int published)
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