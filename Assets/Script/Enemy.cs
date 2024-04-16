using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // �⺻ �� 
    public int enemyValue;
    public float enemyHealth;    
    public float enemyMaxHealth;    
    public float enemySpeed;
    public float enemyPower;

    // �÷��̾� ���� 
    public float detectionRadius;

    // ���� ���� 
    public float enemyAttackcurDel;
    public float enemyAttackDelay;
    public float attackRadius;

    bool enemyDeathCheck = false;

    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider;
    Animator enemyAnime;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        enemyAnime = GetComponent<Animator>();
    }

    // ���������� ���� ���� ü�� ���� 
    public float HealthUp(int count) {
        if(count <= 1) {
            return 1;
        }
        else {
            return 1.5f * HealthUp(count-1);
        }
    }

    void OnEnable() {
        enemyHealth = enemyMaxHealth * HealthUp(SceneLoadManager.instance.stageCount);
        GameManager.instance.enemyCount += enemyValue;
    }

    void Start() {
    }


    void Update()
    {
        EnemyFollowMove();
        EnemyAttack();
        EnemyDead();
    }

    void EnemyFollowMove() {
        RaycastHit2D objectHitTag = Physics2D.CircleCast(transform.position, detectionRadius, Vector2.zero, 0f, LayerMask.GetMask("Player"));
        if(objectHitTag.collider != null) {
            Vector2 direction = (GameManager.instance.playerPrefab.transform.position - transform.position).normalized;

            // �̵� �������� �̵��մϴ�.
            transform.Translate(direction * enemySpeed * Time.deltaTime);
        }
    }

    // ���� ������ ���� ���� ������. 
    void EnemyAttack() {
        enemyAttackcurDel += Time.deltaTime;
        RaycastHit2D objectHitTag = Physics2D.CircleCast(transform.position, attackRadius, Vector2.zero, 0f, LayerMask.GetMask("Player"));
        if (objectHitTag.collider != null) {
            if (enemyAttackcurDel > enemyAttackDelay) {
                capsuleCollider.enabled = true;
            }
            else
                capsuleCollider.enabled = false;
        }
        else {
            enemyAttackcurDel = 0;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    void EnemyDead() {
        if(enemyHealth <= 0) {
            StartCoroutine(Invisible());
            return;
        }
    }

    IEnumerator Invisible() {
        if (!enemyDeathCheck) {
            Debug.Log("���� �׾����ϴ�.");
            enemyAnime.SetTrigger("Death");
            enemyDeathCheck = true;
        }
        yield return new WaitForSeconds(0.4f);
        GameManager.instance.enemyCount -= enemyValue;
        int random = Random.Range(2, 7);
        for (int i = 0; i < random; i++) {
            GameObject coin = GameManager.instance.poolManager.GetObject(4);
            coin.transform.position = this.transform.position;
        }
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D( Collider2D collision ) { 
        if (collision.CompareTag("FireBall")) {
            FireBall fireBallLogic = GameManager.instance.fireBallPrefab.GetComponent<FireBall>();
            enemyHealth -= fireBallLogic.damage;
            enemyAnime.SetTrigger("Hurt");
            Debug.Log(enemyHealth);
        }

        if (collision.gameObject.CompareTag("Player")) {
            Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
            if (!playerLogic.isDamaged) {
                enemyAttackcurDel = 0;
                playerLogic.health -= enemyPower;
                Debug.Log(playerLogic.health);
            }
        }
    }
}
