using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameKeyboardManager;

public class Player : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed;         // 플레이어 스피드 ( 최초 값은 변하지 않음 ) 
    public float upMoveSpeed;       // 종합 스피드 변수 (weaponSpeed, itemSumSpeed, itemSetSpeed)
    [HideInInspector]
    public float weaponSpeed;       // 무기 스피드 변수 
    [HideInInspector]
    public float itemSumSpeed;      // 아이템 스피드 총합 
    [HideInInspector]
    public float itemSetSpeed;      // 아이템 세트 스피드 총합 

    [Header("Power")]
    public float power;             // 플레이어 공격력 
    public float upPower;           // 종합 공격력 변수 ( itemSumPower, itemSetPower)
    [HideInInspector]
    public float itemSumPower;      // 아이템 공격력 총합
    [HideInInspector]
    public float itemSetPower;      // 아이템 세트 공격력 총합 

    [Header ("Jump")]
    public float jumpPower;         // 점프 높이 
    [HideInInspector]
    public int jumpCount;           // 2단 점프 제어 
    [HideInInspector]
    public bool isJumpChek = false; // 점프 확인 변수 
    [HideInInspector]
    public bool downJump = false;   // 아래 점수 변수 

    [Header("Attack")]
    [HideInInspector]
    public int weaponID;            // 획득한 무기의 ID
    public float weaponPower;       // 무기 공격력 
    [HideInInspector]
    public float maxAttackDelay;    // max 공격 딜레이 
    [HideInInspector]
    float curAttackDelay;           // 현재 공격 딜레이 
    [HideInInspector]
    public Transform attackArea;    // 공격이 나가는 위치 
    [HideInInspector]
    public bool isPlayerRot;        // 플레이어의 회전 감지 

    [Header("Health")]
    public float health;            // 현재 체력 
    public float maxHealth;         // 플레이어 최대 체력 ( 강화 대상 ) 
    public float upHealth;          // 종합 체력 변수 ( weaponHealth, itemSumHealth, itemSetHealth)
    [HideInInspector]
    public float weaponHealth;      // 무기의 체력 변수 
    [HideInInspector]
    public float itemSumHealth;     // 아이템 체력 총합 
    [HideInInspector]
    public float itemSetHealth;     // 아이템 세트 체력 총합 
    [HideInInspector]
    public bool isDamaged = false;  // 데미지를 중첩해서 막는 것을 방지하기위한 변수 

    [Header("Desh")]
    [HideInInspector]
    public bool isDashCheck = false;// 대쉬 중인지 확인 
    [HideInInspector]
    public float curDelay;          // 대쉬의 현재 딜레이 
    [HideInInspector]
    public float maxDelay = 0.5f;   // 대쉬의 max 딜레이

    [HideInInspector]
    public Transform weaponPos;     // 플레이어가 현재 가지고 있는 무기를 알기위한 변수 

    [HideInInspector]
    public bool isNPC = false;      // NPC와 상호작용중 이동 제어를 위한 변수 
    bool isDead = false;            // 죽었을 때 확인하기 위한 변수 

    public Animator anime;
    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider2D;
    GameKeyboardManager Keyboard;

    void Awake() {
        // 플레이어 파괴 방지 
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        // 컴포넌트 초기화 
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        Keyboard = GameKeyboardManager.instance.GetComponent<GameKeyboardManager>();
        
        // 최초 들고 있는 무기 초기화 
        PlayerWeaponChange();

        // 능력치 초기화 
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

    // Player 이동 로직 
    void PlayerMove(){
        // NPC와 상호작용 제어 
        if (isNPC) {
            return;
        }

        // 기본 이동 로직 
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
    }

    // Player 점프 로직 
    void PlayerJump(){
        
        // 아래 점프 ( 위에서 아래 블록으로 내려올 때 ) 
        if(Input.GetKey(KeyCode.DownArrow) && (Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Jump))) && !isJumpChek && !isNPC) {
            anime.SetBool("isRunAndJump", true);
            anime.SetBool("isJump", true);

            capsuleCollider2D.isTrigger = true;
            downJump = true;

            StartCoroutine(DownJumpCheck());
        }
        // 점프 로직 
        else if (Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Jump)) && !isJumpChek && !isNPC) {
            anime.SetBool("isRunAndJump", true);
            anime.SetBool("isJump", true);

            capsuleCollider2D.isTrigger = true;

            // 2단 점프 
            jumpCount++;
            if (jumpCount == 2) {
                isJumpChek = true;
                jumpCount = 0;
            }

            // 점프 이후 동일한 점프 높이를 주기 위해 zero로 설정 
            rigid.velocity = Vector2.zero;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        // 아래로 떨어지고 있을 때 땅인지(Floor) 검사 하는 로직 
        // 확인 후 다시 점프 가능 
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

    // Player 공격 로직 
    void PlayerAttack(){
        // 공격 딜레이 시간 
        curAttackDelay += Time.deltaTime; 

        if((Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Attack))) && maxAttackDelay < curAttackDelay && !isNPC) {
            anime.SetTrigger("attack");

            // 현재 캐릭터가 회전을 했는지를 판단 
            if(transform.rotation == Quaternion.Euler(0f, 0f, 0f)) 
                 isPlayerRot = true;
            else
                 isPlayerRot= false;

            // 파이어볼 생성 
            GameObject fireBall = GameManager.instance.poolManager.GetObject(1);
            fireBall.transform.position = attackArea.position;
            fireBall.transform.rotation = Quaternion.identity;

            // 공격시 이동 속도 제어 
            moveSpeed = 0.5f;

            // 딜레이 초기화 
            curAttackDelay = 0;

            // 공격 이후 0.5초뒤 이동 속도 변경 
            StartCoroutine(MoveSet());
        }
    }

    IEnumerator MoveSet(){
        yield return new WaitForSeconds(0.5f);
        moveSpeed = (upMoveSpeed + weaponSpeed + itemSumSpeed) * (1.0f + itemSetSpeed);
    }

    // Player 대쉬 로직 
    void PlayerDash(){
        
        // 대쉬 딜레이가 10보다 커지지 않도록 한다. 
        if (curDelay > 10)  
            curDelay = 10.5f; 
        else 
            curDelay += Time.deltaTime;

        // 대쉬 로직 
        // 대쉬를 하면 순간적으로 이동 속도가 빨라지는 방식 
        if ((Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Dash))) && curDelay > maxDelay && !isDashCheck && !isNPC) {
            anime.SetBool("isDash", true);
            moveSpeed = 30 + (upMoveSpeed + weaponSpeed + itemSumSpeed) * (1.0f + itemSetSpeed);
            isDashCheck = true;
            curDelay = 0;

            // 대쉬 후 이동 속도 변경 
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

    // Player 사망 로직 
    void PlayerDeadth() {
        if(health <= 0 && !isDead) {
            Debug.Log("죽었다");
            anime.SetTrigger("die");
            isDead = true;
            // 메인 씬으로 이동하여 모든 데이터를 초기화 
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

    // Trigger 충돌 로직 
    void OnTriggerEnter2D( Collider2D collision ) {
        // 스테이지에서 모든 몬스터를 잡고 포탈 위치에 왔는지 판단하기 위한 EndPoint 충돌 로직 
        if (collision.CompareTag("EndPoint")) {
            GameManager.instance.enemyCount--;
            collision.gameObject.SetActive(false);
        }

        // 적, 보스 레이저 패턴, 보스 파이어볼 패턴 맞을 시 
        if ((collision.CompareTag("Enemy") || collision.CompareTag("Rayser") || collision.CompareTag("EnemyFireBall")) && !isDashCheck) {
            if (!isDamaged) {
                isDamaged = true;
                anime.SetTrigger("hurt");
                // 넉백 로직 ( 왼쪽, 오른쪽 어느쪽에서 맞았는지 판단하고 그 반대 방향으로 튕겨져 나감 ) 
                if (transform.position.x - collision.transform.position.x < 0) {
                    rigid.AddForce(new Vector2(-1.5f, 1) * 5f, ForceMode2D.Impulse);
                }
                else {
                    rigid.AddForce(new Vector2(1.5f, 1) * 5f, ForceMode2D.Impulse);
                }
                // 0.75초 뒤 다시 데미지를 받을 수 있는 상태로 전환 
                StartCoroutine(IsDamagerdOff());
            }
        }
    }
    IEnumerator IsDamagerdOff() {
        yield return new WaitForSeconds(0.75f);
        isDamaged = false;
    }

    void OnTriggerStay2D( Collider2D collision ) {
        // 맵 벽에 충돌시 뚫고 나가지 못하도록 
        if (collision.CompareTag("Wall")) {
            capsuleCollider2D.isTrigger = false;
        }
    }

    // Player 상단 무기 이미지 변경 로직 
    public void PlayerWeaponChange() {
        // 우선 모든 이미지를 찾는다. 이후 모든 이미지를 비활성화 한다. 
        // 획득한 무기의 ID와 일치한 이미지만 활성화를 한다.
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
