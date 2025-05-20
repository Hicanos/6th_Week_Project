using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UIConditions uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public event Action onTakeDamage; // 대미지 입었을 때 호출되는 이벤트

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

    public void Die() //캐릭터 사망시 작동하는 로직
    {
        // 캐릭터 사망 시 화면에 메세지 출력, Restart 혹은 Exit 버튼을 눌러야 함
        // Restart 버튼을 누르면 게임이 다시 시작됨
        // Exit 버튼을 누르면 게임이 종료됨
        // UI매니저에서 UI를 활성화 시키고 게임매니저에서 게임 종료
        Debug.Log("플레이어가 죽었다.");
    }
}
