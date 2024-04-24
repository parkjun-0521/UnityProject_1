using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpwan : MonoBehaviour
{
    public Transform[] enemyPosition;
    public GameObject middleBoss;
    public bool bossCheck;
    void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.CompareTag("Player") && SceneLoadManager.instance.mapCount % 5 != 0) {
            Debug.Log("�� ����");
            int randSpwanEnemy = Random.Range(3, 6);
            for(int i = 0; i < randSpwanEnemy; i++) {
                GameObject enemy = GameManager.instance.poolManager.GetObject(2);
                enemy.transform.position = enemyPosition[i].transform.position;
                enemy.transform.rotation = Quaternion.identity;
            }
            gameObject.SetActive(false);
        }
        else if(collision.CompareTag("Player") && SceneLoadManager.instance.mapCount % 5 == 0 && SceneLoadManager.instance.mapCount % 10 != 0 && !bossCheck) {
            Debug.Log("�� ����");
            Instantiate(middleBoss);
            bossCheck = true;
        }
    }
}
