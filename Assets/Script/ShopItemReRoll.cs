using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameKeyboardManager;
using UnityEngine.InputSystem;

public class ShopItemReRoll : MonoBehaviour
{
    public float cost;              // ���� ��� 
    public bool isReRoll = false;  
    public TMP_Text valueTextMesh;  // ���� ��� Text 

    bool isPlayerCheck;             // �÷��̾�� �浹 ���� �� Ȯ�� 
    GameKeyboardManager keyboard;   

    void Start() {
        // �ʱ� ���� ��� 
        cost = 100;
        valueTextMesh.text = cost.ToString();
        keyboard = GameKeyboardManager.instance.GetComponent<GameKeyboardManager>();
    }

    void Update() {    
        // �÷��̾ ���� �ӽŰ� �浹 �ϸ� F Ű�� ������ �� 
        if (Input.GetKeyDown(keyboard.GetKeyCode(KeyCodeTypes.Pickup)) && isPlayerCheck) {
            if (GameManager.instance.coinValue > cost) { 
                // coin ���� 
                GameManager.instance.coinValue -= (int)cost;

                // ���� 
                isReRoll = true;

                // 30% �� ���� ���
                cost *= 1.3f;
                valueTextMesh.text = (Mathf.Round(cost)).ToString();
            }
            else
                Debug.Log("������ �����ϴ�.");
        }
    }

    // �÷��̾�� �浹 Ȯ�� 
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
