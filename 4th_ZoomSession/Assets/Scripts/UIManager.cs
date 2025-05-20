using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    #region ��Ŭ�� ����
    //Singleton instance

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            // ���̾��Ű���� UIManager�� ã�� ������ ���� ����
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    instance = new GameObject() { name = "UIManager" }.AddComponent<UIManager>();
                }
            }
            return instance;
        }
    }
    #endregion
}
