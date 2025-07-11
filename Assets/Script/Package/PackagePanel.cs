using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackagePanel : BasePanel
{
    // �Ը��� UI �����ʼ��
    // LeftTop
    private Transform UIPackageText;

    // Right
    private Transform UICloseBtn;

    // Center
    private Transform UICenter;
    private Transform UIScrollView;

    // ��ӱ���������Ԥ�Ƽ�����
    public GameObject PackageUIItemPrefab;



    override protected void Awake()
    {
        base.Awake();
        InitUI();  // ��ʼ�� UI

        // ��ʼ����������Ʒ��Ԥ�Ƽ�
        InitPrefab();
    }

    private void Start()
    {
        RefreshUI();  // UI ˢ�·���
    }


    private void InitUI()
    {
        InitUIName();  // ��ʼ�� UI ���

        InitClick();  // ע�����¼�
    }


    // ��ʼ�� UI ���
    private void InitUIName()
    {
        // �� UI ���
        UIPackageText = transform.Find("LeftTop/PackageText");

        UICloseBtn = transform.Find("Right/ClosePanel/CloseBtn");

        UICenter = transform.Find("Center");
        UIScrollView = transform.Find("Center/Scroll View");
    }


    // ��ʼ����������Ʒ��Ԥ�Ƽ�
    private void InitPrefab()
    {
        PackageUIItemPrefab = Resources.Load("Prefab/Panel/Package/PackageUIItem") as GameObject;
    }


    // ע�ᰴť����¼�
    private void InitClick()
    {
        UICloseBtn.GetComponent<Button>().onClick.AddListener(OnClickClose);
    }


    // ��ӵ���¼�
    // PackagePanel ����Ĺرհ�ť
    private void OnClickClose()
    {
        print(">>>>> OnClickClose");
        ClosePanel();
    }


    // UI ˢ�·���
    private void RefreshUI()
    {
        RefreshScroll();  // ˢ�¹�������

    }


    // ˢ�¹��������ķ���
    private void RefreshScroll()
    {
        int packagecellnum = 0;  // ������ʹ�ü����� 

        // ���������������ԭ������Ʒ
        RectTransform scrollContent = UIScrollView.GetComponent<ScrollRect>().content;
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }

        // ���� 30 ��������
        for(int i = 0; i < 30; i++)
        {
            Transform PackageUIItem = Instantiate(PackageUIItemPrefab.transform, scrollContent) as Transform;
            PackageCell packageCell = PackageUIItem.GetComponent<PackageCell>();

            // ��鵱ǰ���Ƿ�����Ʒ
            if(i < GameManager.Instance.GetSortPackageLocalData().Count)
            {
                // ˢ�������Ʒ��״̬
                packageCell.Refresh(GameManager.Instance.GetSortPackageLocalData()[i], this);
                packagecellnum++;
            }
            else
            {
                // �ո�״̬
                packageCell.SetEmpty();
            }
        }

        // ˢ�±�����
        UIPackageText.GetComponent<Text>().text = "������" + packagecellnum + "/30��";

        //// ���ݱ������ݳ�ʼ����������
        //foreach(PackageLocalItem localData in GameManager.Instance.GetSortPackageLocalData())
        //{
        //    Transform PackageUIItem = Instantiate(PackageUIItemPrefab.transform, scrollContent) as Transform;
        //    PackageCell packageCell = PackageUIItem.GetComponent<PackageCell>();

        //    // ˢ�������Ʒ��״̬
        //    packageCell.Refresh(localData, this);
        //    packagecellnum++;
        //}


        //// ˢ�±�����
        //UIPackageText.GetComponent<Text>().text = "������" + packagecellnum + "/30��";
    }
}
