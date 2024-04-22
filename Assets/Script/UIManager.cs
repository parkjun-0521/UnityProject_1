using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.LowLevel;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject gameManagerObj;
    public GameObject gameUI;

    [Header("인게임 UI")]
    public GameObject mainUI;
    public GameObject statusUpgradeUI;
    public GameObject settingUI;
    public GameObject playerStatusUI;

    [Header("강화 로직")]
    public PlayerStatus playerStatus;

    [Header("강화 이미지 UI")]
    public Image[] healthBar;
    public Image[] speedBar;
    public Image[] powerBar;

    [Header("강화 수치")]
    public int healthCount = 0;
    public int speedCount = 0;
    public int powerCount = 0;

    [Header("강화 비용")]
    public int healthCost;
    public int speedCost;
    public int powerCost;


    [Header("체력 UI")]
    public Text playerHealth;
    public Text playerMaxHealth;
    public Slider healthValue;

    [Header("재화 UI")]
    public Text enemyKillValue;
    public Text coinValue;
    public Text worldCoinValue;

    [Header("플레이어 능력치 Text")]
    public Text statusHealthText;
    public Text statusSpeedText;
    public Text statusPowerText;
    public Text statusWeaponPowerText;
    public Text statusTotalPowerText;

    Player playerLogic;

    void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        mainUI.SetActive(true);
        playerStatus = GetComponent<PlayerStatus>();
        
    }

    void Update() {

        GameUIActive();
        PlayerStatusUI();
        if (Input.GetKeyDown(KeyCode.Escape) && statusUpgradeUI.activeSelf) {
            ExitUpgradeUI();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !statusUpgradeUI.activeSelf && !mainUI.activeSelf && !playerStatusUI.activeSelf) {
            if (!settingUI.activeSelf)
                SettingUI();
            else
                SettingUIExit();
        }
        else if(Input.GetKeyDown(KeyCode.Tab) && !statusUpgradeUI.activeSelf && !mainUI.activeSelf && !settingUI.activeSelf) {
            if (!playerStatusUI.activeSelf) 
                StatusUI();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && playerStatusUI.activeSelf) {
            StatusUIExit();
        }
    }

    public void GameSart() {
        gameManagerObj.SetActive(true);
        mainUI.SetActive(false);
        SceneManager.LoadScene(0);
    }

    // 인게임 메뉴 UI
    public void GameUIActive() {
        if (gameUI.activeSelf) {
            // 플레이어 체력 UI           
            if (playerLogic == null) {
                playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
            }
            healthValue.value = playerLogic.health / playerLogic.maxHealth;
            playerHealth.text = (Mathf.Round(playerLogic.health)).ToString();
            playerMaxHealth.text = " / " + (Mathf.Round(playerLogic.maxHealth)).ToString();

            // 인게임 재화
            coinValue.text = GameManager.instance.coinValue.ToString();
            enemyKillValue.text = GameManager.instance.enemyKillCount.ToString();
            worldCoinValue.text = GameManager.instance.worldCoinValue.ToString();
        }
    }

    // tab 누를 시 등장하는 Player 상태창 
    public void PlayerStatusUI() {
        if (playerStatusUI.activeSelf) {
            Player player = GameManager.instance.playerPrefab.GetComponent<Player>();
            if (playerLogic == null) {
                playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
            }
            statusHealthText.text = (Mathf.Round(playerLogic.maxHealth)).ToString();
            statusSpeedText.text = (Mathf.Round(playerLogic.moveSpeed * 100.0f) / 100.0f).ToString();
            statusPowerText.text = (Mathf.Round(playerLogic.power * 100.0f) / 100.0f).ToString();
            statusWeaponPowerText.text = player.weaponPower.ToString();
            statusTotalPowerText.text = Mathf.Round(player.power + (player.power * (player.weaponPower / 10) * 100.0f) / 100.0f).ToString();
        }
    }

    // 플레이어 이동 제한
    public void PlayerStop() {
        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        playerLogic.isNPC = true;

        Animator playerAnimeLogic = GameManager.instance.playerPrefab.GetComponent<Animator>();
        playerAnimeLogic.SetBool("isJump", false);
        playerAnimeLogic.SetBool("isRunAndJump", false);
        playerAnimeLogic.enabled = false;
    }

    // 플레이어 이동제한 해제 
    public void PlayerStart() {
        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        playerLogic.isNPC = false;

        GameManager.instance.playerPrefab.GetComponent<Animator>().enabled = true;
    }

    // 강화 UI 켜기 
    public void PlayerUpgradeNPC() {
        if (settingUI.activeSelf || playerStatusUI.activeSelf) 
            return;

        statusUpgradeUI.SetActive(true);

        // 플레이어 이동 제한 
        PlayerStop();

    }

    // 강화 UI 끄기 
    public void ExitUpgradeUI() {
        statusUpgradeUI.SetActive(false);

        // 플레이어 이동 제한 해제 
        PlayerStart();
    }

    // 체력 업그레이드 
    public void HealthUpgrade() {

        if(GameManager.instance.worldCoinValue >= healthCost) {
            if(healthCount > 9) { return; }
            GameManager.instance.worldCoinValue -= healthCost;
            healthBar[healthCount].color = Color.yellow;
            healthCount++;
            healthCost += 100;
        }
    }

    // 속도 업그레이드 
    public void SpeedUpgrade() {
        if (GameManager.instance.worldCoinValue >= speedCost) {
            if (speedCount > 9) { return; }
            GameManager.instance.worldCoinValue -= speedCost;
            speedBar[speedCount].color = Color.yellow;
            speedCount++;
            speedCost += 100;
        }
    }

    // 공격력 업그레이드 
    public void PowerUpgrade() {
        if (GameManager.instance.worldCoinValue >= powerCost) {
            if (powerCount > 9) { return; }
            GameManager.instance.worldCoinValue -= powerCost;
            powerBar[powerCount].color = Color.yellow;
            powerCount++;
            powerCost += 100;
        }
    }

    // 현재 상태 UI 켜기 
    public void StatusUI() {
        PlayerStop();
        Time.timeScale = 0.0f;
        playerStatusUI.SetActive(true);
    }

    // 현재 상태 UI 끄기
    public void StatusUIExit() {
        PlayerStart();
        Time.timeScale = 1.0f;
        playerStatusUI.SetActive(false);
    }

    // 셋팅 UI 키고 
    public void SettingUI() {
        PlayerStop();
        Time.timeScale = 0.0f;
        settingUI.SetActive(true);
    }

    // 셋팅 UI 끄기 
    public void SettingUIExit() {
        PlayerStart();
        Time.timeScale = 1.0f;
        settingUI.SetActive(false);
    }
}
