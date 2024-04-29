using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
    public int itemID;
    public int setItemID;

    [Header("�� ������ �� �ɷ�ġ ���׷��̵� ��ġ ")]
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

            // �ɷ�ġ ���� 
            Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
            playerLogic.maxHealth += health;
            playerLogic.upMoveSpeed += speed;
            playerLogic.upPower += power;

            GameManager.instance.setItem.Add(setItemID);

            // ��Ʈ������ ����
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

    // ������ ��Ʈ ȿ�� 
    void SetItemOption() {
        Dictionary<int, int> countDict = new Dictionary<int, int>();

        // ����Ʈ �ȿ� �ִ� �� ������ ������ ����
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
                        Debug.Log("0�� Set ������ ȿ�� 0");
                        // �����Ǵ� ȿ���� ������ �����ϱ� 
                        // max ü�� = X + �����Ǵ� ȿ��  ( playerLogic.maxHealth = playerLogic.upHealth + playerHealth + �����Ǵ� ȿ��; )
                        // �ӵ� = x + �����Ǵ� ȿ��   (  playerLogic.moveSpeed = playerLogic.upMoveSpeed + playerSpeed + �����Ǵ� ȿ��; )
                    }
                    break;
                case 4:
                case 5:
                    if (kvp.Key == 0) {
                        Debug.Log("0�� Set ������ ȿ�� 1");
                    }
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                    if (kvp.Key == 0) {
                        Debug.Log("0�� Set ������ ȿ�� 2");
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
