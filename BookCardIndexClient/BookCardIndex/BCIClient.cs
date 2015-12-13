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
using BookCardIndex.ServiceReference1;
//using System.Xml.Serialization;

namespace BookCardIndexClient
{
    class BCIClient
    {

        //enum Ser_type { BIN, SOAP, XML };

        static void Main(string[] args)
        {
            int choice_int = 0;



            #region App menu
            Console.WriteLine("1. List all books");
            Console.WriteLine("2. Add new book");
            Console.WriteLine("Enter index from menu above");
            using (BookCardIndexServiceClient lib = new BookCardIndexServiceClient("BasicHttpBinding_IBookCardIndexService"))
            {
                try
                {
                    string choice = Console.ReadLine();
                    int num_books = lib.GetBookCount();

                    if (Int32.TryParse(choice, out choice_int))
                    {
                        switch (choice_int)
                        {
                            case 1:
                                if (num_books > 0)
                                {
                                    Console.WriteLine("List all books");

                                    string[] list = lib.GetBookList();

                                    for (int i = 0; i <= num_books-1; i++)
                                        Console.WriteLine("ID: {0} NAME: {1}", i+1, list[i]);

                                    Console.WriteLine("Enter book index to view details");
                                    string selection = "";
                                    int y = 0;
                                    do
                                    {
                                        selection = Console.ReadLine();
                                    } while (!(Int32.TryParse(selection, out y) && y > 0 && y <= num_books));
                                    MyBook selected = lib.GetBookByID(y);
                                    Console.WriteLine("ID: {0}, NAME: {1}, AUTHOR1: {2}, AUTHOR2: {3}, AUTHOR3: {4}, PUBLISHED: {5}", y, selected.Name, selected.Authors[0],
                                        selected.Authors[1], selected.Authors[2], selected.Published.ToString());
                                }
                                else
                                {
                                    Console.WriteLine("No books in library");
                                }
                                break;
                            case 2:

                                Console.WriteLine("Add new book");
                                MyBook new_one = AddBook();
                                MyBook exists = lib.GetBookByName(new_one.Name);
                                bool book_present = false;
                                if (exists.Name == new_one.Name)
                                    book_present = true;

                                if (!book_present)
                                {
                                    int added = lib.StoreNewBook(new_one);
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
                catch (System.ServiceModel.FaultException ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    lib.Close();
                }
            }
            #endregion
            Console.WriteLine("Press any key to exit app");
            Console.ReadKey();

        }
        private static MyBook AddBook()
        {
            MyBook temp = new MyBook(default(string), default(string[]), default(int));

            using (BookCardIndexServiceClient lib = new BookCardIndexServiceClient("BasicHttpBinding_IBookCardIndexService"))
            {
                try
                {
                    string name = "";
                    do
                    {
                        Console.WriteLine("Please add name of the book");
                        name = Console.ReadLine();
                    } while (!lib.VerifyName(name));

                    string authors_str = "";

                    do
                    {
                        Console.WriteLine("Please add authors of the book separated with commas or pipes, up to 3");
                        authors_str = Console.ReadLine();
                    } while (!lib.VerifyAuthors(authors_str));

                    string[] authors_arr = new string[3];
                    authors_arr = authors_str.Split(',', '|');

                    string x = "";
                    do
                    {
                        Console.WriteLine("Please add year the book was published");
                        x = Console.ReadLine();
                    } while (!lib.VerifyYear(x));

                    int published = 0;
                    int y = 0;
                    if (Int32.TryParse(x, out y))
                    {
                        published = y;
                    }
                    //MyBook temp = new MyBook(name, authors, published);
                    temp = new MyBook(name, authors_arr, published);
                    return temp;
                }
                catch (System.ServiceModel.FaultException ex)
                {
                    Console.WriteLine(ex);
                }

                finally
                {
                    lib.Close();
                }
            }
            return temp;
        }
    }
}