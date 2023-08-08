using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using System;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp.Data.Questions
{
    /// <summary>
    /// The class handles CRUD operations for the QuestionFaces table in the database
    /// </summary>
    public class DbQuestionFaces : DbQuestion
    {
        private new const string tableName = "QuestionFaces";

        private readonly string INSERT_QUERY = string.Format("INSERT INTO [{0}] ([{1}],[{2}]) VALUES (@{1},@{2})",
                               tableName, ColumnNames.QuestionId, ColumnNames.FacesNumber);



        private readonly string UPDATE_QUERY = string.Format("UPDATE [{0}] SET [{1}] = @{1} WHERE [{2}] = @{2}",
                                   tableName, ColumnNames.FacesNumber, ColumnNames.QuestionId);

        private readonly string GET_QUERY = string.Format(
                            "SELECT [{0}],[{1}],[{2}] FROM {3} as q INNER JOIN {4} as f ON q.{5}=f.{6} WHERE q.{5}=@{6};",
                            DbQuestion.ColumnNames.Text,
                            ColumnNames.FacesNumber,
                            DbQuestion.ColumnNames.Order,
                            DbQuestion.tableName,
                            tableName,
                            DbQuestion.ColumnNames.Id,
                            ColumnNames.QuestionId
                        );

        public new enum ColumnNames
        {
            QuestionId,
            FacesNumber,
        }
        public DbQuestionFaces() : base() { }



        // Create a new QuestionFaces entry in the database
        public int Add(QuestionFaces pQuestionFaces)
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

                    SqlTransaction pTransaction = Connection.BeginTransaction();
                    tCommand.Transaction = pTransaction;

                    int tResultAdded = base.Add(pQuestionFaces, tCommand);
                    if (tResultAdded != ResultCode.SUCCESS)
                        return tResultAdded;

                    int tQuestionId = base.GetQuestionId();
                    if (tQuestionId < 0)
                        return ResultCode.ERROR;

                    tCommand.CommandText = INSERT_QUERY;

                    tCommand.Parameters.AddWithValue($"@{ColumnNames.QuestionId}", tQuestionId);
                    tCommand.Parameters.AddWithValue($"@{ColumnNames.FacesNumber}", pQuestionFaces.FacesNumber);

                    int tRowsAffected = tCommand.ExecuteNonQuery();
                    if (tRowsAffected > 0)
                    {
                        pTransaction.Commit();
                        return ResultCode.SUCCESS;
                    }
                    pTransaction.Rollback();

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
            finally
            {
                base.CloseConnection();
            }
            return ResultCode.VALIDATION_ERROR;

        }

        // Update a QuestionFaces entry in the database
        public int Update(QuestionFaces pQuestionFaces)
        {


            try
            {
                int tResultIsQuestionExists = IsQuestionExists(pQuestionFaces.GetId());
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

                    tCommand.Parameters.AddWithValue($"@{ColumnNames.FacesNumber}", pQuestionFaces.FacesNumber);
                    tCommand.Parameters.AddWithValue($"@{ColumnNames.QuestionId}", pQuestionFaces.GetId());

                    int tRowsAffected = tCommand.ExecuteNonQuery();

                    if (tRowsAffected <= 0)
                    {
                        // Row not found or not updated
                        return ResultCode.VALIDATION_ERROR;
                    }

                    int tUpdateBaseStatus = base.Update(pQuestionFaces, tCommand);
                    if (tUpdateBaseStatus != ResultCode.SUCCESS)
                    {
                        tTransaction.Rollback();
                        return tUpdateBaseStatus;
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
            catch (Exception e)
            {
                Log.Error(e);
                return ResultCode.ERROR;
            }


        }

        // Read a QuestionFaces entry from the database based on the ID
        public int Get(ref QuestionFaces pQuestionFaces)
        {
            try
            {

                base.OpenConnection();

                using (SqlCommand tCommand = new SqlCommand())
                {

                    tCommand.Connection = base.Connection;
                    tCommand.CommandText = GET_QUERY;
                    tCommand.Parameters.AddWithValue($"@{ColumnNames.QuestionId}", pQuestionFaces.GetId());


                    using (SqlDataReader tReader = tCommand.ExecuteReader())
                    {
                        if (!tReader.HasRows) return ResultCode.DB_RECORD_NOT_EXISTS;
                        if (tReader.Read())
                        {
                            pQuestionFaces.Order = (int)tReader[$"{DbQuestion.ColumnNames.Order}"];
                            // pQuestionFaces.SetId(id);
                            pQuestionFaces.Text = tReader[$"{DbQuestion.ColumnNames.Text}"].ToString();
                            pQuestionFaces.FacesNumber = (int)tReader[$"{ColumnNames.FacesNumber}"];

                        }
                        return ResultCode.SUCCESS;
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
        }
    }

}
