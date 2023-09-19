using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BarProject.App_Code
{
    public static class Converter
    {
        public static BindingList<T> GetBindingList<T>(DataTable dt) where T : class, new()
        {
            BindingList<T> list = new BindingList<T>();

            foreach (DataRow row in dt.Rows)
                list.Add(DataRowToObject<T>(row));

            return list;
        }

        public static List<T> GetList<T>(DataTable dt) where T : class, new()
        {
            List<T> list = new List<T>();

            foreach (DataRow row in dt.Rows)
                list.Add(DataRowToObject<T>(row));

            return list;
        }

        public static T DataRowToObject<T>(DataRow row) where T : class, new()
        {
            T t = new T();

            foreach (DataColumn column in row.Table.Columns)
            {
                PropertyInfo property = GetProperty(typeof(T), column.ColumnName);

                if (property != null && row[column] != DBNull.Value && row[column].ToString() != "NULL")
                {
                    property.SetValue(t, ChangeType(row[column], property.PropertyType), null);
                }
                else if (property == null) // when column name has different CaSe than property
                {
                    foreach (PropertyInfo p in typeof(T).GetProperties())
                    {
                        if (p.Name.Equals(column.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            try
                            {
                                p.SetValue(t, row[column]);
                            }
                            catch
                            {
                                //null
                            }
                        }
                    }
                }
            }
            return t;
        }

        private static PropertyInfo GetProperty(Type type, string attributeName)
        {
            PropertyInfo property = type.GetProperty(attributeName);

            if (property != null)
            {
                return property;
            }

            foreach (PropertyInfo p in type.GetProperties())
            {
                if (p.IsDefined(typeof(DisplayAttribute), false)
                    && p.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single().Name.Equals(attributeName, StringComparison.InvariantCultureIgnoreCase))

                    return p;
            }

            return null;

            //return type.GetProperties()
            //     .Where(p => p.IsDefined(typeof(DisplayAttribute), false) 
            //        && p.GetCustomAttributes(typeof(DisplayAttribute), false)
            //        .Cast<DisplayAttribute>().Single().Name.Equals(attributeName, StringComparison.InvariantCultureIgnoreCase))
            //     .FirstOrDefault();
        }

        public static object ChangeType(object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
            }

            return Convert.ChangeType(value, type);
        }

        public static T GetAttribute<T>(this MemberInfo member, bool isRequired) where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }
    }
}
