using UnityEngine;

public class UIConditions : MonoBehaviour
{
    public Condition health;
    public Condition stamina;

    private void Start()
    {
        // 디버그용 메세지
        if (CharacterManager.Instance == null)
        {
            Debug.LogError("CharacterManager.Instance가 null입니다.");
            return;
        }
        if (CharacterManager.Instance.Player == null)
        {
            Debug.LogError("CharacterManager.Instance.Player가 null입니다.");
            return;
        }
        if (CharacterManager.Instance.Player.condition == null)
        {
            Debug.LogError("Player.condition이 null입니다.");
            return;
        }

        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
