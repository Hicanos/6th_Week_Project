using UnityEngine;

public class UIConditions : MonoBehaviour
{
    public Condition health;
    public Condition stamina;
    public Condition speed;
    public Condition jumpPower;

    private void Start()
    {
        // ����׿� �޼���
        if (CharacterManager.Instance == null)
        {
            Debug.LogError("CharacterManager.Instance�� null�Դϴ�.");
            return;
        }
        if (CharacterManager.Instance.Player == null)
        {
            Debug.LogError("CharacterManager.Instance.Player�� null�Դϴ�.");
            return;
        }
        if (CharacterManager.Instance.Player.condition == null)
        {
            Debug.LogError("Player.condition�� null�Դϴ�.");
            return;
        }

        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
