using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Domain.Questions;
using SurveyConfiguratorApp.Helper;
using System;
using System.Data.SqlClient;

namespace SurveyConfiguratorApp.Data.Questions
{
    public class DbQuestionSlider : DbQuestion
    {
        private new const string tableName = "QuestionSlider";
        public new enum ColumnNames
        {
            QuestionId,
            StartValue,
            EndValue,
            StartCaption,
            EndCaption,
        }
        private readonly string INSERT_QUERY = string.Format("INSERT INTO [{0}] ([{1}],[{2}],[{3}],[{4}],[{5}]) " +
                                   "VALUES (@{1},@{2},@{3},@{4},@{5})",
                                   tableName, ColumnNames.QuestionId, ColumnNames.StartValue,
                                   ColumnNames.EndValue, ColumnNames.EndCaption, ColumnNames.StartCaption);

        private readonly string UPDATE_QUERY = string.Format("UPDATE [{0}] SET [{1}] = @{1}, [{2}] = @{2}, [{3}] = @{3}, [{4}] = @{4} " +
                                   "WHERE [{5}] = @{5}",
                                   tableName, ColumnNames.StartCaption, ColumnNames.EndCaption,
                                   ColumnNames.StartValue, ColumnNames.EndValue, ColumnNames.QuestionId);

        private readonly string GET_QUERY = string.Format(
                            "SELECT [{0}],[{1}],[{2}],[{3}],[{4}],[{5}] FROM {6} as q INNER JOIN {7} as f ON q.{8}=f.{9} WHERE q.{8}=@{9};",
                            DbQuestion.ColumnNames.Text,
                            ColumnNames.StartValue,
                            ColumnNames.EndValue,
                            ColumnNames.StartCaption,
                            ColumnNames.EndCaption,
                            DbQuestion.ColumnNames.Order,
                            DbQuestion.tableName,
                            tableName,
                            DbQuestion.ColumnNames.Id,
                            ColumnNames.QuestionId
                        );
        public int Add(QuestionSlider pQuestionSlider)
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

                    int tAddedResult = base.Add(pQuestionSlider, tCommand);
                    if (tAddedResult != ResultCode.SUCCESS)
                        return tAddedResult;
                    int tQuestionId = base.GetQuestionId();

                    if (tQuestionId <0)
                        return ResultCode.ERROR;

                    tCommand.CommandText = INSERT_QUERY;

                    tCommand.Parameters.AddWithValue($"@{ColumnNames.QuestionId}", tQuestionId);
                    tCommand.Parameters.AddWithValue($"@{ColumnNames.StartValue}", pQuestionSlider.StartValue);
                    tCommand.Parameters.AddWithValue($"@{ColumnNames.EndValue}", pQuestionSlider.EndValue);
                    tCommand.Parameters.AddWithValue($"@{ColumnNames.EndCaption}", pQuestionSlider.EndCaption);
                    tCommand.Parameters.AddWithValue($"@{ColumnNames.StartCaption}", pQuestionSlider.StartCaption);



                    int tRowAffected = tCommand.ExecuteNonQuery();
                    if (tRowAffected > 0)
                    {
                        tTransaction.Commit();
                        return ResultCode.SUCCESS;
                    }
                    tTransaction.Rollback();
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
            return ResultCode.VALIDATION_ERROR;

        }


        public int Update(QuestionSlider pQuestionSlider)
        {
            try
            {
                int tResultIsQuestionExists = IsQuestionExists(pQuestionSlider.GetId());
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

                    tCommand.Parameters.AddWithValue($"@{ColumnNames.StartCaption}", pQuestionSlider.StartCaption);
                    tCommand.Parameters.AddWithValue($"@{ColumnNames.EndCaption}", pQuestionSlider.EndCaption);
                    tCommand.Parameters.AddWithValue($"@{ColumnNames.StartValue}", pQuestionSlider.StartValue);
                    tCommand.Parameters.AddWithValue($"@{ColumnNames.EndValue}", pQuestionSlider.EndValue);
                    tCommand.Parameters.AddWithValue($"@{ColumnNames.QuestionId}", pQuestionSlider.GetId());

                    int tRowsAffected = tCommand.ExecuteNonQuery();

                    if (tRowsAffected <= 0)
                    {
                        // Row not found or not updated
                        tTransaction.Rollback();
                        return ResultCode.VALIDATION_ERROR;
                    }


                    int tUpdatedBaseResult = base.Update(pQuestionSlider, tCommand);
                    if (tUpdatedBaseResult != ResultCode.SUCCESS)
                    {
                        tTransaction.Rollback();
                        return tUpdatedBaseResult;
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

        public int Get(ref QuestionSlider pQuestionSlider)
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



                    tCommand.CommandText = GET_QUERY;

                    tCommand.Parameters.AddWithValue($"@{ColumnNames.QuestionId}", pQuestionSlider.GetId());
                    using (SqlDataReader tReader = tCommand.ExecuteReader())
                    {
                        if (tReader.Read())
                        {
                            pQuestionSlider.Order = (int)tReader[$"{DbQuestion.ColumnNames.Order}"];
                            pQuestionSlider.Text = tReader[$"{DbQuestion.ColumnNames.Text}"].ToString();
                            pQuestionSlider.EndCaption = tReader[$"{ColumnNames.EndCaption}"].ToString();
                            pQuestionSlider.StartCaption = tReader[$"{ColumnNames.StartCaption}"].ToString();
                            pQuestionSlider.StartValue = (int)tReader[$"{ColumnNames.StartValue}"];
                            pQuestionSlider.EndValue = (int)tReader[$"{ColumnNames.EndValue}"];
                            return ResultCode.SUCCESS;
                        }
                    }

                }
            }
            catch (SqlException e)
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
