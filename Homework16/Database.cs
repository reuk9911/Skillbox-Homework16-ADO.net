using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Homework16
{
    public class Database:INotifyPropertyChanged
    {

        private SqlDataAdapter SQLDa;
        private OleDbDataAdapter AccessDa;

        public SqlConnection sqlCon { get; private set; }
        public OleDbConnection accessCon { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        //public DataSet Ds { get; private set; }

        public DataTable SQLDt { get; private set; }
        public DataTable AccessDt { get; private set; }

        //public string SQLConState
        //{
        //    get 
        //    {
        //        return sqlCon.State.ToString();
        //    }
        //    set 
        //    {
        //        if (SQLConState != value)
        //        {
        //            SQLConState = value;
        //            OnPropertyChanged("SQLConState");
        //        }
        //    }
        //}
        
        //public string AccessConState 
        //{
        //    get 
        //    {
        //        OnPropertyChanged("AccessConState");
        //        return accessCon.State.ToString(); 
        //    }
        //}

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public Database(string SQLCon, string AccessCon)
        {
            sqlCon = new SqlConnection(SQLCon);
            accessCon = new OleDbConnection(AccessCon);
            FillDb(sqlCon.ConnectionString, accessCon.ConnectionString);
            
        }

        public void FillDb(string SQLCon, string AccessCon)
        {
            string sql;
            sql = "SELECT * FROM Clients";

            sqlCon.Open();
            DataTable sqlDt = new DataTable("Clients");
            SQLDa = new SqlDataAdapter(sql, sqlCon);
            SQLDa.Fill(sqlDt);
            sqlCon.Close();


            sql = "SELECT * FROM Purchases";
            accessCon.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, accessCon);
            DataTable accessDt = new DataTable("Purchases");
            AccessDa = new OleDbDataAdapter(sql, accessCon);
            AccessDa.Fill(accessDt);
            accessCon.Close();

        }


    }
}
