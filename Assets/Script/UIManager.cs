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
    public int healthCost = 100;
    public int speedCost = 100;
    public int powerCost = 100;
    public Text healthCostText;
    public Text speedCostText;
    public Text powerCostText;


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
    // 아이템 증가량 
    public Text statusItemHealthText;
    public Text statusItemSpeedText;
    public Text statusItemPowerText;
    // 무기 증가량 
    public Text statusWeaponHealthText;
    public Text statusWeaponSpeedText;

    [Header("아이템 UI")]
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

        // 데이터 불러오기
        healthCount = PlayerPrefs.GetInt("HealthLevel");
        speedCount = PlayerPrefs.GetInt("SpeedLevel");
        powerCount = PlayerPrefs.GetInt("PowerLevel");

        // 데이터 불러오기 
        GameManager.instance.worldCoinValue = PlayerPrefs.GetInt("TotalCoin");

        LodingScene.LoadScene(0);
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
            if (playerLogic == null) {
                playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
            }

            for(int i = 0; i < itemSetImage.Length; i++) {
                itemSetImage[i].gameObject.SetActive(false);
                itemSetText[i].gameObject.SetActive(false);
            }

            // 셋트아이템 활성화 설명 
            for(int i = 0; i < GameManager.instance.itemSetKey.Count; i++) {
                itemSetImage[i].gameObject.SetActive(true);
                itemSetText[i].gameObject .SetActive(true);

                itemSetImage[i].text = (GameManager.instance.itemSetKey[i].ToString() + "번째 Set 아이템 활성화");

                for (int j = 0; j < GameManager.instance.itemSetKey.Count; j++) {
                    int key = GameManager.instance.itemSetKey[i];
                    // 딕셔너리에 해당 키가 존재하는지 확인하고, 존재한다면 값을 가져와서 출력
                    if (GameManager.instance.setItemInfo.ContainsKey(key)) {
                        List<float> values = GameManager.instance.setItemInfo[key];
                        string valuesAsString = "";
                        string[] statusText = { "체력", "속도", "공격력" };
                        for(int k = 0; k < values.Count; k++) {
                            valuesAsString += statusText[k] + " " + (values[k] * 100).ToString() + "% 증가\n";
                        }
                        // i번째 Text에 값을 설정
                        itemSetText[i].text = valuesAsString;
                    }
                }
            }

            for(int i = 0; i < itemIconImage.Length; i++) {
                itemIconImage[i].sprite = null;
            }

            // 획득 아이템 이미지 적용 
            for(int i = 0; i < GameManager.instance.itemID.Count; i++) {
                itemIconImage[i].sprite = itemSprite[GameManager.instance.itemID[i]];
            }

            // 기본 능력치 
            statusHealthText.text = (Mathf.Round(playerLogic.maxHealth)).ToString();
            statusSpeedText.text = (Mathf.Round(playerLogic.moveSpeed * 100.0f) / 100.0f).ToString();
            statusPowerText.text = (Mathf.Round(playerLogic.power * 100.0f) / 100.0f).ToString();
            statusWeaponPowerText.text = playerLogic.weaponPower.ToString();
            statusTotalPowerText.text = Mathf.Round((playerLogic.power / 2) + ((playerLogic.power / 10) * (playerLogic.weaponPower / 10))).ToString();

            // 아이템 능력치 적용 
            statusItemHealthText.text = (Mathf.Round(playerLogic.itemSumHealth)).ToString();
            statusItemSpeedText.text = (Mathf.Round(playerLogic.itemSumSpeed * 100.0f) / 100.0f).ToString();
            statusItemPowerText.text = (Mathf.Round(playerLogic.itemSumPower * 100.0f) / 100.0f).ToString();

            // 무기 능력치 적용 
            statusWeaponHealthText.text = playerLogic.weaponHealth.ToString();
            statusWeaponSpeedText.text = playerLogic.weaponSpeed.ToString();

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
            ++healthCount;
            healthCost = healthCount * 300;
            healthCostText.text = "X " + healthCost.ToString();
            PlayerPrefs.SetInt("HealthLevel", healthCount);
            PlayerPrefs.SetInt("TotalCoin", GameManager.instance.worldCoinValue);
        }
    }

    // 속도 업그레이드 
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

    // 공격력 업그레이드 
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
