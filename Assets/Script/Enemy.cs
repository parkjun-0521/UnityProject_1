using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyValue;
    public float enemyHealth;    
    public float enemySpeed;
    public float enemyPower;

    public float detectionRadius;

    bool enemyDeathCheck = false;

    Rigidbody2D rigid;
    Animator enemyAnime;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        enemyAnime = GetComponent<Animator>();
    }

    void Start() {
        GameManager.instance.enemyCount += enemyValue;
    }


    void Update()
    {
        EnemyFollowMove();
        EnemyDead();
    }

    void EnemyFollowMove() {
        RaycastHit2D objectHitTag = Physics2D.CircleCast(transform.position, detectionRadius, Vector2.zero, 0f, LayerMask.GetMask("Player"));
        if(objectHitTag.collider != null) {
            Vector3 direction = (GameManager.instance.playerPrefab.transform.position - transform.position).normalized;

            // 이동 방향으로 이동합니다.
            transform.Translate(direction * enemySpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void EnemyDead() {
        if(enemyHealth <= 0) {
            StartCoroutine(Invisible());
            return;
        }
    }

    IEnumerator Invisible() {
        if (!enemyDeathCheck) {
            Debug.Log("적이 죽었습니다.");
            enemyAnime.SetTrigger("Death");
            enemyDeathCheck = true;
        }
        yield return new WaitForSeconds(0.4f);
        GameManager.instance.enemyCount -= enemyValue;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D( Collider2D collision ) { 
        if (collision.CompareTag("FireBall")) {
            FireBall fireBallLogic = GameManager.instance.fireBallPrefab.GetComponent<FireBall>();
            enemyHealth -= fireBallLogic.damage;
            enemyAnime.SetTrigger("Hurt");
            Debug.Log(enemyHealth);
        }
    }
}
