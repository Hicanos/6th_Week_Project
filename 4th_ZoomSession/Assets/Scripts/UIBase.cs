using UnityEngine;

[RequireComponent(typeof(CanvasGroup))] 
//UIBase ��ũ��Ʈ�� �پ��ִ� ���� ������Ʈ�� CanvasGroup ������Ʈ�� �ʼ��� �־�� ��
public class UIBase : MonoBehaviour
{
    private CanvasGroup _canvasGroup; //CanvasGroup ������Ʈ


    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>(); 
        //UIBase�� �پ��ִ� ���� ������Ʈ�� CanvasGroup ������Ʈ ��������   
    }

    public void SetAlpha(float alpha)
    {
        _canvasGroup.alpha = alpha; //CanvasGroup�� ���İ� ����
    }

    public void Open()
    {
        gameObject.SetActive(value: true); //���� ������Ʈ Ȱ��ȭ
    }
    public void Close()
    {
        gameObject.SetActive(value: false); //���� ������Ʈ ��Ȱ��ȭ
    }
}
