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

public class GMCmd : MonoBehaviour
{
    [MenuItem("CMCmd/数据库测试")]
    public static void Connect()
    {
        // 设置数据库路径
        string db_Path = Application.dataPath + "/" + "SQLDB.db";
        Debug.Log(db_Path);

        // 创建临时 GameObject 并添加 DatabaseManager 组件
        GameObject dbObject = new GameObject("TempDatabaseManager");
        DatabaseManager db_sql = dbObject.AddComponent<DatabaseManager>();

        db_sql.Initialize(db_Path);

        // 使用后销毁临时对象
        //UnityEngine.Object.DestroyImmediate(dbObject);

        SqliteDataReader reader = db_sql.ReaderFullTable("Items");
        while (reader.Read())
        {
            // 读取
            Debug.Log(reader.GetInt32(reader.GetOrdinal("id")) + reader.GetString(reader.GetOrdinal("name")));
        }
    }



    [MenuItem("CMCmd/打开背包主界面")]
    public static void OpenPackagePanel()
    {
        UIManager.Instance.OpenPanel(UIConst.PackagePanel);  // 打开背包界面
    }
}
