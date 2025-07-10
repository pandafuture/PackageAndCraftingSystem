using JetBrains.Annotations;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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
        //UIManager.Instance.OpenPanel(UIConst.PackagePanel);



        // ����
        // �������ݿ�
        Connect();
        // ��ȡ Items ���е�һ������
        //GetPackageItemById(1);
        // ��ȡ PackageLocalData ���е� ��һ������
        //GetPackageLocalDataById(1);

        // ���ؾ�̬���� Items ��
        //GetPackageTable();
        // ���ض�̬���� PackageLocalItem ��
        //GetPackageLocalData();
        //PackageLocalItem package = GetPackageLocalDataByUId(1);
        //if (package != null)
        //{
        //    Debug.Log($"��һ����̬���ݻ�ȡ�ɹ���{package.ID}");
        //}
        //else
        //{
        //    Debug.LogWarning($"δ�ҵ� uid Ϊ 1 �ľ�̬����");
        //}

        //List<PackageTableItem> packagetypeitems = GetPackageTableByType(1);
        //foreach (PackageTableItem packageItem in packagetypeitems)
        //{
        //    Debug.Log($"id��{packageItem.id}  name��{packageItem.name}  description��{packageItem.description}");
        //}

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


    // ���ؾ�̬���� Items ��ķ���
    public List<PackageTableItem> GetPackageTable()
    {
        string tableName = "Items";

        List<PackageTableItem> packageTable = DatabaseManager.Instance.GetTableAsList(tableName, reader =>
        {
            return new PackageTableItem
            {
                id = reader.GetInt32(reader.GetOrdinal("id")),
                name = reader.GetString(reader.GetOrdinal("name")),
                type = reader.GetInt32(reader.GetOrdinal("type")),
                max_stack = reader.GetInt32(reader.GetOrdinal("max_stack")),
                description = reader.GetString(reader.GetOrdinal("description")),
                icon_path = reader.GetString(reader.GetOrdinal("icon_path")),
            };
        });

        Debug.Log($"������ {packageTable.Count} �� Items ������");
        foreach(PackageTableItem packageItem in packageTable)
        {
            Debug.Log($"id��{packageItem.id}  name��{packageItem.name}  type��{packageItem.type}  max_stack��{packageItem.max_stack}  description��{packageItem.description}  icon_path��{packageItem.icon_path}");
        }
        return packageTable;
    }


    // ���ض�̬���� PackageLocalItem ��ķ���
    public List<PackageLocalItem> GetPackageLocalData()
    {
        string tableName = "PackageLocalData";

        List<PackageLocalItem> packageLocalItem = DatabaseManager.Instance.GetTableAsList(tableName, reader =>
        {
            return new PackageLocalItem
            {
                UID = reader.GetInt32(reader.GetOrdinal("uid")),
                ID = reader.GetInt32(reader.GetOrdinal("id")),
                NUM = reader.GetInt32(reader.GetOrdinal("num")),
            };
        });

        Debug.Log($"������ {packageLocalItem.Count} �� PackageLocalItem ������");
        foreach(PackageLocalItem packageItem in packageLocalItem)
        {
            Debug.Log($"UID��{packageItem.UID}  ID��{packageItem.ID}  NUM��{packageItem.NUM}");
        }
        return packageLocalItem;
    }


    // ���� id ȡ�� Items ���ָ�����ݶ���ķ���
    public PackageTableItem GetPackageItemById(int id)
    {
        return DatabaseManager.Instance.GetById<PackageTableItem>(
            tableName: "Items",
            id: id,
            converter: reader => new PackageTableItem
            {
                id = reader.GetInt32(reader.GetOrdinal("id")),
                name = reader.GetString(reader.GetOrdinal("name")),
                type = reader.GetInt32(reader.GetOrdinal("type")),
                max_stack = reader.GetInt32(reader.GetOrdinal("max_stack")),
                description = reader.GetString(reader.GetOrdinal("description")),
                icon_path = reader.GetString(reader.GetOrdinal("icon_path")),
            }
        );
    }


    // ���� uid ȡ�� PacakageLocalData ���ָ�����ݶ���ķ���
    public PackageLocalItem GetPackageLocalDataByUId(int uid)
    {
        return DatabaseManager.Instance.GetByUId<PackageLocalItem>(
            tableName: "PackageLocalData",
            uid: uid,
            converter: reader => new PackageLocalItem
            {
                UID = reader.GetInt32(reader.GetOrdinal("uid")),
                ID = reader.GetInt32(reader.GetOrdinal("id")),
                NUM = reader.GetInt32(reader.GetOrdinal("num")),
            }
        );
    }


    // ��ȡ������Ʒ����������ķ���
    public List<PackageLocalItem> GetSortPackageLocalData()
    {
        List<PackageLocalItem> localItems = GetPackageLocalData();
        localItems.Sort(new PackageItemComparer());
        return localItems;
    }


    // ���ݴ洢��Ʒ��������ȡ��ͬ�����������ݵķ���   ���ϣ�1    ������2    ��ҩ��3
    public List<PackageTableItem> GetPackageTableByType(int type)
    {
        List<PackageTableItem> packageItems = new List<PackageTableItem>();
        foreach(PackageTableItem packageItem in GetPackageTable())
        {
            if(packageItem.type == type)
            {
                packageItems.Add(packageItem);
            }
        }
        return packageItems;
    }



}


// �������
public class PackageItemComparer : IComparer<PackageLocalItem>
{
    public int Compare(PackageLocalItem a, PackageLocalItem b)
    {
        PackageTableItem x = GameManager.Instance.GetPackageItemById(a.ID);
        PackageTableItem y = GameManager.Instance.GetPackageItemById(b.ID);
        // ������Ʒ����
        int typeComparison = y.type.CompareTo(x.type);
        if(typeComparison == 0)
        {
            return b.ID.CompareTo(a.ID);
        }
        return typeComparison;
    }
}


// ���徲̬���ݣ����ڼ���
public class PackageTableItem
{
    public int id {  get; set; }
    public string name {  get; set; }
    public int type {  get; set; }
    public int max_stack { get; set; }
    public string description {  get; set; }
    public string icon_path { get; set; }
}


// ���屾�ر�����Ʒģ�ͣ����ڼ���
public class PackageLocalItem
{
    public int UID { get; set; }
    public int ID { get; set; }
    public int NUM { get; set; }
}


// �洢��Ʒ���͵ĳ�����
public class GameConst
{
    // �������ͳ���
    public const int PackageTypeMaterial = 1;

    // �������ͳ���
    public const int PackageTypeWeapom = 2;

    // ��ҩ���ͳ���
    public const int PackageTypeAmmo = 3;
}