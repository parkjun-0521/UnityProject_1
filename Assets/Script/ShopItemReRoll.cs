using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemReRoll : MonoBehaviour
{
    public float cost;   // ���� ��� 
    public bool isReRoll = false;

    bool isPlayerCheck;

    void Start() {
        cost = 100;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F) && isPlayerCheck) {
            if (GameManager.instance.coinValue > cost) {
                GameManager.instance.coinValue -= (int)cost;

                // ���� 
                isReRoll = true;

                // 30% �� ���� ���
                cost *= 1.3f;
            }
            else
                Debug.Log("������ �����ϴ�.");
        }
    }

    void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            isPlayerCheck = true;
        }
    }

    void OnTriggerExit2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            isPlayerCheck = false;
        }
    }
}
