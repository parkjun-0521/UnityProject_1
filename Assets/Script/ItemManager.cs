using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
    public int itemID;
    public int setItemID;

    [Header("각 아이템 별 능력치 업그래이드 수치 ")]
    public float health;
    public float speed;
    public float power;

    bool itemJump = false;
    bool isPlayerCheck = false;
    bool isItemCheck = false;

    Rigidbody2D rigid;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }


    void OnEnable()
    {
        if (!itemJump) {
            int rendom = Random.Range(0, 2);
            float renPower = Random.Range(0f, 3f);
            Vector2 power = (rendom == 0) ? Vector2.left * renPower : Vector2.right * renPower;
            rigid.AddForce(Vector2.up * 5 + power, ForceMode2D.Impulse);
        }
    }

    void Update()
    {
        SetItemDrop();

        if (Input.GetKeyDown(KeyCode.F) && isPlayerCheck && !isItemCheck) {
            isItemCheck = true;

            // 능력치 증가 
            Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
            playerLogic.maxHealth += health;
            playerLogic.upMoveSpeed += speed;
            playerLogic.upPower += power;

            GameManager.instance.setItem.Add(setItemID);

            // 셋트아이템 적용
            SetItemOption();

            playerLogic.health += this.health;
            playerLogic.moveSpeed = playerLogic.upMoveSpeed;
            playerLogic.power = playerLogic.upPower;

            StartCoroutine(WeaponGet());
        }
    }

    IEnumerator WeaponGet()
    {
        yield return new WaitForSeconds(0.1f);

        rigid.gravityScale = 1;
        isItemCheck = false;
        gameObject.SetActive(false);
    }

    void SetItemDrop()
    {
        if (rigid.velocity.y < 0) {
            Debug.DrawRay(rigid.position, Vector2.down, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(rigid.position, Vector2.down, 0.5f, LayerMask.GetMask("Floor"));
            if (hit.collider != null) {
                if (hit.collider.tag == "Floor") {
                    rigid.velocity = Vector3.zero;
                    rigid.gravityScale = 0;
                }
            }
        }
    }

    // 아이템 셋트 효과 
    void SetItemOption() {
        Dictionary<int, int> countDict = new Dictionary<int, int>();

        // 리스트 안에 있는 각 숫자의 개수를 세기
        foreach (int count in GameManager.instance.setItem) {
            if (countDict.ContainsKey(count)) {
                countDict[count]++;
            }
            else {
                countDict[count] = 1;
            }
        }

        foreach (var kvp in countDict) {
            Debug.Log(kvp.Key + " " + kvp.Value);
            switch (kvp.Value) {
                case 2:
                case 3:
                    if (kvp.Key == 0) {
                        Debug.Log("0번 Set 아이템 효과 0");
                        // 증가되는 효과를 변수에 저장하기 
                        // max 체력 = X + 증가되는 효과  ( playerLogic.maxHealth = playerLogic.upHealth + playerHealth + 증가되는 효과; )
                        // 속도 = x + 증가되는 효과   (  playerLogic.moveSpeed = playerLogic.upMoveSpeed + playerSpeed + 증가되는 효과; )
                    }
                    break;
                case 4:
                case 5:
                    if (kvp.Key == 0) {
                        Debug.Log("0번 Set 아이템 효과 1");
                    }
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                    if (kvp.Key == 0) {
                        Debug.Log("0번 Set 아이템 효과 2");
                    }
                    break;
            }
        }
    }

    void OnTriggerEnter2D( Collider2D collision )
    {
        if (collision.CompareTag("Player")) {
            isPlayerCheck = true;
        }
    }

    void OnTriggerExit2D( Collider2D collision )
    {
        if (collision.CompareTag("Player")) {
            isPlayerCheck = false;
        }
    }
}
