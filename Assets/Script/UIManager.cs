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
    public int healthCost = 100;
    public int speedCost = 100;
    public int powerCost = 100;
    public Text healthCostText;
    public Text speedCostText;
    public Text powerCostText;


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
    // ������ ������ 
    public Text statusItemHealthText;
    public Text statusItemSpeedText;
    public Text statusItemPowerText;
    // ���� ������ 
    public Text statusWeaponHealthText;
    public Text statusWeaponSpeedText;

    [Header("������ UI")]
    public Text[] itemSetImage;
    public Text[] itemSetText;
    public Sprite[] itemSprite;
    public Image[] itemIconImage;

    Player playerLogic;

    public bool isEnding = false;

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
        if (!isEnding) {
            if (Input.GetKeyDown(KeyCode.Escape) && statusUpgradeUI.activeSelf) {
                ExitUpgradeUI();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && !statusUpgradeUI.activeSelf && !mainUI.activeSelf && !playerStatusUI.activeSelf) {
                if (!settingUI.activeSelf)
                    SettingUI();
                else
                    SettingUIExit();
            }
            else if (Input.GetKeyDown(KeyCode.Tab) && !statusUpgradeUI.activeSelf && !mainUI.activeSelf && !settingUI.activeSelf) {
                if (!playerStatusUI.activeSelf)
                    StatusUI();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && playerStatusUI.activeSelf) {
                StatusUIExit();
            }
        }
    }

    public void GameSart() {
        gameManagerObj.SetActive(true);
        mainUI.SetActive(false);

        // ������ �ҷ�����
        healthCount = PlayerPrefs.GetInt("HealthLevel");
        speedCount = PlayerPrefs.GetInt("SpeedLevel");
        powerCount = PlayerPrefs.GetInt("PowerLevel");

        // ������ �ҷ����� 
        GameManager.instance.worldCoinValue = PlayerPrefs.GetInt("TotalCoin");

        LodingScene.LoadScene(0);
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
            if (playerLogic == null) {
                playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
            }

            for(int i = 0; i < itemSetImage.Length; i++) {
                itemSetImage[i].gameObject.SetActive(false);
                itemSetText[i].gameObject.SetActive(false);
            }

            // ��Ʈ������ Ȱ��ȭ ���� 
            for(int i = 0; i < GameManager.instance.itemSetKey.Count; i++) {
                itemSetImage[i].gameObject.SetActive(true);
                itemSetText[i].gameObject .SetActive(true);

                itemSetImage[i].text = (GameManager.instance.itemSetKey[i].ToString() + "��° Set ������ Ȱ��ȭ");

                for (int j = 0; j < GameManager.instance.itemSetKey.Count; j++) {
                    int key = GameManager.instance.itemSetKey[i];
                    // ��ųʸ��� �ش� Ű�� �����ϴ��� Ȯ���ϰ�, �����Ѵٸ� ���� �����ͼ� ���
                    if (GameManager.instance.setItemInfo.ContainsKey(key)) {
                        List<float> values = GameManager.instance.setItemInfo[key];
                        string valuesAsString = "";
                        string[] statusText = { "ü��", "�ӵ�", "���ݷ�" };
                        for(int k = 0; k < values.Count; k++) {
                            valuesAsString += statusText[k] + " " + (values[k] * 100).ToString() + "% ����\n";
                        }
                        // i��° Text�� ���� ����
                        itemSetText[i].text = valuesAsString;
                    }
                }
            }

            for(int i = 0; i < itemIconImage.Length; i++) {
                itemIconImage[i].sprite = null;
            }

            // ȹ�� ������ �̹��� ���� 
            for(int i = 0; i < GameManager.instance.itemID.Count; i++) {
                itemIconImage[i].sprite = itemSprite[GameManager.instance.itemID[i]];
            }

            // �⺻ �ɷ�ġ 
            statusHealthText.text = (Mathf.Round(playerLogic.maxHealth)).ToString();
            statusSpeedText.text = (Mathf.Round(playerLogic.moveSpeed * 100.0f) / 100.0f).ToString();
            statusPowerText.text = (Mathf.Round(playerLogic.power * 100.0f) / 100.0f).ToString();
            statusWeaponPowerText.text = playerLogic.weaponPower.ToString();
            statusTotalPowerText.text = Mathf.Round((playerLogic.power / 2) + ((playerLogic.power / 10) * (playerLogic.weaponPower / 10))).ToString();

            // ������ �ɷ�ġ ���� 
            statusItemHealthText.text = (Mathf.Round(playerLogic.itemSumHealth)).ToString();
            statusItemSpeedText.text = (Mathf.Round(playerLogic.itemSumSpeed * 100.0f) / 100.0f).ToString();
            statusItemPowerText.text = (Mathf.Round(playerLogic.itemSumPower * 100.0f) / 100.0f).ToString();

            // ���� �ɷ�ġ ���� 
            statusWeaponHealthText.text = playerLogic.weaponHealth.ToString();
            statusWeaponSpeedText.text = playerLogic.weaponSpeed.ToString();

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

        for (int i = 0; i < healthCount; i++)
            healthBar[i].color = Color.yellow;
        for (int i = 0; i < speedCount; i++)
            speedBar[i].color = Color.yellow;
        for (int i = 0; i < powerCount; i++)
            powerBar[i].color = Color.yellow;

        healthCost = healthCount * 300;
        speedCost = speedCount * 300;
        powerCost = powerCount * 300;
        healthCostText.text = "X " + healthCost.ToString();
        speedCostText.text = "X " + speedCost.ToString();
        powerCostText.text = "X " + powerCost.ToString();

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
            ++healthCount;
            healthCost = healthCount * 300;
            healthCostText.text = "X " + healthCost.ToString();
            PlayerPrefs.SetInt("HealthLevel", healthCount);
            PlayerPrefs.SetInt("TotalCoin", GameManager.instance.worldCoinValue);
        }
    }

    // �ӵ� ���׷��̵� 
    public void SpeedUpgrade() {
        if (GameManager.instance.worldCoinValue >= speedCost) {
            if (speedCount > 9) { return; }
            GameManager.instance.worldCoinValue -= speedCost;
            speedBar[speedCount].color = Color.yellow;
            ++speedCount;
            speedCost = speedCount * 300;
            speedCostText.text = "X " + speedCost.ToString();
            PlayerPrefs.SetInt("SpeedLevel", speedCount);
            PlayerPrefs.SetInt("TotalCoin", GameManager.instance.worldCoinValue);
        }
    }

    // ���ݷ� ���׷��̵� 
    public void PowerUpgrade() {
        if (GameManager.instance.worldCoinValue >= powerCost) {
            if (powerCount > 9) { return; }
            GameManager.instance.worldCoinValue -= powerCost;
            powerBar[powerCount].color = Color.yellow;
            ++powerCount;
            powerCost = powerCount * 300;
            powerCostText.text = "X " + powerCost.ToString();
            PlayerPrefs.SetInt("PowerLevel", powerCount);
            PlayerPrefs.SetInt("TotalCoin", GameManager.instance.worldCoinValue);
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
