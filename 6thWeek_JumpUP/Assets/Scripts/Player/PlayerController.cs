using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed; // �����̴� ���ǵ�
    private Vector2 curMovementInput; // ���� �����̴� ����� ���� �����ϴ� ����
    private Rigidbody rb; // Rigidbody ������Ʈ�� ����
    public LayerMask groundLayerMask; // �ٴ� ���̾� ����ũ

    [Header("Jump")]
    public float jumpPower; // �����ϴ� ��
    public float jumpStamina; // �����ϴ� ���� ���� ���¹̳�

    [Header("Dash")]
    public float dashPower; // �뽬�ϴ� ��
    public float dashStamina; // �뽬�ϴ� ���� ���� ���¹̳�
    private bool IsDash = false; // �뽬 ������ Ȯ���ϴ� ����


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
    PlayerCondition condition; // �÷��̾� ���¸� �����ϴ� ����

    [Header("ItemEffect")]
    public float itemSpeed; // ������ �ӵ�
    public float itemJumpPower; // ������ ������


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        condition = GetComponent<PlayerCondition>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate() //���� �Լ��� FixedUpdate���� ����ϴ°� ����
    {
        Move();

        if (IsDash)
        {
            //�뽬 ���� ���� �ʴ� dashStamina��ŭ ���¹̳� �Ҹ�
            condition.BeTired(dashStamina * Time.fixedDeltaTime);


            if (condition.stamina.curValue <= 0)
            {
                //�뽬 ���� �� �Ϲ� �ӵ��� �̵�
                IsDash = false;
            }
        }
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
        if (IsDash == false)
        {
            dir *= (moveSpeed + itemSpeed); //�̵��ӵ��� �⺻�� + ������ �ӵ�
        }
        else if(IsDash == true) //�뽬 ���� ���� �뽬 �ӵ� ����
        {
            dir *= (dashPower + itemSpeed);
        }
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
               
        //stamina�� ������ �ʿ��� �纸�� ���� ���� ���� ����
        if (condition.stamina.curValue < jumpStamina)
        {
            return;
        }

        if (context.phase == InputActionPhase.Started && IsGrounded()==true)
        {

            //���������� ������ ���� ��, jumpPower�� ������Ű�� �������� ������ �ش� ����ŭ �߰���
            rb.AddForce(Vector2.up * (jumpPower+itemJumpPower), ForceMode.Impulse);

            //�����ϸ� ���¹̳� �Ҹ�            
            condition.BeTired(jumpStamina);
        }
        //���� ��ư�� ������ ��, IsGrounded()�� true�̸� ����
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        //�뽬 ��ư�� ������ ��, IsGrounded()�� true�̸� �뽬
        //�뽬�� ��ư�� ������ �ִ� ���� ���ӵǸ�, ������ ������ ���¹̳ʸ� �Ҹ��� (�뽬 ��ư�� ���� �뽬 ����)
        //�Ҹ�Ǵ� ���¹̳ʴ� �뽬�� �ʿ��� �纸�� ���� ���� �뽬 ���� (maxStamina�� 0�� �ɶ����� ���������� ����. ���¹̳� �ִ��� �� �� 10�ʵ��� �뽬����)
        //�뽬�� Moveó��, transform.forward�������� �̵��ϸ� ���������� ���� ��
        //�ʴ� �����ϴ� ���¹̳� ��� �ʿ� (Lerp 0~1������ ȯ���Ǿ����)

        // �뽬 ��ư�� ����> IsDash=true, Move()���� �뽬 �ӵ� ����

        if (condition.stamina.curValue < dashStamina)
        {
            return;
        }

        if (context.phase == InputActionPhase.Performed && IsGrounded() == true)
        {
            //�뽬 ��ư�� ������ �ִ� ���� �뽬 ����
            IsDash = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            IsDash =false; //�뽬 ��ư�� ���� �뽬 ����
        }

    }

    public bool IsGrounded() //true�̸� �ٴڿ� �������, false�� �ٴڿ� ������� ����
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
