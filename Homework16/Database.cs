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
    public class Database : INotifyPropertyChanged
    {

        private SqlDataAdapter SQLDa;
        private OleDbDataAdapter AccessDa;
        private string sqlConState;
        private string accessConState;

        public SqlConnection SQLCon { get; private set; }
        public OleDbConnection AccessCon { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public DataSet Ds { get; private set; }

        private DataTable SQLDt;
        private DataTable AccessDt;

        public string SQLConState
        {
            get => this.sqlConState;
            set 
            {
                if (sqlConState!=value)
                {
                    sqlConState = value;
                    OnPropertyChanged(nameof(SQLConState));
                }

            }

        }

        public string AccessConState
        {
            get => this.accessConState;
            set
            {
                if (accessConState != value)
                {
                    accessConState = value;
                    OnPropertyChanged(nameof(AccessConState));
                }

            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public Database(string SQLConString, string AccessConString)
        {
            sqlConState = "Unknown";
            accessConState = "Unknown";

            SQLCon = new SqlConnection(SQLConString);
            SQLDt = new DataTable("Clients");
            string sql = @"SELECT * FROM Clients";
            
            if (SQLCon.State!= ConnectionState.Closed) 
                SQLCon.Close();
            

            SQLCon.Open();
            SQLDa = new SqlDataAdapter(sql, SQLCon);
            SQLDa.Fill(SQLDt);
            //SQLCon.Close();

            AccessCon = new OleDbConnection(AccessConString);
            AccessDt = new DataTable("Purchases");
            sql = "SELECT * FROM Purchases";

            if (AccessCon.State != ConnectionState.Closed)
                AccessCon.Close();
            AccessCon.Open();
            AccessDa = new OleDbDataAdapter(sql, AccessCon);
            AccessDa.Fill(AccessDt);
            //AccessCon.Close();

            Ds = new DataSet();
            Ds.Tables.Add(SQLDt);
            Ds.Tables.Add(AccessDt);

            sqlConState = SQLCon.State.ToString();
            accessConState = AccessCon.State.ToString();
            SQLCon.StateChange += SQLCon_StateChange;
            AccessCon.StateChange += AccessCon_StateChange;
        }
        public Database()
        {
            sqlConState = "Unknown";
            accessConState = "Unknown";
        }


        public async Task Connect(string SQLConString, string AccessConString) 
        {
            sqlConState = "Unknown";
            accessConState = "Unknown";

            SQLCon = new SqlConnection(SQLConString);
            SQLDt = new DataTable("Clients");
            string sql = @"SELECT * FROM Clients";

            //if (SQLCon.State != ConnectionState.Closed)
            //    await SQLCon.CloseAsync();


            await SQLCon.OpenAsync();
            SQLDa = new SqlDataAdapter(sql, SQLCon);
            SQLDa.Fill(SQLDt);
            await SQLCon.CloseAsync();

            AccessCon = new OleDbConnection(AccessConString);
            AccessDt = new DataTable("Purchases");
            sql = "SELECT * FROM Purchases";

            //if (AccessCon.State != ConnectionState.Closed)
            //    await AccessCon.CloseAsync();
            await AccessCon.OpenAsync();
            AccessDa = new OleDbDataAdapter(sql, AccessCon);
            AccessDa.Fill(AccessDt);
            await AccessCon.CloseAsync();

            Ds = new DataSet();
            Ds.Tables.Add(SQLDt);
            Ds.Tables.Add(AccessDt);

            sqlConState = SQLCon.State.ToString();
            accessConState = AccessCon.State.ToString();
            SQLCon.StateChange += SQLCon_StateChange;
            AccessCon.StateChange += AccessCon_StateChange;

        }

        


        private void AccessCon_StateChange(object sender, StateChangeEventArgs e)
        {
            this.AccessConState = e.CurrentState.ToString();
        }

        private void SQLCon_StateChange(object sender, StateChangeEventArgs e)
        {
            this.SQLConState = e.CurrentState.ToString();
        }
    }
}
