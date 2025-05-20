using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    private void Awake()
    {
        CharacterManager.Instance.Player = this; // ���۽� ĳ���� �Ŵ����� �÷��̾ �ش� ��ü�� ����
        controller = GetComponent<PlayerController>(); // �÷��̾� ��Ʈ�ѷ��� ������
    }
}
