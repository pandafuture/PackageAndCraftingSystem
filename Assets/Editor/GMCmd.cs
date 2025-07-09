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
using Unity.VisualScripting;

public class GMCmd : MonoBehaviour
{
    [MenuItem("CMCmd/���ݿ�����")]
    public static void Connect()
    {
        // �������ݿ�·��
        string db_Path = Application.dataPath + "/" + "SQLDB.db";
        Debug.Log("���ݿ�·����" + db_Path);

        // ʹ�õ���ģʽ��ȡ DatabaseManager
        DatabaseManager dbManager = DatabaseManager.Instance;
        dbManager.Initialize(db_Path);  // �������ݿ�
        Debug.Log("���ݿ����ӳɹ�");

        //// ������ʱ GameObject ����� DatabaseManager ���
        //GameObject dbObject = new GameObject("TempDatabaseManager");
        //DatabaseManager db_sql = dbObject.AddComponent<DatabaseManager>();

        //db_sql.Initialize(db_Path);

        //// ʹ�ú�������ʱ����
        ////UnityEngine.Object.DestroyImmediate(dbObject);

        //SqliteDataReader reader = db_sql.ReaderFullTable("Items");
        //while (reader.Read())
        //{
        //    // ��ȡ
        //    Debug.Log(reader.GetInt32(reader.GetOrdinal("id")) + reader.GetString(reader.GetOrdinal("name")));
        //}
    }

    [MenuItem("CMCmd/��ȡItems��")]
    public static void ReadItems()
    {
        // ��ȡ���ݿ������
        DatabaseManager dbManager = DatabaseManager.Instance;

        // ������ݿ��Ƿ�������
        if (dbManager == null || dbManager.GetConnectionState() != System.Data.ConnectionState.Open)
        {
            Debug.LogError("���ݿ�δ���ӣ�����ִ���������ݿ�");
            return;
        }

        // ��ȡ Item ��
        string tableName = "Items";
        SqliteDataReader reader = dbManager.ReaderFullTable(tableName);

        if(reader == null)
        {
            Debug.LogError("��ȡ��ʧ��");
            return;
        }

        // ��ӡ��ͷ
        string header = "| ";
        for(int i = 0; i < reader.FieldCount; i++)
        {
            header += reader.GetName(i) + " | ";
        }
        Debug.Log(header);

        // ��ӡÿһ������
        while (reader.Read())
        {
            string rowData = "| ";
            for(int i = 0; i < reader.FieldCount; i++)
            {
                rowData += reader[i].ToString() + " | ";
            }
            Debug.Log(rowData);
        }

        // �رն�ȡ��
        reader.Close();
        Debug.Log($"�ɹ���ȡ��{tableName}");
    }


    [MenuItem("CMCmd/��ȡRecipes��")]
    public static void ReadRecipes()
    {
        // ��ȡ���ݿ������
        DatabaseManager dbManager = DatabaseManager.Instance;

        // ������ݿ��Ƿ�������
        if (dbManager == null || dbManager.GetConnectionState() != System.Data.ConnectionState.Open)
        {
            Debug.LogError("���ݿ�δ���ӣ�����ִ���������ݿ�");
            return;
        }

        // ��ȡ Item ��
        string tableName = "Recipes";
        SqliteDataReader reader = dbManager.ReaderFullTable(tableName);

        if (reader == null)
        {
            Debug.LogError("��ȡ��ʧ��");
            return;
        }

        // ��ӡ��ͷ
        string header = "| ";
        for (int i = 0; i < reader.FieldCount; i++)
        {
            header += reader.GetName(i) + " | ";
        }
        Debug.Log(header);

        // ��ӡÿһ������
        while (reader.Read())
        {
            string rowData = "| ";
            for (int i = 0; i < reader.FieldCount; i++)
            {
                rowData += reader[i].ToString() + " | ";
            }
            Debug.Log(rowData);
        }

        // �رն�ȡ��
        reader.Close();
        Debug.Log($"�ɹ���ȡ��{tableName}");
    }


    [MenuItem("CMCmd/��ȡPackageLocalData��")]
    public static void ReadPackageLocalData()
    {
        // ��ȡ���ݿ������
        DatabaseManager dbManager = DatabaseManager.Instance;

        // ������ݿ��Ƿ�������
        if (dbManager == null || dbManager.GetConnectionState() != System.Data.ConnectionState.Open)
        {
            Debug.LogError("���ݿ�δ���ӣ�����ִ���������ݿ�");
            return;
        }

        // ��ȡ Item ��
        string tableName = "PackageLocalData";
        SqliteDataReader reader = dbManager.ReaderFullTable(tableName);

        if (reader == null)
        {
            Debug.LogError("��ȡ��ʧ��");
            return;
        }

        // ��ӡ��ͷ
        string header = "| ";
        for (int i = 0; i < reader.FieldCount; i++)
        {
            header += reader.GetName(i) + " | ";
        }
        Debug.Log(header);

        // ��ӡÿһ������
        while (reader.Read())
        {
            string rowData = "| ";
            for (int i = 0; i < reader.FieldCount; i++)
            {
                rowData += reader[i].ToString() + " | ";
            }
            Debug.Log(rowData);
        }

        // �رն�ȡ��
        reader.Close();
        Debug.Log($"�ɹ���ȡ��{tableName}");
    }


    [MenuItem("CMCmd/����������������")]
    public static void CreateLocalPackageData()
    {
        // ��ȡ���ݿ������
        DatabaseManager dbManager = DatabaseManager.Instance;

        // ������ݿ��Ƿ�������
        if (dbManager == null || dbManager.GetConnectionState() != System.Data.ConnectionState.Open)
        {
            Debug.LogError("���ݿ�δ���ӣ�����ִ���������ݿ�");
            return;
        }

        // Ҫ�������������
        string[] insertvalues = new string[]
        {
            "1",  // uid
            "2",  // id
            "5",  // num
        };
        // Ҫ����ı���
        string tableName = "PackageLocalData";

        try
        {
            // ִ�в������
            SqliteDataReader reader = dbManager.InsertValues(tableName, insertvalues);

            // �ر� Reader �ͷ����ݿ���
            if(reader != null)
            {
                reader.Close();
                Debug.Log("�ɹ��������ݵ� PackageLocalData��");
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"����ʧ�ܣ�{e.Message}");
        }

    }

    [MenuItem("CMCmd/�򿪱���������")]
    public static void OpenPackagePanel()
    {
        UIManager.Instance.OpenPanel(UIConst.PackagePanel);  // �򿪱�������
    }
}
