using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// 카메라를 이용해 아이템을 상호작용하는 스크립트
public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // 체크 주기
    private float lastCheckTime; // 마지막 체크 시간
    public float maxCheckDistance; // 최대 체크 거리
    public LayerMask layerMask; // 레이어 마스크

    public GameObject curInteractGameObject; // 현재 상호작용하는 게임 오브젝트
    private IInteractable curInteractable; // 현재 상호작용하는 인터페이스

    public TextMeshProUGUI promptText; // 프롬프트 텍스트 UI
    public Camera cam; // 카메라

    void Start()
    {
        cam = Camera.main; // 메인 카메라 가져오기
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastCheckTime > checkRate) // 체크 주기마다
        {
            lastCheckTime = Time.time; // 마지막 체크 시간 업데이트
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // 카메라에서 중앙으로 향하는 레이 생성

            RaycastHit hit; // 레이캐스트 히트 정보

            if(Physics.Raycast(ray, out hit, maxCheckDistance, layerMask)) // 레이캐스트
            {
                //히트된 오브젝트가 방금 상호작용한 오브젝트가 아닐 경우
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    //현재 상호 중인 오브젝트를 히트된 오브젝트로 설정
                    //히트된 오브젝트가 IInteractable 인터페이스를 구현하고 있을 경우
                    //SetPromptText()를 호출하여 정보 표시
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else 
            {
                //상호작용 가능한 오브젝트가 없을 경우
                //현재 상호작용중인 오브젝트를 null로 설정 - promptText 비활성화
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
