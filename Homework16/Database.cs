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

        public SqlConnection SQLCon { get; private set; }
        public OleDbConnection AccessCon { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public DataSet Ds { get; private set; }

        private DataTable SQLDt;
        private DataTable AccessDt;

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

        public Database(string SQLConString, string AccessConString)
        {
            

            SQLCon = new SqlConnection(SQLConString);
            SQLDt = new DataTable("Clients");
            string sql = @"SELECT * FROM Clients";
            
            SQLCon.Open();

            SQLDa = new SqlDataAdapter(sql, SQLCon);
            SQLDa.Fill(SQLDt);

            
            AccessCon = new OleDbConnection(AccessConString);
            AccessDt = new DataTable("Purchases");
            sql = "SELECT * FROM Purchases";
            AccessCon.Open();

            AccessDa = new OleDbDataAdapter(sql, AccessCon);
            AccessDa.Fill(AccessDt);
            Ds = new DataSet();
            Ds.Tables.Add(SQLDt);
            Ds.Tables.Add(AccessDt);
            SQLCon.Close();
            AccessCon.Close();
        }

        //public void FillDb(string SQLCon, string AccessCon)
        //{
            
        //    //sql = "SELECT * FROM Clients";
            
        //    //sqlCon.Close();


        //    string sql = "SELECT * FROM Purchases";
        //    this.AccessCon.Open();
        //    OleDbDataAdapter adapter = new OleDbDataAdapter(sql, this.AccessCon);
        //    DataTable accessDt = new DataTable("Purchases");
        //    AccessDa = new OleDbDataAdapter(sql, this.AccessCon);
        //    AccessDa.Fill(accessDt);
        //    //accessCon.Close();

        //}
        


    }
}
