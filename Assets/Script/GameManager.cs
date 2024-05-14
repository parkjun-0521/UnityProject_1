using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int worldCoinValue;  // 전체 코인 ( 강화 있음 )
    public int coinValue;

    public int enemyCount;
    public int enemyTotal;
    public int enemyKillCount;
    bool isPotal = false;

    public GameObject player;
    public Camera mainCamera;
    public Pooling poolManager;

    public GameObject fireBallPrefab;
    public GameObject playerPrefab;

    public GameObject[] potalPrefabs;
    public GameObject[] potalPosition;
    public GameObject endPoint;
    public int potalID;

    public List<int> itemSetKey;                            // 아이템 셋트가 활성화 되면 저장되는 셋트 값 
    public List<int> setItem;                               // 아이템의 셋트 id
    public List<int> itemID;                                // 아이템의 id
    public Dictionary<int, List<float>> setItemInfo;        // ui에 띄우기 위함 값 
    public List<List<float>> itemStatus;                    // 아이템 버리기 위해 가져온 아이템의 능력치 값 

    public GameObject[] weaponItem;
    public Transform clearCompensationPos;

    bool randomNumber = false;
    int[] randomPotal;
    public GameObject[] potalImagePos;
    public Sprite[] potalImageSprite;
    void Awake() {
        
        instance = this;
        DontDestroyOnLoad(gameObject);
        Init();
    }

    void Start() {
        // 플레이어 생성 및 위치 초기화 
        StartCoroutine(PlayerSetting());
    }

    IEnumerator PlayerSetting() {
        yield return null;
        if (playerPrefab == null) {
            GameObject playerObj = Instantiate(player, new Vector2(-7f, -5f), Quaternion.Euler(0f, 0f, 0f));

            playerPrefab = playerObj;
            GameObject objCamera = GameObject.Find("Main Camera");
            objCamera.GetComponent<CameraManager>().playerTransform = playerPrefab.transform;

            GameObject objMiniMapCamera = GameObject.Find("MiniMapCamera");
            objMiniMapCamera.GetComponent<CameraManager>().playerTransform = playerPrefab.transform;

            UIManager.Instance.gameUI.SetActive(true);

            itemSetKey = new List<int>();
            setItem = new List<int>();
            itemID = new List<int>();
            setItemInfo = new Dictionary<int, List<float>>();
            itemStatus = new List<List<float>>();
        }
        else {
            Destroy(playerPrefab);
            yield return null;
        }
    }

    void Init() {
        enemyCount = 1;
        isPotal = false;
        randomNumber = false;
        if (playerPrefab != null) {
            playerPrefab.transform.position = new Vector2(-10f, -5f);
            playerPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        for (int i = 0; i < potalPosition.Length; i++) {
            potalPosition[i] = GameObject.Find("PotalPoint_" + (i+1));
        }

        for (int i = 0; i < potalImagePos.Length; i++) {
            potalImagePos[i] = GameObject.Find("Image_" + (i + 1));
        }

        if (!(SceneLoadManager.instance.mapCount == 0 || SceneLoadManager.instance.mapCount % 10 == 4 || SceneLoadManager.instance.mapCount % 10 == 9)) {
            clearCompensationPos = GameObject.Find("clearCompensationPos").transform;
        }

        endPoint = GameObject.Find("EndPoint");
    }


    void Update() {
        // 랜덤 숫자 두개 
        if (!randomNumber) {
            randomPotal = new int[potalPosition.Length];
            for (int i = 0; i < potalPosition.Length; i++) {
                randomPotal[i] = Random.Range(0, 4);
                Debug.Log(randomPotal[i]);
                if (i == 1 && randomPotal[i] == randomPotal[i - 1])
                    i--;
            }
            randomNumber = true;
            StartCoroutine(PotalImageCreate());
        }

        if ((enemyCount == 0 || SceneLoadManager.instance.mapCount % 10 == 4 || SceneLoadManager.instance.mapCount % 10 == 9) && !isPotal) {
            Debug.Log("적이 모두 죽었을 때");
            PotalCreate();
        }
    }

    IEnumerator PotalImageCreate() {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < potalImagePos.Length; i++) {
            potalImagePos[i].GetComponent<SpriteRenderer>().sprite = potalImageSprite[randomPotal[i]];
        }
    }

    void PotalCreate() {
        // 0~3 까지 중복 되지 않는 두개의 숫자 생성 
        // 그 두개의 숫자를 가지고 포탈을 생성 
        // 프리팹으로 생성하되 위치는 potalPosition 위치로
        if (SceneLoadManager.instance.mapCount % 10 == 3 || SceneLoadManager.instance.mapCount % 10 == 8) {
            Instantiate(potalPrefabs[4], clearCompensationPos.transform.position, Quaternion.identity);
        }
        else {
            for (int i = 0; i < potalPosition.Length; i++) {
                Instantiate(potalPrefabs[randomPotal[i]], potalPosition[i].transform.position, Quaternion.identity);
            }
        }

        // potalID 에 맞는 아이템 생성 
        if(potalID == 1 || potalID == 2) {
            // 골드 생성 
            int random = Random.Range(2, 7);
            for (int i = 0; i < random; i++) {
                GameObject coin = poolManager.GetObject(5);
                coin.GetComponent<Coin>().coinValue = Random.Range(10, 15);
                coin.transform.position = clearCompensationPos.position;
            }
        }
        else if(potalID == 3) {
            // 무기 생성 
            int rand = Random.Range(0, weaponItem.Length);
            GameObject weaponObj = Instantiate(weaponItem[rand], transform.position, Quaternion.identity);
            weaponObj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else if(potalID == 4) {
            // 아이템 생성 
            int rand;
            int totalRand = Random.Range(1, 101);
            // 확률형 아이템 드랍 
            // 10% 확률로 성능이 좋은 2개의 아이템중 하나가 나온다.
            if (totalRand < 90)
                rand = Random.Range(11, poolManager.prefabs.Length - 2);
            else
                rand = Random.Range(poolManager.prefabs.Length - 2,poolManager.prefabs.Length);

            GameObject item = poolManager.GetObject(rand);
            item.transform.position = clearCompensationPos.position;
            item.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        isPotal = true;
    }

    public void MainScene() {
        if (playerPrefab.GetComponent<Player>().health <= 0) {

            GameReset();
            LodingScene.LoadScene(0);
        }
    }

    public void GameReset() {
        // 전체 코인 계산 ( 보스잡은 수 * 30 + 중간보스 * 15 + 잡몹 * 3 + 스테이지 수 * 2 ) 
        worldCoinValue += (enemyTotal + ((SceneLoadManager.instance.stageCount * SceneLoadManager.instance.mapCount) * 2));

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
