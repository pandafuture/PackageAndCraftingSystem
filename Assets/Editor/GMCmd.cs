using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mono.Data.Sqlite;  // ����
using System.Data;
using System.IO;  // ����
using System;
using System.Buffers;
using Codice.Client.Common.GameUI;

public class GMCmd : MonoBehaviour
{
    [MenuItem("CMCmd/���ݿ����")]
    public static void Connect()
    {
        // �������ݿ�·��
        string db_Path = Application.dataPath + "/" + "SQLDB.db";
        Debug.Log(db_Path);

        // ������ʱ GameObject ����� DatabaseManager ���
        GameObject dbObject = new GameObject("TempDatabaseManager");
        DatabaseManager db_sql = dbObject.AddComponent<DatabaseManager>();

        db_sql.Initialize(db_Path);

        // ʹ�ú�������ʱ����
        //UnityEngine.Object.DestroyImmediate(dbObject);

        SqliteDataReader reader = db_sql.ReaderFullTable("Items");
        while (reader.Read())
        {
            // ��ȡ
            Debug.Log(reader.GetInt32(reader.GetOrdinal("id")) + reader.GetString(reader.GetOrdinal("name")));
        }
    }



    [MenuItem("CMCmd/�򿪱���������")]
    public static void OpenPackagePanel()
    {
        UIManager.Instance.OpenPanel(UIConst.PackagePanel);  // �򿪱�������
    }
}
