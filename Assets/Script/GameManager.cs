using JetBrains.Annotations;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 单例模式
    private static GameManager _instance;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }


    void Start()
    {
        // 主动打开背包界面
        UIManager.Instance.OpenPanel(UIConst.PackagePanel);



        // 测试
        // 连接数据库
        Connect();
        // 读取 Items 表中第一个数据
        GetPackageItemById(1);
        // 读取 PackageLocalData 表中的 第一个数据
        GetPackageLocalDataById(1);

    }


    // 连接数据库的方法
    public static void Connect()
    {
        // 设置数据库路径
        string db_Path = Application.dataPath + "/" + "SQLDB.db";
        Debug.Log("数据库路径：" + db_Path);

        // 使用单例模式获取 DatabaseManager
        DatabaseManager dbManager = DatabaseManager.Instance;
        dbManager.Initialize(db_Path);  // 连接数据库
        Debug.Log("数据库连接成功");
    }


    // 根据 id 取到 Items 表格指定数据的方法
    public static void GetPackageItemById(int id)
    {
        // 获取数据库管理器
        DatabaseManager dbManager = DatabaseManager.Instance;

        // 检查数据库是否已连接
        if (dbManager == null || dbManager.GetConnectionState() != System.Data.ConnectionState.Open)
        {
            Debug.LogError("数据库未连接，请先执行连接数据库");
            return;
        }

        // 需要查询的字段
        string[] items = new string[]
        {
            "id",
            "name",
            "type",
            "max_stack",
            "num",
            "description",
            "icon_path"
        };
        string[] colNames = new string[]
        {
            "id",
        };
        string[] operations = new string[]
        {
            "="
        };

        int[] ID = { id };

        // 读取 Items 表
        string tableName = "Items";
        SqliteDataReader reader = dbManager.ReadTable(tableName, items, colNames, operations, ID);

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

        // 逐行读取数据
        int rowCount = 0;
        while (reader.Read())
        {
            rowCount++;
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


    // 根据 uid 取到 PacakageLocalData 表格指定数据的方法
    public static void GetPackageLocalDataById(int uid)
    {
        // 获取数据库管理器
        DatabaseManager dbManager = DatabaseManager.Instance;

        // 检查数据库是否已连接
        if (dbManager == null || dbManager.GetConnectionState() != System.Data.ConnectionState.Open)
        {
            Debug.LogError("数据库未连接，请先执行连接数据库");
            return;
        }

        // 需要查询的字段
        string[] items = new string[]
        {
            "uid",
            "id",
            "num",
        };
        string[] colNames = new string[]
        {
            "uid",
        };
        string[] operations = new string[]
        {
            "="
        };

        int[] UID = { uid };

        // 读取 PackageLocalData 表
        string tableName = "PackageLocalData";
        SqliteDataReader reader = dbManager.ReadTable(tableName, items, colNames, operations, UID);

        if (reader == null)
        {
            Debug.LogError("读取表失败");
            return;
        }
        while (reader.Read())
        {
            string Data = "";
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Data += reader[i].ToString();
            }
            Debug.Log(Data);
        }

        // 关闭读取器
        reader.Close();
        Debug.Log($"成功读取表：{tableName}");
    }


}
