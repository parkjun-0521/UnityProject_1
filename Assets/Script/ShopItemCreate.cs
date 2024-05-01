using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemCreate : MonoBehaviour
{
    int rand;
    int totalRand;
    public Transform[] itemCreatePos;
    int[] randNum;
    bool isCreate;

    void Start() {
        randNum = new int[itemCreatePos.Length];
    }

    void Update() {
        if (!isCreate)
            ItemCreate();
    }

    void ItemCreate() {
        for (int i = 0; i < itemCreatePos.Length; i++) {
            totalRand = Random.Range(1, 101);
            do {
                if (totalRand < 90)
                    rand = Random.Range(11, GameManager.instance.poolManager.prefabs.Length - 2);
                else
                    rand = Random.Range(GameManager.instance.poolManager.prefabs.Length - 2, GameManager.instance.poolManager.prefabs.Length);
            } while (IsDuplicate(rand, randNum, i));

            randNum[i] = rand;

            GameObject item = GameManager.instance.poolManager.GetObject(randNum[i]);
            item.transform.position = itemCreatePos[i].position;
            item.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            item.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        isCreate = true;
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
