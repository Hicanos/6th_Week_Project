using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// ī�޶� �̿��� �������� ��ȣ�ۿ��ϴ� ��ũ��Ʈ
public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // üũ �ֱ�
    private float lastCheckTime; // ������ üũ �ð�
    public float maxCheckDistance; // �ִ� üũ �Ÿ�
    public LayerMask layerMask; // ���̾� ����ũ

    public GameObject curInteractGameObject; // ���� ��ȣ�ۿ��ϴ� ���� ������Ʈ
    private IInteractable curInteractable; // ���� ��ȣ�ۿ��ϴ� �������̽�

    public TextMeshProUGUI promptText; // ������Ʈ �ؽ�Ʈ UI
    public Camera cam; // ī�޶�

    void Start()
    {
        cam = Camera.main; // ���� ī�޶� ��������
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastCheckTime > checkRate) // üũ �ֱ⸶��
        {
            lastCheckTime = Time.time; // ������ üũ �ð� ������Ʈ
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // ī�޶󿡼� �߾����� ���ϴ� ���� ����

            RaycastHit hit; // ����ĳ��Ʈ ��Ʈ ����

            if(Physics.Raycast(ray, out hit, maxCheckDistance, layerMask)) // ����ĳ��Ʈ
            {
                //��Ʈ�� ������Ʈ�� ��� ��ȣ�ۿ��� ������Ʈ�� �ƴ� ���
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    //���� ��ȣ ���� ������Ʈ�� ��Ʈ�� ������Ʈ�� ����
                    //��Ʈ�� ������Ʈ�� IInteractable �������̽��� �����ϰ� ���� ���
                    //SetPromptText()�� ȣ���Ͽ� ���� ǥ��
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else 
            {
                //��ȣ�ۿ� ������ ������Ʈ�� ���� ���
                //���� ��ȣ�ۿ����� ������Ʈ�� null�� ���� - promptText ��Ȱ��ȭ
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
