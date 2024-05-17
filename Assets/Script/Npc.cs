using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameKeyboardManager;
using UnityEngine.InputSystem;

public class Npc : MonoBehaviour
{
    public int id;

    bool isPlayerCheck;

    int weaponDropCount = 0;
    bool isWeaponDrop = false;

    int itemDropCount = 0;
    public bool isItemDrop = false;

    public bool isPosionDrop = false;

    public GameObject[] weaponItem;
    public GameObject[] itemItem;

    UIManager uiManager;

    GameKeyboardManager keyboard;

    void Start() {
        keyboard = GameKeyboardManager.instance.GetComponent<GameKeyboardManager>();
    }
    void Update()
    {
        if ((Input.GetKeyDown(keyboard.GetKeyCode(KeyCodeTypes.Interaction))) && isPlayerCheck) {
            uiManager = UIManager.Instance.GetComponent<UIManager>();
            if(id == 1) {
                // ��ȭ NPC 
                uiManager.healthCostText.text = "X " + uiManager.healthCost.ToString();
                uiManager.speedCostText.text = "X " + uiManager.speedCost.ToString();
                uiManager.powerCostText.text = "X " + uiManager.powerCost.ToString();
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
            else if(id==4) {
                // ���� ��ȸ�� NPC
                ShopHealthNPC();
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
            int rand;
            int totalRand = Random.Range(1, 101);
            itemDropCount++;
            // Ȯ���� ������ ��� 
            // 10% Ȯ���� ������ ���� 2���� �������� �ϳ��� ���´�.
            if(totalRand < 90) 
                rand = Random.Range(11, GameManager.instance.poolManager.prefabs.Length - 2);
            else 
                rand = Random.Range(GameManager.instance.poolManager.prefabs.Length - 2, GameManager.instance.poolManager.prefabs.Length);   
            
            GameObject item = GameManager.instance.poolManager.GetObject(rand);
            item.GetComponent<ItemManager>().isThrowing = true;
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


    public void ShopHealthNPC() {
        if(!isPosionDrop) {
            GameObject item = GameManager.instance.poolManager.GetObject(10);
            item.transform.position = this.transform.position;
        }
        isPosionDrop = true;
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
