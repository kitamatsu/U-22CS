﻿using System;  // C#, pure ADO.NET, no Enterprise Library.
using X = System.Text;
using D = System.Data;
using C = System.Data.SqlClient;
using T = System.Threading;

//ほぼコピペです。
namespace ManagementNotification.db
{
    class DB
    {
        // Fields, shared among methods.
        C.SqlConnection sqlConnection;
        C.SqlConnectionStringBuilder scsBuilder;

        /// <summary>
        /// Prepares values for a connection. Then inside a loop, it calls a method
        /// that opens a connection. The called method calls yet another method
        /// that issues a query.
        /// The loop reiterates only if a transient error is encountered.
        /// </summary>
        public void ConnectAndQuery()
        {
            int connectionTimeoutSeconds = 30;  // Default of 15 seconds is too short over the Internet, sometimes.
            int maxCountTriesConnectAndQuery = 3;  // You can adjust the various retry count values.
            int secondsBetweenRetries = 4;  // Simple retry strategy.

            // [A.1] Prepare the connection string to Azure SQL Database.
            this.scsBuilder = new C.SqlConnectionStringBuilder();
            // Change these values to your values.
            this.scsBuilder["Server"] = "tcp:mbvx6h4y1y.database.windows.net,1433";
            this.scsBuilder["User ID"] = "kj4@mbvx6h4y1y";  // @yourservername suffix sometimes.
            this.scsBuilder["Password"] = "Sp8z5n49";
            this.scsBuilder["Database"] = "ManagementNotification";
            // Leave these values as they are.
            this.scsBuilder["Trusted_Connection"] = false;
            this.scsBuilder["Integrated Security"] = false;
            this.scsBuilder["Encrypt"] = true;
            this.scsBuilder["Connection Timeout"] = connectionTimeoutSeconds;

            //-------------------------------------------------------
            // Preparations are complete.

            for (int cc = 1; cc <= maxCountTriesConnectAndQuery; cc++)
            {
                try
                {
                    // [A.2] Connect, which proceeds to issue a query command.
                    this.EstablishConnection();

                    // [A.3] All has gone well, so let the program end.
                    break;
                }
                catch (C.SqlException sqlExc)
                {
                    bool isTransientError;

                    // [A.4] Check whether sqlExc.Number is on the whitelist of transients.
                    isTransientError = Custom_SqlDatabaseTransientErrorDetectionStrategy
                        .IsTransientStatic(sqlExc);

                    if (isTransientError == false)  // Is a persistent error...
                    {
                        Console.WriteLine();
                        Console.WriteLine("Persistent error suffered, SqlException.Number=={0}.  Will terminate.",
                            sqlExc.Number);
                        Console.WriteLine(sqlExc.ToString());

                        // [A.5] Either the connection attempt or the query command attempt suffered a persistent SqlException.
                        // Break the loop, let the hopeless program end.
                        break;
                    }

                    // [A.6] The SqlException identified a transient error from an attempt to issue a query command.
                    // So let this method reloop and try again. However, we recommend that the new query
                    // attempt should start at the beginning and establish a new connection.
                    Console.WriteLine();
                    Console.WriteLine("Transient error encountered.  SqlException.Number=={0}.  Program might retry by itself.", sqlExc.Number);
                    Console.WriteLine("{0} = Attempts so far. Might retry.", cc);
                    Console.WriteLine(sqlExc.Message);
                }
                catch (Exception exc)
                {
                    Console.WriteLine();
                    Console.WriteLine("Unexpected exception type caught in Main. Will terminate.");

                    // [A.7] The program must end, so re-throw the unrecognized error.
                    throw exc;
                }

                // [A.8] Throw an application exception if transient SqlExceptions caused us
                // to exceed our self-imposed maximum count of retries.
                if (cc > maxCountTriesConnectAndQuery)
                {
                    Console.WriteLine();
                    string mesg = String.Format(
                        "Transient errors suffered in too many retries ({0}). Will terminate.",
                        cc - 1);
                    Console.WriteLine(mesg);

                    // [A.9] To end the program, throw a new exception of a different type.
                    ApplicationException appExc = new ApplicationException(mesg);
                    throw appExc;
                }
                // Else, can retry.

                // A very simple retry strategy, a brief pause before looping.
                T.Thread.Sleep(1000 * secondsBetweenRetries);
            } // for cc
            return;
        } // method ConnectAndQuery


        /// <summary>
        /// Open a connection, then call a method that issues a query.
        /// </summary>
        void EstablishConnection()
        {
            try
            {
                // [B.1] The 'using' statement will .Dispose() the connection.
                // If you are working with a connection pool, you might want instead
                // to merely .Close() the connection.
                using (this.sqlConnection = new C.SqlConnection(this.scsBuilder.ToString()))
                {
                    // [B.2] Open a connection.
                    sqlConnection.Open();
                    // [B.3]
                    this.IssueQueryCommand();
                }
            }
            catch (Exception exc)
            {
                // [B.4] This re-throw means we discard the connection whenever
                // any error occurs during query command, even for a transient error.
                throw exc;  // [B.5] Let caller assess any exception, SqlException or any kind.
            }
            return;
        } // method EstablishConnection


        /// <summary>
        /// Issue a query, then write the result rows to the console.
        /// </summary>
        void IssueQueryCommand()
        {
            D.IDataReader dReader = null;
            D.IDbCommand dbCommand = null;
            X.StringBuilder sBuilder = new X.StringBuilder(512);

            try
            {
                // [C.1] Use the connection to create a query command.
                using (dbCommand = this.sqlConnection.CreateCommand())
                {
                    dbCommand.CommandText =
                        @"SELECT * from Account";

                    // [C.2] Issue the query command through the connection.
                    using (dReader = dbCommand.ExecuteReader())
                    {
                        // [C.3] Loop through all returned rows, writing the data to the console.
                        while (dReader.Read())
                        {
                            sBuilder.Length = 0;
                            sBuilder.Append(dReader.GetInt32(0));
                            sBuilder.Append("\t");
                            sBuilder.Append(dReader.GetString(1));

                            Console.WriteLine(sBuilder.ToString());
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc; // Let caller assess any exception.
            }
            return;
        } // method IssueQueryCommand
    } // class Program
}