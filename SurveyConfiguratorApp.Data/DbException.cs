using SurveyConfiguratorApp.Domain;
using SurveyConfiguratorApp.Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyConfiguratorApp.Data
{
    public class DbException
    {

        private const int SQL_CONNECTION_FAILED = 2;
        private const int SQL_VIOLATION_UNIQUE_KEY = 2627;
        private const int SQL_VIOLATION_NOT_NULL = 515;
        public static int HandleSqlException(SqlException sqlException)
        {
            try
            {
                Log.Error(sqlException);
                int tResult;
                switch (sqlException.Number)
                {
                    case SQL_CONNECTION_FAILED:
                        tResult = ResultCode.DB_CONNECTION_FAILED;
                        break;

                    case SQL_VIOLATION_UNIQUE_KEY:
                        tResult = ResultCode.VALIDATION_ERROR_ORDER_EXIST;
                        break;

                    case SQL_VIOLATION_NOT_NULL:
                        tResult = ResultCode.VALIDATION_ERROR_REQUIRED_VALUE;
                        break;

                    default:

                        tResult = ResultCode.ERROR;
                        break;
                }

                return tResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return ResultCode.ERROR;
            }
        }


    }
}
