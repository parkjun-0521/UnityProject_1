using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopItemCreate : MonoBehaviour
{
    int rand;
    int totalRand;
    int[] randNum;
    bool isCreate = false;

    public Transform[] itemCreatePos;   // ������ ���� ��ġ 
    public TMP_Text[] valueTextMesh;    // ������ ���� Text

    ShopItemReRoll shopItemReRoll;      // ���� �ӽ� ��ũ��Ʈ 

    void Start() {
        randNum = new int[itemCreatePos.Length];
        shopItemReRoll = GetComponentInChildren<ShopItemReRoll>();
    }

    void Update() {
        // ���� ������ ���� 
        if (!isCreate) {
            // Item tag ��� ��Ȱ��ȭ ( �ʱ�ȭ ) 
            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
            foreach (GameObject item in items) 
                item.SetActive(false);
            // ���� ���� ������ ���� 
            ItemCreate();   
        }
        
        if (shopItemReRoll.isReRoll) {
            // ������ ������ �� 
            ReRoll();
        }
    }

    // ���� ������ ���� 
    void ItemCreate() {
        for (int i = 0; i < itemCreatePos.Length; i++) {
            // Ȯ�� ���� ���� 
            totalRand = Random.Range(1, 101);
            // �ߺ� ���� ������ ��� 
            do {
                if (totalRand < 90)
                    rand = Random.Range(11, GameManager.instance.poolManager.prefabs.Length - 2);
                else
                    rand = Random.Range(GameManager.instance.poolManager.prefabs.Length - 2, GameManager.instance.poolManager.prefabs.Length);
            } while (IsDuplicate(rand, randNum, i));

            // �ߺ� ���� ������ �����ϱ� ���ؼ� �迭�� ����
            randNum[i] = rand;

            // �ش� ���� ������ �������� ���� 
            GameObject item = GameManager.instance.poolManager.GetObject(randNum[i]);

            // �������� ��ġ �� ���� �ʱ�ȭ 
            item.gameObject.GetComponent<ItemManager>().isThrowing = false;
            item.transform.position = itemCreatePos[i].position;
            item.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            item.GetComponent<Rigidbody2D>().gravityScale = 0;

            // �����ۿ� �ִ� cost ������ Ȱ���Ͽ� UI Text ǥ�� 
            valueTextMesh[i].text = item.gameObject.GetComponent<ItemManager>().upCost.ToString();
        }
        isCreate = true;
    }

    // ���� ������ ���� 
    void ReRoll() {
        // teg�� Item �ΰ��� �� ������ ��Ȱ��ȭ 
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items) {
            if (item.GetComponent<ItemManager>().isThrowing == true)
                continue;
            item.SetActive(false);
        }
        // ���� �ٽ� �������� ���� 
        ItemCreate();
        shopItemReRoll.isReRoll = false;
    }

    bool IsDuplicate( int value, int[] array, int length ) {
        for (int j = 0; j < length; j++) {
            if (value == array[j]) {
                return true;
            }
        }
        return false;
    }
}
