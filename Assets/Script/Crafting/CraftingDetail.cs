using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CraftingDetail : MonoBehaviour
{
    // ��� UI ����
    // Detail_Top
    private Transform UIDetail_Top;
    private Transform UIDetail_Top_Icon;
    private Transform UIDetail_Top_IconName;
    private Transform UIDetail_Top_Num;
    private Transform UIDetail_Top_DetailText;
    // Detail_Center
    private Transform UIDetail_Center;
    // Detail_Center_Bottom
    private Transform UIDetail_Center_Bottom;
    private Transform UIDetail_Center_Bottom_ScrollView;
    // Detail_Bottom
    private Transform UIDetail_Bottom;
    // Detail_Bottom_Center
    private Transform UIDetail_Bottom_Center_CraftingBtn;
    private Transform UIDetail_Bottom_Center_CraftingText;
    // Detail_Bottom_Left
    private Transform UIDetail_Bottom_Left_SubtractionBtn;
    // Detail_Bottom_Right
    private Transform UIDetail_Bottom_Right_AdditionBtn;


    // �õ������Ʒ�Ķ�̬��Ϣ����̬��Ϣ��uiParent
    //private PackageLocalItem packageLocalData;
    private PackageTableItem packageTableItem;
    private CraftingPanel uiParent;


    // ��ǰ��������
    private int craftCount = 1;
    // ��ǰѡ�е���Ʒ
    private PackageTableItem currentItem;
    // ��ǰ�䷽
    private Recipe currentRecipe;

    // ������Ԥ����
    public GameObject materialPrefab;



    private void Awake()
    {
        InitUIName();  // ��ʼ�� UI

        InitClick();  // ��ʼ������¼�

        // ����
        //Refresh(GameManager.Instance.GetPackageLocalData()[0], null);
    }


    // �� UI ���
    private void InitUIName()
    {
        UIDetail_Top = transform.Find("Top");
        UIDetail_Top_Icon = transform.Find("Top/Icon");
        UIDetail_Top_IconName = transform.Find("Top/IconName");
        UIDetail_Top_Num = transform.Find("Top/Num");
        UIDetail_Top_DetailText = transform.Find("Top/DetailText");

        UIDetail_Center = transform.Find("Center");
        UIDetail_Center_Bottom = transform.Find("Center/Bottom");
        UIDetail_Center_Bottom_ScrollView = transform.Find("Center/Bottom/Scroll View");

        UIDetail_Bottom = transform.Find("Bottom");
        UIDetail_Bottom_Center_CraftingBtn = transform.Find("Bottom/Center/CraftingBtn");
        UIDetail_Bottom_Center_CraftingText = transform.Find("Bottom/Center/CraftingText");
        UIDetail_Bottom_Left_SubtractionBtn = transform.Find("Bottom/Left/Subtraction");
        UIDetail_Bottom_Right_AdditionBtn = transform.Find("Bottom/Right/Addition");

    }


    // ע�ᰴť����¼�
    private void InitClick()
    {
        UIDetail_Bottom_Center_CraftingBtn.GetComponent<Button>().onClick.AddListener(OnClickCrafting);
        UIDetail_Bottom_Left_SubtractionBtn.GetComponent<Button>().onClick.AddListener(OnClickSubtracton);
        UIDetail_Bottom_Right_AdditionBtn.GetComponent<Button>().onClick.AddListener(OnClickAddition);

    }


    // ��ť����¼�
    // ������ť
    private void OnClickCrafting()
    {
        print(">>>>>  OnClickCrafting");

        if (currentRecipe == null)
            return;

        // �������Ƿ��㹻
        if (!HasEnoughMaterials())
        {
            Debug.Log("���ϲ��㣬�޷�����");
            return;
        }

        // �۳�����
        DeductMaterial(MaterialType.shengtie, currentRecipe.shengtie);
        DeductMaterial(MaterialType.mutou, currentRecipe.mutou);
        DeductMaterial(MaterialType.jiaodai, currentRecipe.jiaodai);
        DeductMaterial(MaterialType.masheng, currentRecipe.masheng);
        DeductMaterial(MaterialType.yumao, currentRecipe.yumao);

        Debug.Log($"Ҫ����������Ϊ��{craftCount}");
        // ��ӳ�Ʒ
        GameManager.Instance.AddItem(currentRecipe.result_item_id, craftCount);


        // ���µ�ǰ��Ʒ����
        currentItem = GameManager.Instance.GetPackageItemById(currentRecipe.result_item_id);
        // ˢ�� UI 
        Refresh(currentItem, uiParent);
    }

    // ���ٰ�ť
    private void OnClickSubtracton()
    {
        print(">>>>>  OnClickSubtracton");
        craftCount = Mathf.Max(1, craftCount - 1);

        RefreshScroll();
        RefreshCraftCount();
    }

    // ���Ӱ�ť
    private void OnClickAddition()
    {
        print(">>>>>  OnClickAddition");
        craftCount = Mathf.Max(1, craftCount + 1);

        RefreshScroll();
        RefreshCraftCount();
    }


    // ˢ���������ķ���
    public void Refresh(PackageTableItem packageData, CraftingPanel uiParent)
    {
        // ��ʼ������̬���ݡ���̬���ݡ��������߼�
        //this.packageLocalData = GameManager.Instance.GetPackageLocalDataById(packageData.id);
        this.packageTableItem = packageData;
        this.uiParent = uiParent;


        // ��ȡ��ǰ��Ʒ���䷽
        currentRecipe = GameManager.Instance.GetRecipeByResultId(packageData.id);


        // ��ʼ�� UI �������Ϣ
        // ��Ʒ����
        UIDetail_Top_IconName.GetComponent<Text>().text = string.Format(this.packageTableItem.name.ToString());

        // ��Ʒ����
        UIDetail_Top_Num.GetComponent<Text>().text = string.Format("ӵ�У�{0}", GameManager.Instance.GetItemCount(packageData.id));

        // ��ϸ����
        UIDetail_Top_DetailText.GetComponent<Text>().text = string.Format(this.packageTableItem.description.ToString());

        // ����ͼƬ
        Texture2D t = (Texture2D)Resources.Load(this.packageTableItem.icon_path);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        UIDetail_Top_Icon.GetComponent<Image>().sprite = temp;
        Debug.Log("��ˢ���������");

        RefreshScroll();  // ˢ�¹�������
        RefreshCraftCount();  // ˢ�����������ķ���
    }


    // ˢ�¹��������ķ���
    private void RefreshScroll()
    {
        // ���������������ԭ������Ʒ
        RectTransform scrollContent = UIDetail_Center_Bottom_ScrollView.GetComponent<ScrollRect>().content;
        for(int i = 0; i < scrollContent.childCount; i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }

        // ����ǰ�䷽Ϊ�վͷ���
        if (currentRecipe == null)
        {
            Debug.LogError("��ǰ�䷽Ϊ��");
            return;
        }

        // ��Ӳ�����
        AddMaterialItem(scrollContent, MaterialType.shengtie, currentRecipe.shengtie);
        AddMaterialItem(scrollContent, MaterialType.mutou, currentRecipe.mutou);
        AddMaterialItem(scrollContent, MaterialType.jiaodai, currentRecipe.jiaodai);
        AddMaterialItem(scrollContent, MaterialType.masheng, currentRecipe.masheng);
        AddMaterialItem(scrollContent, MaterialType.yumao, currentRecipe.yumao);
    }


    // ��Ӳ�����ķ���
    private void AddMaterialItem(Transform parent, int materialId,int? requiredCount)
    {
        // �ò�����������Ϊ 0���Ͳ����
        if (!requiredCount.HasValue || requiredCount <= 0)
            return;

        // ʵ����������
        GameObject materialItem = Instantiate(materialPrefab, parent);
        MaterialItem item = materialItem.GetComponent<MaterialItem>();

        // ��ȡ����
        int currentCount = GameManager.Instance.GetItemCount(materialId);
        int totalRequired = requiredCount.Value * craftCount;

        item.Refresh(materialId, currentCount, totalRequired);
    }


    // ˢ�����������ķ���
    private void RefreshCraftCount()
    {
        UIDetail_Bottom_Center_CraftingText.GetComponent<Text>().text = string.Format("����X" + craftCount.ToString());
    }


    // �����������Ƿ��㹻�ķ���
    private bool HasEnoughMaterials()
    {
        return CheckMaterial(MaterialType.shengtie, currentRecipe.shengtie) &&
               CheckMaterial(MaterialType.mutou, currentRecipe.mutou) &&
               CheckMaterial(MaterialType.jiaodai, currentRecipe.jiaodai) &&
               CheckMaterial(MaterialType.masheng, currentRecipe.masheng) &&
               CheckMaterial(MaterialType.yumao, currentRecipe.yumao);
    }

    // ���ĳһ�����Ƿ��㹻�ķ���
    private bool CheckMaterial(int materialId, int? require)
    {
        if (!require.HasValue || require <= 0)
            return true;

        int available = GameManager.Instance.GetItemCount(materialId);
        return available >= require.Value * craftCount;
    }

    
    // �۳�������ϵķ���
    private void DeductMaterials()
    {
        DeductMaterial(MaterialType.shengtie, currentRecipe.shengtie);
        DeductMaterial(MaterialType.mutou, currentRecipe.mutou);
        DeductMaterial(MaterialType.jiaodai, currentRecipe.jiaodai);
        DeductMaterial(MaterialType.masheng, currentRecipe.masheng);
        DeductMaterial(MaterialType.yumao, currentRecipe.yumao);
    }

    // �۳�ĳһ���ϵķ���
    private void DeductMaterial(int materialId, int? amount)
    {
        if (!amount.HasValue || amount <= 0)
            return;
        
        GameManager.Instance.RemoveItem(materialId, amount.Value * craftCount);
    }
}
