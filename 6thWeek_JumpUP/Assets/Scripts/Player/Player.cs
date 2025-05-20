using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller; // �÷��̾� ��Ʈ�ѷ��� �����ϴ� ����
    public PlayerCondition condition; // �÷��̾��� ���¸� �����ϴ� ����

    private void Awake()
    {
        CharacterManager.Instance.Player = this; // ���۽� ĳ���� �Ŵ����� �÷��̾ �ش� ��ü�� ����
        controller = GetComponent<PlayerController>(); // �÷��̾� ��Ʈ�ѷ��� ������
        condition = GetComponent<PlayerCondition>(); // �÷��̾� ���¸� ������
    }
}
