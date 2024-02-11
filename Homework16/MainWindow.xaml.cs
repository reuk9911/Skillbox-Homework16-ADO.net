using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.OleDb;
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
using System.Data;

namespace Homework16
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection con;
        SqlDataAdapter da;
        DataTable dt;
        DataRowView row;

        Database Db;

        public MainWindow()
        {
            InitializeComponent(); 

        }

        //private void Preparing2()
        //{
        //    #region Init
        //    //string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\reuk\\source\\repos\\Homework16\\Homework16\\Db\\PurchasesDb.accdb";
        //    //OleDbConnection connection = new OleDbConnection(connectionString);
        //    //connection.Open();

        //    var connectionStringBuilder = new SqlConnectionStringBuilder
        //    {
        //        DataSource = @"(localdb)\MSSQLLocalDB",
        //        InitialCatalog = "ClientsDb",
        //        AttachDBFilename = @"C:\Users\reuk\source\repos\Homework16\Homework16\Db\ClientsDb.mdf",
        //        UserID = "user0",
        //        Password = "123"
        //    };

        //    dt = new DataTable();
        //    da = new SqlDataAdapter();
        //    con = new SqlConnection(connectionStringBuilder.ConnectionString);

        //    TextBlockConState.DataContext = con;
        //    try
        //    {
        //        con.Open();
        //    }
        //    catch (Exception ex)
        //    {
        //        con.Close();
        //    }



        //    #endregion
        //}
        private async Task Preparing()
        {
            #region Init



            //string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\reuk\\source\\repos\\Homework16\\Homework16\\Db\\PurchasesDb.accdb";
            //OleDbConnection connection = new OleDbConnection(connectionString);
            //connection.Open();

            var SQLConString = new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "ClientsDb",
                AttachDBFilename = @"C:\Users\reuk\source\repos\Homework16\Homework16\Db\ClientsDb.mdf",
                UserID = "user0",
                Password = "123"
            };

            var AccConString = new OleDbConnectionStringBuilder
            {
                Provider = "Microsoft.ACE.OLEDB.12.0",
                DataSource = @"C:\Users\reuk\source\repos\Homework16\Homework16\Db\PurchasesDb.accdb"
            };

            //Db = new Database(SQLConString.ConnectionString, 
            //    AccConString.ConnectionString);
            Db = new Database();

            await Db.Connect(SQLConString.ConnectionString,
                AccConString.ConnectionString);

            TextBlockSQLConState.DataContext = Db;
            TextBlockAccessConState.DataContext = Db;



            sqlGridView.DataContext = Db.Ds.Tables["Clients"].DefaultView;
            accessGridView.DataContext = Db.Ds.Tables["Purchases"].DefaultView;

            
            



            #endregion
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Db.SQLCon.State == ConnectionState.Open)
            {
                Db.SQLCon.Close();
            }
            //if (Db.SQLCon.State == ConnectionState.Closed)
            //{
            //    Db.SQLCon.Open();

            //}
            if (Db.AccessCon.State == ConnectionState.Open)
            {
                Db.AccessCon.Close();
            }
            //if (Db.AccessCon.State == ConnectionState.Closed)
            //{
            //    Db.AccessCon.Open();
            //}
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Preparing();
        }
    }
}
