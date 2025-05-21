using System;
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

    [HideInInspector]
    public bool canLook = true;
    public Action inventory; // �κ��丮 ���� �̺�Ʈ

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
        if (canLook)
        {
            CameraLook();
        }
            
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
        if(context.phase == InputActionPhase.Started && IsGrounded()==true)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse); //���������� ������ ���� ��
            Debug.Log("����");
            //�����ϸ� ���¹̳� �Ҹ�
        }
        //���� ��ư�� ������ ��, IsGrounded()�� true�̸� ����
        else Debug.Log("���� �Ұ�");
    }

    bool IsGrounded() //true�̸� �ٴڿ� �������, false�� �ٴڿ� ������� ����
    {
        //�ٴڿ� ����ִ��� Ȯ��
        // 4���� Ray�� �����ϴ� �迭
        Ray[] rays = new Ray[4]
        {
            // �÷��̾��� ��ġ���� ��, ��, ������, �������� 0.2f ������ ��ġ�� Ray�� ���, �Ʒ� �������� ���
            // ����: �� ���⿡���� Ray�� ���, �ٴ��� ����̰ų� �𼭸��� ���� ��, �ٴڿ� ���� �ʴ� ��찡 �߻��� �� ����
            //������� ��(+transform.forward), ��(-transform.forward),������(+transform.right), ����(-transform.right)
            
            //1�� Ray: ������Ʈ�� �߾���ġ���� �ణ ����, ���� �ణ ���� �̵��� ��ġ���� �Ʒ��� Ray�� ���
            new(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            //2�� Ray: ������Ʈ�� �߾���ġ���� �ణ ����, ���� �ణ ���� �̵��� ��ġ���� �Ʒ��� Ray�� ���
            new(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            //3�� Ray: ������Ʈ�� �߾���ġ���� �ణ ������, ���� �ణ ���� �̵��� ��ġ���� �Ʒ��� Ray�� ���
            new(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            //4�� Ray: ������Ʈ�� �߾���ġ���� �ణ ����, ���� �ణ ���� �̵��� ��ġ���� �Ʒ��� Ray�� ���
            new(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        // �� Ray�� ���� �ٴ� ���̾�� �浹�ϴ��� Ȯ��
        for (int i = 0; i < rays.Length; i++)
        {
            // Ray�� �ð������� Ȯ���ϱ� ���� Debug.DrawRay ���
            Debug.DrawRay(rays[i].origin, rays[i].direction * 0.6f, Color.red);

            //Physics.Raycast�� ����Ͽ� Ray�� �ٴ� ���̾�� �浹�ϴ��� Ȯ��
            //rays[i]: �߻��� Ray(������ ������ Ray�� ���ʴ�� ���)
            //0.6f: Ray�� ����(0.6f��ŭ �Ʒ��� ��� ���𰡿� �浹�ߴ��� Ȯ��)
            //groundLayerMask: �ٴ� ���̾� ����ũ(Raycast�� ������ �ٴ� ���̾�)
            if (Physics.Raycast(rays[i], 0.6f, groundLayerMask))
            {
                Debug.Log("�ٴڿ� �������");
                return true;
            }
        }
        return false;
    }


    //�κ��丮 ���� ��ư�� ������ ȣ��
    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    // ���콺 Ŀ���� ���̰ų� ������ �ʰ� ��
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }


}
