using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;  // 新增
using System.Data;
using System.IO;  // 新增
using System;
using System.Buffers;


/// <summary>
/// SQLite 数据库管理器
/// SQLite 连接、初始化、指令操作
/// </summary>
public class DatabaseManager : MonoBehaviour
{
    // 单例模式
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


    // 辅助外部使用单例
    public ConnectionState GetConnectionState()
    {
        return (dbConnection != null) ? dbConnection.State : ConnectionState.Closed;
    }



    // 保存 Unity 与 SQLite 连接的变量
    private SqliteConnection dbConnection;

    // 保存 SQLite 命令的变量
    private SqliteCommand dbCommand;

    // 保存数据读取的变量
    private SqliteDataReader dataReader;


    // 初始化方法，初始化 dbConnection，即连接数据库
    public void Initialize(string dbPath)
    {
        try
        {
            // 连接数据库，传入数据库的地址路径
            dbConnection = new SqliteConnection(new SqliteConnectionStringBuilder() { DataSource = dbPath }.ToString());
            // 打开数据库
            dbConnection.Open();
        }
        catch(Exception e)
        {
            Debug.LogError($"数据库连接异常:{e.Message}");
        }
    }


    // 执行 SQL 命令的方法，传入要执行的 SQL 指令的字符串
    public SqliteDataReader ExecuteQuery(string queryString)
    {
        // 初始化 dbCommand，创建命令对象
        dbCommand = dbConnection.CreateCommand();
        // 获取要执行的 SQL 指令的字符串
        dbCommand.CommandText = queryString;
        // 执行指令，并保存执行结果
        dataReader = dbCommand.ExecuteReader();
        // 返回执行结果
        return dataReader;
    }


    // 关闭数据库连接的方法
    public void CloseConnection()
    {
        // 销毁 dbCommand
        if(dbCommand != null)
        {
            dbCommand.Cancel();
        }
        dbCommand = null;

        // 销毁 dataReader 
        if(dataReader != null)
        {
            dataReader.Close();
        }
        dataReader = null;

        // 销毁 Connection
        if(dbConnection != null)
        {
            dbConnection.Close();
        }
        dbConnection = null;
    }


    // 读取整张数据表的方法，传入要读的表名
    public SqliteDataReader ReaderFullTable(string tableName)
    {
        // 设置 SQL 指令字符串
        string queryString = "SELECT * FROM " + tableName;
        // 返回 SQL 指令执行结果
        return ExecuteQuery(queryString);
    }


    // 向指定数据表中插入一行数据，传入表名，和新增的表数据
    public SqliteDataReader InsertValues(string tableName,string[] values)
    {
        // 获取数据表中字段数目
        int fieldCount = ReaderFullTable(tableName).FieldCount;

        // 当插入数据的长度不等于字段数目时引发异常
        if(values.Length != fieldCount)
        {
            throw new SqliteException("values.Length != fieldCount");
        }

        // 设置 SQL 指令字符串
        string queryString = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for(int i = 1; i < values.Length; i++)
        {
            queryString += "," + values[i];
        }
        queryString += " )";

        // 返回 SQL 指令执行结果
        return ExecuteQuery(queryString);
    }


    // 更新指定数据表内的数据，传入表名，要更新的字段名，更新后的字段值，条件字段关键字，操作符，条件值
    public SqliteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues,string key,string operation,string value)
    {
        // 当字段名称数和字段数值的数量不相等时引发异常
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


    // 删除指定数据表内的数据OR，需要传入表名，字段名，操作符，字段值
    public SqliteDataReader DeleteValuesOR(string tableName, string[] colNames, string[] operations, string[] colValues)
    {
        //当字段名称和字段数值不对应时引发异常
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


    // 删除指定数据表内的数据AND，需要传入表名，字段名，操作符，字段值
    public SqliteDataReader DeleteValueAND(string tableName, string[] colNames, string[] operations, string[] colValues)
    {
        // 当字段名称数和字段数值不对应时引发异常
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


    // 创建数据表
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


    // 条件查询表，需要传入表名，要查询的字段，条件字段，条件操作符，条件值
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




    // 把整张表转换为对象列表，传入表名，行数据转换函数 converter
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
                    Debug.LogError($"数据转换错误{e.Message}");
                }
            }
        }
        return resultList;
    }


    // 执行条件查询并转换为对象列表，传入 SQL 查询语句，行数据转换函数 converter
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
                    Debug.LogError($"数据转换错误：{e.Message}");
                }
            }
        }
        return resultList;
    }


    // 根据 ID 获取单个对象
    public T GetById<T>(string tableName, int id, Func<SqliteDataReader, T> converter, string idColumnName = "id")
    {
        string query = $"SELECT * FROM {tableName} WHERE {idColumnName} = @id LIMIT 1";

        try
        {
            // 创建命令对象
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = query;

                // 添加参数
                var idParam = command.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = id;
                idParam.DbType = DbType.Int32;
                command.Parameters.Add(idParam);

                // 执行查询
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        try
                        {
                            // 使用转换函数创建对象
                            return converter(reader);
                        }
                        catch(Exception e)
                        {
                            Debug.LogError($"数据转换错误：{e.Message}");
                            return default(T);
                        }
                    }
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"数据库查询错误：{e.Message}");
        }

        // 未找到记录
        return default(T);
    }


    // 根据 UID 获取单个对象
    public T GetByUId<T>(string tableName, int uid, Func<SqliteDataReader, T> converter, string uidColumnName = "uid")
    {
        string query = $"SELECT * FROM {tableName} WHERE {uidColumnName} = @uid LIMIT 1";

        try
        {
            // 创建命令对象
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = query;

                // 添加参数
                var uidParam = command.CreateParameter();
                uidParam.ParameterName = "@uid";
                uidParam.Value = uid;
                uidParam.DbType = DbType.Int32;
                command.Parameters.Add(uidParam);

                // 执行查询
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        try
                        {
                            // 使用转换函数创建对象
                            return converter(reader);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"数据转换错误：{e.Message}");
                            return default(T);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"数据库查询错误：{e.Message}");
        }

        // 未找到记录
        return default(T);
    }


    // 关闭当前活动的 DataReader
    public void CloseReader()
    {
        if(dataReader != null)
        {
            dataReader.Close();
            dataReader = null;
        }
    }
}
