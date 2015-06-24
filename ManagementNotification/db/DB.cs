using System;  // C#, pure ADO.NET, no retry logic, no Exception handling.
using X = System.Text;
using D = System.Data;
using C = System.Data.SqlClient;

namespace ConsoleApplication1_dn864744
{
    class DB
    {
        C.SqlConnection sqlConnection;
        C.SqlConnectionStringBuilder scsBuilder;

        void ConnectAndQuery()
        {
            this.scsBuilder = new C.SqlConnectionStringBuilder();
            // Change these values to your values.
            this.scsBuilder["Server"] = "tcp:myazuresqldbserver.database.windows.net,1433";
            this.scsBuilder["User ID"] = "MyLogin";
            this.scsBuilder["Password"] = "MyPassword";
            this.scsBuilder["Database"] = "MyDatabase";
            this.scsBuilder["Trusted_Connection"] = false;
            this.scsBuilder["Integrated Security"] = false;
            this.scsBuilder["Encrypt"] = true;
            this.scsBuilder["Connection Timeout"] = 30;

            this.EstablishConnection();
        } // method ConnectAndQuery

        void EstablishConnection()
        {
            using (this.sqlConnection = new C.SqlConnection(this.scsBuilder.ToString()))
            {
                sqlConnection.Open();
                this.IssueQueryCommand();
            }
        } // method EstablishConnection

        void IssueQueryCommand()
        {
            D.IDataReader dReader = null;
            D.IDbCommand dbCommand = null;
            X.StringBuilder sBuilder = new X.StringBuilder(512);

            using (dbCommand = this.sqlConnection.CreateCommand())
            {
                dbCommand.CommandText =
                    @"SELECT TOP 3 ob.name, CAST(ob.object_id as nvarchar(32)) as [object_id]
                        FROM sys.objects as ob
                        WHERE ob.type='IT'
                        ORDER BY ob.name;";

                using (dReader = dbCommand.ExecuteReader())
                {
                    while (dReader.Read())
                    {
                        sBuilder.Length = 0;
                        sBuilder.Append(dReader.GetString(0));
                        sBuilder.Append("\t");
                        sBuilder.Append(dReader.GetString(1));
                        Console.WriteLine(sBuilder.ToString());
                    }
                }
            }
        } // method IssueQueryCommand
    } // class Program
}