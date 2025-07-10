using JetBrains.Annotations;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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
        //UIManager.Instance.OpenPanel(UIConst.PackagePanel);



        // 测试
        // 连接数据库
        Connect();
        // 读取 Items 表中第一个数据
        //GetPackageItemById(1);
        // 读取 PackageLocalData 表中的 第一个数据
        //GetPackageLocalDataById(1);

        // 加载静态数据 Items 表
        //GetPackageTable();
        // 加载动态数据 PackageLocalItem 表
        //GetPackageLocalData();
        //PackageLocalItem package = GetPackageLocalDataByUId(1);
        //if (package != null)
        //{
        //    Debug.Log($"第一个静态数据获取成功：{package.ID}");
        //}
        //else
        //{
        //    Debug.LogWarning($"未找到 uid 为 1 的静态数据");
        //}

        //List<PackageTableItem> packagetypeitems = GetPackageTableByType(1);
        //foreach (PackageTableItem packageItem in packagetypeitems)
        //{
        //    Debug.Log($"id：{packageItem.id}  name：{packageItem.name}  description：{packageItem.description}");
        //}

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


    // 加载静态数据 Items 表的方法
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

        Debug.Log($"加载了 {packageTable.Count} 个 Items 表数据");
        foreach(PackageTableItem packageItem in packageTable)
        {
            Debug.Log($"id：{packageItem.id}  name：{packageItem.name}  type：{packageItem.type}  max_stack：{packageItem.max_stack}  description：{packageItem.description}  icon_path：{packageItem.icon_path}");
        }
        return packageTable;
    }


    // 加载动态数据 PackageLocalItem 表的方法
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

        Debug.Log($"加载了 {packageLocalItem.Count} 个 PackageLocalItem 表数据");
        foreach(PackageLocalItem packageItem in packageLocalItem)
        {
            Debug.Log($"UID：{packageItem.UID}  ID：{packageItem.ID}  NUM：{packageItem.NUM}");
        }
        return packageLocalItem;
    }


    // 根据 id 取到 Items 表格指定数据对象的方法
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


    // 根据 uid 取到 PacakageLocalData 表格指定数据对象的方法
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


    // 获取背包物品并进行排序的方法
    public List<PackageLocalItem> GetSortPackageLocalData()
    {
        List<PackageLocalItem> localItems = GetPackageLocalData();
        localItems.Sort(new PackageItemComparer());
        return localItems;
    }


    // 根据存储物品类型来获取不同类型所有数据的方法   材料：1    武器：2    弹药：3
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


// 排序规则
public class PackageItemComparer : IComparer<PackageLocalItem>
{
    public int Compare(PackageLocalItem a, PackageLocalItem b)
    {
        PackageTableItem x = GameManager.Instance.GetPackageItemById(a.ID);
        PackageTableItem y = GameManager.Instance.GetPackageItemById(b.ID);
        // 按照物品类型
        int typeComparison = y.type.CompareTo(x.type);
        if(typeComparison == 0)
        {
            return b.ID.CompareTo(a.ID);
        }
        return typeComparison;
    }
}


// 定义静态数据，用于加载
public class PackageTableItem
{
    public int id {  get; set; }
    public string name {  get; set; }
    public int type {  get; set; }
    public int max_stack { get; set; }
    public string description {  get; set; }
    public string icon_path { get; set; }
}


// 定义本地背包物品模型，用于加载
public class PackageLocalItem
{
    public int UID { get; set; }
    public int ID { get; set; }
    public int NUM { get; set; }
}


// 存储物品类型的常量表
public class GameConst
{
    // 材料类型常量
    public const int PackageTypeMaterial = 1;

    // 武器类型常量
    public const int PackageTypeWeapom = 2;

    // 弹药类型常量
    public const int PackageTypeAmmo = 3;
}