using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mono.Data.Sqlite;  // 新增
using System.Data;
using System.IO;  // 新增
using System;
using System.Buffers;
using Codice.Client.Common.GameUI;
using Unity.VisualScripting;

public class GMCmd : MonoBehaviour
{
    [MenuItem("CMCmd/数据库连接")]
    public static void Connect()
    {
        // 设置数据库路径
        string db_Path = Application.dataPath + "/" + "SQLDB.db";
        Debug.Log("数据库路径：" + db_Path);

        // 使用单例模式获取 DatabaseManager
        DatabaseManager dbManager = DatabaseManager.Instance;
        dbManager.Initialize(db_Path);  // 连接数据库
        Debug.Log("数据库连接成功");

        //// 创建临时 GameObject 并添加 DatabaseManager 组件
        //GameObject dbObject = new GameObject("TempDatabaseManager");
        //DatabaseManager db_sql = dbObject.AddComponent<DatabaseManager>();

        //db_sql.Initialize(db_Path);

        //// 使用后销毁临时对象
        ////UnityEngine.Object.DestroyImmediate(dbObject);

        //SqliteDataReader reader = db_sql.ReaderFullTable("Items");
        //while (reader.Read())
        //{
        //    // 读取
        //    Debug.Log(reader.GetInt32(reader.GetOrdinal("id")) + reader.GetString(reader.GetOrdinal("name")));
        //}
    }

    [MenuItem("CMCmd/读取Items表")]
    public static void ReadItems()
    {
        // 获取数据库管理器
        DatabaseManager dbManager = DatabaseManager.Instance;

        // 检查数据库是否已连接
        if (dbManager == null || dbManager.GetConnectionState() != System.Data.ConnectionState.Open)
        {
            Debug.LogError("数据库未连接，请先执行连接数据库");
            return;
        }

        // 读取 Item 表
        string tableName = "Items";
        SqliteDataReader reader = dbManager.ReaderFullTable(tableName);

        if(reader == null)
        {
            Debug.LogError("读取表失败");
            return;
        }

        // 打印表头
        string header = "| ";
        for(int i = 0; i < reader.FieldCount; i++)
        {
            header += reader.GetName(i) + " | ";
        }
        Debug.Log(header);

        // 打印每一行数据
        while (reader.Read())
        {
            string rowData = "| ";
            for(int i = 0; i < reader.FieldCount; i++)
            {
                rowData += reader[i].ToString() + " | ";
            }
            Debug.Log(rowData);
        }

        // 关闭读取器
        reader.Close();
        Debug.Log($"成功读取表：{tableName}");
    }


    [MenuItem("CMCmd/读取Recipes表")]
    public static void ReadRecipes()
    {
        // 获取数据库管理器
        DatabaseManager dbManager = DatabaseManager.Instance;

        // 检查数据库是否已连接
        if (dbManager == null || dbManager.GetConnectionState() != System.Data.ConnectionState.Open)
        {
            Debug.LogError("数据库未连接，请先执行连接数据库");
            return;
        }

        // 读取 Item 表
        string tableName = "Recipes";
        SqliteDataReader reader = dbManager.ReaderFullTable(tableName);

        if (reader == null)
        {
            Debug.LogError("读取表失败");
            return;
        }

        // 打印表头
        string header = "| ";
        for (int i = 0; i < reader.FieldCount; i++)
        {
            header += reader.GetName(i) + " | ";
        }
        Debug.Log(header);

        // 打印每一行数据
        while (reader.Read())
        {
            string rowData = "| ";
            for (int i = 0; i < reader.FieldCount; i++)
            {
                rowData += reader[i].ToString() + " | ";
            }
            Debug.Log(rowData);
        }

        // 关闭读取器
        reader.Close();
        Debug.Log($"成功读取表：{tableName}");
    }


    [MenuItem("CMCmd/读取PackageLocalData表")]
    public static void ReadPackageLocalData()
    {
        // 获取数据库管理器
        DatabaseManager dbManager = DatabaseManager.Instance;

        // 检查数据库是否已连接
        if (dbManager == null || dbManager.GetConnectionState() != System.Data.ConnectionState.Open)
        {
            Debug.LogError("数据库未连接，请先执行连接数据库");
            return;
        }

        // 读取 Item 表
        string tableName = "PackageLocalData";
        SqliteDataReader reader = dbManager.ReaderFullTable(tableName);

        if (reader == null)
        {
            Debug.LogError("读取表失败");
            return;
        }

        // 打印表头
        string header = "| ";
        for (int i = 0; i < reader.FieldCount; i++)
        {
            header += reader.GetName(i) + " | ";
        }
        Debug.Log(header);

        // 打印每一行数据
        while (reader.Read())
        {
            string rowData = "| ";
            for (int i = 0; i < reader.FieldCount; i++)
            {
                rowData += reader[i].ToString() + " | ";
            }
            Debug.Log(rowData);
        }

        // 关闭读取器
        reader.Close();
        Debug.Log($"成功读取表：{tableName}");
    }


    [MenuItem("CMCmd/创建背包测试数据")]
    public static void CreateLocalPackageData()
    {
        // 获取数据库管理器
        DatabaseManager dbManager = DatabaseManager.Instance;

        // 检查数据库是否已连接
        if (dbManager == null || dbManager.GetConnectionState() != System.Data.ConnectionState.Open)
        {
            Debug.LogError("数据库未连接，请先执行连接数据库");
            return;
        }

        // 要插入的数据数组
        string[] insertvalues = new string[]
        {
            "1",  // uid
            "2",  // id
            "5",  // num
        };
        // 要插入的表名
        string tableName = "PackageLocalData";

        try
        {
            // 执行插入操作
            SqliteDataReader reader = dbManager.InsertValues(tableName, insertvalues);

            // 关闭 Reader 释放数据库锁
            if(reader != null)
            {
                reader.Close();
                Debug.Log("成功插入数据到 PackageLocalData表");
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"插入失败：{e.Message}");
        }

    }

    [MenuItem("CMCmd/打开背包主界面")]
    public static void OpenPackagePanel()
    {
        UIManager.Instance.OpenPanel(UIConst.PackagePanel);  // 打开背包界面
    }
}
