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
                // ��ȭ NPC 
                uiManager.PlayerUpgradeNPC();
            }
            else if (id == 2) {
                // ���� NPC
                // ���� ��ü���� ���� 
                WeaponNPC();
            }
            else if(id == 3) {
                // ������ NPC
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
            Debug.Log("���̻� ���⸦ ���� �� �����ϴ�.");
        }
    }

    public void ItemNPC() {
        Debug.Log("�������� �ϳ� �ݴϴ�.");
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
