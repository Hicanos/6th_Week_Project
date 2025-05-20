using UnityEngine;

[RequireComponent(typeof(CanvasGroup))] 
//UIBase 스크립트가 붙어있는 게임 오브젝트에 CanvasGroup 컴포넌트가 필수로 있어야 함
public class UIBase : MonoBehaviour
{
    private CanvasGroup _canvasGroup; //CanvasGroup 컴포넌트


    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>(); 
        //UIBase가 붙어있는 게임 오브젝트의 CanvasGroup 컴포넌트 가져오기   
    }

    public void SetAlpha(float alpha)
    {
        _canvasGroup.alpha = alpha; //CanvasGroup의 알파값 설정
    }

    public void Open()
    {
        gameObject.SetActive(value: true); //게임 오브젝트 활성화
    }
    public void Close()
    {
        gameObject.SetActive(value: false); //게임 오브젝트 비활성화
    }
}
