using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSpace : MonoBehaviour
{
    public float jumpPower; // �������� ������

    //������ ���� Player���, JumpSpace�� ��������� ���. �÷��̾� ��ü�� ���� ������Ŵ.
    // �÷��̾ �����뿡 �ִ� ���� ��� �����ϰ� ��
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾��� Rigidbody ������Ʈ�� ������
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // �÷��̾ ���� ������Ŵ
                playerRigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning("�÷��̾��� Rigidbody�� ã�� �� �����ϴ�.");
            }
        }
    }
}