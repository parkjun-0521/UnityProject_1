using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;             // 싱글톤 

    public int worldCoinValue;                      // 전체 코인 ( 강화 있음 )
    public int coinValue;

    public int enemyCount;                          // 현재 스테이지의 몬스터 숫자
    public int enemyTotal;                          // 모든 스테이지의 몬스터 숫자 
    public int enemyKillCount;                      // 잡은 몬스터 숫자 
    bool isPotal = false;                           // 포탈을 열 수 있는 상황인지 확인하는 변수 

    public GameObject player;                       // 플레이어 프리팹 
    public Camera mainCamera;                       // 메인 카메라 
    public Pooling poolManager;                     // 오브젝트 풀링 스크립트 

    public GameObject fireBallPrefab;               // 파이어볼 프리펩 
    public GameObject playerPrefab;                 // 플레이어 프리펩의 실질적 값 변경을 위한 프리팹 

    public GameObject[] potalPrefabs;               // 포탈 프리팹    
    public GameObject[] potalPosition;              // 포탈의 위치 ( Transform ) 
    public GameObject endPoint;                     // 마지막 위치 ( Transform ) 
    public int potalID;                             // 들어간 포탈의 ID 

    public List<int> itemSetKey;                    // 아이템 셋트가 활성화 되면 저장되는 셋트 값 
    public List<int> setItem;                       // 아이템의 셋트 id
    public List<int> itemID;                        // 아이템의 id
    public Dictionary<int, List<float>> setItemInfo;// ui에 띄우기 위함 값 
    public List<List<float>> itemStatus;            // 아이템 버리기 위해 가져온 아이템의 능력치 값 

    public GameObject[] weaponItem;                 // 무기 프리팹 
    public Transform clearCompensationPos;          // 클리어 보상 위치 

    bool randomNumber = false;
    int[] randomPotal;
    public GameObject[] potalImagePos;              // 포탈 이미지 위치 
    public GameObject ShopBossPotalImagePos;        // 상점, 보스방 포탕 이미지 위치 
    public Sprite[] potalImageSprite;               // 이미지 스프라이트 변수 

    PlayerStatus playerStatus;
    void Awake() {
        // 싱글톤 초기화 
        instance = this;

        // 오브젝트 파괴 방지 
        DontDestroyOnLoad(gameObject);
        Init();
    }
    // Awake의 Init 로직 
    void Init() {
        // 맵 몬스터 숫자 초기화 ( 0으로 하면 바로나오기 때문에 1로 지정 후 Player가 endPoint를 지날 시 -1을 하여 0으로 만들고 포탈 생성 ) 
        enemyCount = 1;

        // 변수 초기화 
        isPotal = false;
        randomNumber = false;

        // 플레이어 위치 초기화 
        if (playerPrefab != null) {
            playerPrefab.transform.position = new Vector2(-10f, -5f);
            playerPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        // PotalPoint 오브젝트 찾기 
        for (int i = 0; i < potalPosition.Length; i++) {
            potalPosition[i] = GameObject.Find("PotalPoint_" + (i + 1));
        }

        // Image 오브젝트 찾기 ( 포탈 이미지 ) 
        for (int i = 0; i < potalImagePos.Length; i++) {
            potalImagePos[i] = GameObject.Find("Image_" + (i + 1));
        }
        ShopBossPotalImagePos = GameObject.Find("Image_3");

        // EndPoint 오브젝트 찾기 
        endPoint = GameObject.Find("EndPoint");
    }


    void Start() {
        // 플레이어 생성 및 위치 초기화 
        StartCoroutine(PlayerSetting());
    }

    IEnumerator PlayerSetting() {
        yield return null;
        if (playerPrefab == null) {
            // 플레이어 생성 
            GameObject playerObj = Instantiate(player, new Vector2(-7f, -5f), Quaternion.Euler(0f, 0f, 0f));
            playerPrefab = playerObj;

            // 카메라 오브젝트를 찾아서 Player를 따라다니도록 assinged 해준다. 
            GameObject objCamera = GameObject.Find("Main Camera");
            objCamera.GetComponent<CameraManager>().playerTransform = playerPrefab.transform;

            // 미니맵 카메라 Player를 따라다니면서 주변의 미니맵만 보여준다. 
            GameObject objMiniMapCamera = GameObject.Find("MiniMapCamera");
            objMiniMapCamera.GetComponent<CameraManager>().playerTransform = playerPrefab.transform;

            // 게임 UI 활성화 
            UIManager.Instance.gameUI.SetActive(true);

            // 모든 아이템 관련 리스트 초기화 
            itemSetKey = new List<int>();
            setItem = new List<int>();
            itemID = new List<int>();
            setItemInfo = new Dictionary<int, List<float>>();
            itemStatus = new List<List<float>>();

            yield return new WaitForSeconds(0.1f);

            // 플레이어 assinged 후 0.1초 뒤 저장한 데이터를 기반으로 Player 능력치 재설정 
            Player playerLogic = playerPrefab.GetComponent<Player>();
            playerStatus = GetComponent<PlayerStatus>();

            // 저장된 능력치 적용 
            // 체력 
            playerLogic.upHealth = playerStatus.HealthUp(UIManager.Instance.healthCount + 1) * GameManager.instance.player.GetComponent<Player>().health;
            playerLogic.maxHealth = (playerLogic.upHealth + playerLogic.weaponHealth + playerLogic.itemSumHealth) * (1.0f + playerLogic.itemSetHealth);
            playerLogic.health = playerLogic.maxHealth;
 
            // 속도 
            playerLogic.upMoveSpeed = playerStatus.SpeedUp(UIManager.Instance.speedCount + 1) * GameManager.instance.player.GetComponent<Player>().upMoveSpeed;
            playerLogic.moveSpeed = (playerLogic.upMoveSpeed + playerLogic.weaponSpeed + playerLogic.itemSumSpeed) * (1.0f + playerLogic.itemSetSpeed);
            
            // 공격력 
            playerLogic.upPower = playerStatus.PowerUp(UIManager.Instance.powerCount + 1) * GameManager.instance.player.GetComponent<Player>().upPower;
            playerLogic.power = (playerLogic.upPower + playerLogic.itemSumPower) * (1.0f + playerLogic.itemSetPower);
        }
        else {
            Destroy(playerPrefab);
            yield return null;
        }
    }

    void Update() {
        // 랜덤 숫자 두개 추출 
        if (!randomNumber) {
            randomPotal = new int[potalPosition.Length];
            for (int i = 0; i < potalPosition.Length; i++) {
                randomPotal[i] = Random.Range(0, 4);
                Debug.Log(randomPotal[i]);
                if (i == 1 && randomPotal[i] == randomPotal[i - 1])
                    i--;
            }
            randomNumber = true;
            // 랜덤 숫자 두개를 미리 추출하여 포탈을 예측하여 미리 포탈 이미지를 띄워준다. 
            StartCoroutine(PotalImageCreate());
        }

        // 맵을 클리어 했을 때 또는 상점일 경우 포탈을 즉시 생성 
        if ((enemyCount == 0 || SceneLoadManager.instance.mapCount % 10 == 4 || SceneLoadManager.instance.mapCount % 10 == 9) && !isPotal) {
            PotalCreate();
        }
    }

    // 포탈 이미지 생성 
    IEnumerator PotalImageCreate() {
        yield return new WaitForSeconds(0.5f);
        // 상점방 전, 보스방 전을 제외 하고 랜덤 포탈 이미지 2개를 생성 
        if (!(SceneLoadManager.instance.mapCount % 10 == 3 || SceneLoadManager.instance.mapCount % 10 == 8 || SceneLoadManager.instance.mapCount % 10 == 4 || SceneLoadManager.instance.mapCount % 10 == 9)) {
            for (int i = 0; i < potalImagePos.Length; i++) {
                potalImagePos[i].GetComponent<SpriteRenderer>().sprite = potalImageSprite[randomPotal[i]];
            }
        }
        // 상점, 보스 방일 때는 포탈 이미지 1개만 생성 
        else if(SceneLoadManager.instance.mapCount % 10 == 3 || SceneLoadManager.instance.mapCount % 10 == 8) {
            ShopBossPotalImagePos.GetComponent<SpriteRenderer>().sprite = potalImageSprite[4];
        }
        else if(SceneLoadManager.instance.mapCount % 10 == 4 || SceneLoadManager.instance.mapCount % 10 == 9) {
            ShopBossPotalImagePos.GetComponent<SpriteRenderer>().sprite = potalImageSprite[5];
        }
    }

    // 포탈 생성 
    void PotalCreate() {
        // 클리어 보상 위치 오브젝트 찾기 
        if (!(SceneLoadManager.instance.mapCount == 0)) {
            clearCompensationPos = GameObject.Find("clearCompensationPos").transform;
        }

        // 상점 전, 보스방 전 방에서는 1개의 포탈 만 생성 
        if (SceneLoadManager.instance.mapCount % 10 == 3 || SceneLoadManager.instance.mapCount % 10 == 8 || SceneLoadManager.instance.mapCount % 10 == 4 || SceneLoadManager.instance.mapCount % 10 == 9) {
            Instantiate(potalPrefabs[4], clearCompensationPos.position, Quaternion.identity);
        }
        // 나머지는 2개의 포탈을 생성 
        else {
            for (int i = 0; i < potalPosition.Length; i++) {
                Instantiate(potalPrefabs[randomPotal[i]], potalPosition[i].transform.position, Quaternion.identity);
            }
        }


        // potalID 에 맞는 아이템 생성 
        if (enemyCount == 0 && !(SceneLoadManager.instance.mapCount % 10 == 4 || SceneLoadManager.instance.mapCount % 10 == 9)) {
            if (potalID == 1 || potalID == 2) {
                // 골드 생성 
                int random = Random.Range(2, 7);
                for (int i = 0; i < random; i++) {
                    GameObject coin = poolManager.GetObject(5);
                    coin.GetComponent<Coin>().coinValue = Random.Range(10, 15);
                    coin.transform.position = clearCompensationPos.position;
                }
            }
            else if (potalID == 3) {
                // 무기 생성 
                int rand = Random.Range(0, weaponItem.Length);
                GameObject weaponObj = Instantiate(weaponItem[rand], clearCompensationPos.position, Quaternion.identity);
                weaponObj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else if (potalID == 4) {
                // 아이템 생성 
                int rand;
                int totalRand = Random.Range(1, 101);
                // 확률형 아이템 드랍 
                // 10% 확률로 성능이 좋은 2개의 아이템중 하나가 나온다.
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

    // 메인씬 이동 시 초기화 로직 
    public void MainScene() {
        if (playerPrefab.GetComponent<Player>().health <= 0) {

            GameReset();
            LodingScene.LoadScene(0);
        }
    }

    public void GameReset() {
        // 전체 코인 계산 ( 보스잡은 수 * 30 + 중간보스 * 15 + 잡몹 * 3 + 스테이지 수 * 2 ) 
        worldCoinValue += (enemyTotal + ((SceneLoadManager.instance.stageCount * SceneLoadManager.instance.mapCount) * 2));
        
        // 코인 저장 
        PlayerPrefs.SetInt("TotalCoin", worldCoinValue);

        // 맵 카운트 및 UI 초기화 
        SceneLoadManager.instance.mapCount = 0;
        SceneLoadManager.instance.stageCount = 0;
        enemyKillCount = 0;
        coinValue = 0;
        enemyTotal = 0;

        // 아이템 저장 관련 리스트 초기화 
        itemSetKey = new List<int>();
        setItem = new List<int>();
        itemID = new List<int>();
        setItemInfo = new Dictionary<int, List<float>>();
        itemStatus = new List<List<float>>();

        // 플레이어 체력 초기화 
        playerPrefab.GetComponent<Player>().health = 100f;
        playerPrefab.GetComponent<Player>().maxHealth = 100f;
        playerPrefab.GetComponent<Player>().itemSumHealth = 0f;
        playerPrefab.GetComponent<Player>().weaponHealth = 0f;
        playerPrefab.GetComponent<Player>().itemSetHealth = 0f;
        // 플레이어 속도 초기화 
        playerPrefab.GetComponent<Player>().power = 5f;
        playerPrefab.GetComponent<Player>().itemSumPower = 0f;
        playerPrefab.GetComponent<Player>().itemSetPower = 0f;
        playerPrefab.GetComponent<Player>().weaponPower = 7f;
        // 플레이어 공격력 초기화 
        playerPrefab.GetComponent<Player>().moveSpeed = 5f;
        playerPrefab.GetComponent<Player>().itemSumSpeed = 0f;
        playerPrefab.GetComponent<Player>().itemSetSpeed = 0f;
        playerPrefab.GetComponent<Player>().weaponSpeed = 0f;

        // 플레이어 무기 이미지 초기화 
        PlayerWeaponIcon[] childComponents = playerPrefab.GetComponentsInChildren<PlayerWeaponIcon>(true);
        foreach (PlayerWeaponIcon component in childComponents) {
            component.gameObject.SetActive(false);
            if (component.weaponId == 0) {
                component.gameObject.SetActive(true);
            }
        }

        // 카메라 초기화 
        mainCamera.GetComponent<Camera>().orthographicSize = 7;
        mainCamera.GetComponent<CameraManager>().mapSize = new Vector2(25f, 10);
        mainCamera.GetComponent<CameraManager>().center = new Vector2(0, 0); 
        
        // 엔딩때 UI를 제어하기위한 bool 변수 초기화
        UIManager.Instance.isEnding = false;
    }
}
