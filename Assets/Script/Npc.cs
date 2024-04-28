using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public int id;

    bool isPlayerCheck;

    int weaponDropCount = 0;
    bool isWeaponDrop = false;

    int itemDropCount = 0;
    bool isItemDrop = false;

    public GameObject[] weaponItem;
    public GameObject[] itemItem;

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
            weaponDropCount++;
            int rand = Random.Range(0, weaponItem.Length);
            Instantiate(weaponItem[rand], transform.position, Quaternion.identity);
            if (weaponDropCount == 2) {
                isWeaponDrop = true;
                return;
            }
        }
        else {
            Debug.Log("���̻� ���⸦ ���� �� �����ϴ�.");
        }
    }

    public void ItemNPC() {
        if (!isItemDrop) {
            itemDropCount++;
            int rand = Random.Range(11, GameManager.instance.poolManager.prefabs.Length);
            GameObject item = GameManager.instance.poolManager.GetObject(rand);
            item.transform.position = this.transform.position;
            if (itemDropCount == 2) {
                isItemDrop = true;
                return;
            }
        }
        else {
            Debug.Log("���̻� �������� ���� �� �����ϴ�.");
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
