using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject gameManagerObj;
    public GameObject gameUI;
    public GameObject mainUI;
    public GameObject statusUpgradeUI;
    public PlayerStatus playerStatus;

    public Image[] healthBar;
    public Image[] speedBar;
    public Image[] powerBar;

    public int healthCount = 0;
    public int speedCount = 0;
    public int powerCount = 0;

    public int healthCost;
    public int speedCost;
    public int powerCost;

    public Text coinValue;

    void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        mainUI.SetActive(true);
        playerStatus = GetComponent<PlayerStatus>();
    }

    void Update() {
        if (gameUI.activeSelf) {
            coinValue.text = GameManager.instance.coinValue.ToString();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            ExitUpgradeUI();
        }
    }

    public void GameSart() {
        gameManagerObj.SetActive(true);
        gameUI.SetActive(true);
        mainUI.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void PlayerUpgradeNPC() {
        // 강화 UI 띄우기 
        statusUpgradeUI.SetActive(true);

        // 플레이어 이동 제한 
        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        playerLogic.isNPC = true;

        Animator playerAnimeLogic = GameManager.instance.playerPrefab.GetComponent<Animator>();
        playerAnimeLogic.SetBool("isJump", false);
        playerAnimeLogic.SetBool("isRunAndJump", false);
        playerAnimeLogic.enabled = false;
    }

    public void ExitUpgradeUI() {
        // 강화 UI 없애기 
        statusUpgradeUI.SetActive(false);

        // 플레이어 이동 제한 해제 
        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        playerLogic.isNPC = false;

        GameManager.instance.playerPrefab.GetComponent<Animator>().enabled = true;
    }

    public void HealthUpgrade() {
        // 체력 업그레이드 
        if(GameManager.instance.worldCoinValue >= healthCost) {
            if(healthCount > 9) { return; }
            GameManager.instance.worldCoinValue -= healthCost;
            healthBar[healthCount].color = Color.yellow;
            healthCount++;
            healthCost += 100;
        }
    }

    public void SpeedUpgrade() {
        // 속도 업그레이드 
        if (GameManager.instance.worldCoinValue >= speedCost) {
            if (speedCount > 9) { return; }
            GameManager.instance.worldCoinValue -= speedCost;
            speedBar[speedCount].color = Color.yellow;
            speedCount++;
            speedCost += 100;
        }
    }

    public void PowerUpgrade() {
        // 공격력 업그레이드 
        if (GameManager.instance.worldCoinValue >= powerCost) {
            if (powerCount > 9) { return; }
            GameManager.instance.worldCoinValue -= powerCost;
            powerBar[powerCount].color = Color.yellow;
            powerCount++;
            powerCost += 100;
        }
    }
}
