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
using System.Security.Cryptography;

namespace Homework16
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //SqlConnection con;
        //SqlDataAdapter da;
        //DataTable dt;
        DataRowView row;

        Database Db;

        public MainWindow()
        {
            InitializeComponent(); 

        }

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
                AttachDBFilename = @"C:\repos\Homework16\Homework16\Db\ClientsDb.mdf",
                UserID = "user0",
                Password = "12345"
            };

            var AccConString = new OleDbConnectionStringBuilder
            {
                Provider = "Microsoft.ACE.OLEDB.12.0",
                DataSource = @"C:\repos\Homework16\Homework16\Db\PurchasesDb.accdb"
            };

            //Db = new Database(SQLConString.ConnectionString, 
            //    AccConString.ConnectionString);
            Db = new Database();

            Db.GetData2(SQLConString.ConnectionString,
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

        private void sqlGridView_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            row = (DataRowView)sqlGridView.SelectedItem;
            row.BeginEdit();

        }

        private void sqlGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            if (row == null) return;
            row.EndEdit();
            Db.SQLDa.Update(Db.Ds, "Clients");
        }

        private void sqlGridView_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            //row.EndEdit();
            //row = (DataRowView)sqlGridView.SelectedItem;
            //Db.SQLDa.Update(Db.SQLDt);
        }

        /// <summary>
        /// Удаление клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemDeleteClientClick(object sender, RoutedEventArgs e)
        {
            row = (DataRowView)sqlGridView.SelectedItem;
            row.Row.Delete();
            Db.SQLDa.Update(Db.Ds, "Clients");
        }

        /// <summary>
        /// Добавление клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemAddClientClick(object sender, RoutedEventArgs e)
        {
            DataRow r = Db.Ds.Tables["Clients"].NewRow();
            AddClientWindow add = new AddClientWindow(r);
            add.ShowDialog();


            if (add.DialogResult.Value)
            {
                Db.Ds.Tables["Clients"].Rows.Add(r);
                Db.SQLDa.Update(Db.Ds, "Clients");
            }
        }

        private void sqlGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            row = (DataRowView)sqlGridView.SelectedItem;
            Db.AccessDa.Fill(Db.Ds, "Purchases");
        }
    }
}
