using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int coinValue;

    public int enemyCount;
    bool isPotal = false;

    public GameObject player;
    public Pooling poolManager;
    public CinemachineVirtualCamera cameraPlayer;

    public GameObject fireBallPrefab;
    public GameObject playerPrefab;

    public GameObject[] potalPrefabs;
    public GameObject[] potalPosition;
    public GameObject endPoint;


    void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
        Init(); 
    }

    void Init() {
        enemyCount = 1;
        isPotal = false;
        // 플레이어 생성 및 위치 초기화 
        GameObject playerObj = Instantiate(player, new Vector2(-10f, -5f), Quaternion.Euler(0f, 0f, 0f));

        cameraPlayer.Follow = playerObj.transform;
        playerPrefab = playerObj;

        for (int i = 0; i < potalPosition.Length; i++) {
            potalPosition[i] = GameObject.Find("PotalPoint_" + (i+1));
        }
        endPoint = GameObject.Find("EndPoint");
    }


    void Update() {
        if((enemyCount == 0 || SceneLoadManager.instance.mapCount == 4 || SceneLoadManager.instance.mapCount == 9) && !isPotal) {
            Debug.Log("적이 모두 죽었을 때");
            PotalCreate();
        }
    }

    void PotalCreate() {
        // 0~3 까지 중복 되지 않는 두개의 숫자 생성 
        // 그 두개의 숫자를 가지고 포탈을 생성 
        // 프리팹으로 생성하되 위치는 potalPosition 위치로
        int[] random = new int[potalPosition.Length];
        for (int i = 0; i < potalPosition.Length; i++) {
            random[i] = Random.Range(0, 4);
            Debug.Log(random[i]);
            if (i == 1 && random[i] == random[i - 1])
                i--;
            else {
                GameObject potal = Instantiate(potalPrefabs[random[i]], potalPosition[i].transform.position, Quaternion.identity); 
            }
        }
        isPotal = true;
    }
}
