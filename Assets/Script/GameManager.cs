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


    private List<PackageLocalItem> _cachedPackageData;  // ���汳������


    void Start()
    {
        // �������ݿ�
        Connect();
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


    // ���� id ȡ�� PackageLocalData ���ָ�����ݶ���ķ���
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


    // ���������� id �ҵ� Recipes ���ָ���䷽���ݶ���ķ���
    public Recipe GetRecipeByResultId(int resultId)
    {
        // ��ȡ�䷽
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


    // ���� id ��ȡ������ĳһ��Ʒ�����ķ���
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


    // ��ȡ������Ʒ����������ķ���
    public List<PackageLocalItem> GetSortPackageLocalData()
    {
        List<PackageLocalItem> localItems = GetPackageLocalData();
        localItems.Sort(new PackageItemComparer());
        return localItems;
    }


    // ���ݴ洢��Ʒ��������ȡ��ͬ�����������ݵķ���   ���ϣ�1    ��ҩ��2    ������3
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




    // �����Ʒ�������ķ���
    public void AddItem(int itemId, int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"�����Ʒ������Ч��ID = {itemId}, ���� = {amount}");
            return;
        }
        // ˢ�»���
        if (_cachedPackageData == null)
        {
            _cachedPackageData = GetPackageLocalData();
        }
        // ��ȡ��Ʒ��̬��Ϣ
        PackageTableItem itemData = GetPackageItemById(itemId);
        if(itemData == null)
        {
            Debug.LogError($"�����Ʒʧ�ܣ�{itemId} ������");
            return;
        }

        // ��ȡ��Ʒ�����ѵ�����
        int maxStack = itemData.max_stack;
        int remainingAmount = amount;  // ʣ����Ҫ��ӵ�����

        // 1.�ȳ�����ӵ�����δ���Ķѵ�
        // ��ȡ������ͬ��Ʒ��δ���Ķѵ������������ٵ�������
        List<PackageLocalItem> existingStacks = _cachedPackageData
            .Where(i => i.ID == itemId && i.NUM < maxStack)  // ֻѡ��δ���Ķѵ�
            .OrderBy(i => i.NUM)  // ������������ٵĶѵ�
            .ToList();

        // ����ÿ��δ���ѵ�
        foreach(PackageLocalItem stack in existingStacks)
        {
            // û��ʣ����Ʒ��Ҫ���
            if (remainingAmount <= 0)
                break;

            // �öѵ��������ɵ�����
            int spaceAvailable = maxStack - stack.NUM;
            // ������ӵ�����
            int amountToAdd = Mathf.Min(spaceAvailable, remainingAmount);

            // ���¶ѵ�����
            stack.NUM += amountToAdd;
            // �������ݿ�
            UpdateItemInDatabase(stack.UID, stack.NUM);
            remainingAmount -= amountToAdd;
        }

        // 2.�������ʣ�࣬�����¶ѵ�
        while(remainingAmount > 0)
        {
            // ��鱳���Ƿ����������30��
            if(_cachedPackageData.Count >= 30)
            {
                Debug.LogWarning($"�����������޷����������Ʒ��ID = {itemId}, ʣ������ = {remainingAmount}");
                break;
            }

            // ���㱾�δ����ѵ�������
            int amountToAdd = Mathf.Min(maxStack, remainingAmount);

            // �����µ�Ψһ��ʶ����UID��
            int newUid = GetNextUid();

            // �����ݿ��д����¶ѵ�
            InsertItemIntoDatabase(newUid, itemId, amountToAdd);

            // �ڻ���������µĶѵ�
            _cachedPackageData.Add(new PackageLocalItem
            {
                UID = newUid,
                ID = itemId,
                NUM = amountToAdd
            });

            // ����ʣ����Ҫ��ӵ�����
            remainingAmount -= amountToAdd;
        }
        // ��¼��ӽ��
        Debug.Log($"�����Ʒ��ID = {itemId}, �������� = {amount}, ʵ����� = {amount - remainingAmount}");
    }


    // �Ƴ�������Ʒ�ķ���
    public void RemoveItem(int itemId,int amount)
    {
        // ����Ƴ������Ƿ���Ч
        if(amount <= 0)
        {
            Debug.LogWarning($"�Ƴ���Ʒ������Ч��ID = {itemId}, ���� = {amount}");
            return;
        }

        // ��ȡ��ˢ�±�����������
        if(_cachedPackageData == null)
        {
            _cachedPackageData = GetPackageLocalData();  // �����ݿ���ر�������
        }

        // ��ȡ������ͬ��Ʒ�Ķѵ�
        List<PackageLocalItem> stacks = _cachedPackageData
            .Where(i => i.ID == itemId)  // ѡ������ƥ����Ʒ�Ķѵ�
            .OrderBy(i => i.NUM)  // ���ȴ������ٵĶѵ���ʼ�Ƴ�
            .ToList();

        // �����Ʒ�Ƿ����
        if (!stacks.Any())
        {
            Debug.LogWarning($"�����Ƴ������ڵ���Ʒ��ID = {itemId}");
        }

        // ������Ʒ����
        int totalAvailable = stacks.Sum(i => i.NUM);

        // ��������Ƿ��㹻
        if(totalAvailable < amount)
        {
            Debug.LogError($"��Ʒ�������㣺ID = {itemId}, ��ǰ = {totalAvailable}, ��Ҫ = {amount}");
            return;
        }

        int remainingToRemove = amount;  // ʣ����Ҫ�Ƴ�������

        // ����ÿ���ѵ�
        foreach(PackageLocalItem stack in stacks)
        {
            // û��ʣ����Ʒ��Ҫ�Ƴ�
            if (remainingToRemove <= 0)
                break;

            // ���㱾���Ƴ�������
            int amountToRemove = Mathf.Min(stack.NUM, remainingToRemove);

            // ���¶ѵ�����
            stack.NUM -= amountToRemove;
            // ����ʣ����Ҫ�Ƴ�������
            remainingToRemove -= amountToRemove;

            if(stack.NUM > 0)
            {
                // ����ѵ�����ʣ�࣬�������ݿ�
                UpdateItemInDatabase(stack.UID, stack.NUM);
            }
            else
            {
                // ����ѵ�Ϊ�գ������ݿ�ɾ��
                DeleteItemFromDatabase(stack.UID);
                // �ӻ������Ƴ�
                _cachedPackageData.Remove(stack);
            }
        }
        Debug.Log($"�Ƴ���Ʒ��ID = {itemId}, ���� = {amount}");
    }


    // �������ݿ�����Ʒ�����ķ���
    private void UpdateItemInDatabase(int uid, int newNum)
    {
        try
        {
            // SQL ���
            string query = $"UPDATE PackageLocalData SET num = {newNum} WHERE uid = {uid}";

            // ִ��ָ��
            DatabaseManager.Instance.ExecuteQuery(query);

            Debug.Log($"���ݿ���£�UID = {uid}, ������ = {newNum}");
        }
        catch(Exception e)
        {
            Debug.LogError($"������Ʒʧ�ܣ�{e.Message}");
        }
    }


    // ��ȡ��һ�����õ� UID
    private int GetNextUid()
    {
        int maxUid = 0;
        // ��������������ݣ�������� UID
        if (_cachedPackageData != null && _cachedPackageData.Count > 0)
        {
            foreach (var item in _cachedPackageData)
            {
                if (item.UID > maxUid)
                    maxUid = item.UID;
            }
        }

        // ������� UID+1 ��Ϊ��UID
        return maxUid + 1;
    }


    // �����ݿ��в�������Ʒ�ķ���
    private void InsertItemIntoDatabase(int uid,int itemId, int num)
    {
        try
        {
            // ���� SQL �������
            string query = $"INSERT INTO PackageLocalData (uid, id, num) VALUES ({uid}, {itemId}, {num})";

            // ִ�в�ѯ
            DatabaseManager.Instance.ExecuteQuery(query);
            Debug.Log($"���ݿ���룺UID = {uid}, ID = {itemId}, NUM = {num}");
        }
        catch(Exception e)
        {
            Debug.LogError($"������Ʒʧ�ܣ�{e.Message}");
        }
    }


    // �����ݿ���ɾ����Ʒ�ķ���
    private void DeleteItemFromDatabase(int uid)
    {
        try
        {
            // ���� SQL ɾ�����
            string query = $"DELETE FROM PackageLocalData WHERE uid = {uid}";

            // ִ��ָ��
            DatabaseManager.Instance.ExecuteQuery(query);
            Debug.Log($"���ݿ�ɾ����UID = {uid}");
        }
        catch(Exception e)
        {
            Debug.LogError($"ɾ����Ʒʧ�ܣ�{e.Message}");
        }
    }


    // ˢ�±�������ķ���
    public void RefreshPackageCache()
    {
        _cachedPackageData = null;  // �������
        GetPackageLocalData();  // �´η���ʱ���¼���
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
            return a.ID.CompareTo(b.ID);
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


// ����ϳ��䷽����ģ�ͣ����ڼ���
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
    


// �洢��Ʒ���͵ĳ�����
public class GameConst
{
    // �������ͳ���
    public const int PackageTypeMaterial = 1;

    // ��ҩ���ͳ���
    public const int PackageTypeAmmo = 2;

    // �������ͳ���
    public const int PackageTypeWeapom = 3;
}


// �������ͳ�����
public class MaterialType
{
    public const int shengtie = 1;
    public const int mutou = 2;
    public const int jiaodai = 3;
    public const int masheng = 4;
    public const int yumao = 5;
}