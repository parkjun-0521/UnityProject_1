using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpwan : MonoBehaviour
{
    public GameObject[] enemies;
    public Transform[] enemyPosition;
    void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            Debug.Log("Àû »ý¼º");
            int randSpwanEnemy = Random.Range(3, 11);
            for(int i = 0; i < randSpwanEnemy; i++) {
                Instantiate(enemies[0], enemyPosition[i].transform.position, Quaternion.identity);
            }
            gameObject.SetActive(false);
        }
    }
}
