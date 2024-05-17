using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpwan : MonoBehaviour
{
    public Transform[] enemyPosition;
    public GameObject middleBoss;
    public GameObject boss;
    public bool bossCheck;

    void Start() {
        bossCheck = false;
    }

    void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.CompareTag("Player") && SceneLoadManager.instance.mapCount % 5 != 0) {
            Debug.Log("利 积己");
            int randSpwanEnemy = Random.Range(3, enemyPosition.Length);
            for(int i = 0; i < randSpwanEnemy; i++) {
                int random = Random.Range(2, 5);
                GameObject enemy = GameManager.instance.poolManager.GetObject(random);
                enemy.transform.position = enemyPosition[i].transform.position;
                enemy.transform.rotation = Quaternion.identity;
            }
            gameObject.SetActive(false);
        }
        else if(collision.CompareTag("Player") && SceneLoadManager.instance.mapCount % 5 == 0 && SceneLoadManager.instance.mapCount % 10 != 0 && !bossCheck) {
            Debug.Log("利 积己");
            Instantiate(middleBoss, new Vector2(16f, -3f), transform.rotation);
            bossCheck = true;
        }
        else if(collision.CompareTag("Player") && SceneLoadManager.instance.mapCount % 10 == 0 && !bossCheck) {
            Instantiate(boss, new Vector2(16f, -3f), transform.rotation);
            bossCheck = true;
        }
    }
}
