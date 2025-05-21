using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller; // 플레이어 컨트롤러를 저장하는 변수
    public PlayerCondition condition; // 플레이어의 상태를 저장하는 변수

    public ItemData itemData; // 플레이어가 소지한 아이템 데이터
    public Action addItem; // 아이템 획득 이벤트

    private void Awake()
    {
        CharacterManager.Instance.Player = this; // 시작시 캐릭터 매니저의 플레이어를 해당 개체로 생성
        controller = GetComponent<PlayerController>(); // 플레이어 컨트롤러를 가져옴
        condition = GetComponent<PlayerCondition>(); // 플레이어 상태를 가져옴
    }
}
