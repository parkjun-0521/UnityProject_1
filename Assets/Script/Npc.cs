using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public int id;

    bool isPlayerCheck;

    UIManager uiManager;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && isPlayerCheck) {
            uiManager = UIManager.Instance.GetComponent<UIManager>();
            if(id == 1) {
                // ��ȭ NPC 
                uiManager.PlayerUpgradeNPC();
            }
            else if (id == 2) {
                // ���� NPC
                // ���� ��ü���� ���� 
                uiManager.WeaponNPC();
            }
            else if(id == 3) {
                // ������ NPC
                uiManager.ItemNPC();
            }
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
