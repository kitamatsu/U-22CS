using System;
using G = System.Collections.Generic;
using C = System.Data.SqlClient;

namespace ManagementNotification.db
{
    /// <summary>
    /// A custom alternative to class SqlDatabaeTransientErrorDetectionStrategy.
    /// </summary>
    public class Custom_SqlDatabaseTransientErrorDetectionStrategy
    {
        static private G.List<int> M_listTransientErrorNumbers;


        /// <summary>
        /// This method happens to match ITransientErrorDetectionStrategy of EntLib60.
        /// </summary>
        public bool IsTransient(Exception exc)
        {
            return IsTransientStatic(exc);
        }


        /// <summary>
        /// For general use beyond formal Enterprise Library classes.
        /// </summary>
        static public bool IsTransientStatic(Exception exc)
        {
            bool returnBool = false;  // Assume is a persistent error.
            C.SqlException sqlExc;

            if (exc is C.SqlException)
            {
                sqlExc = exc as C.SqlException;
                if (M_listTransientErrorNumbers.Contains(sqlExc.Number) == true)
                {
                    returnBool = true;  // Error is transient, not persistent.
                }
            }
            return returnBool;
        }


        /// <summary>
        /// Lists the SqlException.Number values that are considered
        /// to indicate transient errors.
        /// </summary>
        static Custom_SqlDatabaseTransientErrorDetectionStrategy()
        {
            int[] arrayOfTransientErrorNumbers =
                {4060, 10928, 10929, 40197, 40501, 40544, 40549,
                    40550, 40551, 40552, 40553, 40613};

            M_listTransientErrorNumbers = new G.List<int>(arrayOfTransientErrorNumbers);
        }
    } // class Custom_SqlDatabaseTransientErrorDetectionStrategy
}