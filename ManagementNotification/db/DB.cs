using System;  // C#, pure ADO.NET, no Enterprise Library.
using X = System.Text;
using D = System.Data;
using C = System.Data.SqlClient;
using T = System.Threading;
using System.Windows.Forms;
using ManagementNotification.util;
using System.Collections.Generic;
using System.Collections;


//ほぼコピペです。
namespace ManagementNotification.db
{
    
    class DB
    {
        //Confirmationクラス、AccountCertificationクラスのインスタンス
        Confirmation con;
        AccountCertification ac;
        
        // Fields, shared among methods.
        C.SqlConnection sqlConnection;
        C.SqlConnectionStringBuilder scsBuilder;

////////////////////アカウント追加メソッド/////////////////////////////////////////

        public void ConnectAndQuery(String user,String email,String pass)
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
            this.scsBuilder["Database"] = "managementnotificationdb";
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
                    this.EstablishConnection(user,email,pass);

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
                        Console.WriteLine("メールアドレスが重複しています。",
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
////////////// アカウント追加 EstablishConnection();
        /// </summary>
        void EstablishConnection(String User,String Email,String Pass)
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
                    this.addAccount(User, Email, Pass);
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


//////////////////////アカウント追加/////////////////////////////////
        void addAccount(String username, String email, String pass)
        {
            D.IDataReader dReader = null;
            C.SqlCommand com = new C.SqlCommand();

            try
            {
                // [C.1] Use the connection to create a query command.
                using (com = this.sqlConnection.CreateCommand())
                {

                    com.CommandText = @"INSERT INTO mnMobile.accountMobile (complete,userName,email,password) " +
                            "VALUES (0,@userName,@Email,@Pass)";

                    com = new C.SqlCommand(com.CommandText, sqlConnection);

                    try
                    {
                        AddSqlParameter(com, "@userName", D.SqlDbType.NVarChar, username);
                        AddSqlParameter(com, "@Email", D.SqlDbType.NVarChar, email);
                        AddSqlParameter(com, "@Pass", D.SqlDbType.NVarChar, pass);

                    }
                    catch (C.SqlException exc)
                    {
                        throw exc;
                    }

                    // [C.2] Issue the query command through the connection.
                    dReader = com.ExecuteReader();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
                throw exc; // Let caller assess any exception.
            }
            return;
        }


////////////////////Transmit追加メソッド/////////////////////////////////////////

        public void TransmitConnectAndQuery(String user, String pass)
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
            this.scsBuilder["Database"] = "managementnotificationdb";
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
                    this.TransmitEstablishConnection(user, pass);

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
        /// Transmit追加 EstablishConnection();
        /// </summary>
        void TransmitEstablishConnection(String UserName, String Pass)
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
                    this.addTransmit(UserName, Pass);
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

        ////////////////////通知管理テーブルにaccountId追加////////////////////////////
        void addTransmit(String username, String pass)
        {
            D.IDataReader dReader = null;
            C.SqlCommand com = new C.SqlCommand();
            X.StringBuilder sBuilder = new X.StringBuilder(512);
            int accountId = 0;

            try
            {
                // [C.1] Use the connection to create a query command.
                using (com = this.sqlConnection.CreateCommand())
                {

                    com.CommandText = @"SELECT accountid FROM mnMobile.accountMobile " +
                                    "WHERE userName = @userName " +
                                    "AND password = @Pass";

                    com = new C.SqlCommand(com.CommandText, sqlConnection);

                    try
                    {
                        AddSqlParameter(com, "@userName", D.SqlDbType.NChar, username);
                        AddSqlParameter(com, "@Pass", D.SqlDbType.NChar, pass);

                    }
                    catch (C.SqlException exc)
                    {
                        throw exc;
                    }

                    // [C.2] Issue the query command through the connection.
                    using (dReader = com.ExecuteReader())
                    {
                        while (dReader.Read())
                        {
                            accountId = dReader.GetInt32(0);
                        }
                    }

                    com.CommandText = @"INSERT INTO Transmit (accountId, notificationId)" +
                                    "VALUES (" + accountId + ", 0)";

                    dReader = com.ExecuteReader();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
                throw exc; // Let caller assess any exception.
            }
            return;
        }


////////////////////期限切れの通知削除(期限一カ月）/////////////////////////////////////////

        public void DeleteConnectAndQuery()
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
            this.scsBuilder["Database"] = "managementnotificationdb";
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
                    this.DeleteEstablishConnection();

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
////////////// 期限切れデータ削除 EstablishConnection();
        /// </summary>
        void DeleteEstablishConnection()
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
                    this.deleteTable();
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


//////////////////////通知削除/////////////////////////////////
        void deleteTable()
        {
            D.IDataReader dReader = null;
            C.SqlCommand com = new C.SqlCommand();
            DateTimeOffset dt = DateTimeOffset.Now;
            //1か月前の月を取得する
            String strdtNow = dt.AddMonths(-1).ToString();

            try
            {
                // [C.1] Use the connection to create a query command.
                using (com = this.sqlConnection.CreateCommand())
                {
                    
                    com.CommandText = @"DELETE FROM mnMobile.notificationMobile " +
                                    "WHERE date < '" + strdtNow + "'";

                    dReader = com.ExecuteReader();

                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
                throw exc; // Let caller assess any exception.
            }
            return;
        }


///////////////////////通知用　i = 1は未送信通知、i = 2は削除済み通知の再送信///////////////////
        void EstablishConnection(String email, int i)
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

                    switch (i)
                    {
                        case 1:
                            // [A.2] Connect, which proceeds to issue a query command.
                            this.getNotification(email);
                            break;

                        case 2:
                            this.retransmition(email);
                            break;

                        default:
                            break;
                    }
                    
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


/////////////////ログイン認証///////////////////////////////////////////////////
        public String LoginConnectAndQuery(String username,String password)
        {
            String Email = "";
            int connectionTimeoutSeconds = 30;  // Default of 15 seconds is too short over the Internet, sometimes.
            int maxCountTriesConnectAndQuery = 3;  // You can adjust the various retry count values.
            int secondsBetweenRetries = 4;  // Simple retry strategy.

            // [A.1] Prepare the connection string to Azure SQL Database.
            this.scsBuilder = new C.SqlConnectionStringBuilder();
            // Change these values to your values.
            this.scsBuilder["Server"] = "tcp:mbvx6h4y1y.database.windows.net,1433";
            this.scsBuilder["User ID"] = "kj4@mbvx6h4y1y";  // @yourservername suffix sometimes.
            this.scsBuilder["Password"] = "Sp8z5n49";
            this.scsBuilder["Database"] = "managementnotificationdb";
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
                    Email = this.LoginEstablishConnection(username, password);

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
            return Email;
        } // method ConnectAndQuery


        /// <summary>
////////////// ログイン認証 EstablishConnection()
        /// </summary>
        String LoginEstablishConnection(String User, String Pass)
        {
            //ログイン認証の結果を取得
            String email = "";

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
                    email = this.loginAccount(User, Pass);
                }
            }
            catch (Exception exc)
            {
                // [B.4] This re-throw means we discard the connection whenever
                // any error occurs during query command, even for a transient error.
                throw exc;  // [B.5] Let caller assess any exception, SqlException or any kind.
            }
            return email;
        } // method EstablishConnection


///////////////////////////ログイン認証///////////////////////////////////////////////
        String loginAccount(String username, String pass)
        {
            D.IDataReader dReader = null;
            C.SqlCommand com = new C.SqlCommand();
            con = new Confirmation();
            ac = new AccountCertification();
            String mail = "";

            try
            {
                // [C.1] Use the connection to create a query command.
                using (com = this.sqlConnection.CreateCommand())
                {

                    com.CommandText = @"SELECT email FROM mnMobile.accountMobile " +
                                    "WHERE userName = @userName " +
                                    "AND password = @Pass";

                    com = new C.SqlCommand(com.CommandText, sqlConnection);

                    try
                    {
                        AddSqlParameter(com, "@userName", D.SqlDbType.NChar, username);
                        AddSqlParameter(com, "@Pass", D.SqlDbType.NChar, pass);

                    }
                    catch (C.SqlException exc)
                    {
                        throw exc;
                    }

                    // [C.2] Issue the query command through the connection.
                    using (dReader = com.ExecuteReader())
                    {
                        while (dReader.Read())
                        {
                            mail = dReader.GetString(0);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
                throw exc; // Let caller assess any exception.
            }
            return mail;
        }


///////////////////////アカウント再発行時の入力メールアドレスがあるか確認/////////////////
        public String CheckEmailConnectAndQuery(String inputEmail)
        {
            String email = "";
            int connectionTimeoutSeconds = 30;  // Default of 15 seconds is too short over the Internet, sometimes.
            int maxCountTriesConnectAndQuery = 3;  // You can adjust the various retry count values.
            int secondsBetweenRetries = 4;  // Simple retry strategy.

            // [A.1] Prepare the connection string to Azure SQL Database.
            this.scsBuilder = new C.SqlConnectionStringBuilder();
            // Change these values to your values.
            this.scsBuilder["Server"] = "tcp:mbvx6h4y1y.database.windows.net,1433";
            this.scsBuilder["User ID"] = "kj4@mbvx6h4y1y";  // @yourservername suffix sometimes.
            this.scsBuilder["Password"] = "Sp8z5n49";
            this.scsBuilder["Database"] = "managementnotificationdb";
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
                    email = this.CheckEmailEstablishConnection(inputEmail);

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
            return email;
        } // method ConnectAndQuery


/////////////////////アカウント再発行時の入力メールアドレスがあるか確認/////////////////
        String CheckEmailEstablishConnection(String inputEmail)
        {
            String email = "";
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
                    email = this.checkEmail(inputEmail);
                }
            }
            catch (Exception exc)
            {
                // [B.4] This re-throw means we discard the connection whenever
                // any error occurs during query command, even for a transient error.
                throw exc;  // [B.5] Let caller assess any exception, SqlException or any kind.
            }
            return email;
        } // method EstablishConnection


        //アカウント再発行
        public String[] ReissueConnectAndQuery(String email)
        {
            String[] accountData = new String[2];
            int connectionTimeoutSeconds = 30;  // Default of 15 seconds is too short over the Internet, sometimes.
            int maxCountTriesConnectAndQuery = 3;  // You can adjust the various retry count values.
            int secondsBetweenRetries = 4;  // Simple retry strategy.

            // [A.1] Prepare the connection string to Azure SQL Database.
            this.scsBuilder = new C.SqlConnectionStringBuilder();
            // Change these values to your values.
            this.scsBuilder["Server"] = "tcp:mbvx6h4y1y.database.windows.net,1433";
            this.scsBuilder["User ID"] = "kj4@mbvx6h4y1y";  // @yourservername suffix sometimes.
            this.scsBuilder["Password"] = "Sp8z5n49";
            this.scsBuilder["Database"] = "managementnotificationdb";
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
                    accountData = this.ReissueEstablishConnection(email);

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
            return accountData;
        } // method ConnectAndQuery


        String[] ReissueEstablishConnection(String email)
        {
            String[] accountData = new String[2];
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
                    accountData = this.Reissue(email);
                }
            }
            catch (Exception exc)
            {
                // [B.4] This re-throw means we discard the connection whenever
                // any error occurs during query command, even for a transient error.
                throw exc;  // [B.5] Let caller assess any exception, SqlException or any kind.
            }
            return accountData;
        } // method EstablishConnection

        //ユーザ情報変更
        public void ChangeDataConnectAndQuery(String[] accountData)
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
            this.scsBuilder["Database"] = "managementnotificationdb";
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
                    this.ChangeDataEstablishConnection(accountData);

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
            
        } // method ConnectAndQuery


        void ChangeDataEstablishConnection(String[] accountData)
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
                    this.changeData(accountData);
                }
            }
            catch (Exception exc)
            {
                // [B.4] This re-throw means we discard the connection whenever
                // any error occurs during query command, even for a transient error.
                throw exc;  // [B.5] Let caller assess any exception, SqlException or any kind.
            }

        } // method EstablishConnection


        //通知送信のコネクション
        public void ConnectAndQuery(string email, int i)
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
            this.scsBuilder["Database"] = "managementnotificationdb";
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
                    this.EstablishConnection(email, i);
                    


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



        //データベースから通知を取得
        public void getNotification(string email)
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
                        @"select mnMobile.notificationMobile.notificationId, cast(mnMobile.notificationMobile.date as datetime) as 'datetime',
                                 mnMobile.notificationMobile.title, mnMobile.notificationMobile.body, mnMobile.childMobile.childName
                          from   mnMobile.notificationMobile inner join mnMobile.childMobile
                          on     mnMobile.notificationMobile.serialID = mnMobile.childMobile.serialID
                          where  accountId in (select accountid from mnMobile.accountMobile where mnMobile.accountMobile.email = '"
                        + email + "' and mnMobile.accountMobile.accountid in(select accountid from Transmit where mnMobile.notificationMobile.notificationid > Transmit.notificationId)) order by mnMobile.notificationMobile.notificationId";

                    // [C.2] Issue the query command through the connection.
                    using (dReader = dbCommand.ExecuteReader())
                    {
                        int transmitId = 0;
                        // [C.3] Loop through all returned rows, writing the data to the console.
                        while (dReader.Read())
                        {
                            ArrayList notification = new ArrayList();

                            sBuilder.Length = 0;
                            notification.Add(sBuilder.Append(dReader.GetInt32(0)));
                            sBuilder.Append("\t");
                            notification.Add(sBuilder.Append(dReader.GetDateTime(1)));
                            sBuilder.Append("\t");
                            notification.Add(sBuilder.Append(dReader.GetString(2)));
                            sBuilder.Append("\t");
                            notification.Add(sBuilder.Append(dReader.GetString(3)));
                            sBuilder.Append("\t");
                            notification.Add(sBuilder.Append(dReader.GetString(4)));

                            Notification nt = new Notification(dReader.GetInt32(0), dReader.GetDateTime(1), dReader.GetString(2),
                                                               dReader.GetString(3), dReader.GetString(4));
                            //list.Add(nt);
                            NotificationList.list.Add(nt);


                            transmitId = dReader.GetInt32(0);
                            
                            
                            Console.WriteLine(sBuilder.ToString());
                        }

                        dReader.Close();
                        if (transmitId != 0)
                        {
                            updateTransmit(transmitId, email);
                        }
                            
                        

                    }
                }
            }
            catch (Exception exc)
            {
                throw exc; // Let caller assess any exception.
            }
            return;
        }

        //受信済みの通知をデータベースに記録
        void updateTransmit(int id, String email)
        {

            D.IDbCommand dbCommand = null;

            dbCommand = sqlConnection.CreateCommand();

            // 実行する SQL コマンドを設定する
            dbCommand.CommandText = @"update Transmit set notificationId = " + id + 
                " where accountId in (select accountId from mnMobile.accountMobile where email = '" + email + "')";

            //dbCommand.Connection = sqlConnection;


            // SQL コマンドを実行し、影響を受けた行を返す
            dbCommand.ExecuteNonQuery();



            
            return;
        }

        //削除した通知の再送信
        void retransmition(string email)
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
                        @"select mnMobile.notificationMobile.notificationId, cast(mnMobile.notificationMobile.date as datetime) as 'datetime',
                                 mnMobile.notificationMobile.title, mnMobile.notificationMobile.body, mnMobile.childMobile.childName
                          from   mnMobile.notificationMobile inner join mnMobile.childMobile
                          on     mnMobile.notificationMobile.serialID = mnMobile.childMobile.serialID
                          where  accountId in (select accountid from mnMobile.accountMobile where mnMobile.accountMobile.email = '"
                        + email + "' and mnMobile.accountMobile.accountid in(select accountid from Transmit where mnMobile.notificationMobile.notificationid <= Transmit.notificationId)) order by mnMobile.notificationMobile.notificationId";

                    // [C.2] Issue the query command through the connection.
                    using (dReader = dbCommand.ExecuteReader())
                    {
                        // [C.3] Loop through all returned rows, writing the data to the console.
                        while (dReader.Read())
                        {
                            
                            ArrayList notification = new ArrayList();

                            sBuilder.Length = 0;
                            notification.Add(sBuilder.Append(dReader.GetInt32(0)));
                            sBuilder.Append("\t");
                            notification.Add(sBuilder.Append(dReader.GetDateTime(1)));
                            sBuilder.Append("\t");
                            notification.Add(sBuilder.Append(dReader.GetString(2)));
                            sBuilder.Append("\t");
                            notification.Add(sBuilder.Append(dReader.GetString(3)));
                            sBuilder.Append("\t");
                            notification.Add(sBuilder.Append(dReader.GetString(4)));

                            Notification nt = new Notification(dReader.GetInt32(0), dReader.GetDateTime(1), dReader.GetString(2),
                                                               dReader.GetString(3), dReader.GetString(4));
                            
                            Boolean exist = false;

                            for (int i = 0; NotificationList.list.Count > i; i++)
                            {
                                if (NotificationList.list[i].NotificationID == nt.NotificationID)
                                {
                                    exist = true;
                                    break;
                                }
                            }

                            if (!exist)
                            {
                                NotificationList.list.Add(nt);
                                Console.WriteLine(sBuilder.ToString());
                            }

                        }
                        dReader.Close();

                    }
                }
            }
            catch (C.SqlException exc)
            {
                throw exc;
            }
        }

        //アカウント再発行時のメールアドレスの確認
        String checkEmail(string inputEmail)
        {
            D.IDataReader dReader = null;
            C.SqlCommand com = new C.SqlCommand();
            con = new Confirmation();
            String email = "";
            try
            {
                // [C.1] Use the connection to create a query command.
                using (com = this.sqlConnection.CreateCommand())
                {

                    com.CommandText = @"SELECT email FROM mnMobile.accountMobile " +
                                    "WHERE email = @email";

                    com = new C.SqlCommand(com.CommandText, sqlConnection);

                    try
                    {
                        AddSqlParameter(com, "@email", D.SqlDbType.NChar, inputEmail);

                    }
                    catch (C.SqlException exc)
                    {
                        throw exc;
                    }

                    // [C.2] Issue the query command through the connection.
                    using (dReader = com.ExecuteReader())
                    {
                        while (dReader.Read())
                        {
                            email = dReader.GetString(0);
                        }
                    }

                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
                throw exc; // Let caller assess any exception.
            }
            return email;
        }

        //アカウント再発行
        String[] Reissue(String email){
            D.IDataReader dReader = null;
            C.SqlCommand com = new C.SqlCommand();
            con = new Confirmation();
            String[] account = new String[2];

            try
            {
                using (com = this.sqlConnection.CreateCommand())
                {
                    string guidResult = System.Guid.NewGuid().ToString();

                    account[0] = guidResult.Substring(0, 8);
                    guidResult = System.Guid.NewGuid().ToString();
                    account[1] = guidResult.Substring(0, 8);

                    com.CommandText = @"UPDATE mnMobile.accountMobile SET username = '" + account[0] + 
                        "', password = '" + account[1] + "' where email = '" + email + "'";

                    com = new C.SqlCommand(com.CommandText, sqlConnection);

                    using (dReader = com.ExecuteReader())
                    {
                        while (dReader.Read())
                        {

                        }
                    }
                }
            }
            catch (Exception exc)
        {
                Console.WriteLine(exc.ToString());
                throw exc; // Let caller assess any exception.
            }

            return account;

        }

        //ユーザ情報変更
        void changeData(String[] accountData)
        {
            D.IDataReader dReader = null;
            C.SqlCommand com = new C.SqlCommand();
            con = new Confirmation();
            int setCount = 0;

            try
            {
                using (com = this.sqlConnection.CreateCommand())
                {


                    com.CommandText = @"UPDATE mnMobile.accountMobile SET ";

                    if (!accountData[0].Equals(""))
                    {
                        com.CommandText += "username = @username";
                        setCount++;
                    }

                    if (!accountData[1].Equals(""))
                    {
                        if (setCount != 0)
                        {
                            com.CommandText += ",";
                        }
                        com.CommandText += "password = @password";
                        setCount++;
                    }

                    if (!accountData[2].Equals(""))
                    {
                        if (setCount != 0)
                        {
                            com.CommandText += ",";
                        }
                        com.CommandText += "email = @email";
                    }

                    com.CommandText += " where email = @nowEmail";

                    com = new C.SqlCommand(com.CommandText, sqlConnection);

                    try
                    {
                        AddSqlParameter(com, "@username", D.SqlDbType.NChar, accountData[0]);
                        AddSqlParameter(com, "@password", D.SqlDbType.NChar, accountData[1]);
                        AddSqlParameter(com, "@email", D.SqlDbType.NChar, accountData[2]);
                        AddSqlParameter(com, "@nowEmail", D.SqlDbType.NChar, accountData[3]);
                    }
                    catch (C.SqlException exc)
                    {
                        throw exc;
                    }

                    using (dReader = com.ExecuteReader())
                    {
                        while (dReader.Read())
                        {

                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
                throw exc; // Let caller assess any exception.
            }

        }


///////////////////////////SQLパラメータの設定/////////////////////////////////////////
        public static void AddSqlParameter(
            C.SqlCommand com, string ParameterName,D.SqlDbType type, Object value)
        {
            C.SqlParameter param = com.CreateParameter();
            param.ParameterName = ParameterName;
            //param.SqlDbType = type;
            param.Direction = D.ParameterDirection.Input;
            param.Value = value;
            com.Parameters.Add(param);
        }
    } // class Program
}