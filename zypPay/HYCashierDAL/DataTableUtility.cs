using System;

using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.ComponentModel;

namespace HYCashierDAL
{
    //public static class DataTableUtility
    //{
    //    public static List<T> ConvertToList<T>(this DataTable table) where T : new()
    //    {
    //        Type t = typeof(T);

    //        // Create a list of the entities we want to return
    //        List<T> returnObject = new List<T>();

    //        // Iterate through the DataTable's rows
    //        foreach (DataRow dr in table.Rows)
    //        {
    //            // Convert each row into an entity object and add to the list
    //            T newRow = dr.ConvertToEntity<T>();
    //            returnObject.Add(newRow);
    //        }

    //        // Return the finished list
    //        return returnObject;
    //    }

    //    public static T ConvertToEntity<T>(this DataRow tableRow) where T : new()
    //    {
    //        // Create a new type of the entity I want
    //        Type t = typeof(T);
    //        T returnObject = new T();

    //        foreach (DataColumn col in tableRow.Table.Columns)
    //        {
    //            string colName = col.ColumnName;

    //            // Look for the object's property with the columns name, ignore case
    //            PropertyInfo pInfo = t.GetProperty(colName.ToLower(),
    //                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

    //            // did we find the property ?
    //            if (pInfo != null)
    //            {
    //                object val = tableRow[colName];

    //                // is this a Nullable<> type
    //                bool IsNullable = (Nullable.GetUnderlyingType(pInfo.PropertyType) != null);
    //                if (IsNullable)
    //                {
    //                    if (val is System.DBNull)
    //                    {
    //                        val = null;
    //                    }
    //                    else
    //                    {
    //                        // Convert the db type into the T we have in our Nullable<T> type
    //                        val = Convert.ChangeType
    //                (val, Nullable.GetUnderlyingType(pInfo.PropertyType));
    //                    }
    //                }
    //                else
    //                {
    //                    // Convert the db type into the type of the property in our entity
    //                    val = Convert.ChangeType(val, pInfo.PropertyType);
    //                }
    //                // Set the value of the property with the value from the db
    //                pInfo.SetValue(returnObject, val, null);
    //            }
    //        }

    //        // return the entity object with values
    //        return returnObject;
    //    }
    //}
    public static class DataTableUtility
    {
        // <summary>
        /// DataTable To IList<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> ConvertToList<T>(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return null;
            IList<T> list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T obj = row.ToEntity<T>();
                list.Add(obj);
            }
            return list;
        }

        /// <summary>
        /// DataRow To T
        /// </summary>
        public static T ToEntity<T>(this DataRow row)
        {
            Type objType = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in row.Table.Columns)
            {
                PropertyInfo property =
                objType.GetProperty(column.ColumnName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property == null || !property.CanWrite)
                {
                    continue;
                }
                object value = row[column.ColumnName];
                if (value == DBNull.Value) value = null;

                property.SetValue(obj, value, null);

            }
            return obj;
        }


        /// <summary>
        /// List To DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            try
            {
                Type objType = typeof(T);
                DataTable dataTable = new DataTable(objType.Name);
                if (list != null ? list.Count > 0 : false)
                {
                    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(objType);
                    foreach (PropertyDescriptor property in properties)
                    {
                        Type propertyType = property.PropertyType;

                        //nullables must use underlying types
                        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            propertyType = Nullable.GetUnderlyingType(propertyType);
                        //enums also need special treatment
                        if (propertyType.IsEnum)
                            propertyType = Enum.GetUnderlyingType(propertyType); //probably Int32

                        dataTable.Columns.Add(property.Name, propertyType);
                    }

                    foreach (T li in list)
                    {
                        DataRow row = dataTable.NewRow();
                        foreach (PropertyDescriptor property1 in properties)
                        {
                            row[property1.Name] = property1.GetValue(li) ?? DBNull.Value; //can't use null
                        }
                        dataTable.Rows.Add(row);

                    }
                }
                return dataTable;
            }
            catch
            {
                return null;
            }
        }
    }
}
