using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed; // 움직이는 스피드
    private Vector2 curMovementInput; // 현재 움직이는 방향과 힘을 저장하는 변수
    private Rigidbody rb; // Rigidbody 컴포넌트를 저장
    public LayerMask groundLayerMask; // 바닥 레이어 마스크

    [Header("Jump")]
    public float jumpPower; // 점프하는 힘
    public float jumpStamina; // 점프하는 힘을 위한 스태미너

    [Header("Dash")]
    public float dashPower; // 대쉬하는 힘
    public float dashStamina; // 대쉬하는 힘을 위한 스태미너
    private bool IsDash = false; // 대쉬 중인지 확인하는 변수


    [Header("Look")]
    public Transform cameraContainer; // 카메라를 담고 있는 컨테이너
    public float minXLook; // 카메라의 최소 X축 회전값
    public float maxXLook; // 카메라의 최대 X축 회전값
    private float camCurXRot; // 카메라의 현재 X축 회전값
    public float lookSensitivity; // 카메라 회전 민감도
    private Vector2 mouseDelta; // 마우스 이동값

    [HideInInspector]
    public bool canLook = true;
    public Action inventory; // 인벤토리 열기 이벤트

    private bool isRightMouseDown = false; //오른쪽 마우스 버튼을 눌렀는지 확인하는 변수
    PlayerCondition condition; // 플레이어 상태를 저장하는 변수

    [Header("ItemEffect")]
    public float itemSpeed; // 아이템 속도
    public float itemJumpPower; // 아이템 점프력


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        condition = GetComponent<PlayerCondition>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate() //물리 함수는 FixedUpdate에서 사용하는게 좋다
    {
        Move();

        if (IsDash)
        {
            //대쉬 중일 때는 초당 dashStamina만큼 스태미너 소모
            condition.BeTired(dashStamina * Time.fixedDeltaTime);


            if (condition.stamina.curValue <= 0)
            {
                //대쉬 종료 시 일반 속도로 이동
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
            dir *= (moveSpeed + itemSpeed); //이동속도는 기본값 + 아이템 속도
        }
        else if(IsDash == true) //대쉬 중일 때는 대쉬 속도 적용
        {
            dir *= (dashPower + itemSpeed);
        }
        dir.y = rb.velocity.y;
        //점프를 했을 때만 위아래로 움직임. 즉, 그냥 움직일 때는 통상 상태 유지

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
        if (!isRightMouseDown) return; //오른쪽 마우스 버튼을 누르지 않았다면, 카메라 회전하지 않음
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
        //오른쪽 마우스 버튼을 누르면, OnLook이 호출됨
        //오른쪽 마우스 버튼을 떼어내면, OnLook은 중지됨
        if (context.phase == InputActionPhase.Started)
        {
            isRightMouseDown = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRightMouseDown = false;
            mouseDelta = Vector2.zero; //마우스 버튼을 떼면, 마우스 이동값을 0으로 초기화
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
               
        //stamina가 점프에 필요한 양보다 많을 때만 점프 가능
        if (condition.stamina.curValue < jumpStamina)
        {
            return;
        }

        if (context.phase == InputActionPhase.Started && IsGrounded()==true)
        {

            //순간적으로 점프에 힘을 줌, jumpPower를 증가시키는 아이템을 먹으면 해당 값만큼 추가됨
            rb.AddForce(Vector2.up * (jumpPower+itemJumpPower), ForceMode.Impulse);

            //점프하면 스태미너 소모            
            condition.BeTired(jumpStamina);
        }
        //점프 버튼을 눌렀을 때, IsGrounded()가 true이면 점프
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        //대쉬 버튼을 눌렀을 때, IsGrounded()가 true이면 대쉬
        //대쉬는 버튼을 누르고 있는 동안 지속되며, 일정한 비율로 스태미너를 소모함 (대쉬 버튼을 떼면 대쉬 종료)
        //소모되는 스태미너는 대쉬에 필요한 양보다 많을 때만 대쉬 가능 (maxStamina가 0이 될때까지 지속적으로 감소. 스태미너 최대일 때 총 10초동안 대쉬가능)
        //대쉬는 Move처럼, transform.forward방향으로 이동하며 지속적으로 힘을 줌
        //초당 감소하는 스태미너 계산 필요 (Lerp 0~1까지로 환원되어야함)

        // 대쉬 버튼을 누름> IsDash=true, Move()에서 대쉬 속도 적용

        if (condition.stamina.curValue < dashStamina)
        {
            return;
        }

        if (context.phase == InputActionPhase.Performed && IsGrounded() == true)
        {
            //대쉬 버튼을 누르고 있는 동안 대쉬 지속
            IsDash = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            IsDash =false; //대쉬 버튼을 떼면 대쉬 종료
        }

    }

    public bool IsGrounded() //true이면 바닥에 닿아있음, false면 바닥에 닿아있지 않음
    {
        //바닥에 닿아있는지 확인
        // 4개의 Ray를 저장하는 배열
        Ray[] rays = new Ray[4]
        {
            // 플레이어의 위치에서 앞, 뒤, 오른쪽, 왼쪽으로 0.2f 떨어진 위치에 Ray를 쏘고, 아래 방향으로 쏘기
            // 이유: 한 방향에서만 Ray를 쏘면, 바닥이 경사이거나 모서리에 있을 때, 바닥에 닿지 않는 경우가 발생할 수 있음
            //순서대로 앞(+transform.forward), 뒤(-transform.forward),오른쪽(+transform.right), 왼쪽(-transform.right)
            
            //1번 Ray: 오브젝트의 중앙위치에서 약간 앞쪽, 아주 약간 위로 이동한 위치에서 아래로 Ray를 쏘기
            new(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            //2번 Ray: 오브젝트의 중앙위치에서 약간 뒤쪽, 아주 약간 위로 이동한 위치에서 아래로 Ray를 쏘기
            new(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            //3번 Ray: 오브젝트의 중앙위치에서 약간 오른쪽, 아주 약간 위로 이동한 위치에서 아래로 Ray를 쏘기
            new(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            //4번 Ray: 오브젝트의 중앙위치에서 약간 왼쪽, 아주 약간 위로 이동한 위치에서 아래로 Ray를 쏘기
            new(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        // 각 Ray에 대해 바닥 레이어와 충돌하는지 확인
        for (int i = 0; i < rays.Length; i++)
        {
            // Ray를 시각적으로 확인하기 위해 Debug.DrawRay 사용
            Debug.DrawRay(rays[i].origin, rays[i].direction * 0.6f, Color.red);

            //Physics.Raycast를 사용하여 Ray가 바닥 레이어와 충돌하는지 확인
            //rays[i]: 발사할 Ray(위에서 정의한 Ray를 차례대로 사용)
            //0.6f: Ray의 길이(0.6f만큼 아래로 쏘아 무언가와 충돌했는지 확인)
            //groundLayerMask: 바닥 레이어 마스크(Raycast가 감지할 바닥 레이어)
            if (Physics.Raycast(rays[i], 0.6f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }


    //인벤토리 열기 버튼을 누르면 호출
    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    // 마우스 커서를 보이거나 보이지 않게 함
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }


}
