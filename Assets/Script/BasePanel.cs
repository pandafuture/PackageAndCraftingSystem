using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected bool isRemove = false;  // ��ǰ�����Ƿ��ѹرգ���־λ
    protected new string name;  // ���������


    protected virtual void Awake()
    {

    }


    // ���濪��
    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }


    // �򿪽���ķ���
    public virtual void OpenPanel(string name)
    {
        this.name = name;  // ����ǰ�������Ƹ�ֵ
        gameObject.SetActive(true);  // �򿪽���
    }

    // �رս���ķ���
    public virtual void ClosePanel()
    {
        isRemove = true;  // ��ǵ�ǰ�����ѹر�
        gameObject.SetActive(false);  // �رս���
        Destroy(gameObject);  // ���ٽ�������

        // ��鵱ǰ�����Ƿ���ڣ����������Ƴ����棬��ʾδ��
        if (UIManager.Instance.panelDict.ContainsKey(name))
        {
            UIManager.Instance.panelDict.Remove(name);
        }
    }
}
