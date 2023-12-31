﻿using System;
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

        public MainWindow()
        {
            InitializeComponent(); Preparing();

        }

        private void Preparing()
        {
            #region Init

            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "ClientsDb",
                AttachDBFilename = @"C:\Users\reuk\source\repos\Homework16\Homework16\Db\ClientsDb.mdf",
                UserID = "user0",
                Password = "123"
            };

            con = new SqlConnection(connectionStringBuilder.ConnectionString);
            dt = new DataTable();
            da = new SqlDataAdapter();

            TextBlockConState.DataContext = con;
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                con.Close();
            }



            #endregion
        }
    }
}
