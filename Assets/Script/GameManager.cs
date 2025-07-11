using JetBrains.Annotations;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System;


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


    private List<PackageLocalItem> _cachedPackageData;  // 缓存背包数据


    void Start()
    {
        // 连接数据库
        Connect();
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


    // 根据 id 取到 PackageLocalData 表格指定数据对象的方法
    public PackageLocalItem GetPackageLocalDataById(int id)
    {
        return DatabaseManager.Instance.GetById<PackageLocalItem>(
            tableName: "PackageLocalData",
            id: id,
            converter: reader => new PackageLocalItem
            {
                UID = reader.GetInt32(reader.GetOrdinal("uid")),
                ID = reader.GetInt32(reader.GetOrdinal("id")),
                NUM = reader.GetInt32(reader.GetOrdinal("num")),
            }
        );
    }


    // 根据生成物 id 找到 Recipes 表格指定配方数据对象的方法
    public Recipe GetRecipeByResultId(int resultId)
    {
        // 获取配方
        return DatabaseManager.Instance.ExecuteQueryAsList<Recipe>(
            $"SELECT * FROM Recipes WHERE result_item_id = {resultId}",
            reader => new Recipe
            {
                id = reader.GetInt32(reader.GetOrdinal("id")),
                result_item_id = reader.GetInt32(reader.GetOrdinal("result_item_id")),
                result_item_num = reader.GetInt32(reader.GetOrdinal("result_item_num")),
                shengtie = reader.IsDBNull(reader.GetOrdinal("shengtie")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("shengtie")),
                mutou = reader.IsDBNull(reader.GetOrdinal("mutou")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("mutou")),
                jiaodai = reader.IsDBNull(reader.GetOrdinal("jiaodai")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("jiaodai")),
                masheng = reader.IsDBNull(reader.GetOrdinal("masheng")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("masheng")),
                yumao = reader.IsDBNull(reader.GetOrdinal("yumao")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("yumao"))
            }).FirstOrDefault();
    }


    // 根据 id 获取背包中某一物品数量的方法
    public int GetItemCount(int itemId)
    {
        int total = 0;
        List<PackageLocalItem> items = GetPackageLocalData();
        foreach(var item in items)
        {
            if(item.ID == itemId)
            {
                total += item.NUM;
            }
        }
        return total;
    }


    // 获取背包物品并进行排序的方法
    public List<PackageLocalItem> GetSortPackageLocalData()
    {
        List<PackageLocalItem> localItems = GetPackageLocalData();
        localItems.Sort(new PackageItemComparer());
        return localItems;
    }


    // 根据存储物品类型来获取不同类型所有数据的方法   材料：1    弹药：2    武器：3
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




    // 添加物品到背包的方法
    public void AddItem(int itemId, int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"添加物品数量无效：ID = {itemId}, 数量 = {amount}");
            return;
        }
        // 刷新缓存
        if (_cachedPackageData == null)
        {
            _cachedPackageData = GetPackageLocalData();
        }
        // 获取物品静态信息
        PackageTableItem itemData = GetPackageItemById(itemId);
        if(itemData == null)
        {
            Debug.LogError($"添加物品失败：{itemId} 不存在");
            return;
        }

        // 获取物品的最大堆叠数量
        int maxStack = itemData.max_stack;
        int remainingAmount = amount;  // 剩余需要添加的数量

        // 1.先尝试添加到现有未满的堆叠
        // 获取所有相同物品且未满的堆叠（按数量从少到多排序）
        List<PackageLocalItem> existingStacks = _cachedPackageData
            .Where(i => i.ID == itemId && i.NUM < maxStack)  // 只选择未满的堆叠
            .OrderBy(i => i.NUM)  // 优先填充数量少的堆叠
            .ToList();

        // 遍历每个未满堆叠
        foreach(PackageLocalItem stack in existingStacks)
        {
            // 没有剩余物品需要添加
            if (remainingAmount <= 0)
                break;

            // 该堆叠还能容纳的数量
            int spaceAvailable = maxStack - stack.NUM;
            // 本次添加的数量
            int amountToAdd = Mathf.Min(spaceAvailable, remainingAmount);

            // 更新堆叠数量
            stack.NUM += amountToAdd;
            // 更新数据库
            UpdateItemInDatabase(stack.UID, stack.NUM);
            remainingAmount -= amountToAdd;
        }

        // 2.如果还有剩余，创建新堆叠
        while(remainingAmount > 0)
        {
            // 检查背包是否已满（最大30格）
            if(_cachedPackageData.Count >= 30)
            {
                Debug.LogWarning($"背包已满，无法添加所有物品：ID = {itemId}, 剩余数量 = {remainingAmount}");
                break;
            }

            // 计算本次创建堆叠的数量
            int amountToAdd = Mathf.Min(maxStack, remainingAmount);

            // 生成新的唯一标识符（UID）
            int newUid = GetNextUid();

            // 再数据库中创建新堆叠
            InsertItemIntoDatabase(newUid, itemId, amountToAdd);

            // 在缓存中添加新的堆叠
            _cachedPackageData.Add(new PackageLocalItem
            {
                UID = newUid,
                ID = itemId,
                NUM = amountToAdd
            });

            // 减少剩余需要添加的数量
            remainingAmount -= amountToAdd;
        }
        // 记录添加结果
        Debug.Log($"添加物品：ID = {itemId}, 请求数量 = {amount}, 实际添加 = {amount - remainingAmount}");
    }


    // 移除背包物品的方法
    public void RemoveItem(int itemId,int amount)
    {
        // 检查移除数量是否有效
        if(amount <= 0)
        {
            Debug.LogWarning($"移除物品数量无效：ID = {itemId}, 数量 = {amount}");
            return;
        }

        // 获取或刷新背包缓存数据
        if(_cachedPackageData == null)
        {
            _cachedPackageData = GetPackageLocalData();  // 从数据库加载背包数据
        }

        // 获取所有相同物品的堆叠
        List<PackageLocalItem> stacks = _cachedPackageData
            .Where(i => i.ID == itemId)  // 选择所有匹配物品的堆叠
            .OrderBy(i => i.NUM)  // 优先从数量少的堆叠开始移除
            .ToList();

        // 检查物品是否存在
        if (!stacks.Any())
        {
            Debug.LogWarning($"尝试移除不存在的物品：ID = {itemId}");
        }

        // 计算物品总量
        int totalAvailable = stacks.Sum(i => i.NUM);

        // 检查数量是否足够
        if(totalAvailable < amount)
        {
            Debug.LogError($"物品数量不足：ID = {itemId}, 当前 = {totalAvailable}, 需要 = {amount}");
            return;
        }

        int remainingToRemove = amount;  // 剩余需要移除的数量

        // 遍历每个堆叠
        foreach(PackageLocalItem stack in stacks)
        {
            // 没有剩余物品需要移除
            if (remainingToRemove <= 0)
                break;

            // 计算本次移除的数量
            int amountToRemove = Mathf.Min(stack.NUM, remainingToRemove);

            // 跟新堆叠数量
            stack.NUM -= amountToRemove;
            // 减少剩余需要移除的数量
            remainingToRemove -= amountToRemove;

            if(stack.NUM > 0)
            {
                // 如果堆叠还有剩余，更新数据库
                UpdateItemInDatabase(stack.UID, stack.NUM);
            }
            else
            {
                // 如果堆叠为空，从数据库删除
                DeleteItemFromDatabase(stack.UID);
                // 从缓存中移除
                _cachedPackageData.Remove(stack);
            }
        }
        Debug.Log($"移除物品：ID = {itemId}, 数量 = {amount}");
    }


    // 更新数据库中物品数量的方法
    private void UpdateItemInDatabase(int uid, int newNum)
    {
        try
        {
            // SQL 语句
            string query = $"UPDATE PackageLocalData SET num = {newNum} WHERE uid = {uid}";

            // 执行指令
            DatabaseManager.Instance.ExecuteQuery(query);

            Debug.Log($"数据库更新：UID = {uid}, 新数量 = {newNum}");
        }
        catch(Exception e)
        {
            Debug.LogError($"更新物品失败：{e.Message}");
        }
    }


    // 获取下一个可用的 UID
    private int GetNextUid()
    {
        int maxUid = 0;
        // 如果缓存中有数据，查找最大 UID
        if (_cachedPackageData != null && _cachedPackageData.Count > 0)
        {
            foreach (var item in _cachedPackageData)
            {
                if (item.UID > maxUid)
                    maxUid = item.UID;
            }
        }

        // 返回最大 UID+1 作为新UID
        return maxUid + 1;
    }


    // 在数据库中插入新物品的方法
    private void InsertItemIntoDatabase(int uid,int itemId, int num)
    {
        try
        {
            // 创建 SQL 插入语句
            string query = $"INSERT INTO PackageLocalData (uid, id, num) VALUES ({uid}, {itemId}, {num})";

            // 执行查询
            DatabaseManager.Instance.ExecuteQuery(query);
            Debug.Log($"数据库插入：UID = {uid}, ID = {itemId}, NUM = {num}");
        }
        catch(Exception e)
        {
            Debug.LogError($"插入物品失败：{e.Message}");
        }
    }


    // 从数据库中删除物品的方法
    private void DeleteItemFromDatabase(int uid)
    {
        try
        {
            // 创建 SQL 删除语句
            string query = $"DELETE FROM PackageLocalData WHERE uid = {uid}";

            // 执行指令
            DatabaseManager.Instance.ExecuteQuery(query);
            Debug.Log($"数据库删除：UID = {uid}");
        }
        catch(Exception e)
        {
            Debug.LogError($"删除物品失败：{e.Message}");
        }
    }


    // 刷新背包缓存的方法
    public void RefreshPackageCache()
    {
        _cachedPackageData = null;  // 清除缓存
        GetPackageLocalData();  // 下次访问时重新加载
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
            return a.ID.CompareTo(b.ID);
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


// 定义合成配方数据模型，用于加载
public class Recipe
{
    public int id { get; set; }
    public int result_item_id { get; set; }
    public int result_item_num {  get; set; }
    public int? shengtie { get; set; }
    public int? mutou { get; set; }
    public int? jiaodai { get; set; }
    public int? masheng { get; set; }
    public int? yumao { get; set; }
}
    


// 存储物品类型的常量表
public class GameConst
{
    // 材料类型常量
    public const int PackageTypeMaterial = 1;

    // 弹药类型常量
    public const int PackageTypeAmmo = 2;

    // 武器类型常量
    public const int PackageTypeWeapom = 3;
}


// 材料类型常量表
public class MaterialType
{
    public const int shengtie = 1;
    public const int mutou = 2;
    public const int jiaodai = 3;
    public const int masheng = 4;
    public const int yumao = 5;
}