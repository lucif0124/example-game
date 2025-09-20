using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Deadzone : MonoBehaviour
{
    // �ּ� : ���α׷����� ���� ����, �ڸ�Ʈ �뵵�� ���
    // Ʈ���� �浹 �� �� �� ȣ��
    // ���� �浹�� ���� �浹ü�� ������ other�� �������ش�.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Word"))
        {
            GameManager.Instance.LoseScore(100);
            // �� ��� ����Ʈ���� ����
            GameManager.Instance.wordList.Remove(other.GetComponent<TextMeshPro>());

            Destroy(other.gameObject);
        }
    }
}
