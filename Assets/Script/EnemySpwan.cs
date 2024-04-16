using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpwan : MonoBehaviour
{
    public Transform[] enemyPosition;
    void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            Debug.Log("Àû »ý¼º");
            int randSpwanEnemy = Random.Range(3, 6);
            for(int i = 0; i < randSpwanEnemy; i++) {
                GameObject enemy = GameManager.instance.poolManager.GetObject(2);
                enemy.transform.position = enemyPosition[i].transform.position;
                enemy.transform.rotation = Quaternion.identity;
            }
            gameObject.SetActive(false);
        }
    }
}
