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

    [Header("�ΰ��� UI")]
    public GameObject mainUI;
    public GameObject statusUpgradeUI;
    public GameObject settingUI;
    public GameObject playerStatusUI;

    [Header("��ȭ ����")]
    public PlayerStatus playerStatus;

    [Header("��ȭ �̹��� UI")]
    public Image[] healthBar;
    public Image[] speedBar;
    public Image[] powerBar;

    [Header("��ȭ ��ġ")]
    public int healthCount = 0;
    public int speedCount = 0;
    public int powerCount = 0;

    [Header("��ȭ ���")]
    public int healthCost;
    public int speedCost;
    public int powerCost;


    [Header("ü�� UI")]
    public Text playerHealth;
    public Text playerMaxHealth;
    public Slider healthValue;

    [Header("��ȭ UI")]
    public Text enemyKillValue;
    public Text coinValue;
    public Text worldCoinValue;

    [Header("�÷��̾� �ɷ�ġ Text")]
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

    // �ΰ��� �޴� UI
    public void GameUIActive() {
        if (gameUI.activeSelf) {
            // �÷��̾� ü�� UI           
            if (playerLogic == null) {
                playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
            }
            healthValue.value = playerLogic.health / playerLogic.maxHealth;
            playerHealth.text = (Mathf.Round(playerLogic.health)).ToString();
            playerMaxHealth.text = " / " + (Mathf.Round(playerLogic.maxHealth)).ToString();

            // �ΰ��� ��ȭ
            coinValue.text = GameManager.instance.coinValue.ToString();
            enemyKillValue.text = GameManager.instance.enemyKillCount.ToString();
            worldCoinValue.text = GameManager.instance.worldCoinValue.ToString();
        }
    }

    // tab ���� �� �����ϴ� Player ����â 
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

    // �÷��̾� �̵� ����
    public void PlayerStop() {
        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        playerLogic.isNPC = true;

        Animator playerAnimeLogic = GameManager.instance.playerPrefab.GetComponent<Animator>();
        playerAnimeLogic.SetBool("isJump", false);
        playerAnimeLogic.SetBool("isRunAndJump", false);
        playerAnimeLogic.enabled = false;
    }

    // �÷��̾� �̵����� ���� 
    public void PlayerStart() {
        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        playerLogic.isNPC = false;

        GameManager.instance.playerPrefab.GetComponent<Animator>().enabled = true;
    }

    // ��ȭ UI �ѱ� 
    public void PlayerUpgradeNPC() {
        if (settingUI.activeSelf || playerStatusUI.activeSelf) 
            return;

        statusUpgradeUI.SetActive(true);

        // �÷��̾� �̵� ���� 
        PlayerStop();

    }

    // ��ȭ UI ���� 
    public void ExitUpgradeUI() {
        statusUpgradeUI.SetActive(false);

        // �÷��̾� �̵� ���� ���� 
        PlayerStart();
    }

    // ü�� ���׷��̵� 
    public void HealthUpgrade() {

        if(GameManager.instance.worldCoinValue >= healthCost) {
            if(healthCount > 9) { return; }
            GameManager.instance.worldCoinValue -= healthCost;
            healthBar[healthCount].color = Color.yellow;
            healthCount++;
            healthCost += 100;
        }
    }

    // �ӵ� ���׷��̵� 
    public void SpeedUpgrade() {
        if (GameManager.instance.worldCoinValue >= speedCost) {
            if (speedCount > 9) { return; }
            GameManager.instance.worldCoinValue -= speedCost;
            speedBar[speedCount].color = Color.yellow;
            speedCount++;
            speedCost += 100;
        }
    }

    // ���ݷ� ���׷��̵� 
    public void PowerUpgrade() {
        if (GameManager.instance.worldCoinValue >= powerCost) {
            if (powerCount > 9) { return; }
            GameManager.instance.worldCoinValue -= powerCost;
            powerBar[powerCount].color = Color.yellow;
            powerCount++;
            powerCost += 100;
        }
    }

    // ���� ���� UI �ѱ� 
    public void StatusUI() {
        PlayerStop();
        Time.timeScale = 0.0f;
        playerStatusUI.SetActive(true);
    }

    // ���� ���� UI ����
    public void StatusUIExit() {
        PlayerStart();
        Time.timeScale = 1.0f;
        playerStatusUI.SetActive(false);
    }

    // ���� UI Ű�� 
    public void SettingUI() {
        PlayerStop();
        Time.timeScale = 0.0f;
        settingUI.SetActive(true);
    }

    // ���� UI ���� 
    public void SettingUIExit() {
        PlayerStart();
        Time.timeScale = 1.0f;
        settingUI.SetActive(false);
    }
}
