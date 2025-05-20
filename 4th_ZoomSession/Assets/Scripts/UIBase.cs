using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(value: true); //게임 오브젝트 활성화
    }
    public void Close()
    {
        gameObject.SetActive(value: false); //게임 오브젝트 비활성화
    }
}
