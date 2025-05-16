using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugExscripts : MonoBehaviour
{
    public delegate void TestDelegate();
    public TestDelegate testDelegate;

    public delegate void TestDelegate2(int A);
    public TestDelegate2 testDelegate2;


    private void Start()
    {
        Show1();
        Show2();
    }
    public void Show1()
    {
        testDelegate = TestFun;

        testDelegate.Invoke();
    }

    public void TestFun()
    {
        Debug.Log("�̰� �׽�Ʈ �뵵�Դϴ�.");
    }

    public void Show2()
    {
        testDelegate2 = TestFun2;
        testDelegate2.Invoke(10);
    }

    public void TestFun2(int A)
    {
        Debug.Log("�̰� �׽�Ʈ �뵵�Դϴ�." + A);
    }


}
