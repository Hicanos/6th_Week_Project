using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(value: true); //���� ������Ʈ Ȱ��ȭ
    }
    public void Close()
    {
        gameObject.SetActive(value: false); //���� ������Ʈ ��Ȱ��ȭ
    }
}
