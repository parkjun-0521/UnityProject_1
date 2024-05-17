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

    public Transform[] itemCreatePos;   // 아이템 생성 위치 
    public TMP_Text[] valueTextMesh;    // 아이템 가격 Text

    ShopItemReRoll shopItemReRoll;      // 리롤 머신 스크립트 

    void Start() {
        randNum = new int[itemCreatePos.Length];
        shopItemReRoll = GetComponentInChildren<ShopItemReRoll>();
    }

    void Update() {
        // 상점 아이템 생성 
        if (!isCreate) {
            // Item tag 모두 비활성화 ( 초기화 ) 
            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
            foreach (GameObject item in items) 
                item.SetActive(false);
            // 상점 입장 아이템 생성 
            ItemCreate();   
        }
        
        if (shopItemReRoll.isReRoll) {
            // 리롤을 눌렀을 때 
            ReRoll();
        }
    }

    // 상점 아이템 생성 
    void ItemCreate() {
        for (int i = 0; i < itemCreatePos.Length; i++) {
            // 확률 변수 생성 
            totalRand = Random.Range(1, 101);
            // 중복 없는 난수를 출력 
            do {
                if (totalRand < 90)
                    rand = Random.Range(11, GameManager.instance.poolManager.prefabs.Length - 2);
                else
                    rand = Random.Range(GameManager.instance.poolManager.prefabs.Length - 2, GameManager.instance.poolManager.prefabs.Length);
            } while (IsDuplicate(rand, randNum, i));

            // 중복 없는 난수를 생성하기 위해서 배열에 저장
            randNum[i] = rand;

            // 해당 랜덤 변수의 아이템을 생성 
            GameObject item = GameManager.instance.poolManager.GetObject(randNum[i]);

            // 아이템의 위치 및 설정 초기화 
            item.gameObject.GetComponent<ItemManager>().isThrowing = false;
            item.transform.position = itemCreatePos[i].position;
            item.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            item.GetComponent<Rigidbody2D>().gravityScale = 0;

            // 아이템에 있는 cost 변수를 활용하여 UI Text 표기 
            valueTextMesh[i].text = item.gameObject.GetComponent<ItemManager>().upCost.ToString();
        }
        isCreate = true;
    }

    // 상점 아이템 리롤 
    void ReRoll() {
        // teg가 Item 인것을 다 가져와 비활성화 
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items) {
            if (item.GetComponent<ItemManager>().isThrowing == true)
                continue;
            item.SetActive(false);
        }
        // 이후 다시 아이템을 생성 
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
