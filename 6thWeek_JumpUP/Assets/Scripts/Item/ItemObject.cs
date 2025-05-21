using UnityEngine;

public interface IInteractable
{ 
    public string GetInteractPrompt();//��ȣ�ۿ� ������Ʈ ��������
    public void OnInteract(); //��ȣ�ۿ�� ������
}
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data; //������ ������
    public string GetInteractPrompt()
    {
        string prompt = $"Press E to pick up {data.displayName}\n{data.description}"; //��ȣ�ۿ� ������Ʈ
        return prompt;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data; //�÷��̾��� ������ �����Ϳ� �ش� ������ ������ ����
        CharacterManager.Instance.Player.addItem?.Invoke(); //������ ȹ�� �̺�Ʈ �߻�
        Destroy(gameObject); //������ ������Ʈ ����
    }
}
