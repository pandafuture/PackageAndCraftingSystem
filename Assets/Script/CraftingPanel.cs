using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CraftingPanel : BasePanel
{
    // ��� UI ����
    // Right
    private Transform UIClosePanel;  // �رղ˵���
    private Transform UICloseBtn;  // �رհ�ť

    // Center
    private Transform UICenter;
    private Transform UIScrollView;
    // Detail
    private Transform UIDetailPaenl;
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
    // Detail_Bottom_Left
    private Transform UIDetail_Bottom_Left_SubtractionBtn;
    // Detail_Bottom_Right
    private Transform UIDetail_Bottom_Right_AdditionBtn;


    // ��Ӻϳɽ���������Ԥ�Ƽ�����
    public GameObject CraftingUIItemPrefab;


    protected override void Awake()
    {
        base.Awake();
        InitUI();  // ��ʼ�� UI
    }


    private void Start()
    {
        RefreshUI();  // UI ˢ�·���
    }


    private void InitUI()
    {
        InitUIName();  // ��ʼ�� UI ���

        InitClick();  // ��ʼ������¼�
    }


    // �� UI ���
    private void InitUIName()
    {
        UIClosePanel = transform.Find("Right/ClosePanel");
        UICloseBtn = transform.Find("Right/ClosePanel/CloseBtn");

        UICenter = transform.Find("Center");
        UIScrollView = transform.Find("Center/Scroll View");

        UIDetailPaenl = transform.Find("Center/DetailPanel");

        UIDetail_Top = transform.Find("Center/DetailPanel/Top");
        UIDetail_Top_Icon = transform.Find("Center/DetailPanel/Top/Icon");
        UIDetail_Top_IconName = transform.Find("Center/DetailPanel/Top/IconName");
        UIDetail_Top_Num = transform.Find("Center/DetailPanel/Top/Num");
        UIDetail_Top_DetailText = transform.Find("Center/DetailPanel/Top/DetailText");

        UIDetail_Center = transform.Find("Center/DetailPanel/Center");
        UIDetail_Center_Bottom = transform.Find("Center/DetailPanel/Center/Bottom");
        UIDetail_Center_Bottom_ScrollView = transform.Find("Center/DetailPanel/Center/Bottom/Scroll View");

        UIDetail_Bottom = transform.Find("Center/DetailPanel/Bottom");
        UIDetail_Bottom_Center_CraftingBtn = transform.Find("Center/DetailPanel/Bottom/Center/CraftingBtn");
        UIDetail_Bottom_Left_SubtractionBtn = transform.Find("Center/DetailPanel/Bottom/Left/Subtraction");
        UIDetail_Bottom_Right_AdditionBtn = transform.Find("Center/DetailPanel/Bottom/Right/Addition");
    }


    // ע�ᰴť����¼�
    private void InitClick()
    {
        UICloseBtn.GetComponent<Button>().onClick.AddListener(OnClickClose);

        UIDetail_Bottom_Center_CraftingBtn.GetComponent<Button>().onClick.AddListener(OnClickCrafting);
        UIDetail_Bottom_Left_SubtractionBtn.GetComponent<Button>().onClick.AddListener(OnClickSubtracton);
        UIDetail_Bottom_Right_AdditionBtn.GetComponent<Button>().onClick.AddListener(OnClickAddition);
    }


    // ��ť����¼�
    private void OnClickClose()
    {
        print(">>>>>  OnClickClose");
        ClosePanel();  // �رս���
    }

    private void OnClickCrafting()
    {
        print(">>>>>  OnClickCrafting");
    }

    private void OnClickSubtracton()
    {
        print(">>>>>  OnClickSubtracton");
    }

    private void OnClickAddition()
    {
        print(">>>>>  OnClickAddition");
    }


    // UI ˢ�·���
    private void RefreshUI()
    {
        RefreshScroll();  // ˢ�¹�������
    }

    // ˢ�¹��������ķ���
    private void RefreshScroll()
    {
        // ���������������ԭ������Ʒ
        RectTransform scrollContent = UIScrollView.GetComponent<ScrollRect>().content;
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }

        // ���ݾ�̬�����е� �����͵�ҩ ��ʼ����������
        // �Ȼ�ȡ ���� �� ��ҩ ����
        List<PackageTableItem> weapons = GameManager.Instance.GetPackageTableByType(2);
        List<PackageTableItem> ammo = GameManager.Instance.GetPackageTableByType(3);
        List<PackageTableItem> weaponsAndammo = new List<PackageTableItem>();
        weaponsAndammo.AddRange(weapons);
        weaponsAndammo.AddRange(ammo);

        // ��ˢ�¹�������
        foreach(PackageTableItem Item in weaponsAndammo)
        {
            Transform CraftingUIItem = Instantiate(CraftingUIItemPrefab.transform, scrollContent) as Transform;
            CraftingCell craftingCell = CraftingUIItem.GetComponent<CraftingCell>();

            // ˢ�������Ʒ��״̬
            craftingCell.Refresh(Item, this);
        }
    }
}
