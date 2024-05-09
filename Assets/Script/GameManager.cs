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

    public List<int> itemSetKey;                            // 아이템 셋트가 활성화 되면 저장되는 셋트 값 
    public List<int> setItem;                               // 아이템의 셋트 id
    public List<int> itemID;                                // 아이템의 id
    public Dictionary<int, List<float>> setItemInfo;        // ui에 띄우기 위함 값 
    public List<List<float>> itemStatus;                    // 아이템 버리기 위해 가져온 아이템의 능력치 값 

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
            Instantiate(potalPrefabs[randomPotal[0]], potalPosition[0].transform.position, Quaternion.identity);
        }
        else {
            for (int i = 0; i < potalPosition.Length; i++) {
                Instantiate(potalPrefabs[randomPotal[i]], potalPosition[i].transform.position, Quaternion.identity);
            }
        }
        isPotal = true;
    }

    public void MainScene() {
        if (playerPrefab.GetComponent<Player>().health <= 0) {

            // 전체 코인 계산 ( 보스잡은 수 * 30 + 중간보스 * 15 + 잡몹 * 3 + 스테이지 수 * 2 ) 
            worldCoinValue += (enemyTotal + ((SceneLoadManager.instance.stageCount * SceneLoadManager.instance.mapCount) * 2));

            SceneLoadManager.instance.mapCount = 0;
            SceneLoadManager.instance.stageCount = 0;
            enemyKillCount = 0;
            coinValue = 0;
            enemyTotal = 0;

            itemSetKey = new List<int>();
            setItem = new List<int>();
            itemID = new List<int>();
            setItemInfo = new Dictionary<int, List<float>>();
            itemStatus = new List<List<float>>();

            mainCamera.GetComponent<Camera>().orthographicSize = 7;
            mainCamera.GetComponent<CameraManager>().mapSize = new Vector2(25f, 10);
            mainCamera.GetComponent<CameraManager>().center = new Vector2(0, 0);
            LodingScene.LoadScene(0);
        }
    }
}
