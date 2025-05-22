using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UIConditions uiCondition;
    public PlayerController controller; // 플레이어 컨트롤러를 저장하는 변수

    Condition health { get { return uiCondition.health; } }
    public Condition stamina { get { return uiCondition.stamina; } }

    public float originSpeed; // 원래 스피드
    public float originJumpPower; // 원래 점프력
    private float lastYPosition; // 마지막 Y좌표를 저장하는 변수



    public event Action onTakeDamage; // 대미지 입었을 때 호출되는 이벤트
    public event Action onStaminaChange; // 스태미너 변화 시 호출되는 이벤트
    // 체력: 높은 곳에서 떨어졌을 때 소모됨 : 낙차계산 필요
    // 스태미나: 대쉬, 점프 등으로 소모됨


    private void Update()
    {
        // 체력과 스태미너의 자연 회복
        health.Add(health.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);


        if (health.curValue < 0f)
        {
            Die();
        }
    }
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void RecoverStamina(float amount)
    {
        stamina.Add(amount);
        onStaminaChange?.Invoke();
    }

    public void BeTired(float amount)
    {
        stamina.Subtract(amount);
        onStaminaChange?.Invoke();
    }

    public void TakeDamage(float amount)
    {
        // 대미지 입었을 때 호출되는 이벤트
        health.Subtract(amount);
        onTakeDamage?.Invoke();
    }

    public void SpeedUP(float amount)
    {
        //스피드 증가, 효과가 끝나면 본래 속도로(코루틴)
        originSpeed = controller.moveSpeed; // 원래 스피드 저장
        controller.moveSpeed += amount;
    }

    public void JumpUP(float amount)
    {
        //점프력 증가, 효과가 끝나면 본래 점프력으로(코루틴)
        originJumpPower = controller.jumpPower; // 원래 점프력 저장
        controller.jumpPower += amount;
    }

    public void Die() //캐릭터 사망시 작동하는 로직
    {
        // 캐릭터 사망 시 화면에 메세지 출력, Restart 혹은 Exit 버튼을 눌러야 함
        // Restart 버튼을 누르면 게임이 다시 시작됨
        // Exit 버튼을 누르면 게임이 종료됨
        // UI매니저에서 UI를 활성화 시키고 게임매니저에서 게임 종료
        Debug.Log("플레이어가 죽었다.");
    }
}
