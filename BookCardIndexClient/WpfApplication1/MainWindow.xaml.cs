using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication1.ServiceReference1;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /*class Info
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public String Author1 { get; set; }
        public int Published { get; set; }
        public Info(int ID, String Name, String Author1, int Published)
        {
            this.ID = ID;
            this.Name = Name;
            this.Author1 = Author1;
            this.Published = Published;
        }
    }*/


    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //SetBindings();
        }

        private void SetBindingList(MyBook[] books)
        {
            //forms = new Info[] { new Info("Anastasia", "Orlova", 25), new Info("Petr", "Ivanov", 30) };
            //var data = from MyBook b in books select new {b.Id, b.Name, b.Authors, b.Published };
            var data = books
                .Select(g => new
                {
                    g.Id,
                    g.Name,
                    /*Author1 = g.Authors[0],
                    Author2 = g.Authors[1],
                    Author3 = g.Authors[2],
                    g.Published*/
                });

            this.BCIGrid.ItemsSource = data;
        }

        private void SetBindingDetails(MyBook book)
        {
            //forms = new Info[] { new Info("Anastasia", "Orlova", 25), new Info("Petr", "Ivanov", 30) };
            //var data = from MyBook b in books select new {b.Id, b.Name, b.Authors, b.Published };
            MyBook[] books = new MyBook[] { book };
            var data = books
                .Select(g => new
                {
                    g.Id,
                    g.Name,
                    Author1 = g.Authors[0],
                    Author2 = g.Authors[1],
                    Author3 = g.Authors[2],
                    g.Published
                })
                .Where(i => i.Id == book.Id);

            this.BCIDetailsGrid.ItemsSource = data;
        }

        private void ButtonClickHandlerStore(object sender, RoutedEventArgs e)
        {
            if ( (txtName.Text == "") || (txtAuthors.Text == "") || (txtPublished.Text == ""))
            {
                MessageBox.Show("MandatoryFields can not be empty");
                return;
            }
            using (BookCardIndexServiceClient lib = new BookCardIndexServiceClient("BasicHttpBinding_IBookCardIndexService"))
            {
                try
                {
                    string name = "";
                    string authors_str = "";
                    string published_str = "";
                    int published_int = 0;

                    int id = lib.GetBookCount() + 1;

                    do
                    {
                        name = txtName.Text;
                    } while (!lib.VerifyName(name));

                    do
                    {
                        authors_str = txtAuthors.Text;
                    } while (!lib.VerifyAuthors(authors_str));

                    string[] authors_arr = new string[3];
                    authors_arr = authors_str.Split(',', '|');

                    do
                    {
                       //Console.WriteLine("Please add year the book was published");
                        published_str = txtPublished.Text;
                    } while (!lib.VerifyYear(published_str));

                    int y = 0;
                    if (Int32.TryParse(published_str, out y))
                    {
                        published_int = y;
                    }
                    MyBook new_one = new MyBook(name, authors_arr, published_int, id);

                    MyBook exists = lib.GetBookByName(new_one.Name);
                    bool book_present = false;
                    if (exists.Name == new_one.Name)
                        book_present = true;

                    if (!book_present)
                    {
                        int added = lib.StoreNewBook(new_one);
                        MessageBox.Show("New book has been added");
                    }
                    else
                    {
                        MessageBox.Show("This book is already present");
                    }
                    txtName.Text = "";
                    txtAuthors.Text = "";
                    txtPublished.Text = "";
                }
                                
                catch (System.ServiceModel.FaultException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    lib.Close();
                }
            }
        }


        /*
        private void TabControl_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (ListTab != null && ListTab.IsSelected)
            {
                tab.SelectedIndex = 0;
                MessageBox.Show("ClickedList!");
                //tab.t
            }
                
            if (DetailsTab != null && DetailsTab.IsSelected)
            {
                tab.SelectedIndex = 1;
                MessageBox.Show("ClickedDetails!");
            }
                

            if (NewBookTab != null && NewBookTab.IsSelected)
            {
                tab.SelectedIndex = 2;
                MessageBox.Show("ClickedNew!");

            }
        }*/


        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
            if (row == null) return;
            int ClickedRow = row.GetIndex() + 1;
            //tab.SelectedIndex = 1;
            Dispatcher.BeginInvoke((Action)(() => tab.SelectedIndex = 1));
            //tab.SelectedItem = DetailsTab;
            using (BookCardIndexServiceClient lib = new BookCardIndexServiceClient("BasicHttpBinding_IBookCardIndexService"))
            {
                try
                {
                    SetBindingDetails(lib.GetBookByID(ClickedRow));
                }

                catch (System.ServiceModel.FaultException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    lib.Close();
                }
            }
        }


        private void btnGet_Click(object sender, RoutedEventArgs e)
        {
            using (BookCardIndexServiceClient lib = new BookCardIndexServiceClient("BasicHttpBinding_IBookCardIndexService"))
            {
                try
                {
                    int num_books = lib.GetBookCount();

                    if (num_books > 0)
                    {
                        //Console.WriteLine("List all books");

                        MyBook[] list = lib.GetBookList();
                        /*
                        Info[] forms = new Info[num_books];

                        for (int i = 0; i <= num_books - 1; i++)
                            //forms.SetValue(new Info(i, list[i], "Orlova", 1525), i);
                            forms.SetValue(list[i], i);
                            */
                        SetBindingList(list);
                        //Console.WriteLine("ID: {0} NAME: {1}", i + 1, list[i]);

                        //Console.WriteLine("Enter book index to view details");
                        /*
                        string selection = "";
                        int y = 0;
                        do
                        {
                            selection = Console.ReadLine();
                        } while (!(Int32.TryParse(selection, out y) && y > 0 && y <= num_books));
                        MyBook selected = lib.GetBookByID(y);
                        Console.WriteLine("ID: {0}, NAME: {1}, AUTHOR1: {2}, AUTHOR2: {3}, AUTHOR3: {4}, PUBLISHED: {5}", y, selected.Name, selected.Authors[0],
                            selected.Authors[1], selected.Authors[2], selected.Published.ToString());
                        */
                    }
                    else
                    {
                        Console.WriteLine("No books in library");
                    }
                }
                catch (System.ServiceModel.FaultException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    lib.Close();
                }
            }
        }

        /*
        private void OnNewBookTabSelected(object sender, SelectionChangedEventArgs e)
        {
            var tab = sender as TabItem;
            if (tab != null)
            {
                // this tab is selected!
                MessageBox.Show("ClickedNew!");
            }
            e.Handled = true;
        }*/




    }
}
