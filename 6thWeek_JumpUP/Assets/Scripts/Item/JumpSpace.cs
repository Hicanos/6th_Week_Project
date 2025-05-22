using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSpace : MonoBehaviour
{
    public float jumpPower; // 점프대의 점프력

    //접근한 것이 Player라면, JumpSpace의 점프기능을 사용. 플레이어 개체를 높이 점프시킴.
    // 플레이어가 점프대에 있는 동안 계속 점프하게 함
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어의 Rigidbody 컴포넌트를 가져옴
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // 플레이어를 위로 점프시킴
                playerRigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning("플레이어의 Rigidbody를 찾을 수 없습니다.");
            }
        }
    }
}