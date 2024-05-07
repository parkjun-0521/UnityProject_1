using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
    [Header("�� ������ ����")]
    public int itemID;
    public int setItemID;
    public int cost;
    public int upCost;

    [Header("�� ������ �� �ɷ�ġ ���׷��̵� ��ġ ")]
    public float health;
    public float speed;
    public float power;

    bool itemJump = false;
    bool isPlayerCheck = false;
    bool isItemCheck = false;

    public bool isThrowing;

    Rigidbody2D rigid;
    SetItem setItemLogic;
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
        upCost = (int)(cost * (1f + (SceneLoadManager.instance.mapCount * 0.1f)));
    }

    void Update()
    {
        SetItemDrop();

        ItemAcquisition();
    }

    public void ItemAcquisition() {
        if (Input.GetKeyDown(KeyCode.F) && isPlayerCheck && !isItemCheck) {
            // ������ â�� ���� á�� ��� ����ó��
            if(GameManager.instance.setItem.Count >= 9) {
                // ������ �ƴ����� �������� ���� á�� �� UI�� ����� ( ��ü �Ҳ��� UI�� ������ �ϳ�? ) 
                //UIManager.Instance.StatusUI();
                return;
            }

            // ������ ���� ��� ����ó�� ( ���� �������� ����� �����ʰ� ���� �� �־�ߵ� bool�� ���� ó�� ) 
            if ((SceneLoadManager.instance.mapCount % 10 == 4 || SceneLoadManager.instance.mapCount % 10 == 9) && !isThrowing) {
                if(GameManager.instance.coinValue < upCost) {
                    Debug.Log("������ �����մϴ�.");
                    return;
                }
                GameManager.instance.coinValue -= upCost;
            }
            isItemCheck = true;

            // �ɷ�ġ ���� ( ���� ���� id�� ������ �����ͼ� ������ ) 
            Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();

            playerLogic.itemSumHealth += health;
            playerLogic.itemSumSpeed += speed;
            playerLogic.itemSumPower += power;

            playerLogic.maxHealth = playerLogic.upHealth + playerLogic.weaponHealth + playerLogic.itemSumHealth;
            playerLogic.moveSpeed = playerLogic.upMoveSpeed + playerLogic.weaponSpeed + playerLogic.itemSumSpeed;
            playerLogic.power = playerLogic.upPower + playerLogic.itemSumPower;
            playerLogic.itemSetSpeed = 0;

            if (playerLogic.health > playerLogic.maxHealth)
                playerLogic.health = playerLogic.maxHealth;
            else
                playerLogic.health += this.health;

            GameManager.instance.setItem.Add(setItemID);
            GameManager.instance.itemID.Add(itemID);
            GameManager.instance.itemStatus.Add(new List<float>{ itemID, setItemID, health, speed, power });


            // ��Ʈ������ ����            
            SetItemOption();

            StartCoroutine(ItemGet());
        }
    }

    IEnumerator ItemGet()
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
    public void SetItemOption() {
        setItemLogic = GetComponent<SetItem>();
        Dictionary<int, int> countDict = new Dictionary<int, int>();

        // ����Ʈ �ȿ� �ִ� �� ������ ������ ����
        foreach (int count in GameManager.instance.setItem) {
            if (countDict.ContainsKey(count)) {
                ++countDict[count];
            }
            else {
                countDict[count] = 1;
            }
        }

        GameManager.instance.itemSetKey.Clear();
        GameManager.instance.setItemInfo.Clear();

        foreach (var kvp in countDict) {
            Debug.Log(kvp.Key + " " + kvp.Value);
            if (kvp.Value == 1) {
                setItemLogic.Set_1(kvp.Key);
            }
            else if (kvp.Value == 2 || kvp.Value == 3) {
                setItemLogic.Set_2(kvp.Key);
            }
            else if( kvp.Value == 4 || kvp.Value == 5) {
                setItemLogic.Set_4(kvp.Key);
            }
            else if( kvp.Value >= 6) {
                setItemLogic.Set_6(kvp.Key);
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
