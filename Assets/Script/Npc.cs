using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public int id;

    bool isPlayerCheck;

    int WeaponDropCount = 0;
    bool isWeaponDrop = false;

    public GameObject[] weaponItem;

    UIManager uiManager;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && isPlayerCheck) {
            uiManager = UIManager.Instance.GetComponent<UIManager>();
            if(id == 1) {
                // 강화 NPC 
                uiManager.PlayerUpgradeNPC();
            }
            else if (id == 2) {
                // 무기 NPC
                // 무기 교체까지 구현 
                WeaponNPC();
            }
            else if(id == 3) {
                // 아이템 NPC
                ItemNPC();
            }
        }
    }

    public void WeaponNPC() {
        if (!isWeaponDrop) {
            WeaponDropCount++;
            int rand = Random.Range(0, weaponItem.Length);
            Instantiate(weaponItem[rand], transform.position, Quaternion.identity);
            if (WeaponDropCount == 2) {
                isWeaponDrop = true;
                return;
            }
        }
        else {
            Debug.Log("더이상 무기를 받을 수 없습니다.");
        }
    }

    public void ItemNPC() {
        Debug.Log("아이템을 하나 줍니다.");
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
