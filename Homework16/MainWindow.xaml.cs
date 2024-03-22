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

            Db = new Database();

            Db.GetData2(SQLConString.ConnectionString,
                AccConString.ConnectionString);

            TextBlockSQLConState.DataContext = Db;
            TextBlockAccessConState.DataContext = Db;

            sqlGridView.DataContext = Db.SQLDt.DefaultView;
            accessGridView.DataContext = Db.AccessDt.DefaultView;

            #endregion
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
            Db.SQLDa.Update(Db.SQLDt);
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
            Db.SQLDa.Update(Db.SQLDt);
        }

        /// <summary>
        /// Добавление клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemAddClientClick(object sender, RoutedEventArgs e)
        {
            DataRow r = Db.SQLDt.NewRow();
            AddClientWindow add = new AddClientWindow(r);
            add.ShowDialog();


            if (add.DialogResult.Value)
            {
                Db.SQLDt.Rows.Add(r);
                Db.SQLDa.Update(Db.SQLDt);
            }
        }

        private void MenuItemDeletePurchaseClick(object sender, RoutedEventArgs e)
        {
            row = (DataRowView)sqlGridView.SelectedItem;
            row.Row.Delete();
            Db.AccessDa.Update(Db.AccessDt);
        }

        /// <summary>
        /// Добавление клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemAddPurchaseClick(object sender, RoutedEventArgs e)
        {
            DataRow r = Db.AccessDt.NewRow();
            AddPurchaseWindow add = new AddPurchaseWindow(r);
            add.ShowDialog();


            if (add.DialogResult.Value)
            {
                Db.AccessDt.Rows.Add(r);
                Db.AccessDa.Update(Db.AccessDt);
            }
        }

        private void sqlGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            row = (DataRowView)sqlGridView.SelectedItem;
            if (row != null)
            {
                string sql = @"SELECT * FROM Purchases WHERE Purchases.email = @email Order By Purchases.Id";
                Db.AccessDa.SelectCommand = new OleDbCommand(sql, Db.AccessCon);
                Db.AccessDa.SelectCommand.Parameters.AddWithValue("@email", row["email"]);
                Db.AccessDt.Clear();
                Db.AccessDa.Fill(Db.AccessDt);
            }
        }
    }
}
