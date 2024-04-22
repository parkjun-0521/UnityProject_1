using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    // �⺻ �� 
    public int enemyValue;
    public float enemyBasicHealth;
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
    bool enemyHitCheck = false;

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
        enemyMaxHealth = HealthUp(SceneLoadManager.instance.stageCount) * enemyBasicHealth;
        enemyHealth = enemyMaxHealth;
        GameManager.instance.enemyCount += enemyValue;
        enemyHitCheck = false;
        enemyDeathCheck = false;
    }

    void Start() {
    }


    void Update()
    {
        EnemyMainSceneDestroy();
        EnemyFollowMove();
        EnemyAttack();
        EnemyDead();
    }

    void EnemyMainSceneDestroy() {
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            gameObject.SetActive(false);
        }
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
        GameManager.instance.enemyKillCount++;

        // worldCoinValue ��� �� �� �ʿ��� ���� �� 
        GameManager.instance.enemyTotal += enemyValue;


        int random = Random.Range(2, 7);
        for (int i = 0; i < random; i++) {
            GameObject coin = GameManager.instance.poolManager.GetObject(4);
            coin.transform.position = this.transform.position;
        }
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D( Collider2D collision ) { 
        if (collision.CompareTag("FireBall") && !enemyHitCheck) {
            FireBall fireBallLogic = GameManager.instance.fireBallPrefab.GetComponent<FireBall>();
            enemyHealth -= fireBallLogic.damage;
            enemyHitCheck = true;
            enemyAnime.SetTrigger("Hurt");
            StartCoroutine(EnemyHitCheck());
            Debug.Log(enemyHealth);
        }

        // �÷��̾ �뽬 �����̸� �������� ���� �ʴ´�. ( ���� ���� ) 
        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        if (collision.gameObject.CompareTag("Player") && !playerLogic.isDashCheck && !playerLogic.isDamaged) {
            if (!playerLogic.isDamaged) {
                enemyAttackcurDel = 0;
                playerLogic.health -= enemyPower;
                Debug.Log(playerLogic.health);
            }
        }
    }

    IEnumerator EnemyHitCheck() {
        yield return new WaitForSeconds(0.1f);
        enemyHitCheck = false;
    }


}
