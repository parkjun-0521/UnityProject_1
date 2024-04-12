using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    public CinemachineVirtualCamera cameraPlayer;

    public GameObject fireBallPrefab;
    public GameObject playerPrefab;

    public GameObject[] potalPrefabs;
    public GameObject[] potalPosition;
    public GameObject endPoint;

    public int enemyCount;

    bool isPotal = false;

    void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
        Init();
    }

    void Init() {
        enemyCount = 1;
        isPotal = false;
        GameObject playerObj = Instantiate(player, transform.position = new Vector2(-10, -5), Quaternion.Euler(0f, 0f, 0f));
        cameraPlayer.Follow = playerObj.transform;
        playerPrefab = playerObj;
        for (int i = 0; i < potalPosition.Length; i++) {
            potalPosition[i] = GameObject.Find("PotalPoint_" + (i+1));
        }
        endPoint = GameObject.Find("EndPoint");
    }


    void Update() {
        if((enemyCount == 0 || SceneLoadManager.instance.mapCount == 4 || SceneLoadManager.instance.mapCount == 9) && !isPotal) {
            Debug.Log("���� ��� �׾��� ��");
            PotalCreate();
        }
    }

    void PotalCreate() {
        // 0~3 ���� �ߺ� ���� �ʴ� �ΰ��� ���� ���� 
        // �� �ΰ��� ���ڸ� ������ ��Ż�� ���� 
        // ���������� �����ϵ� ��ġ�� potalPosition ��ġ��
        int[] random = new int[potalPosition.Length];
        for (int i = 0; i < potalPosition.Length; i++) {
            random[i] = Random.Range(0, potalPrefabs.Length);
            Debug.Log(random[i]);
            if (i == 1 && random[i] == random[i - 1])
                i--;
            else
                Instantiate(potalPrefabs[random[i]], potalPosition[i].transform.position, Quaternion.identity);
        }
        isPotal = true;
    }
}
