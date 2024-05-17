using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;             // �̱��� 

    public int worldCoinValue;                      // ��ü ���� ( ��ȭ ���� )
    public int coinValue;

    public int enemyCount;                          // ���� ���������� ���� ����
    public int enemyTotal;                          // ��� ���������� ���� ���� 
    public int enemyKillCount;                      // ���� ���� ���� 
    bool isPotal = false;                           // ��Ż�� �� �� �ִ� ��Ȳ���� Ȯ���ϴ� ���� 

    public GameObject player;                       // �÷��̾� ������ 
    public Camera mainCamera;                       // ���� ī�޶� 
    public Pooling poolManager;                     // ������Ʈ Ǯ�� ��ũ��Ʈ 

    public GameObject fireBallPrefab;               // ���̾ ������ 
    public GameObject playerPrefab;                 // �÷��̾� �������� ������ �� ������ ���� ������ 

    public GameObject[] potalPrefabs;               // ��Ż ������    
    public GameObject[] potalPosition;              // ��Ż�� ��ġ ( Transform ) 
    public GameObject endPoint;                     // ������ ��ġ ( Transform ) 
    public int potalID;                             // �� ��Ż�� ID 

    public List<int> itemSetKey;                    // ������ ��Ʈ�� Ȱ��ȭ �Ǹ� ����Ǵ� ��Ʈ �� 
    public List<int> setItem;                       // �������� ��Ʈ id
    public List<int> itemID;                        // �������� id
    public Dictionary<int, List<float>> setItemInfo;// ui�� ���� ���� �� 
    public List<List<float>> itemStatus;            // ������ ������ ���� ������ �������� �ɷ�ġ �� 

    public GameObject[] weaponItem;                 // ���� ������ 
    public Transform clearCompensationPos;          // Ŭ���� ���� ��ġ 

    bool randomNumber = false;
    int[] randomPotal;
    public GameObject[] potalImagePos;              // ��Ż �̹��� ��ġ 
    public GameObject ShopBossPotalImagePos;        // ����, ������ ���� �̹��� ��ġ 
    public Sprite[] potalImageSprite;               // �̹��� ��������Ʈ ���� 

    PlayerStatus playerStatus;
    void Awake() {
        // �̱��� �ʱ�ȭ 
        instance = this;

        // ������Ʈ �ı� ���� 
        DontDestroyOnLoad(gameObject);
        Init();
    }
    // Awake�� Init ���� 
    void Init() {
        // �� ���� ���� �ʱ�ȭ ( 0���� �ϸ� �ٷγ����� ������ 1�� ���� �� Player�� endPoint�� ���� �� -1�� �Ͽ� 0���� ����� ��Ż ���� ) 
        enemyCount = 1;

        // ���� �ʱ�ȭ 
        isPotal = false;
        randomNumber = false;

        // �÷��̾� ��ġ �ʱ�ȭ 
        if (playerPrefab != null) {
            playerPrefab.transform.position = new Vector2(-10f, -5f);
            playerPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        // PotalPoint ������Ʈ ã�� 
        for (int i = 0; i < potalPosition.Length; i++) {
            potalPosition[i] = GameObject.Find("PotalPoint_" + (i + 1));
        }

        // Image ������Ʈ ã�� ( ��Ż �̹��� ) 
        for (int i = 0; i < potalImagePos.Length; i++) {
            potalImagePos[i] = GameObject.Find("Image_" + (i + 1));
        }
        ShopBossPotalImagePos = GameObject.Find("Image_3");

        // EndPoint ������Ʈ ã�� 
        endPoint = GameObject.Find("EndPoint");
    }


    void Start() {
        // �÷��̾� ���� �� ��ġ �ʱ�ȭ 
        StartCoroutine(PlayerSetting());
    }

    IEnumerator PlayerSetting() {
        yield return null;
        if (playerPrefab == null) {
            // �÷��̾� ���� 
            GameObject playerObj = Instantiate(player, new Vector2(-7f, -5f), Quaternion.Euler(0f, 0f, 0f));
            playerPrefab = playerObj;

            // ī�޶� ������Ʈ�� ã�Ƽ� Player�� ����ٴϵ��� assinged ���ش�. 
            GameObject objCamera = GameObject.Find("Main Camera");
            objCamera.GetComponent<CameraManager>().playerTransform = playerPrefab.transform;

            // �̴ϸ� ī�޶� Player�� ����ٴϸ鼭 �ֺ��� �̴ϸʸ� �����ش�. 
            GameObject objMiniMapCamera = GameObject.Find("MiniMapCamera");
            objMiniMapCamera.GetComponent<CameraManager>().playerTransform = playerPrefab.transform;

            // ���� UI Ȱ��ȭ 
            UIManager.Instance.gameUI.SetActive(true);

            // ��� ������ ���� ����Ʈ �ʱ�ȭ 
            itemSetKey = new List<int>();
            setItem = new List<int>();
            itemID = new List<int>();
            setItemInfo = new Dictionary<int, List<float>>();
            itemStatus = new List<List<float>>();

            yield return new WaitForSeconds(0.1f);

            // �÷��̾� assinged �� 0.1�� �� ������ �����͸� ������� Player �ɷ�ġ �缳�� 
            Player playerLogic = playerPrefab.GetComponent<Player>();
            playerStatus = GetComponent<PlayerStatus>();

            // ����� �ɷ�ġ ���� 
            // ü�� 
            playerLogic.upHealth = playerStatus.HealthUp(UIManager.Instance.healthCount + 1) * GameManager.instance.player.GetComponent<Player>().health;
            playerLogic.maxHealth = (playerLogic.upHealth + playerLogic.weaponHealth + playerLogic.itemSumHealth) * (1.0f + playerLogic.itemSetHealth);
            playerLogic.health = playerLogic.maxHealth;
 
            // �ӵ� 
            playerLogic.upMoveSpeed = playerStatus.SpeedUp(UIManager.Instance.speedCount + 1) * GameManager.instance.player.GetComponent<Player>().upMoveSpeed;
            playerLogic.moveSpeed = (playerLogic.upMoveSpeed + playerLogic.weaponSpeed + playerLogic.itemSumSpeed) * (1.0f + playerLogic.itemSetSpeed);
            
            // ���ݷ� 
            playerLogic.upPower = playerStatus.PowerUp(UIManager.Instance.powerCount + 1) * GameManager.instance.player.GetComponent<Player>().upPower;
            playerLogic.power = (playerLogic.upPower + playerLogic.itemSumPower) * (1.0f + playerLogic.itemSetPower);
        }
        else {
            Destroy(playerPrefab);
            yield return null;
        }
    }

    void Update() {
        // ���� ���� �ΰ� ���� 
        if (!randomNumber) {
            randomPotal = new int[potalPosition.Length];
            for (int i = 0; i < potalPosition.Length; i++) {
                randomPotal[i] = Random.Range(0, 4);
                Debug.Log(randomPotal[i]);
                if (i == 1 && randomPotal[i] == randomPotal[i - 1])
                    i--;
            }
            randomNumber = true;
            // ���� ���� �ΰ��� �̸� �����Ͽ� ��Ż�� �����Ͽ� �̸� ��Ż �̹����� ����ش�. 
            StartCoroutine(PotalImageCreate());
        }

        // ���� Ŭ���� ���� �� �Ǵ� ������ ��� ��Ż�� ��� ���� 
        if ((enemyCount == 0 || SceneLoadManager.instance.mapCount % 10 == 4 || SceneLoadManager.instance.mapCount % 10 == 9) && !isPotal) {
            PotalCreate();
        }
    }

    // ��Ż �̹��� ���� 
    IEnumerator PotalImageCreate() {
        yield return new WaitForSeconds(0.5f);
        // ������ ��, ������ ���� ���� �ϰ� ���� ��Ż �̹��� 2���� ���� 
        if (!(SceneLoadManager.instance.mapCount % 10 == 3 || SceneLoadManager.instance.mapCount % 10 == 8 || SceneLoadManager.instance.mapCount % 10 == 4 || SceneLoadManager.instance.mapCount % 10 == 9)) {
            for (int i = 0; i < potalImagePos.Length; i++) {
                potalImagePos[i].GetComponent<SpriteRenderer>().sprite = potalImageSprite[randomPotal[i]];
            }
        }
        // ����, ���� ���� ���� ��Ż �̹��� 1���� ���� 
        else if(SceneLoadManager.instance.mapCount % 10 == 3 || SceneLoadManager.instance.mapCount % 10 == 8) {
            ShopBossPotalImagePos.GetComponent<SpriteRenderer>().sprite = potalImageSprite[4];
        }
        else if(SceneLoadManager.instance.mapCount % 10 == 4 || SceneLoadManager.instance.mapCount % 10 == 9) {
            ShopBossPotalImagePos.GetComponent<SpriteRenderer>().sprite = potalImageSprite[5];
        }
    }

    // ��Ż ���� 
    void PotalCreate() {
        // Ŭ���� ���� ��ġ ������Ʈ ã�� 
        if (!(SceneLoadManager.instance.mapCount == 0)) {
            clearCompensationPos = GameObject.Find("clearCompensationPos").transform;
        }

        // ���� ��, ������ �� �濡���� 1���� ��Ż �� ���� 
        if (SceneLoadManager.instance.mapCount % 10 == 3 || SceneLoadManager.instance.mapCount % 10 == 8 || SceneLoadManager.instance.mapCount % 10 == 4 || SceneLoadManager.instance.mapCount % 10 == 9) {
            Instantiate(potalPrefabs[4], clearCompensationPos.position, Quaternion.identity);
        }
        // �������� 2���� ��Ż�� ���� 
        else {
            for (int i = 0; i < potalPosition.Length; i++) {
                Instantiate(potalPrefabs[randomPotal[i]], potalPosition[i].transform.position, Quaternion.identity);
            }
        }


        // potalID �� �´� ������ ���� 
        if (enemyCount == 0 && !(SceneLoadManager.instance.mapCount % 10 == 4 || SceneLoadManager.instance.mapCount % 10 == 9)) {
            if (potalID == 1 || potalID == 2) {
                // ��� ���� 
                int random = Random.Range(2, 7);
                for (int i = 0; i < random; i++) {
                    GameObject coin = poolManager.GetObject(5);
                    coin.GetComponent<Coin>().coinValue = Random.Range(10, 15);
                    coin.transform.position = clearCompensationPos.position;
                }
            }
            else if (potalID == 3) {
                // ���� ���� 
                int rand = Random.Range(0, weaponItem.Length);
                GameObject weaponObj = Instantiate(weaponItem[rand], clearCompensationPos.position, Quaternion.identity);
                weaponObj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else if (potalID == 4) {
                // ������ ���� 
                int rand;
                int totalRand = Random.Range(1, 101);
                // Ȯ���� ������ ��� 
                // 10% Ȯ���� ������ ���� 2���� �������� �ϳ��� ���´�.
                if (totalRand < 90)
                    rand = Random.Range(11, poolManager.prefabs.Length - 2);
                else
                    rand = Random.Range(poolManager.prefabs.Length - 2, poolManager.prefabs.Length);

                GameObject item = poolManager.GetObject(rand);
                item.transform.position = clearCompensationPos.position;
                item.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }

        isPotal = true;
    }

    // ���ξ� �̵� �� �ʱ�ȭ ���� 
    public void MainScene() {
        if (playerPrefab.GetComponent<Player>().health <= 0) {

            GameReset();
            LodingScene.LoadScene(0);
        }
    }

    public void GameReset() {
        // ��ü ���� ��� ( �������� �� * 30 + �߰����� * 15 + ��� * 3 + �������� �� * 2 ) 
        worldCoinValue += (enemyTotal + ((SceneLoadManager.instance.stageCount * SceneLoadManager.instance.mapCount) * 2));
        
        // ���� ���� 
        PlayerPrefs.SetInt("TotalCoin", worldCoinValue);

        // �� ī��Ʈ �� UI �ʱ�ȭ 
        SceneLoadManager.instance.mapCount = 0;
        SceneLoadManager.instance.stageCount = 0;
        enemyKillCount = 0;
        coinValue = 0;
        enemyTotal = 0;

        // ������ ���� ���� ����Ʈ �ʱ�ȭ 
        itemSetKey = new List<int>();
        setItem = new List<int>();
        itemID = new List<int>();
        setItemInfo = new Dictionary<int, List<float>>();
        itemStatus = new List<List<float>>();

        // �÷��̾� ü�� �ʱ�ȭ 
        playerPrefab.GetComponent<Player>().health = 100f;
        playerPrefab.GetComponent<Player>().maxHealth = 100f;
        playerPrefab.GetComponent<Player>().itemSumHealth = 0f;
        playerPrefab.GetComponent<Player>().weaponHealth = 0f;
        playerPrefab.GetComponent<Player>().itemSetHealth = 0f;
        // �÷��̾� �ӵ� �ʱ�ȭ 
        playerPrefab.GetComponent<Player>().power = 5f;
        playerPrefab.GetComponent<Player>().itemSumPower = 0f;
        playerPrefab.GetComponent<Player>().itemSetPower = 0f;
        playerPrefab.GetComponent<Player>().weaponPower = 7f;
        // �÷��̾� ���ݷ� �ʱ�ȭ 
        playerPrefab.GetComponent<Player>().moveSpeed = 5f;
        playerPrefab.GetComponent<Player>().itemSumSpeed = 0f;
        playerPrefab.GetComponent<Player>().itemSetSpeed = 0f;
        playerPrefab.GetComponent<Player>().weaponSpeed = 0f;

        // �÷��̾� ���� �̹��� �ʱ�ȭ 
        PlayerWeaponIcon[] childComponents = playerPrefab.GetComponentsInChildren<PlayerWeaponIcon>(true);
        foreach (PlayerWeaponIcon component in childComponents) {
            component.gameObject.SetActive(false);
            if (component.weaponId == 0) {
                component.gameObject.SetActive(true);
            }
        }

        // ī�޶� �ʱ�ȭ 
        mainCamera.GetComponent<Camera>().orthographicSize = 7;
        mainCamera.GetComponent<CameraManager>().mapSize = new Vector2(25f, 10);
        mainCamera.GetComponent<CameraManager>().center = new Vector2(0, 0); 
        
        // ������ UI�� �����ϱ����� bool ���� �ʱ�ȭ
        UIManager.Instance.isEnding = false;
    }
}
