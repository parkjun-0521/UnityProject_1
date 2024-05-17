using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItemReRoll : MonoBehaviour
{
    public float cost;   // 리롤 비용 
    public bool isReRoll = false;

    bool isPlayerCheck;

    public TMP_Text valueTextMesh;

    void Start() {
        cost = 100;
        valueTextMesh.text = cost.ToString();
    }

    void Update() {    
        if (Input.GetKeyDown(KeyCode.F) && isPlayerCheck) {
            if (GameManager.instance.coinValue > cost) { 
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
