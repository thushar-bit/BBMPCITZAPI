using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BBMPCITZAPI.Database
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BBMPCITZAPIConnection")!;
        }

        public int ExecuteNonQuery(string storedProcedureName, OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (OracleCommand command = new OracleCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters);
                        return command.ExecuteNonQuery();
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
        public DataSet ExecuteQuery(string storedProcedureName, OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (OracleCommand command = new OracleCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters);

                        OracleDataAdapter adapter = new OracleDataAdapter(command);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        return dataSet;
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
        public DataSet ExecuteDataset(string storedProcedureName, OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (OracleCommand command = new OracleCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters);

                        OracleDataAdapter adapter = new OracleDataAdapter(command);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        return dataSet;
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
        public int ExecuteStoredProceduresWithTransaction(
    string[] storedProcedureNames,
    OracleParameter[][] parameterSets,
    string logProcedureName)
        {
            if (storedProcedureNames.Length != parameterSets.Length)
                throw new ArgumentException("Mismatch between stored procedures and parameter sets.");

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < storedProcedureNames.Length; i++)
                        {
                            try
                            {
                                using (OracleCommand command = new OracleCommand(storedProcedureNames[i], connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Transaction = transaction;
                                    command.Parameters.AddRange(parameterSets[i]);
                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                // Log the error using a logging stored procedure
                           //     LogError(connection, logProcedureName, storedProcedureNames[i], ex.Message, transaction);
                                throw; // Re-throw to trigger the main rollback
                            }
                        }

                        // Commit transaction if all succeed
                        transaction.Commit();
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        // Rollback the entire transaction if any stored procedure fails
                        transaction.Rollback();
                
                       
                        return 0;
                    }
                }
            }
        }
        public int ExecuteStoredProceduresWithLoop(
    string[] storedProcedureNames,
    Dictionary<string, List<OracleParameter[]>> parametersMap,
    string logProcedureName,
    string EPID,
    Int64 KRSId
    )
        {
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var storedProcedure in storedProcedureNames)
                        {
                            if (parametersMap.ContainsKey(storedProcedure))
                            {
                                var parameterSets = parametersMap[storedProcedure];
                                foreach (var parameters in parameterSets)
                                {
                                    try
                                    {
                                        using (OracleCommand command = new OracleCommand(storedProcedure, connection))
                                        {
                                            command.CommandType = CommandType.StoredProcedure;
                                            command.Transaction = transaction;
                                            command.Parameters.AddRange(parameters);
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // Log error
                                       
                                        LogError(connection, logProcedureName, storedProcedure, ex.Message, KRSId,EPID);
                                        throw; // Trigger rollback
                                    }
                                }
                            }
                        }

                        // Commit transaction if all succeed
                        transaction.Commit();
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        // Rollback the entire transaction
                        transaction.Rollback();
                        return 0;
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }

            }
        }

        private void LogError(
     OracleConnection mainConnection,
     string logProcedureName,
     string procedureName,
     string errorMessage,
     Int64 KRSID,
     string propertyId)
        {
            // Use a new connection for logging
            using (OracleConnection logConnection = new OracleConnection(_connectionString))
            {
                try
                {
                    logConnection.Open();

                    using (OracleCommand logCommand = new OracleCommand(logProcedureName, logConnection))
                    {
                        logCommand.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        logCommand.Parameters.Add(new OracleParameter("P_KRSId", OracleDbType.Varchar2) { Value = KRSID });
                        logCommand.Parameters.Add(new OracleParameter("P_PROPERTYID", OracleDbType.Varchar2) { Value = propertyId });
                        logCommand.Parameters.Add(new OracleParameter("P_PROCEDURE_NAME", OracleDbType.Varchar2) { Value = procedureName });
                        logCommand.Parameters.Add(new OracleParameter("P_ERROR_MESSAGE", OracleDbType.Clob) { Value = errorMessage });

                        // Execute the logging command
                        logCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception logEx)
                {
                    throw;
                }
                finally
                {
                    if (logConnection.State == ConnectionState.Open)
                    {
                        logConnection.Close();
                    }
                }
            }
        }
    }
}
