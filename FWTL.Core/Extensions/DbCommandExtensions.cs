using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FWTL.Core.Extensions
{
    public static class DbCommandExtensions
    {
        public static string ToSql(this IDbCommand command)
        {
            StringBuilder sb = new StringBuilder();
            foreach (SqlParameter parameter in command.Parameters)
            {
                sb.AppendLine($"DECLARE @{parameter.ParameterName} {GetSqlType(parameter)}");
                sb.AppendLine($"SET @{parameter.ParameterName} = {GetSqlValue(parameter)}");
            }

            sb.AppendLine(command.CommandText);
            return sb.ToString();
        }

        private static string GetSqlType(SqlParameter parameter)
        {
            switch (parameter.SqlDbType)
            {
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                    {
                        return $"{parameter.SqlDbType}({parameter.Size})";
                    }
                default:
                    {
                        return parameter.SqlDbType.ToString();
                    }
            }
        }

        private static string GetSqlValue(SqlParameter parameter)
        {
            switch (parameter.SqlDbType)
            {
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                    {
                        return $"'{parameter.SqlValue}'";
                    }
                default:
                    {
                        return $"{parameter.SqlValue}";
                    }
            }
        }
    }
}