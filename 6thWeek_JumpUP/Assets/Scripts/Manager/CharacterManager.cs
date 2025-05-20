using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance; // 캐릭터 매니저의 싱글톤 인스턴스

    public static CharacterManager Instance // 싱글톤 인스턴스를 반환하는 프로퍼티
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
                // 만약 인스턴스가 null이라면 새로운 게임 오브젝트를 생성하고 CharacterManager 컴포넌트를 추가
            }
            return instance;
            // 인스턴스가 null이 아닐 경우 기존의 인스턴스를 반환
        }
    }

    public Player player;
    // 플레이어 캐릭터를 저장하는 변수

    public Player Player
    {
        
        get { return player; } // 플레이어 캐릭터를 가져오는 프로퍼티
        set { player = value; } // 플레이어 캐릭터의 값을 설정
        // 플레이어 캐릭터를 설정하는 프로퍼티
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // 싱글톤 인스턴스를 현재 객체로 설정
            DontDestroyOnLoad(gameObject); // 파괴 금지 처리
        }
        else
        {
            if (instance == this)
            {
                Destroy(gameObject);
                // 중복 개체 파괴
            }
        }
    }
}
