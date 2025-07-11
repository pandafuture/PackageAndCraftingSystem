using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingCell : MonoBehaviour,IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // ��� UI ����
    private Transform UIItem;
    private Transform UISelect;
    private Transform UIBottom;
    private Transform UIIconName;
    private Transform UICenter;
    private Transform UIIcon;

    private PackageLocalItem PackageLocalData;  // ��ǰ��Ʒ�Ķ�̬����
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
        UISelect = transform.Find("Item/Select");

        UISelect.gameObject.SetActive(false);
    }


    // ˢ����Ʒ״̬�ķ���
    public void Refresh(PackageTableItem packageItem, CraftingPanel uiParent)
    {
        // ���ݳ�ʼ��
        this.PackageLocalData = GameManager.Instance.GetPackageLocalDataById(packageItem.id);
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


    // ʵ��������Ļص�����
    public void OnPointerClick(PointerEventData eventData)
    {
        // ��ӡ��ǰִ�еķ��������Լ���һ����
        Debug.Log("OnPonterClick: " + eventData.ToString());

        // �ȹر�֮ǰ��ѡ��Ч��
        GameObject select = GameObject.Find("Select");
        if (select != null)
        {
            select.SetActive(false);
        }

        // ѡ��Ч��
        UISelect.gameObject.SetActive(true);

        // �жϵ�ǰ���ѡ�е���Ʒ�Ƿ�͸���Ʒ�� uid һ������һ����Ϊ�ظ��������ִ���߼�
        if (this.uiParent.chooseID == this.packageTableData.id)
            return;
        // ����һ�������������������Ʒ���ϣ�
        this.uiParent.chooseID = this.packageTableData.id;
    }


    // ʵ��������Ļص�����
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter: " + eventData.ToString());
    }


    // ʵ������˳��Ļص�����
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit: " + eventData.ToString());
    }

}
