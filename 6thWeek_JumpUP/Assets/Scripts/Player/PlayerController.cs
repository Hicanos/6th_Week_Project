using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed; // 움직이는 스피드
    public float jumpPower; // 점프하는 힘
    private Vector2 curMovementInput; // 현재 움직이는 방향과 힘을 저장하는 변수
    private Rigidbody rb; // Rigidbody 컴포넌트를 저장
    public LayerMask groundLayerMask; // 바닥 레이어 마스크


    [Header("Look")]
    public Transform cameraContainer; // 카메라를 담고 있는 컨테이너
    public float minXLook; // 카메라의 최소 X축 회전값
    public float maxXLook; // 카메라의 최대 X축 회전값
    private float camCurXRot; // 카메라의 현재 X축 회전값
    public float lookSensitivity; // 카메라 회전 민감도
    private Vector2 mouseDelta; // 마우스 이동값


    private bool isRightMouseDown = false; //오른쪽 마우스 버튼을 눌렀는지 확인하는 변수

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate() //물리 함수는 FixedUpdate에서 사용하는게 좋다
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
        Debug.Log("점프");
        rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse); //순간적으로 점프에 힘을 줌

    }
}
