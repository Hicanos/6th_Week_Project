using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    private void Awake()
    {
        CharacterManager.Instance.Player = this; // 시작시 캐릭터 매니저의 플레이어를 해당 개체로 생성
        controller = GetComponent<PlayerController>(); // 플레이어 컨트롤러를 가져옴
    }
}
