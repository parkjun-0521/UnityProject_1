using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
    [Header("각 아이템 정보")]
    public int itemID;
    public int setItemID;
    public int cost;
    public int upCost;

    [Header("각 아이템 별 능력치 업그래이드 수치 ")]
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
            // 아이템 창이 가득 찼을 경우 예외처리
            if(GameManager.instance.setItem.Count >= 9) {
                // 아직은 아니지만 아이템이 가득 찼을 시 UI를 띄워줌 ( 교체 할껀지 UI를 띄워줘야 하나? ) 
                //UIManager.Instance.StatusUI();
                return;
            }

            // 코인이 없을 경우 예외처리 ( 버린 아이템은 비용을 내지않고 먹을 수 있어야됨 bool로 예외 처리 ) 
            if ((SceneLoadManager.instance.mapCount % 10 == 4 || SceneLoadManager.instance.mapCount % 10 == 9) && !isThrowing) {
                if(GameManager.instance.coinValue < upCost) {
                    Debug.Log("코인이 부족합니다.");
                    return;
                }
                GameManager.instance.coinValue -= upCost;
            }
            isItemCheck = true;

            // 능력치 증가 ( 버릴 때는 id로 정보를 가져와서 역과정 ) 
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


            // 셋트아이템 적용            
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

    // 아이템 셋트 효과 
    public void SetItemOption() {
        setItemLogic = GetComponent<SetItem>();
        Dictionary<int, int> countDict = new Dictionary<int, int>();

        // 리스트 안에 있는 각 숫자의 개수를 세기
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
