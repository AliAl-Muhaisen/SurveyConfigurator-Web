using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using System;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp.Data.Questions
{
    public class DbQuestionStars : DbQuestion
    {
        private new const string tableName = "QuestionStars";

        private readonly string INSERT_QUERY = string.Format("INSERT INTO [{0}] ([{1}],[{2}]) VALUES (@{1},@{2})",
                                           tableName, ColumnNames.QuestionId, ColumnNames.StarsNumber);
     
        private readonly string UPDATE_QUERY =string.Format("UPDATE [{0}] SET [{1}] = @{1} WHERE [{2}] = @{2}",
                                       TableName, ColumnNames.StarsNumber, ColumnNames.QuestionId);

      private readonly string GET_QUERY = string.Format(
                    "SELECT [{0}],[{1}],[{2}] FROM {3} as q INNER JOIN {4} as s ON q.{5}=s.{6} WHERE q.{5}=@{6};",
                    DbQuestion.ColumnNames.Text,
                    ColumnNames.StarsNumber,
                    DbQuestion.ColumnNames.Order,
                    DbQuestion.tableName,
                    tableName,
                    DbQuestion.ColumnNames.Id,
                    ColumnNames.QuestionId
                );
        private new enum ColumnNames
        {
            QuestionId,
            StarsNumber,
        }
        public DbQuestionStars() : base() { }

        static public string TableName { get { return tableName; } }
        public int Add(QuestionStars pQuestionStars)
        {
            try
            {

                using (SqlCommand tCommand = new SqlCommand())
                {
                    int tConnectionResult = base.OpenConnection();
                    if (tConnectionResult != ResultCode.SUCCESS)
                    {
                        return tConnectionResult;
                    }

                    tCommand.Connection = base.Connection;

                    SqlTransaction tTransaction = Connection.BeginTransaction();
                    tCommand.Transaction = tTransaction;

                    int tResult = base.Add(pQuestionStars, tCommand);


                    if (tResult != ResultCode.SUCCESS)
                        return tResult;

                    int tQuestionId = base.GetQuestionId();

                    if (tQuestionId <0)
                        return ResultCode.ERROR;

                    tCommand.CommandText = INSERT_QUERY;

                    tCommand.Parameters.AddWithValue($"@{ColumnNames.QuestionId}", tQuestionId);
                    tCommand.Parameters.AddWithValue($"@{ColumnNames.StarsNumber}", pQuestionStars.StarsNumber);



                    int tRowAffected = tCommand.ExecuteNonQuery();
                    if (tRowAffected > 0)
                    {
                        tTransaction.Commit();
                        return ResultCode.SUCCESS;
                    }
                    // If the stars question not added successfully, Delete the base question 
                    tTransaction.Rollback();
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return ResultCode.ERROR;
            }
            finally
            {
                base.CloseConnection();
            }
            return ResultCode.VALIDATION_ERROR;
        }



        public int Update(QuestionStars tQuestionStars)
        {
            try
            {
                int tResultIsQuestionExists = IsQuestionExists(tQuestionStars.GetId());
                if (tResultIsQuestionExists != ResultCode.SUCCESS)
                {
                    return tResultIsQuestionExists;
                }

                using (SqlCommand tCommand = new SqlCommand())
                {
                    int tConnectionResult = base.OpenConnection();
                    if (tConnectionResult != ResultCode.SUCCESS)
                    {
                        return tConnectionResult;
                    }

                    tCommand.Connection = base.Connection;

                    SqlTransaction tTransaction = Connection.BeginTransaction();

                    tCommand.Transaction = tTransaction;

                    tCommand.CommandText = UPDATE_QUERY;

                    tCommand.Parameters.AddWithValue($"@{ColumnNames.StarsNumber}", tQuestionStars.StarsNumber);
                    tCommand.Parameters.AddWithValue($"@{ColumnNames.QuestionId}", tQuestionStars.GetId());



                    int rowsAffected = tCommand.ExecuteNonQuery();

                    if (rowsAffected <= 0)
                    {
                        // Row not found or not updated
                        return ResultCode.VALIDATION_ERROR;
                    }


                    int tUpdatedBaseStatus = base.Update(tQuestionStars, tCommand);
                    if (tUpdatedBaseStatus != ResultCode.SUCCESS)
                    {
                        tTransaction.Rollback();
                        return tUpdatedBaseStatus;
                    }
                    tTransaction.Commit();
                    base.CloseConnection();
                    return ResultCode.SUCCESS;
                }

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

        public int Get(ref QuestionStars pQuestionStars)
        {
            try
            {

                int tResult = base.OpenConnection();
                if (tResult != ResultCode.SUCCESS)
                {
                    return tResult;
                }

                using (SqlCommand tCommand = new SqlCommand())
                {
                    tCommand.Connection = base.Connection;
                    string query =


                    tCommand.CommandText =GET_QUERY ;
                    tCommand.Parameters.AddWithValue($"@{ColumnNames.QuestionId}", pQuestionStars.GetId());
                    using (SqlDataReader pReader = tCommand.ExecuteReader())
                    {
                        if (pReader.Read())
                        {
                            pQuestionStars.Order = (int)pReader[$"{DbQuestion.ColumnNames.Order}"];
                            pQuestionStars.Text = pReader[$"{DbQuestion.ColumnNames.Text}"].ToString();
                            pQuestionStars.StarsNumber = (int)pReader[$"{ColumnNames.StarsNumber}"];
                            return ResultCode.SUCCESS;

                        }
                    }

                }
            }

            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }
            finally
            {
                base.CloseConnection();

            }
            return ResultCode.DB_RECORD_NOT_EXISTS;
        }
    }
}
