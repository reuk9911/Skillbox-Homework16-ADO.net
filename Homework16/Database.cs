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

        public SqlDataAdapter SQLDa;
        public OleDbDataAdapter AccessDa;
        private string sqlConState;
        private string accessConState;

        public SqlConnection SQLCon { get; private set; }
        public OleDbConnection AccessCon { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;


        public DataTable SQLDt;
        public DataTable AccessDt;

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


        public Database()
        {
            sqlConState = "Unknown";
            accessConState = "Unknown";
        }


        //public async Task GetData(string SQLConString, string AccessConString) 
        //{
        //    sqlConState = "Unknown";
        //    accessConState = "Unknown";

        //    SQLCon = new SqlConnection(SQLConString);
        //    SQLDt = new DataTable("Clients");
        //    string sql = @"SELECT * FROM Clients";
        //    if (SQLCon.State != ConnectionState.Closed)
        //        await SQLCon.CloseAsync();

        //    #region Init

        //    var connectionStringBuilder = new SqlConnectionStringBuilder
        //    {
        //        DataSource = @"(localdb)\MSSQLLocalDB",
        //        InitialCatalog = "MSSQLLocalDemo"
        //    };

        //    SQLDa = new SqlDataAdapter(sql, SQLCon);

        //    #endregion

        //    #region select


        //    sql = @"SELECT * FROM Clients Order By Clients.Id";
        //    SQLDa.SelectCommand = new SqlCommand(sql, SQLCon);

        //    #endregion

        //    #region insert

        //    sql = @"INSERT INTO Clients (lastName,  firstName,  middleName, phone, email) 
        //                         VALUES (@lastName, @firstName, @middleName, @phone, @email); 
        //             SET @id = @@IDENTITY;";

        //    SQLDa.InsertCommand = new SqlCommand(sql, SQLCon);

        //    SQLDa.InsertCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id").Direction = ParameterDirection.Output;
        //    SQLDa.InsertCommand.Parameters.Add("@lastName", SqlDbType.NVarChar, 40, "lastName");
        //    SQLDa.InsertCommand.Parameters.Add("@firstName", SqlDbType.NVarChar, 40, "firstName");
        //    SQLDa.InsertCommand.Parameters.Add("@middleName", SqlDbType.NVarChar, 40, "middleName");
        //    SQLDa.InsertCommand.Parameters.Add("@phone", SqlDbType.VarChar, 12, "phone");
        //    SQLDa.InsertCommand.Parameters.Add("@email", SqlDbType.NVarChar, 80, "email");

        //    #endregion

        //    #region update

        //    sql = @"UPDATE Clients SET 
        //                   lastName = @lastName,
        //                   firstName = @firstName, 
        //                   middleName = @middleName,
        //                   phone = @phone,
        //                   email = @email
        //            WHERE id = @id";

        //    SQLDa.UpdateCommand = new SqlCommand(sql, SQLCon);
        //    SQLDa.UpdateCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id").SourceVersion = DataRowVersion.Original;
        //    SQLDa.UpdateCommand.Parameters.Add("@lastName", SqlDbType.NVarChar, 40, "lastName");
        //    SQLDa.UpdateCommand.Parameters.Add("@firstName", SqlDbType.NVarChar, 40, "firstName");
        //    SQLDa.UpdateCommand.Parameters.Add("@middleName", SqlDbType.NVarChar, 40, "middleName");
        //    SQLDa.UpdateCommand.Parameters.Add("@phone", SqlDbType.VarChar, 12, "phone");
        //    SQLDa.UpdateCommand.Parameters.Add("@email", SqlDbType.NVarChar, 80, "email");

        //    #endregion

        //    #region delete

        //    sql = "DELETE FROM Clients WHERE id = @id";

        //    SQLDa.DeleteCommand = new SqlCommand(sql, SQLCon);
        //    SQLDa.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");

        //    #endregion


        //    await SQLCon.OpenAsync();
            
        //    SQLDa.Fill(SQLDt);
        //    await SQLCon.CloseAsync();



        //    AccessCon = new OleDbConnection(AccessConString);
        //    AccessDt = new DataTable("Purchases");
        //    sql = "SELECT * FROM Purchases";

        //    if (AccessCon.State != ConnectionState.Closed)
        //        await AccessCon.CloseAsync();
        //    await AccessCon.OpenAsync();
        //    AccessDa = new OleDbDataAdapter(sql, AccessCon);
        //    AccessDa.Fill(AccessDt);
        //    await AccessCon.CloseAsync();

        //    Ds = new DataSet();
        //    Ds.Tables.Add(SQLDt);
        //    Ds.Tables.Add(AccessDt);


        //    //ForeignKeyConstraint foreignKey = new ForeignKeyConstraint(SQLDt.Columns["email"], AccessDt.Columns["email"])
        //    //{
        //    //    ConstraintName = "EmailPurchases",
        //    //    DeleteRule = Rule.Cascade,
        //    //    UpdateRule = Rule.Cascade
        //    //};
        //    ////добавляем внешний ключ в dataset
        //    //Ds.Tables["Clients"].Constraints.Add(foreignKey);
        //    //// применяем внешний ключ
        //    //Ds.EnforceConstraints = true;
        //    //Ds.Relations.Add("EmailPurchases", SQLDt.Columns[5], AccessDt.Columns[1]);

        //    sqlConState = SQLCon.State.ToString();
        //    accessConState = AccessCon.State.ToString();
        //    SQLCon.StateChange += SQLCon_StateChange;
        //    AccessCon.StateChange += AccessCon_StateChange;

        //}

        public void GetData2(string SQLConString, string AccessConString)
        {
            sqlConState = "Unknown";
            accessConState = "Unknown";


            #region SQLCon
            
            SQLCon = new SqlConnection(SQLConString);
            string sql = @"SELECT * FROM Clients";
            SQLDa = new SqlDataAdapter(sql, SQLCon);
            SQLDt = new DataTable(sql, "Clients");
            SQLDa.Fill(SQLDt);
            SetSQLConCommands();

            #endregion

            #region AccessCon

            AccessCon = new OleDbConnection(AccessConString);
            sql = "SELECT * FROM Purchases";
            AccessDa = new OleDbDataAdapter(sql, AccessCon);
            AccessDt = new DataTable("Purchases");
            AccessDa.Fill(AccessDt);

            #endregion

            sqlConState = SQLCon.State.ToString();
            accessConState = AccessCon.State.ToString();
            SQLCon.StateChange += SQLCon_StateChange;
            AccessCon.StateChange += AccessCon_StateChange;

        }

        private void SetSQLConCommands() 
        {

            #region select


            string sql = @"SELECT * FROM Clients Order By Clients.Id";
            SQLDa.SelectCommand = new SqlCommand(sql, SQLCon);

            #endregion

            #region insert

            sql = @"INSERT INTO Clients (lastName,  firstName,  middleName, phone, email) 
                                 VALUES (@lastName, @firstName, @middleName, @phone, @email); 
                     SET @id = @@IDENTITY;";

            SQLDa.InsertCommand = new SqlCommand(sql, SQLCon);

            SQLDa.InsertCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id").Direction = ParameterDirection.Output;
            SQLDa.InsertCommand.Parameters.Add("@lastName", SqlDbType.NVarChar, 40, "lastName");
            SQLDa.InsertCommand.Parameters.Add("@firstName", SqlDbType.NVarChar, 40, "firstName");
            SQLDa.InsertCommand.Parameters.Add("@middleName", SqlDbType.NVarChar, 40, "middleName");
            SQLDa.InsertCommand.Parameters.Add("@phone", SqlDbType.VarChar, 12, "phone");
            SQLDa.InsertCommand.Parameters.Add("@email", SqlDbType.NVarChar, 80, "email");

            #endregion

            #region update

            sql = @"UPDATE Clients SET 
                           lastName = @lastName,
                           firstName = @firstName, 
                           middleName = @middleName,
                           phone = @phone,
                           email = @email
                    WHERE id = @id";

            SQLDa.UpdateCommand = new SqlCommand(sql, SQLCon);
            SQLDa.UpdateCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id").SourceVersion = DataRowVersion.Original;
            SQLDa.UpdateCommand.Parameters.Add("@lastName", SqlDbType.NVarChar, 40, "lastName");
            SQLDa.UpdateCommand.Parameters.Add("@firstName", SqlDbType.NVarChar, 40, "firstName");
            SQLDa.UpdateCommand.Parameters.Add("@middleName", SqlDbType.NVarChar, 40, "middleName");
            SQLDa.UpdateCommand.Parameters.Add("@phone", SqlDbType.VarChar, 12, "phone");
            SQLDa.UpdateCommand.Parameters.Add("@email", SqlDbType.NVarChar, 80, "email");

            #endregion

            #region delete

            sql = "DELETE FROM Clients WHERE id = @id";

            SQLDa.DeleteCommand = new SqlCommand(sql, SQLCon);
            SQLDa.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");

            #endregion

        }

        private void SetAccessConCommands()
        {

            #region select


            string sql = @"SELECT * FROM Purchases WHERE Purchases.email = @email Order By Purchases.Id";
            AccessDa.SelectCommand = new OleDbCommand(sql, AccessCon);
            AccessDa.SelectCommand.Parameters.Add("@email", OleDbType.VarChar, 40, "reuk9911@yandex.ru");

            #endregion

            #region insert

            sql = @"INSERT INTO Purchases (email,  productCode,  productName) 
                                 VALUES (@email,  @productCode,  @productName)"; 
                     //SET @id = @@IDENTITY;";

            AccessDa.InsertCommand = new OleDbCommand(sql, AccessCon);

            //AccessDa.InsertCommand.Parameters.Add("@id", OleDbType.Integer, 4, "id").Direction = ParameterDirection.Output;
            AccessDa.InsertCommand.Parameters.Add("@email", OleDbType.VarChar, 40, "email");
            AccessDa.InsertCommand.Parameters.Add("@productCode", OleDbType.VarChar, 20, "productCode");
            AccessDa.InsertCommand.Parameters.Add("@productName", OleDbType.VarChar, 255, "productName");

            #endregion

            #region update

            sql = @"UPDATE Purchases SET 
                           email = @email,
                           productCode = @productCode, 
                           productName = @productName,
                    WHERE id = @id";

            AccessDa.UpdateCommand = new OleDbCommand(sql, AccessCon);
            AccessDa.UpdateCommand.Parameters.Add("@id", OleDbType.Integer, 4, "id").SourceVersion = DataRowVersion.Original;
            AccessDa.UpdateCommand.Parameters.Add("@email", OleDbType.VarChar, 40, "email");
            AccessDa.UpdateCommand.Parameters.Add("@productCode ", OleDbType.VarChar, 20, "productCode");
            AccessDa.UpdateCommand.Parameters.Add("@productName", OleDbType.VarChar, 255, "productName");

            #endregion

            //#region delete

            //sql = "DELETE FROM Purchases WHERE id = @id";

            //AccessDa.DeleteCommand = new SqlCommand(sql, SQLCon);
            //AccessDa.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");

            //#endregion

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
