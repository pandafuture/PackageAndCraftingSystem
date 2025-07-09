using JetBrains.Annotations;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ����ģʽ
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
        // �����򿪱�������
        UIManager.Instance.OpenPanel(UIConst.PackagePanel);



        // ����
        // �������ݿ�
        Connect();
        // ��ȡ Items ���е�һ������
        GetPackageItemById(1);
        // ��ȡ PackageLocalData ���е� ��һ������
        GetPackageLocalDataById(1);

    }


    // �������ݿ�ķ���
    public static void Connect()
    {
        // �������ݿ�·��
        string db_Path = Application.dataPath + "/" + "SQLDB.db";
        Debug.Log("���ݿ�·����" + db_Path);

        // ʹ�õ���ģʽ��ȡ DatabaseManager
        DatabaseManager dbManager = DatabaseManager.Instance;
        dbManager.Initialize(db_Path);  // �������ݿ�
        Debug.Log("���ݿ����ӳɹ�");
    }


    // ���� id ȡ�� Items ���ָ�����ݵķ���
    public static void GetPackageItemById(int id)
    {
        // ��ȡ���ݿ������
        DatabaseManager dbManager = DatabaseManager.Instance;

        // ������ݿ��Ƿ�������
        if (dbManager == null || dbManager.GetConnectionState() != System.Data.ConnectionState.Open)
        {
            Debug.LogError("���ݿ�δ���ӣ�����ִ���������ݿ�");
            return;
        }

        // ��Ҫ��ѯ���ֶ�
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

        // ��ȡ Items ��
        string tableName = "Items";
        SqliteDataReader reader = dbManager.ReadTable(tableName, items, colNames, operations, ID);

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

        // ���ж�ȡ����
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

        // �رն�ȡ��
        reader.Close();
        Debug.Log($"�ɹ���ȡ��{tableName}");
    }


    // ���� uid ȡ�� PacakageLocalData ���ָ�����ݵķ���
    public static void GetPackageLocalDataById(int uid)
    {
        // ��ȡ���ݿ������
        DatabaseManager dbManager = DatabaseManager.Instance;

        // ������ݿ��Ƿ�������
        if (dbManager == null || dbManager.GetConnectionState() != System.Data.ConnectionState.Open)
        {
            Debug.LogError("���ݿ�δ���ӣ�����ִ���������ݿ�");
            return;
        }

        // ��Ҫ��ѯ���ֶ�
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

        // ��ȡ PackageLocalData ��
        string tableName = "PackageLocalData";
        SqliteDataReader reader = dbManager.ReadTable(tableName, items, colNames, operations, UID);

        if (reader == null)
        {
            Debug.LogError("��ȡ��ʧ��");
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

        // �رն�ȡ��
        reader.Close();
        Debug.Log($"�ɹ���ȡ��{tableName}");
    }


}
