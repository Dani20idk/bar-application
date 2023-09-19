using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BarProject.App_Code
{
    public class Global
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string conn()
            => $"Data Source={ConfigurationManager.AppSettings["Instanca"]};Initial Catalog={ConfigurationManager.AppSettings["Baza"]};Integrated Security=False;Uid={DecryptSetting("Perdorues")};PWD={DecryptSetting("FjaleKalim")};";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string DecryptSetting(string field) => Decrypt(ConfigurationManager.AppSettings[field]);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        public static void ExecuteSql(string command, string connection)
        {
            try
            {
                var lidhja = new SqlConnection(connection);
                var komanda = new SqlCommand(command, lidhja);
                if (lidhja.State != ConnectionState.Open)
                    lidhja.Open();
                komanda.ExecuteNonQuery();
                if (lidhja.State != ConnectionState.Closed)
                    lidhja.Close();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vleraPerTuKriptuar"></param>
        /// <returns></returns>
        public static string Crypt(string vleraPerTuKriptuar)
        {
            var rc = new Rc4
            {
                ÇelësiKriptimit = @"l19.03mc.w@#@djsjohfqw.2ew",
                TekstiBurim = vleraPerTuKriptuar
            };
            rc.Kripto();
            return rc.TekstiKriptuar;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vleraPerTudeKriptuar"></param>
        /// <returns></returns>
        public static string Decrypt(string vleraPerTudeKriptuar)
        {
            //var rc = new Rc4
            //{
            //    ÇelësiKriptimit = @"l19.03mc.w@#@djsjohfqw.2ew",
            //    TekstiKriptuar = vleraPerTudeKriptuar
            //};
            //rc.Dekripto();
            //return rc.TekstiBurim;

            return vleraPerTudeKriptuar;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="searchName"></param>
        /// <returns></returns>
        public static string GetSelectStatement(string objectName, string searchName)
        {
            var cnn = new SqlConnection(conn());

            var cmd = new SqlCommand(@"  DECLARE @columns NVARCHAR(max) 
	                                     SELECT @columns = COALESCE(@columns + ', [' + s.name+ '] = ISNULL([' + s.name+'],'
						                                    + CASE WHEN tt.Name IN ('bigint', 'decimal', 'float', 'int', 'money', 'tinyint', 'real', 'numeric','smallint','smallmoney') THEN '0' ELSE '''''' END+')',  
											                                    '[' + s.name+'] = ISNULL([' + s.name+'],'
						                                    + CASE WHEN tt.Name IN ('bigint', 'decimal', 'float', 'int', 'money', 'tinyint', 'real', 'numeric','smallint','smallmoney') THEN '0' ELSE '''''' END+')')
	                                     FROM sys.columns s
	                                     INNER JOIN sys.tables t ON s.object_id = t.object_id
		                                 LEFT JOIN sys.types tt ON s.user_type_id = tt.user_type_id
	                                     WHERE t.name = @objectName
                                    SELECT @columns", cnn);
            cmd.Parameters.AddWithValue("@objectName", objectName);
            cmd.CommandType = CommandType.Text;

            try
            {
                cnn.Open();

                return string.Format("SELECT {1} FROM {2} " + ((searchName.Length > 0) ? "WHERE {0}=@{0}" : ""),
                    searchName,
                    cmd.ExecuteScalar(), objectName);
            }
            catch
            {
                if (cnn.State != ConnectionState.Closed)
                    cnn.Close();

                return string.Empty;
            }
            finally
            {
                cnn.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetObjectColumns(string objectName, Dictionary<string, string> columns)
        {
            var cnn = new SqlConnection(conn());

            var cmd = new SqlCommand(@"  SELECT s.Name
	                                     FROM sys.columns s
	                                     INNER JOIN sys.tables t ON s.object_id = t.object_id
		                                 LEFT JOIN sys.types tt ON s.user_type_id = tt.user_type_id
	                                     WHERE t.name = @objectName", cnn);
            cmd.Parameters.AddWithValue("@objectName", objectName);
            cmd.CommandType = CommandType.Text;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Dictionary<string, string> returnList = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> k in columns)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Name"].ToString().Equals(k.Key, StringComparison.InvariantCultureIgnoreCase))
                    {
                        returnList.Add(k.Key, k.Value);
                        break;
                    }
                }
            }

            return returnList;
        }
    }
}