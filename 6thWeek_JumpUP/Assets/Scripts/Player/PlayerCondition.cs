using System;
using System.Collections;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UIConditions uiCondition;
    public PlayerController controller; // 플레이어 컨트롤러를 저장하는 변수

    Condition health { get { return uiCondition.health; } }
    public Condition stamina { get { return uiCondition.stamina; } }
    public Condition speed { get { return uiCondition.speed; } }
    public Condition jumpPower { get { return uiCondition.jumpPower; } }

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

    // 아이템 착용하고 있는 동안 계속 증가하는 상태
    // 장착 해제시 값을 제거함
    public void SpeedUP()
    {

    }

    public void JumpUP()
    {

    }


    public IEnumerator ApplyBuffWithDuration(ConsumableType type, float value, float duration)
    {
        switch (type)
        {
            case ConsumableType.Speed:
                    speed.curValue += value; //스피드 현재수치+아이템 value추가
                controller.itemSpeed += value; //컨트롤러 스피드 현재수치+아이템 value추가 (실제 스피드 적용)
                yield return new WaitForSeconds(duration); //효과 지속시간
                speed.curValue -= value;
                controller.itemSpeed -= value; //컨트롤러 스피드 현재수치-아이템 value제거
                break;
            case ConsumableType.JumpPower:
                    jumpPower.curValue += value;
                controller.itemJumpPower += value; //컨트롤러 점프력 현재수치+아이템 value추가
                yield return new WaitForSeconds(duration);
                jumpPower.curValue -= value;
                controller.itemJumpPower -= value; //컨트롤러 점프력 현재수치-아이템 value제거
                break;
        }
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
