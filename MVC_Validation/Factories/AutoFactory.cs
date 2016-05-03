using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;

/// <summary>
/// AutoFactory class for MSSQL Databases
/// </summary>
/// <typeparam name="T">Any BaseModel</typeparam>
public abstract class AutoFactory<T>
{
    /// <summary>
    /// Set this ConnectionString always -> Different for each DataBase
    /// </summary>
    private string connectionString = "";
    private List<PropertyInfo> properties = new List<PropertyInfo>();

    protected List<T> allEntities = new List<T>();

    private T GetGenericType()
    {
        T result;
        return result = Activator.CreateInstance<T>();
    }

    public AutoFactory()
    {
        properties.AddRange(GetGenericType().GetType().GetProperties());
        connectionString = ConfigurationManager.ConnectionStrings["String"].ConnectionString;
    }

    public void Add(T entity)
    {
        string insertQuery = string.Format("INSERT INTO {0} (", typeof(T).Name);

        for (int i = 1; i < properties.Count; i++)
        {
            PropertyInfo property = properties[i];

            insertQuery += property.Name;
            insertQuery += (i + 1 == properties.Count ? "" : ", ");
        }

        insertQuery += ")";
        insertQuery += " VALUES (";

        for (int i = 1; i < properties.Count; i++)
        {
            PropertyInfo p = properties[i];
            insertQuery += "@" + properties[i].Name + (i + 1 == properties.Count ? "" : ", ");
        }

        insertQuery += ")";

        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        SqlCommand cmd = new SqlCommand(insertQuery, connection);

        for (int i = 0; i < properties.Count; i++)
        {
            cmd.Parameters.AddWithValue("@" + properties[i].Name, properties[i].GetValue(entity, null));
        }

        cmd.ExecuteNonQuery();

        cmd.Dispose();
        connection.Dispose();
        connection.Close();
    }

    public void Update(T entity)
    {
        string updateQuery = string.Format("UPDATE {0} SET ", typeof(T).Name);

        for (int i = 1; i < properties.Count; i++)
        {
            PropertyInfo property = properties[i];

            updateQuery += property.Name + "=@" + properties[i].Name;
            updateQuery += (i + 1 == properties.Count ? " " : ", ");
        }

        updateQuery += string.Format("WHERE {0}={1}", properties[0].Name, properties[0].GetValue(entity, null));

        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        SqlCommand cmd = new SqlCommand(updateQuery, connection);

        for (int i = 0; i < properties.Count; i++)
        {
            cmd.Parameters.AddWithValue("@" + properties[i].Name, properties[i].GetValue(entity, null));
        }

        cmd.ExecuteNonQuery();

        cmd.Dispose();
        connection.Dispose();
        connection.Close();
    }

    public void Delete(int id)
    {
        T entity = Get(id);

        string deleteQuery = string.Format("DELETE FROM {0} ", typeof(T).Name);
        deleteQuery += string.Format("WHERE {0}={1}", properties[0].Name, id);

        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        SqlCommand cmd = new SqlCommand(deleteQuery, connection);
        cmd.ExecuteNonQuery();

        cmd.Dispose();
        connection.Dispose();
        connection.Close();
    }

    public T Get(int id)
    {
        string selectQuery = string.Format("SELECT * FROM {0} WHERE {1}={2}", typeof(T).Name, properties.Find(x => x.Name.Contains("ID")).Name, id);

        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        SqlCommand cmd = new SqlCommand(selectQuery, connection);
        SqlDataReader reader = cmd.ExecuteReader();

        T result = GetGenericType();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                for (int i = 0; i < properties.Count; i++)
                {
                    if (reader[i] == DBNull.Value) continue;
                    properties[i].SetValue(result, reader[i], null);
                }
            }
        }

        cmd.Dispose();
        connection.Dispose();
        connection.Close();

        return result;
    }

    public List<T> GetAll()
    {
        List<T> result = new List<T>();
        string selectQuery = string.Format("SELECT * FROM {0}", typeof(T).Name);

        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        SqlCommand cmd = new SqlCommand(selectQuery, connection);
        SqlDataReader reader = cmd.ExecuteReader();

        T entry = default(T);

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                entry = Activator.CreateInstance<T>();
                for (int i = 0; i < properties.Count; i++)
                {
                    if (reader[i] == DBNull.Value) continue;
                    properties[i].SetValue(entry, reader[i], null);
                }
                result.Add(entry);
            }
        }

        cmd.Dispose();
        connection.Dispose();
        connection.Close();

        return result;
    }
}