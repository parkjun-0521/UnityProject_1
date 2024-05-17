using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameKeyboardManager;

public class Player : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed;         // �÷��̾� ���ǵ� ( ���� ���� ������ ���� ) 
    public float upMoveSpeed;       // ���� ���ǵ� ���� (weaponSpeed, itemSumSpeed, itemSetSpeed)
    [HideInInspector]
    public float weaponSpeed;       // ���� ���ǵ� ���� 
    [HideInInspector]
    public float itemSumSpeed;      // ������ ���ǵ� ���� 
    [HideInInspector]
    public float itemSetSpeed;      // ������ ��Ʈ ���ǵ� ���� 

    [Header("Power")]
    public float power;             // �÷��̾� ���ݷ� 
    public float upPower;           // ���� ���ݷ� ���� ( itemSumPower, itemSetPower)
    [HideInInspector]
    public float itemSumPower;      // ������ ���ݷ� ����
    [HideInInspector]
    public float itemSetPower;      // ������ ��Ʈ ���ݷ� ���� 

    [Header ("Jump")]
    public float jumpPower;         // ���� ���� 
    [HideInInspector]
    public int jumpCount;           // 2�� ���� ���� 
    [HideInInspector]
    public bool isJumpChek = false; // ���� Ȯ�� ���� 
    [HideInInspector]
    public bool downJump = false;   // �Ʒ� ���� ���� 

    [Header("Attack")]
    [HideInInspector]
    public int weaponID;            // ȹ���� ������ ID
    public float weaponPower;       // ���� ���ݷ� 
    [HideInInspector]
    public float maxAttackDelay;    // max ���� ������ 
    [HideInInspector]
    float curAttackDelay;           // ���� ���� ������ 
    [HideInInspector]
    public Transform attackArea;    // ������ ������ ��ġ 
    [HideInInspector]
    public bool isPlayerRot;        // �÷��̾��� ȸ�� ���� 

    [Header("Health")]
    public float health;            // ���� ü�� 
    public float maxHealth;         // �÷��̾� �ִ� ü�� ( ��ȭ ��� ) 
    public float upHealth;          // ���� ü�� ���� ( weaponHealth, itemSumHealth, itemSetHealth)
    [HideInInspector]
    public float weaponHealth;      // ������ ü�� ���� 
    [HideInInspector]
    public float itemSumHealth;     // ������ ü�� ���� 
    [HideInInspector]
    public float itemSetHealth;     // ������ ��Ʈ ü�� ���� 
    [HideInInspector]
    public bool isDamaged = false;  // �������� ��ø�ؼ� ���� ���� �����ϱ����� ���� 

    [Header("Desh")]
    [HideInInspector]
    public bool isDashCheck = false;// �뽬 ������ Ȯ�� 
    [HideInInspector]
    public float curDelay;          // �뽬�� ���� ������ 
    [HideInInspector]
    public float maxDelay = 0.5f;   // �뽬�� max ������

    [HideInInspector]
    public Transform weaponPos;     // �÷��̾ ���� ������ �ִ� ���⸦ �˱����� ���� 

    [HideInInspector]
    public bool isNPC = false;      // NPC�� ��ȣ�ۿ��� �̵� ��� ���� ���� 
    bool isDead = false;            // �׾��� �� Ȯ���ϱ� ���� ���� 

    public Animator anime;
    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider2D;
    GameKeyboardManager Keyboard;

    void Awake() {
        // �÷��̾� �ı� ���� 
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        // ������Ʈ �ʱ�ȭ 
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        Keyboard = GameKeyboardManager.instance.GetComponent<GameKeyboardManager>();
        
        // ���� ��� �ִ� ���� �ʱ�ȭ 
        PlayerWeaponChange();

        // �ɷ�ġ �ʱ�ȭ 
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

    // Player �̵� ���� 
    void PlayerMove(){
        // NPC�� ��ȣ�ۿ� ���� 
        if (isNPC) {
            return;
        }

        // �⺻ �̵� ���� 
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

    // Player ���� ���� 
    void PlayerJump(){
        
        // �Ʒ� ���� ( ������ �Ʒ� ������� ������ �� ) 
        if(Input.GetKey(KeyCode.DownArrow) && (Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Jump))) && !isJumpChek && !isNPC) {
            anime.SetBool("isRunAndJump", true);
            anime.SetBool("isJump", true);

            capsuleCollider2D.isTrigger = true;
            downJump = true;

            StartCoroutine(DownJumpCheck());
        }
        // ���� ���� 
        else if (Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Jump)) && !isJumpChek && !isNPC) {
            anime.SetBool("isRunAndJump", true);
            anime.SetBool("isJump", true);

            capsuleCollider2D.isTrigger = true;

            // 2�� ���� 
            jumpCount++;
            if (jumpCount == 2) {
                isJumpChek = true;
                jumpCount = 0;
            }

            // ���� ���� ������ ���� ���̸� �ֱ� ���� zero�� ���� 
            rigid.velocity = Vector2.zero;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        // �Ʒ��� �������� ���� �� ������(Floor) �˻� �ϴ� ���� 
        // Ȯ�� �� �ٽ� ���� ���� 
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

    // Player ���� ���� 
    void PlayerAttack(){
        // ���� ������ �ð� 
        curAttackDelay += Time.deltaTime; 

        if((Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Attack))) && maxAttackDelay < curAttackDelay && !isNPC) {
            anime.SetTrigger("attack");

            // ���� ĳ���Ͱ� ȸ���� �ߴ����� �Ǵ� 
            if(transform.rotation == Quaternion.Euler(0f, 0f, 0f)) 
                 isPlayerRot = true;
            else
                 isPlayerRot= false;

            // ���̾ ���� 
            GameObject fireBall = GameManager.instance.poolManager.GetObject(1);
            fireBall.transform.position = attackArea.position;
            fireBall.transform.rotation = Quaternion.identity;

            // ���ݽ� �̵� �ӵ� ���� 
            moveSpeed = 0.5f;

            // ������ �ʱ�ȭ 
            curAttackDelay = 0;

            // ���� ���� 0.5�ʵ� �̵� �ӵ� ���� 
            StartCoroutine(MoveSet());
        }
    }

    IEnumerator MoveSet(){
        yield return new WaitForSeconds(0.5f);
        moveSpeed = (upMoveSpeed + weaponSpeed + itemSumSpeed) * (1.0f + itemSetSpeed);
    }

    // Player �뽬 ���� 
    void PlayerDash(){
        
        // �뽬 �����̰� 10���� Ŀ���� �ʵ��� �Ѵ�. 
        if (curDelay > 10)  
            curDelay = 10.5f; 
        else 
            curDelay += Time.deltaTime;

        // �뽬 ���� 
        // �뽬�� �ϸ� ���������� �̵� �ӵ��� �������� ��� 
        if ((Input.GetKeyDown(Keyboard.GetKeyCode(KeyCodeTypes.Dash))) && curDelay > maxDelay && !isDashCheck && !isNPC) {
            anime.SetBool("isDash", true);
            moveSpeed = 30 + (upMoveSpeed + weaponSpeed + itemSumSpeed) * (1.0f + itemSetSpeed);
            isDashCheck = true;
            curDelay = 0;

            // �뽬 �� �̵� �ӵ� ���� 
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

    // Player ��� ���� 
    void PlayerDeadth() {
        if(health <= 0 && !isDead) {
            Debug.Log("�׾���");
            anime.SetTrigger("die");
            isDead = true;
            // ���� ������ �̵��Ͽ� ��� �����͸� �ʱ�ȭ 
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

    // Trigger �浹 ���� 
    void OnTriggerEnter2D( Collider2D collision ) {
        // ������������ ��� ���͸� ��� ��Ż ��ġ�� �Դ��� �Ǵ��ϱ� ���� EndPoint �浹 ���� 
        if (collision.CompareTag("EndPoint")) {
            GameManager.instance.enemyCount--;
            collision.gameObject.SetActive(false);
        }

        // ��, ���� ������ ����, ���� ���̾ ���� ���� �� 
        if ((collision.CompareTag("Enemy") || collision.CompareTag("Rayser") || collision.CompareTag("EnemyFireBall")) && !isDashCheck) {
            if (!isDamaged) {
                isDamaged = true;
                anime.SetTrigger("hurt");
                // �˹� ���� ( ����, ������ ����ʿ��� �¾Ҵ��� �Ǵ��ϰ� �� �ݴ� �������� ƨ���� ���� ) 
                if (transform.position.x - collision.transform.position.x < 0) {
                    rigid.AddForce(new Vector2(-1.5f, 1) * 5f, ForceMode2D.Impulse);
                }
                else {
                    rigid.AddForce(new Vector2(1.5f, 1) * 5f, ForceMode2D.Impulse);
                }
                // 0.75�� �� �ٽ� �������� ���� �� �ִ� ���·� ��ȯ 
                StartCoroutine(IsDamagerdOff());
            }
        }
    }
    IEnumerator IsDamagerdOff() {
        yield return new WaitForSeconds(0.75f);
        isDamaged = false;
    }

    void OnTriggerStay2D( Collider2D collision ) {
        // �� ���� �浹�� �հ� ������ ���ϵ��� 
        if (collision.CompareTag("Wall")) {
            capsuleCollider2D.isTrigger = false;
        }
    }

    // Player ��� ���� �̹��� ���� ���� 
    public void PlayerWeaponChange() {
        // �켱 ��� �̹����� ã�´�. ���� ��� �̹����� ��Ȱ��ȭ �Ѵ�. 
        // ȹ���� ������ ID�� ��ġ�� �̹����� Ȱ��ȭ�� �Ѵ�.
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
