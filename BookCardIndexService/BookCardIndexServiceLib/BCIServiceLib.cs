using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BookCardIndexServiceLib
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IBookCardIndexService
    {
        [OperationContract]
        MyBook GetBookByID(int ID);

        [OperationContract]
        MyBook GetBookByName(string Name);

        [OperationContract]
        int GetBookCount();

        [OperationContract]
        List<string> GetBookList();

        [OperationContract]
        int StoreNewBook(MyBook new_one);

        [OperationContract]
        bool VerifyName(string name);

        [OperationContract]
        bool VerifyAuthors(string authors);

        [OperationContract]
        bool VerifyYear(string year);
    }

    // Используйте контракт данных, как показано на следующем примере, чтобы добавить сложные типы к сервисным операциям.
    // В проект можно добавлять XSD-файлы. После построения проекта вы можете напрямую использовать в нем определенные типы данных с пространством имен "BookCardIndexServiceLib.ContractType".


    [DataContract]
    public class MyBook
    {
        private string name = default(string);
        private string[] authors = default(string[]);
        private int published = default(int);

        public MyBook()
        {
            this.name = default(string);
            this.authors = default(string[]);
            this.published = default(int);
        }

        public MyBook(string name, string[] authors, int published)
        {
            this.name = name;
            this.authors = authors;
            this.published = published;
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public string[] Authors
        {
            get { return authors; }
            set { authors = value; }
        }

        [DataMember]
        public int Published
        {
            get { return published; }
            set { published = value; }
        }
    }


    public class BookCardIndexService : IBookCardIndexService

    {
        private string cnStr;

        // Для отображения на хосте.
        public BookCardIndexService()
        {
            cnStr = ConfigurationManager.ConnectionStrings["my_db"].ConnectionString;

            Console.WriteLine("Service is ready...");
        }

        public MyBook GetBookByID(int ID)
        {
            //throw new NotImplementedException();
            MyBook book = new MyBook();
            book.Authors = new string[3];
            book.Authors.SetValue("", 0);
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = this.cnStr;
                try
                {
                    cn.Open();

                    string strSQL = "SELECT * FROM books WHERE ID = " + ID;
                    SqlCommand myCommand = new SqlCommand(strSQL, cn);
                    SqlDataReader dr = myCommand.ExecuteReader();

                    while (dr.Read())
                    {
                       // Console.WriteLine("ID: {0}, NAME: {1}, AUTHOR1: {2}, AUTHOR2: {3}, AUTHOR3: {4}, PUBLISHED: {5}", dr[0], dr[5], dr[1], dr[2], dr[3], dr[4]);
                        book.Name = (string)dr[5];
                        for (int j = 0; j <= 2; j++)
                        {
                            if (dr[j + 1].GetType() != typeof(DBNull))
                                book.Authors.SetValue((string)dr[j + 1], j);

                        }
                            
                        book.Published = (int)dr[4];
                    }
                    dr.Close();
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
            return book;

        }

        public MyBook GetBookByName(string Name)
        {
            //throw new NotImplementedException();
            MyBook book = new MyBook();
            book.Authors = new string[3];
            book.Authors.SetValue("", 0);
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = this.cnStr;
                try
                {
                    cn.Open();
                    string strSQL = string.Format("SELECT * FROM books WHERE NAME = '{0}'", Name);
                    SqlCommand myCommand = new SqlCommand(strSQL, cn);
                    SqlDataReader dr = myCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        // Console.WriteLine("ID: {0}, NAME: {1}, AUTHOR1: {2}, AUTHOR2: {3}, AUTHOR3: {4}, PUBLISHED: {5}", dr[0], dr[5], dr[1], dr[2], dr[3], dr[4]);
                        book.Name = (string)dr[5];
                        for (int j = 0; j <= 2; j++)
                        {
                            if (dr[j + 1].GetType() != typeof(DBNull))
                                book.Authors.SetValue((string)dr[j + 1], j);
                        }
                            
                        book.Published = (int)dr[4];
                    }
                    dr.Close();
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
            return book;
        }

        public int GetBookCount()
        {
            //throw new NotImplementedException();
            int num_books = -1;
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = this.cnStr;
                try
                {
                    cn.Open();
                    string strSQL = "SELECT COUNT(ID) FROM books";
                    SqlCommand myCommand = new SqlCommand(strSQL, cn);
                    num_books = (int)myCommand.ExecuteScalar();
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
            return num_books;
        }

        public List<string> GetBookList()
        {
            //throw new NotImplementedException();
            List<string> list_books = new List<string>();
            using (SqlConnection cn = new SqlConnection())
            {

                cn.ConnectionString = this.cnStr;
                try
                {
                    cn.Open();
                    string strSQL = "SELECT * FROM books";
                    SqlCommand myCommand = new SqlCommand(strSQL, cn);
                    //num_books = (int)myCommand1.ExecuteScalar

                    SqlDataReader dr = myCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        //Console.WriteLine("ID: {0} NAME: {1}", dr[0], dr[5]);
                        list_books.Add(dr[5].ToString());
                    }
                    dr.Close();
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
            return list_books;
        }

        public int StoreNewBook(MyBook new_one)
        {
            // new NotImplementedException();
            int num_books = -1;
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = this.cnStr;
                try
                {
                    cn.Open();

                    string strSQL = string.Format("SELECT * FROM books WHERE NAME = '{0}'", new_one.Name);
                    SqlCommand myCommand = new SqlCommand(strSQL, cn);
                    SqlDataReader dr = myCommand.ExecuteReader();
                    bool book_present = dr.HasRows;
                    dr.Close();

                    if (!book_present)//(presentBooks.Contains(localPath + new_one.name) != true)
                    {
                        string fields = "NAME, PUBLISHED";
                        string values = "'{0}', {1}";

                        for (int i = 1; i <= new_one.Authors.Count(); i++)
                        {
                            fields += string.Format(", AUTHOR{0}", i);
                            values += string.Format(", '{0}'", new_one.Authors[i - 1]);
                        }
                        /*
                        if (new_one.authors.Count() >= 2)
                        {

                        }*/

                        string strSQL1 = string.Format("INSERT INTO books (" + fields + ") VALUES (" + values + ")",
                            new_one.Name,
                            new_one.Published);


                        SqlCommand myCommand1 = new SqlCommand(strSQL1, cn);
                        num_books = myCommand1.ExecuteNonQuery();
                        //Console.WriteLine("New book has been added");
                    }
                    else
                    {
                        //Console.WriteLine("This book is already present");
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
            return num_books;

        }
		
		public bool VerifyName(string name)
        {
            if (name != "" && name != " " && name.Trim() != "")
                return true;

            return false;
        }

        public bool VerifyAuthors(string authors)
        {
            if (authors != "" && authors != " " && authors.Trim() != "")
                return true;

            return false;
        }
		
        public bool VerifyYear(string year)
        {
            if (year != "" && year != " " && year.Trim() != "")
                return true;

            return false;
        }
    }
}
