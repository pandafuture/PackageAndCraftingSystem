using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private Transform UIDetailPanel;


    // ��Ӻϳɽ���������Ԥ�Ƽ�����
    public GameObject CraftingUIItemPrefab;

    // ��ǰѡ�е���Ʒ����һ�� id
    private int _chooseId;

    // �ⲿʹ��ʱ����ʹ�� chooseID ����ȡ��ǰѡ�е���Ʒ
    public int chooseID
    {
        get
        {
            return _chooseId;
        }
        set
        {
            // �����ȡ��һ���µ�ֵ����ˢ�������������
            _chooseId = value;
            RefreshDetail();  // ˢ���������
        }
    }


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
        UIDetailPanel = transform.Find("Center/DetailPanel");
    }


    // ע�ᰴť����¼�
    private void InitClick()
    {
        UICloseBtn.GetComponent<Button>().onClick.AddListener(OnClickClose);

    }


    // ��ť����¼�
    private void OnClickClose()
    {
        print(">>>>>  OnClickClose");
        ClosePanel();  // �رս���
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


    // ˢ���������ķ���
    private void RefreshDetail()
    {
        // �ҵ� id ��Ӧ�ľ�̬����
        PackageTableItem Item = GameManager.Instance.GetPackageItemById(chooseID);

        // ˢ���������
        UIDetailPanel.GetComponent<CraftingDetail>().Refresh(Item, this);
    }
}
