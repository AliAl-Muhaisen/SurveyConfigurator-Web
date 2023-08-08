using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp.Data.Questions
{

    /// <summary>
    /// The DbQuestion class extends the DB class and implements the ICRUD<Question> interface. 
    /// It provides methods to perform CRUD operations (Add, read, update, Delete) on the Question entity.
    /// The class includes a ColumnsName enumeration representing the column names in the Question table. 
    /// It also includes additional methods,
    /// retrieve the last inserted ID, and read all questions from the database.
    /// </summary>
    public class DbQuestion : DbConnection
    {
        public static event EventHandler DataChanged;
        public DbQuestion() : base() { }
        public const string tableName = "Question";
        public enum ColumnNames
        {
            Id,
            Order,
            Text,
            TypeNumber

        }
        private int questionId = -1;


        private readonly string INSERT_QUERY = string.Format("INSERT INTO [{0}] ([{1}],[{2}],[{3}]) VALUES " +
                        "(@{1},@{2},@{3});SELECT SCOPE_IDENTITY();",
                        tableName, ColumnNames.Order, ColumnNames.Text, ColumnNames.TypeNumber);



        private readonly string UPDATE_QUERY = string.Format("UPDATE [{0}] SET [{1}] = @{1}, [{2}] = @{2} WHERE [{3}] = @{3}",
                                   tableName, ColumnNames.Text, ColumnNames.Order, ColumnNames.Id);

        private readonly string DELETE_QUERY = string.Format("DELETE FROM [{0}] WHERE [{1}]=@{1};", tableName, ColumnNames.Id);

        private readonly string GET_QUERY = string.Format("SELECT * FROM [{0}] WHERE [{1}]=@{1};", tableName, ColumnNames.Id);

        private readonly string GET_ALL_QUERY = string.Format("SELECT * FROM [{0}];", tableName);
        /// <summary>
        /// The Add method inserts a new record into the Question table by constructing a parameterized SQL GET_QUERY.
        /// It catches any SQL exceptions and returns false in case of an error.
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public int Add(Question pData, SqlCommand pSqlCommand)
        {
            try
            {
                if (pSqlCommand == null)
                {
                    return ResultCode.DB_CONNECTION_FAILED;
                }

                //SCOPE_IDENTITY() is a function in SQL Server that returns the last identity value inserted into an identity column within the current scope.
                //It is commonly used to retrieve the generated ID value after performing an insert operation.

                pSqlCommand.CommandText = INSERT_QUERY;

                pSqlCommand.Parameters.AddWithValue($"@{ColumnNames.Order}", pData.Order);
                pSqlCommand.Parameters.AddWithValue($"@{ColumnNames.Text}", pData.Text);
                pSqlCommand.Parameters.AddWithValue($"@{ColumnNames.TypeNumber}", pData.GetTypeNumber());
                // Execute the GET_QUERY and retrieve the generated ID
                questionId = Convert.ToInt32(pSqlCommand.ExecuteScalar());

                if (questionId > 0)
                {
                    OnDataChanged();
                    return ResultCode.SUCCESS;
                }


            }
            catch (SqlException ex)
            {

                return DbException.HandleSqlException(ex);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }

            return ResultCode.VALIDATION_ERROR;

        }

        public int IsQuestionExists(int pQuestionId)
        {
            try
            {
                Question tQuestion = this.Get(pQuestionId);
                if
                    (tQuestion == null)
                    return ResultCode.DB_RECORD_NOT_EXISTS;

                return ResultCode.SUCCESS;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return ResultCode.ERROR;
            }
        }

        public int Delete(int pID)
        {

            try
            {
               
              
                using (SqlConnection tConnection = new SqlConnection(DbConnection.GetConfigConnectionString()))
                {
                    tConnection.Open();
                    using (SqlTransaction tTransaction = tConnection.BeginTransaction())
                    {
                        using (SqlCommand tCommand = new SqlCommand())
                        {
                            tCommand.Connection = tConnection;
                            tCommand.Transaction = tTransaction;
                            tCommand.CommandText = DELETE_QUERY;
                            tCommand.Parameters.AddWithValue($"@{ColumnNames.Id}", pID);

                            
                            int pRowsAffected = tCommand.ExecuteNonQuery();


                            if (pRowsAffected > 0)
                            {
                                tTransaction.Commit();
                                tConnection.Close();
                                // Row deleted successfully
                                return ResultCode.SUCCESS;

                            }
                            tTransaction.Rollback();
                            tConnection.Close();
                        }

                    }

                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
            finally { }

            return ResultCode.DB_FAILED_DELETE_ERROR;
        }




        /// <summary>
        /// The update method updates a specific record in the Question table based on the provided Question object.
        /// It catches SQL exceptions and returns false in case of an error.
        /// </summary>
        public int Update(Question pQuestion, SqlCommand pSqlCommand)
        {

            try
            {
                if (pSqlCommand == null)
                {
                    return ResultCode.DB_CONNECTION_FAILED;
                }

                pSqlCommand.CommandText = UPDATE_QUERY;

                pSqlCommand.Parameters.AddWithValue($"@{ColumnNames.Text}", pQuestion.Text);
                pSqlCommand.Parameters.AddWithValue($"@{ColumnNames.Order}", pQuestion.Order);
                pSqlCommand.Parameters.AddWithValue($"@{ColumnNames.Id}", pQuestion.GetId());

                int tRowsAffected = pSqlCommand.ExecuteNonQuery();

                if (tRowsAffected > 0)
                {
                    // Row updated successfully
                    return ResultCode.SUCCESS;
                }
                // Row not found or not updated
                return ResultCode.VALIDATION_ERROR;

            }
            catch (SqlException ex)
            {
                return DbException.HandleSqlException(ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return ResultCode.ERROR;
            }


        }



        /// <summary>
        /// The GetQuestionId method retrieves the maximum ID from the Question table.
        /// It catches any SQL exceptions and returns a default value of 1 in case of an error.
        /// </summary>
        /// <returns></returns>
        public int GetQuestionId()
        {
            try
            {
                return questionId;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return -1;//record not found
        }

        public int GetQuestions(ref List<Question> pQuestionsList)
        {
            try
            {
                base.OpenConnection();
                using (SqlCommand pCommand = new SqlCommand())
                {
                    pCommand.Connection = base.Connection;
                    pCommand.CommandText = GET_ALL_QUERY;
                    using (SqlDataReader pReader = pCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (pReader.HasRows)
                        {
                            while (pReader.Read())
                            {
                                Question tQuestion = new Question(
                                       (int)pReader[$"{ColumnNames.Id}"],
                                       pReader[$"{ColumnNames.Text}"].ToString(),
                                       (int)pReader[$"{ColumnNames.TypeNumber}"],
                                       (int)pReader[$"{ColumnNames.Order}"]
                                       );

                                tQuestion.SetId(Convert.ToInt32(pReader[$"{ColumnNames.Id}"]));

                                pQuestionsList.Add(tQuestion);
                            }

                        }

                    }

                }
                CloseConnection();

                return ResultCode.SUCCESS;
            }
            catch (SqlException ex)
            {
                CloseConnection();
                return DbException.HandleSqlException(ex);
            }
            catch (Exception ex)
            {
                CloseConnection();
                Log.Error(ex);
                return ResultCode.ERROR;
            }

        }



        public Question Get(int pID)
        {
            try
            {

                base.OpenConnection();

                using (SqlCommand tCommand = new SqlCommand())
                {
                    tCommand.Connection = base.Connection;
                    tCommand.CommandText = GET_QUERY;
                    tCommand.Parameters.AddWithValue($"@{ColumnNames.Id}", pID);

                    using (SqlDataReader tReader = tCommand.ExecuteReader())
                    {
                        if (tReader.Read())
                        {
                            Question tQuestion = new Question(
                                pID,
                                tReader[$"{ColumnNames.Text}"].ToString(),
                                (int)tReader[$"{ColumnNames.TypeNumber}"],
                                (int)tReader[$"{ColumnNames.Order}"]
                                );

                            return tQuestion;

                        }
                    }

                }
             
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            finally
            {
                base.CloseConnection();
            }
            return null;
        }

        protected virtual void OnDataChanged()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}
