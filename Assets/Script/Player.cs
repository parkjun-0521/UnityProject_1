using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed;         // 플레이어 스피드 ( 강화 대상 ) 
    public float upMoveSpeed;
    public float weaponSpeed;
    public float itemSumSpeed;
    public float itemSetSpeed;

    [Header("Power")]
    public float power;             // 플레이어 공격력 
    public float upPower;
    public float itemSumPower;

    [Header ("Jump")]
    public float jumpPower;
    public float maxJumpSpeed;
    public int jumpCount;
    public bool isJumpChek = false;

    [Header("Attack")]
    public int weaponID;
    public float weaponPower;
    public float maxAttackDelay;
    float curAttackDelay;
    public Transform attackArea;
    public bool isPlayerRot;

    [Header("Health")]
    public float health;
    public float maxHealth;         // 플레이어 체력 ( 강화 대상 ) 
    public float upHealth;
    public float itemSumHealth;
    public float weaponHealth;
    public bool isDamaged = false;

    [Header("Desh")]
    public bool isDashCheck = false;
    public float curDelay;
    public float maxDelay = 0.5f;

    public Transform weaponPos;

    public bool isNPC = false;
    bool isDead = false;

    public Animator anime;
    Rigidbody2D rigid;

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        PlayerWeaponChange();
        health = maxHealth;
        isDead = false;
        isDamaged = false;
    }

    void Update()
    {
        PlayerMove();
        PlayerJump();
        PlayerAttack();
        PlayerDash();
        PlayerDeadth();
    }

    void PlayerMove(){
        Vector3 movePosition = Vector3.zero;
        if (isNPC) {
            return;
        }
        float h = Input.GetAxisRaw("Horizontal");
        if(h < 0){
            movePosition = Vector3.left;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            anime.SetBool("isRun", true);
        }
        else if (h == 0){
            movePosition = Vector3.zero;
            anime.SetBool("isRun", false);
        }
        else {
            movePosition = Vector3.right;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            anime.SetBool("isRun", true);
        }

        transform.position += movePosition * moveSpeed * Time.deltaTime;  
    }

    void PlayerJump(){
        
        if(Input.GetKeyDown(KeyCode.X) && !isJumpChek && !isNPC) {
            anime.SetBool("isRunAndJump", true);
            anime.SetBool("isJump", true);
            jumpCount++;
            if(jumpCount == 2){
                isJumpChek = true;
                jumpCount = 0;
            }
            if(Mathf.Abs(rigid.velocity.y) < maxJumpSpeed){
                rigid.velocity = Vector2.zero;
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }
        }

        if (rigid.velocity.y < 0){
            Debug.DrawRay(rigid.position, Vector2.down, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("Floor"));
            if (hit.collider != null)
            {
                if(hit.collider.tag == "Floor"){
                    isJumpChek = false;
                    anime.SetBool("isRunAndJump", false);
                    anime.SetBool("isJump", false);
                    anime.SetBool("isDash", false);
                }
            }   
        }
    }

    void PlayerAttack(){
        curAttackDelay += Time.deltaTime; 

        if(Input.GetKeyDown(KeyCode.Z) && maxAttackDelay < curAttackDelay && !isNPC) {
            anime.SetTrigger("attack");
            if(transform.rotation == Quaternion.Euler(0f, 0f, 0f)) 
                 isPlayerRot = true;
            else
                 isPlayerRot= false;

            GameObject fireBall = GameManager.instance.poolManager.GetObject(1);
            fireBall.transform.position = attackArea.position;
            fireBall.transform.rotation = Quaternion.identity;

            moveSpeed = 0.5f;
            curAttackDelay = 0;
            // 코루틴으로 수정 
            StartCoroutine(MoveSet());
        }
    }

    IEnumerator MoveSet(){
        yield return new WaitForSeconds(0.5f);
        moveSpeed = (upMoveSpeed + weaponSpeed + itemSumSpeed) * (1.0f + itemSetSpeed);
    }

    void PlayerDash(){
        
        if (curDelay > 10)  
            curDelay = 10.5f; 
        else 
            curDelay += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.C) && curDelay > maxDelay && !isDashCheck && !isNPC) {
            anime.SetBool("isDash", true);
            moveSpeed = 30 + (upMoveSpeed + weaponSpeed + itemSumSpeed) * (1.0f + itemSetSpeed);
            isDashCheck = true;
            curDelay = 0;
            StartCoroutine(StopDashAnime());
        }
    }

    IEnumerator StopDashAnime(){
        yield return new WaitForSeconds(0.15f);
        moveSpeed = (upMoveSpeed + weaponSpeed + itemSumSpeed) * (1.0f + itemSetSpeed);
        anime.SetBool("isDash", false);
        yield return new WaitForSeconds(0.1f);
        isDashCheck = false;
    }


    void PlayerDeadth() {
        if(health <= 0 && !isDead) {
            Debug.Log("죽었다");
            anime.SetTrigger("die");
            isDead = true;
            StartCoroutine(LoadMainScene());
        }
    }

    IEnumerator LoadMainScene() {
        yield return new WaitForSeconds(1f);
        GameManager.instance.MainScene();

        yield return new WaitForSeconds(0.2f);
        if(SceneManager.GetActiveScene().buildIndex == 0) {
            anime.SetTrigger("idle");
            health = maxHealth;
            isDead = false;
            isDamaged = false;
        }
    }

    void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.CompareTag("EndPoint")) {
            GameManager.instance.enemyCount--;
            collision.gameObject.SetActive(false);
        }

        // 대쉬 상태일 때는 피격되지 않는다. 
        if ((collision.CompareTag("Enemy") || collision.CompareTag("Rayser") || collision.CompareTag("EnemyFireBall")) && !isDashCheck) {
            if (!isDamaged) {
                isDamaged = true;
                anime.SetTrigger("hurt");
                if (transform.position.x - collision.transform.position.x < 0) {
                    rigid.AddForce(new Vector2(-1.5f, 1) * 5f, ForceMode2D.Impulse);
                }
                else {
                    rigid.AddForce(new Vector2(1.5f, 1) * 5f, ForceMode2D.Impulse);
                }
                StartCoroutine(IsDamagerdOff());
            }
        }      
    }

    IEnumerator IsDamagerdOff() {
        yield return new WaitForSeconds(0.75f);
        isDamaged = false;
    }

    public void PlayerWeaponChange() {
        PlayerWeaponIcon[] childComponents = GetComponentsInChildren<PlayerWeaponIcon>(true);
        foreach (PlayerWeaponIcon component in childComponents) {
            component.gameObject.SetActive(false);
            if(component.weaponId == this.weaponID) {
                component.gameObject.SetActive(true);
                Debug.Log("무기 교체");
            }
        }
    }
}
