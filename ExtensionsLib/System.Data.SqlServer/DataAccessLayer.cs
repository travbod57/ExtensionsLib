using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ExtensionsLib.System.Data.SqlServer
{
    public static class DataAccessLayer
    {

        public static bool IsContextAvailable(DbContext dbContext)
        {
            // Open the connection to the database
            return OpenConnection(dbContext);
        }

        /// <summary>
        /// Runs a stored proc using a given EF DbContext, and returns a Generic List of items
        /// </summary>
        /// <typeparam name="T">The type of item returned in the Generic List</typeparam>
        /// <param name="dbContext">The DbContext that the stored proc will run in</param>
        /// <param name="storedProcedureName">The name of the stored proc to run</param>
        /// <param name="entitySetName">The EntitySet name - can be found in your EF project in the xxxContext.cs file, under DbSet</param>
        /// <param name="parameters">The parameters to pass to the stored proc to run</param>
        /// <returns>Generic List of items of type T</returns>
        public static IList<T> GetList<T>(DbContext dbContext, string storedProcedureName, string entitySetName, List<SqlParameter> parameters = null)
        {

            IList<T> list = null;

            // Make sure Code First has built the model before we open the connection
            dbContext.Database.Initialize(force: false);

            // Create a SQL command to execute the sproc
            using (var cmd = dbContext.Database.Connection.CreateCommand())
            {
                cmd.CommandText = storedProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;

                // Append parameters
                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        SqlParameter param = (SqlParameter)parameter;
                        cmd.Parameters.Add(param);
                    }
                }

                // Open the connection to the database
                OpenConnection(dbContext);

                // Run the sproc
                using (var reader = cmd.ExecuteReader())
                {
                    // Read from the first result set
                    var lst = ((IObjectContextAdapter)dbContext)
                        .ObjectContext
                        .Translate<T>(reader, entitySetName, MergeOption.AppendOnly);

                    list = (List<T>)lst.ToList();
                }
                dbContext.Database.Connection.Close();
            }
            return list;
        }

        /// <summary>
        /// Runs a stored proc using a given EF DbContext, and returns a Generic Type T
        /// </summary>
        /// <typeparam name="T">The type of item returned</typeparam>
        /// <param name="dbContext">The DbContext that the stored proc will run in</param>
        /// <param name="storedProcedureName">The name of the stored proc to run</param>
        /// <param name="entitySetName">The EntitySet name - can be found in your EF project in the xxxContext.cs file, under DbSet</param>
        /// <param name="parameters">The parameters to pass to the stored proc to run</param>
        /// <returns>Instance of an object of Generic Type T</returns>
        public static T GetObject<T>(DbContext dbContext, string storedProcedureName, string entitySetName, List<SqlParameter> parameters = null)
        {

            T obj = default(T);

            // Make sure Code First has built the model before we open the connection
            dbContext.Database.Initialize(force: false);

            // Create a SQL command to execute the sproc
            using (var cmd = dbContext.Database.Connection.CreateCommand())
            {
                cmd.CommandText = storedProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;

                // Append parameters
                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        SqlParameter param = (SqlParameter)parameter;
                        cmd.Parameters.Add(param);
                    }
                }

                // Open the connection to the database
                OpenConnection(dbContext);

                // Run the sproc
                using (var reader = cmd.ExecuteReader())
                {
                    // Read from the first result set
                    var objct = ((IObjectContextAdapter)dbContext)
                        .ObjectContext
                        .Translate<T>(reader, entitySetName, MergeOption.AppendOnly);

                    obj = (T)objct.FirstOrDefault();
                }
                dbContext.Database.Connection.Close();
            }
            return obj;
        }

        /// <summary>
        /// Runs a stored proc using a given EF DbContext, and returns a primitive generic value of Type T
        /// </summary>
        /// <typeparam name="T">The type of value returned</typeparam>
        /// <param name="dbContext">The DbContext that the stored proc will run in</param>
        /// <param name="storedProcedureName">The name of the stored proc to run</param>
        /// <param name="entitySetName">The EntitySet name - can be found in your EF project in the xxxContext.cs file, under DbSet</param>
        /// <param name="parameters">The parameters to pass to the stored proc to run</param>
        /// <returns>Value of generic type T</returns>
        public static T GetValue<T>(DbContext dbContext, string storedProcedureName, string entitySetName, List<SqlParameter> parameters = null)
        {

            T obj = default(T);

            // Make sure Code First has built the model before we open the connection
            dbContext.Database.Initialize(force: false);

            // Create a SQL command to execute the sproc
            using (var cmd = dbContext.Database.Connection.CreateCommand())
            {
                cmd.CommandText = storedProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;

                // Append parameters
                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        SqlParameter param = (SqlParameter)parameter;
                        cmd.Parameters.Add(param);
                    }
                }

                // Open the connection to the database
                OpenConnection(dbContext);

                // Run the sproc
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    if (reader.HasRows)
                    {
                        try
                        {
                            obj = (T)reader.GetValue(0);
                        }
                        catch (InvalidCastException ex)
                        {
                            obj = default(T);
                        }
                    }
                    else
                    {
                        obj = default(T);
                    }
                }
                dbContext.Database.Connection.Close();
            }
            return obj;
        }

        /// <summary>
        /// Runs a stored proc using a given EF DbContext, and returns a Generic List of items
        /// </summary>
        /// <typeparam name="T">The type of item returned in the Generic List</typeparam>
        /// <param name="dbContext">The DbContext that the stored proc will run in</param>
        /// <param name="storedProcedureName">The name of the stored proc to run</param>
        /// <param name="entitySetName">The EntitySet name - can be found in your EF project in the xxxContext.cs file, under DbSet</param>
        /// <param name="parameters">The parameters to pass to the stored proc to run</param>
        /// <returns>Generic List of items of type T</returns>
        /// <remarks>
        /// The connection gets automatically closed once done with the execution
        /// </remarks>
        public static bool ExecuteNonQuery(DbContext dbContext, string storedProcedureName, List<SqlParameter> parameters = null)
        {
            return ExecuteNonQuery(dbContext, true, storedProcedureName, parameters);
        }

        /// <summary>
        /// Runs a stored proc using a given EF DbContext, and returns a Generic List of items
        /// </summary>
        /// <typeparam name="T">The type of item returned in the Generic List</typeparam>
        /// <param name="dbContext">The DbContext that the stored proc will run in</param>
        /// <param name="closeConnectionWhenDone">If true the connection gets closed when done with the execution</param>
        /// <param name="storedProcedureName">The name of the stored proc to run</param>
        /// <param name="entitySetName">The EntitySet name - can be found in your EF project in the xxxContext.cs file, under DbSet</param>
        /// <param name="parameters">The parameters to pass to the stored proc to run</param>
        /// <returns>Generic List of items of type T</returns>
        public static bool ExecuteNonQuery(DbContext dbContext, bool closeConnectionWhenDone, string storedProcedureName, List<SqlParameter> parameters = null)
        {

            bool success = false;

            // Make sure Code First has built the model before we open the connection
            dbContext.Database.Initialize(force: false);

            // Create a SQL command to execute the sproc
            using (var cmd = dbContext.Database.Connection.CreateCommand())
            {
                cmd.CommandText = storedProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;

                // Append parameters
                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        SqlParameter param = (SqlParameter)parameter;
                        cmd.Parameters.Add(param);
                    }
                }

                // Open the connection to the database
                OpenConnection(dbContext);

                // Run the sproc
                try
                {
                    cmd.ExecuteNonQuery();
                    success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    //ErrorHandler.LogError(ex);
                }
                finally
                {
                    if (closeConnectionWhenDone)
                        dbContext.Database.Connection.Close();
                }
            }
            return success;
        }

        private static bool OpenConnection(DbContext dbContext)
        {
            bool result = false;

            try
            {
                if (dbContext.Database.Connection.State != ConnectionState.Open)
                    dbContext.Database.Connection.Open();
                result = true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                //ErrorHandler.LogError(ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //ErrorHandler.LogError(ex);
            }

            return result;
        }
    }
}
