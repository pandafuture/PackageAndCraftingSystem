using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// �̳� ��������롢�˳��ص������Ľӿ�
public class PackageCell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // �����Ʒ UI ����
    private Transform UIIcon;
    private Transform UIIconName;
    private Transform UINum;
    private Transform UISelect;


    private PackageLocalItem packageLocalData;  // ��ǰ��Ʒ�Ķ�̬����
    private PackageTableItem packageTableItem;  // ��ǰ��Ʒ�ľ�̬����
    private PackagePanel uiParent;  // ��ǰ��Ʒ�ĸ���Ʒ��PackagePanel��


    private void Awake()
    {
        InitUIName();  // ��ʼ�� UI
    }


    // ��ȡ UI ���
    private void InitUIName()
    {
        // �� UI ���
        UIIcon = transform.Find("Item/Center/Icon");
        UIIconName = transform.Find("Item/Bottom/IconName");
        UINum = transform.Find("Item/Top/Num");
        UISelect = transform.Find("Item/Select");
    }


    // ˢ����Ʒ״̬�ķ���
    public void Refresh(PackageLocalItem packageLcoalData, PackagePanel uiParent)
    {
        // ���ݳ�ʼ��
        this.packageLocalData = packageLcoalData;
        this.packageTableItem = GameManager.Instance.GetPackageItemById(packageLocalData.ID);
        this.uiParent = uiParent;


        // �� UI �������Ϣ���г�ʼ��
        // ��Ʒ����
        UIIconName.GetComponent<Text>().text = this.packageTableItem.name.ToString();

        // ��Ʒ��ͼƬ
        Texture2D t = (Texture2D)Resources.Load(this.packageTableItem.icon_path);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        UIIcon.GetComponent<Image>().sprite = temp;

        // ��Ʒ������
        UINum.GetComponent<Text>().text = this.packageLocalData.NUM.ToString();
    }


    // ʵ��������Ļص�����
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick��" + eventData.ToString());
    }


    // ʵ��������Ļص�����
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter��" + eventData.ToString());
    }


    // ʵ������˳��Ļص�����
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit��" + eventData.ToString());
    }



}
