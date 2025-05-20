using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed; // �����̴� ���ǵ�
    public float jumpPower; // �����ϴ� ��
    private Vector2 curMovementInput; // ���� �����̴� ����� ���� �����ϴ� ����
    private Rigidbody rb; // Rigidbody ������Ʈ�� ����
    public LayerMask groundLayerMask; // �ٴ� ���̾� ����ũ


    [Header("Look")]
    public Transform cameraContainer; // ī�޶� ��� �ִ� �����̳�
    public float minXLook; // ī�޶��� �ּ� X�� ȸ����
    public float maxXLook; // ī�޶��� �ִ� X�� ȸ����
    private float camCurXRot; // ī�޶��� ���� X�� ȸ����
    public float lookSensitivity; // ī�޶� ȸ�� �ΰ���
    private Vector2 mouseDelta; // ���콺 �̵���


    private bool isRightMouseDown = false; //������ ���콺 ��ư�� �������� Ȯ���ϴ� ����

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate() //���� �Լ��� FixedUpdate���� ����ϴ°� ����
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y
                      + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y;
        //������ ���� ���� ���Ʒ��� ������. ��, �׳� ������ ���� ��� ���� ����

        rb.velocity = dir;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    void CameraLook()
    {
        if (!isRightMouseDown) return; //������ ���콺 ��ư�� ������ �ʾҴٸ�, ī�޶� ȸ������ ����
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }



    public void OnLook(InputAction.CallbackContext context)
    {

        mouseDelta = context.ReadValue<Vector2>();
    }

    public void LookButton(InputAction.CallbackContext context)
    {
        //������ ���콺 ��ư�� ������, OnLook�� ȣ���
        //������ ���콺 ��ư�� �����, OnLook�� ������
        if (context.phase == InputActionPhase.Started)
        {
            isRightMouseDown = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRightMouseDown = false;
            mouseDelta = Vector2.zero; //���콺 ��ư�� ����, ���콺 �̵����� 0���� �ʱ�ȭ
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("����");
        rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse); //���������� ������ ���� ��

    }
}
