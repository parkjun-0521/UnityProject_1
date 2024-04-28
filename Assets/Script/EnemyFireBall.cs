using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireBall : MonoBehaviour
{
    public int damage;

    Rigidbody2D rigid;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnEnable() {
        StartCoroutine(Destroy());
    }

    void Update()
    {
        if (GameManager.instance.playerPrefab.GetComponent<Player>().health <= 0) {
            gameObject.SetActive(false);
        }
    }

    IEnumerator Destroy() {
        yield return new WaitForSeconds(3f);
        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.CompareTag("Floor")) {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }

        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        if (collision.CompareTag("Player") && !playerLogic.isDashCheck && !playerLogic.isDamaged) {
            playerLogic.health -= damage;
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
            Debug.Log(playerLogic.health);
        }

    }
}
