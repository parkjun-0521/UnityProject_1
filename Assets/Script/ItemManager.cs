using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ItemManager : MonoBehaviour
{
    public int itemID;
    public int setItemID;

    [Header("각 아이템 별 능력치 업그래이드 수치 ")]
    public int health;
    public int speed;
    public int power;

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
