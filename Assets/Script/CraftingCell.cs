using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingCell : MonoBehaviour
{
    // ��� UI ����
    private Transform UIItem;
    private Transform UISelect;
    private Transform UIBottom;
    private Transform UIIconName;
    private Transform UICenter;
    private Transform UIIcon;

    private PackageTableItem packageTableData;  // ��ǰ��Ʒ�ľ�̬����
    private CraftingPanel uiParent;  // ��ǰ��Ʒ�ĸ���Ʒ��CraftingPanel)


    private void Awake()
    {
        InitUIName();  // ��ʼ�� UI ���
    }


    // ��ȡ UI ���
    private void InitUIName()
    {
        UIItem = transform.Find("Item");
        UIIconName = transform.Find("Item/Bottom/IconName");
        UIIcon = transform.Find("Item/Center/Icon");
    }


    // ˢ����Ʒ״̬�ķ���
    public void Refresh(PackageTableItem packageItem, CraftingPanel uiParent)
    {
        // ���ݳ�ʼ��
        this.packageTableData = packageItem;
        this.uiParent = uiParent;

        // �� UI �������Ϣ���г�ʼ��
        // ��Ʒ����
        UIIconName.GetComponent<Text>().text = this.packageTableData.name.ToString();

        // ��ƷͼƬ
        Texture2D t = (Texture2D)Resources.Load(this.packageTableData.icon_path);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        UIIcon.GetComponent<Image>().sprite = temp;
    }
}
