using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance; // ĳ���� �Ŵ����� �̱��� �ν��Ͻ�

    public static CharacterManager Instance // �̱��� �ν��Ͻ��� ��ȯ�ϴ� ������Ƽ
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
                // ���� �ν��Ͻ��� null�̶�� ���ο� ���� ������Ʈ�� �����ϰ� CharacterManager ������Ʈ�� �߰�
            }
            return instance;
            // �ν��Ͻ��� null�� �ƴ� ��� ������ �ν��Ͻ��� ��ȯ
        }
    }

    public Player player;
    // �÷��̾� ĳ���͸� �����ϴ� ����

    public Player Player
    {
        
        get { return player; } // �÷��̾� ĳ���͸� �������� ������Ƽ
        set { player = value; } // �÷��̾� ĳ������ ���� ����
        // �÷��̾� ĳ���͸� �����ϴ� ������Ƽ
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // �̱��� �ν��Ͻ��� ���� ��ü�� ����
            DontDestroyOnLoad(gameObject); // �ı� ���� ó��
        }
        else
        {
            if (instance == this)
            {
                Destroy(gameObject);
                // �ߺ� ��ü �ı�
            }
        }
    }
}
