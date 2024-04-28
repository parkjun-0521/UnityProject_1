using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.VersionControl.Asset;

public class Enemy : MonoBehaviour
{
    public enum Type{
        enemy_S,
        enemy_M,
        enemy_B
    }
    public Type enemyType;

    // 기본 값 
    public int enemyId = 0;
    public int enemyValue;
    public float enemyBasicHealth;
    public float enemyHealth;    
    public float enemyMaxHealth;    
    public float enemySpeed;
    public float enemyPower;

    // 플레이어 감지 
    public float detectionRadius;

    // 공격 변수 
    public float enemyAttackcurDel;
    public float enemyAttackDelay;
    public float attackRadius;

    bool enemyDeathCheck = false;
    bool enemyHitCheck = false;

    // 보스 패턴 관련
    public int patternIndex;
    public int pastPatternIndex;
    EnemyNiddleBoss1Controller enemyNiddleBoss1Controller;

    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider;
    Animator enemyAnime;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        enemyAnime = GetComponent<Animator>();
        enemyNiddleBoss1Controller = GetComponent<EnemyNiddleBoss1Controller>();
    }

    // 스테이지에 따른 몬스터 체력 증가 
    public float HealthUp(int count) {
        if(count <= 1) {
            return 1;
        }
        else {
            return 1.5f * HealthUp(count-1);
        }
    }

    void OnEnable() {
        enemyHealth = enemyMaxHealth;
        GameManager.instance.enemyCount += enemyValue;
        enemyHitCheck = false;
        enemyDeathCheck = false;

        if (enemyType == Type.enemy_S) {
            enemyMaxHealth = HealthUp(SceneLoadManager.instance.stageCount) * enemyBasicHealth; 
        }
        else if(enemyType == Type.enemy_M || enemyType == Type.enemy_B) {
            Debug.Log("멈춤");
            Invoke("Stop", 2);
        }
        
    }

    void Stop() {
        if (!gameObject.activeSelf)
            return;

        rigid.velocity = Vector2.zero;
        Debug.Log("패턴 생각");
        Invoke("Think", 1);
    }

    public void Think() {
        patternIndex = Random.Range(0,4);
        if (pastPatternIndex != patternIndex) {
            switch (patternIndex) {
                case 0:
                    Pattern1();
                    break;
                case 1:
                    Pattern2();
                    break;
                case 2:
                    Pattern3();
                    break;
                case 3:
                    Pattern4();
                    break;
            }
        }
        else {
            Debug.Log("패턴 중복 그로기 상태");
            Invoke("Think", 1f);
        }
    }

    public void Pattern1() {
        if (enemyId == 10) {
            enemyNiddleBoss1Controller.FireBall();
        }

        pastPatternIndex = 0;
        Invoke("Think", 10);
    }

    public void Pattern2() {
        if (enemyId == 10) {
            enemyNiddleBoss1Controller.Sword();
        }

        pastPatternIndex = 1;
        Invoke("Think", 5);
    }

    public void Pattern3() {
        if (enemyId == 10) {
            enemyNiddleBoss1Controller.RayserRain();
        }

        pastPatternIndex = 2;
        Invoke("Think", 5);
    }

    public void Pattern4() {
        if (enemyId == 10) {
            enemyNiddleBoss1Controller.Heal();
        }

        pastPatternIndex = 3;
        Invoke("Think", 5);
    }



    void Update()
    {
        if (enemyType == Type.enemy_S) {
            EnemyMainSceneDestroy();
            EnemyFollowMove();
            EnemyAttack();
        }
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

            // 이동 방향으로 이동합니다.
            transform.Translate(direction * enemySpeed * Time.deltaTime);
        }
    }

    // 공격 범위에 왔을 때만 때린다. 
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
            Debug.Log("적이 죽었습니다.");
            enemyAnime.SetTrigger("Death");
            enemyDeathCheck = true;
        }

        yield return new WaitForSeconds(0.4f);
        GameManager.instance.enemyCount -= enemyValue;
        GameManager.instance.enemyKillCount++;

        // worldCoinValue 계산 할 때 필요한 변수 값 
        if (enemyType == Type.enemy_S)
            GameManager.instance.enemyTotal += enemyValue * 3;
        else
            GameManager.instance.enemyTotal += enemyValue;

        int random = Random.Range(2, 7);
        for (int i = 0; i < random; i++) {
            GameObject coin = GameManager.instance.poolManager.GetObject(5);
            coin.transform.position = this.transform.position;
        }
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.CompareTag("FireBall") && !enemyHitCheck) {
            FireBall fireBallLogic = GameManager.instance.fireBallPrefab.GetComponent<FireBall>();
            enemyHealth -= fireBallLogic.damage;
            enemyHitCheck = true;
            if (enemyType == Type.enemy_S) {
                enemyAnime.SetTrigger("Hurt");
            }
            StartCoroutine(EnemyHitCheck());
            Debug.Log(enemyHealth);
        }

        // 플레이어가 대쉬 상태이면 데미지를 주지 않는다. ( 무적 판정 ) 
        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        if (collision.gameObject.CompareTag("Player") && !playerLogic.isDashCheck && !playerLogic.isDamaged) {
            enemyAttackcurDel = 0;
            playerLogic.health -= enemyPower;
            Debug.Log(playerLogic.health);
        }
    }

    IEnumerator EnemyHitCheck() {
        yield return new WaitForSeconds(0.1f);
        enemyHitCheck = false;
    }


}
