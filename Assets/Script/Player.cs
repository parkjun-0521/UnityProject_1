using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static GameKeyboardManager;

public class Player : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed;         // �÷��̾� ���ǵ� ( ��ȭ ��� ) 
    public float upMoveSpeed;
    public float weaponSpeed;
    public float itemSumSpeed;
    public float itemSetSpeed;

    [Header("Power")]
    public float power;             // �÷��̾� ���ݷ� 
    public float upPower;
    public float itemSumPower;
    public float itemSetPower;

    [Header ("Jump")]
    public float jumpPower;
    public int jumpCount;
    public bool isJumpChek = false;
    public bool downJump = false;

    [Header("Attack")]
    public int weaponID;
    public float weaponPower;
    public float maxAttackDelay;
    float curAttackDelay;
    public Transform attackArea;
    public bool isPlayerRot;

    [Header("Health")]
    public float health;
    public float maxHealth;         // �÷��̾� ü�� ( ��ȭ ��� ) 
    public float upHealth;
    public float itemSumHealth;
    public float weaponHealth;
    public float itemSetHealth;
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
    CapsuleCollider2D capsuleCollider2D;
    GameKeyboardManager Keyboard;

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        Keyboard = GameKeyboardManager.instance.GetComponent<GameKeyboardManager>();
        PlayerWeaponChange();
        health = maxHealth;
        weaponPower = 7f;
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
        //Vector3 movePosition = Vector3.zero;
        if (isNPC) {
            return;
        }

        if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.LeftMove))) {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            anime.SetBool("isRun", true);
        }
        else if (Input.GetKey(Keyboard.GetKeyCode(KeyCodeTypes.RightMove))) {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            anime.SetBool("isRun", true);
        }

        if((Input.GetKeyUp(Keyboard.GetKeyCode(KeyCodeTypes.LeftMove))) || (Input.GetKeyUp(Keyboard.GetKeyCode(KeyCodeTypes.RightMove)))) {
            anime.SetBool("isRun", false);
        }

        /*float h = Input.GetAxisRaw("Horizontal");
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

        transform.position += movePosition * moveSpeed * Time.deltaTime;  */
    }

    void PlayerJump(){
        
        // �Ʒ� ���� ( ������ �Ʒ� ������� ������ �� ) 
        if(Input.GetKey(KeyCode.DownArrow) && (Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Jump))) && !isJumpChek && !isNPC) {
            anime.SetBool("isRunAndJump", true);
            anime.SetBool("isJump", true);

            capsuleCollider2D.isTrigger = true;
            downJump = true;

            StartCoroutine(DownJumpCheck());
        }
        else if (Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Jump)) && !isJumpChek && !isNPC) {
            anime.SetBool("isRunAndJump", true);
            anime.SetBool("isJump", true);

            capsuleCollider2D.isTrigger = true;

            jumpCount++;
            if (jumpCount == 2) {
                isJumpChek = true;
                jumpCount = 0;
            }
            rigid.velocity = Vector2.zero;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        if(rigid.velocity.y < -2.0f || rigid.velocity.y == 0.0f) {
            Debug.DrawRay(rigid.position, Vector2.down, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("Floor"));
            if (hit.collider != null) {
                if (hit.collider.tag == "Floor") {
                    capsuleCollider2D.isTrigger = false;
                    isJumpChek = false;
                    anime.SetBool("isRunAndJump", false);
                    anime.SetBool("isJump", false);
                    anime.SetBool("isDash", false);
                }
            }
        }
    }

    IEnumerator DownJumpCheck() {
        yield return new WaitForSeconds(0.3f);
        downJump = false;
    }

    void PlayerAttack(){
        curAttackDelay += Time.deltaTime; 

        if((Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Attack))) && maxAttackDelay < curAttackDelay && !isNPC) {
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
            // �ڷ�ƾ���� ���� 
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

        if ((Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Dash))) && curDelay > maxDelay && !isDashCheck && !isNPC) {
            anime.SetBool("isDash", true);
            moveSpeed = 30 + (upMoveSpeed + weaponSpeed + itemSumSpeed) * (1.0f + itemSetSpeed);
            isDashCheck = true;
            curDelay = 0;
            StartCoroutine(StopDashAnime());
        }
    }

    public IEnumerator StopDashAnime(){
        yield return new WaitForSeconds(0.15f);
        moveSpeed = (upMoveSpeed + weaponSpeed + itemSumSpeed) * (1.0f + itemSetSpeed);
        anime.SetBool("isDash", false);
        yield return new WaitForSeconds(0.1f);
        isDashCheck = false;
    }


    void PlayerDeadth() {
        if(health <= 0 && !isDead) {
            Debug.Log("�׾���");
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

        // �뽬 ������ ���� �ǰݵ��� �ʴ´�. 
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

    void OnTriggerStay2D( Collider2D collision ) {
        if (collision.CompareTag("Wall")) {
            capsuleCollider2D.isTrigger = false;
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
                Debug.Log("���� ��ü");
            }
        }
    }
}
