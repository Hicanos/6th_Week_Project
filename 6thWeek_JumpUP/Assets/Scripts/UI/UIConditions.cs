using UnityEngine;

public class UIConditions : MonoBehaviour
{
    public Condition health;
    public Condition stamina;
    public Condition speed;
    public Condition jumpPower;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
