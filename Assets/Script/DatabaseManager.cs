using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;  // ����
using System.Data;
using System.IO;  // ����
using System;
using System.Buffers;


/// <summary>
/// SQLite ���ݿ������
/// SQLite ���ӡ���ʼ����ָ�����
/// </summary>
public class DatabaseManager : MonoBehaviour
{
    // ����ģʽ
    private static DatabaseManager _instance;
    public static DatabaseManager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject dbManager = new GameObject("DatabaseManager");
                _instance = dbManager.AddComponent<DatabaseManager>();
            }
            return _instance;
        }
    }


    // �����ⲿʹ�õ���
    public ConnectionState GetConnectionState()
    {
        return (dbConnection != null) ? dbConnection.State : ConnectionState.Closed;
    }



    // ���� Unity �� SQLite ���ӵı���
    private SqliteConnection dbConnection;

    // ���� SQLite ����ı���
    private SqliteCommand dbCommand;

    // �������ݶ�ȡ�ı���
    private SqliteDataReader dataReader;


    // ��ʼ����������ʼ�� dbConnection�����������ݿ�
    public void Initialize(string dbPath)
    {
        try
        {
            // �������ݿ⣬�������ݿ�ĵ�ַ·��
            dbConnection = new SqliteConnection(new SqliteConnectionStringBuilder() { DataSource = dbPath }.ToString());
            // �����ݿ�
            dbConnection.Open();
        }
        catch(Exception e)
        {
            Debug.LogError($"���ݿ������쳣:{e.Message}");
        }
    }


    // ִ�� SQL ����ķ���������Ҫִ�е� SQL ָ����ַ���
    public SqliteDataReader ExecuteQuery(string queryString)
    {
        // ��ʼ�� dbCommand�������������
        dbCommand = dbConnection.CreateCommand();
        // ��ȡҪִ�е� SQL ָ����ַ���
        dbCommand.CommandText = queryString;
        // ִ��ָ�������ִ�н��
        dataReader = dbCommand.ExecuteReader();
        // ����ִ�н��
        return dataReader;
    }


    // �ر����ݿ����ӵķ���
    public void CloseConnection()
    {
        // ���� dbCommand
        if(dbCommand != null)
        {
            dbCommand.Cancel();
        }
        dbCommand = null;

        // ���� dataReader 
        if(dataReader != null)
        {
            dataReader.Close();
        }
        dataReader = null;

        // ���� Connection
        if(dbConnection != null)
        {
            dbConnection.Close();
        }
        dbConnection = null;
    }


    // ��ȡ�������ݱ�ķ���������Ҫ���ı���
    public SqliteDataReader ReaderFullTable(string tableName)
    {
        // ���� SQL ָ���ַ���
        string queryString = "SELECT * FROM " + tableName;
        // ���� SQL ָ��ִ�н��
        return ExecuteQuery(queryString);
    }


    // ��ָ�����ݱ��в���һ�����ݣ�����������������ı�����
    public SqliteDataReader InsertValues(string tableName,string[] values)
    {
        // ��ȡ���ݱ����ֶ���Ŀ
        int fieldCount = ReaderFullTable(tableName).FieldCount;

        // ���������ݵĳ��Ȳ������ֶ���Ŀʱ�����쳣
        if(values.Length != fieldCount)
        {
            throw new SqliteException("values.Length != fieldCount");
        }

        // ���� SQL ָ���ַ���
        string queryString = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for(int i = 1; i < values.Length; i++)
        {
            queryString += "," + values[i];
        }
        queryString += " )";

        // ���� SQL ָ��ִ�н��
        return ExecuteQuery(queryString);
    }


    // ����ָ�����ݱ��ڵ����ݣ����������Ҫ���µ��ֶ��������º���ֶ�ֵ�������ֶιؼ��֣�������������ֵ
    public SqliteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues,string key,string operation,string value)
    {
        // ���ֶ����������ֶ���ֵ�����������ʱ�����쳣
        if(colNames.Length != colValues.Length)
        {
            throw new SqliteException("colNames.Length != colValues.Length");
        }

        string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + colValues[0];
        for(int i = 1; i < colValues.Length; i++)
        {
            queryString += ", " + colNames[i] + "=" + colValues[i];
        }
        queryString += " WHERE " + key + operation + value;
        return ExecuteQuery(queryString);
    }


    // ɾ��ָ�����ݱ��ڵ�����OR����Ҫ����������ֶ��������������ֶ�ֵ
    public SqliteDataReader DeleteValuesOR(string tableName, string[] colNames, string[] operations, string[] colValues)
    {
        //���ֶ����ƺ��ֶ���ֵ����Ӧʱ�����쳣
        if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
        {
            throw new SqliteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
        }

        string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
        for (int i = 1; i < colValues.Length; i++)
        {
            queryString += "OR " + colNames[i] + operations[i] + colValues[i];
        }
        return ExecuteQuery(queryString);
    }


    // ɾ��ָ�����ݱ��ڵ�����AND����Ҫ����������ֶ��������������ֶ�ֵ
    public SqliteDataReader DeleteValueAND(string tableName, string[] colNames, string[] operations, string[] colValues)
    {
        // ���ֶ����������ֶ���ֵ����Ӧʱ�����쳣
        if(colNames.Length!= colValues.Length || operations.Length!=colNames.Length || operations.Length!= colValues.Length)
        {
            throw new SqliteException("colNames.Length!= colValues.Length || operations.Length!=colNames.Length || operations.Length!= colValues.Length");
        }

        string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
        for (int i = 1; i < colValues.Length; i++)
        {
            queryString += " AND " + colNames[i] + operations[i] + colValues[i];
        }
        return ExecuteQuery(queryString);
    }


    // �������ݱ�
    public SqliteDataReader CreateTable(string tableName, string[] colNames, string[] colTypes)
    {
        string queryString = "CREATE TABLE " + tableName + "( " + colNames[0] + " " + colTypes[0];
        for(int i = 1; i < colNames.Length; i++)
        {
            queryString += ", " + colNames[i] + " " + colTypes[i];
        }
        queryString += " )";
        return ExecuteQuery(queryString);
    }


    // ������ѯ����Ҫ���������Ҫ��ѯ���ֶΣ������ֶΣ�����������������ֵ
    public SqliteDataReader ReadTable(string tableName, string[] items, string[] colNames, string[] operations, int[] colValues)
    {
        string queryString = "SELECT " + items[0];
        for(int i = 1; i < items.Length; i++)
        {
            queryString += ", " + items[i];
        }
        queryString += " FROM " + tableName + " WHERE " + colNames[0] + " " + operations[0] + " " + colValues[0];
        for(int i = 0; i < colNames.Length; i++)
        {
            queryString += " AND " + colNames[i] + " " + operations[i] + " " + colValues[0] + " ";
        }
        return ExecuteQuery(queryString);
    }




    // �����ű�ת��Ϊ�����б����������������ת������ converter
    public List<T> GetTableAsList<T>(string tableName,Func<SqliteDataReader, T> converter)
    {
        List<T> resultList = new List<T>();
        using (SqliteDataReader reader = ReaderFullTable(tableName))
        {
            while (reader.Read())
            {
                try
                {
                    resultList.Add(converter(reader));
                }
                catch (Exception e)
                {
                    Debug.LogError($"����ת������{e.Message}");
                }
            }
        }
        return resultList;
    }


    // ִ��������ѯ��ת��Ϊ�����б����� SQL ��ѯ��䣬������ת������ converter
    public List<T> ExecuteQueryAsList<T>(string query, Func<SqliteDataReader, T> converter)
    {
        List<T> resultList = new List<T>();

        using (SqliteDataReader reader = ExecuteQuery(query))
        {
            while (reader.Read())
            {
                try
                {
                    resultList.Add(converter(reader));
                }
                catch(Exception e)
                {
                    Debug.LogError($"����ת������{e.Message}");
                }
            }
        }
        return resultList;
    }


    // ���� ID ��ȡ��������
    public T GetById<T>(string tableName, int id, Func<SqliteDataReader, T> converter, string idColumnName = "id")
    {
        string query = $"SELECT * FROM {tableName} WHERE {idColumnName} = @id LIMIT 1";

        try
        {
            // �����������
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = query;

                // ��Ӳ���
                var idParam = command.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = id;
                idParam.DbType = DbType.Int32;
                command.Parameters.Add(idParam);

                // ִ�в�ѯ
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        try
                        {
                            // ʹ��ת��������������
                            return converter(reader);
                        }
                        catch(Exception e)
                        {
                            Debug.LogError($"����ת������{e.Message}");
                            return default(T);
                        }
                    }
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"���ݿ��ѯ����{e.Message}");
        }

        // δ�ҵ���¼
        return default(T);
    }


    // ���� UID ��ȡ��������
    public T GetByUId<T>(string tableName, int uid, Func<SqliteDataReader, T> converter, string uidColumnName = "uid")
    {
        string query = $"SELECT * FROM {tableName} WHERE {uidColumnName} = @uid LIMIT 1";

        try
        {
            // �����������
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = query;

                // ��Ӳ���
                var uidParam = command.CreateParameter();
                uidParam.ParameterName = "@uid";
                uidParam.Value = uid;
                uidParam.DbType = DbType.Int32;
                command.Parameters.Add(uidParam);

                // ִ�в�ѯ
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        try
                        {
                            // ʹ��ת��������������
                            return converter(reader);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"����ת������{e.Message}");
                            return default(T);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"���ݿ��ѯ����{e.Message}");
        }

        // δ�ҵ���¼
        return default(T);
    }


    // �رյ�ǰ��� DataReader
    public void CloseReader()
    {
        if(dataReader != null)
        {
            dataReader.Close();
            dataReader = null;
        }
    }
}
