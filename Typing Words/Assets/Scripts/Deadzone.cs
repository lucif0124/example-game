using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Deadzone : MonoBehaviour
{
    // 주석 : 프로그래밍이 읽지 않음, 코멘트 용도로 사용
    // 트리거 충돌 시 한 번 호출
    // 나와 충돌한 상대방 충돌체의 정보를 other에 전달해준다.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Word"))
        {
            GameManager.Instance.LoseScore(100);
            // 실 사용 리스트에서 제거
            GameManager.Instance.wordList.Remove(other.GetComponent<TextMeshPro>());

            Destroy(other.gameObject);
        }
    }
}
