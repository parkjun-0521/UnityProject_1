using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameKeyboardManager;
using UnityEngine.InputSystem;

public class ShopItemReRoll : MonoBehaviour
{
    public float cost;              // 리롤 비용 
    public bool isReRoll = false;  
    public TMP_Text valueTextMesh;  // 리롤 비용 Text 

    bool isPlayerCheck;             // 플레이어와 충돌 했을 때 확인 
    GameKeyboardManager keyboard;   

    void Start() {
        // 초기 리롤 비용 
        cost = 100;
        valueTextMesh.text = cost.ToString();
        keyboard = GameKeyboardManager.instance.GetComponent<GameKeyboardManager>();
    }

    void Update() {    
        // 플레이어가 리롤 머신과 충돌 하며 F 키를 눌렀을 시 
        if (Input.GetKeyDown(keyboard.GetKeyCode(KeyCodeTypes.Pickup)) && isPlayerCheck) {
            if (GameManager.instance.coinValue > cost) { 
                // coin 감소 
                GameManager.instance.coinValue -= (int)cost;

                // 리롤 
                isReRoll = true;

                // 30% 씩 가격 상승
                cost *= 1.3f;
                valueTextMesh.text = (Mathf.Round(cost)).ToString();
            }
            else
                Debug.Log("코인이 없습니다.");
        }
    }

    // 플레이어와 충돌 확인 
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
