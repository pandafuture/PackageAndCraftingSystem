using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialItem : MonoBehaviour
{
    // ��� UI ����
    private Transform UIConditionIcon;
    private Transform UIIcon;
    private Transform UIConditionText;

    private void Awake()
    {
        InitUIName();  // ��ʼ�� UI
    }


    // ��ʼ�� UI ���
    private void InitUIName()
    {
        UIConditionIcon = transform.Find("ConditionIcon");
        UIIcon = transform.Find("ConditionIcon/Icon");
        UIConditionText = transform.Find("ConditionText");
    }


    // ˢ�²��Ϲ��������ķ���
    public void Refresh(int itemId, int currentCount, int requiredCount)
    {
        PackageTableItem itemData = GameManager.Instance.GetPackageItemById(itemId);

        // ����ͼƬ
        Texture2D t = (Texture2D)Resources.Load(itemData.icon_path);
        Debug.Log("ͼƬ·��Ϊ��" + itemData.icon_path);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0, 0));
        UIIcon.GetComponent<Image>().sprite = temp;

        // �������ƺ�����
        UIConditionText.GetComponent<Text>().text = $"{currentCount}/{requiredCount} {itemData.name}";

        // ������ɫ������ʱΪ��ɫ��
        Color ziti = UIConditionText.GetComponent<Text>().color;
        UIConditionText.GetComponent<Text>().color = currentCount >= requiredCount ? ziti : Color.red;
    }
}
