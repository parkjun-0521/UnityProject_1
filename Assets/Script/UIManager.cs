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
        // ��ȭ UI ���� 
        statusUpgradeUI.SetActive(true);

        // �÷��̾� �̵� ���� 
        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        playerLogic.isNPC = true;

        Animator playerAnimeLogic = GameManager.instance.playerPrefab.GetComponent<Animator>();
        playerAnimeLogic.SetBool("isJump", false);
        playerAnimeLogic.SetBool("isRunAndJump", false);
        playerAnimeLogic.enabled = false;
    }

    public void ExitUpgradeUI() {
        // ��ȭ UI ���ֱ� 
        statusUpgradeUI.SetActive(false);

        // �÷��̾� �̵� ���� ���� 
        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        playerLogic.isNPC = false;

        GameManager.instance.playerPrefab.GetComponent<Animator>().enabled = true;
    }

    public void HealthUpgrade() {
        // ü�� ���׷��̵� 
        if(GameManager.instance.worldCoinValue >= healthCost) {
            if(healthCount > 9) { return; }
            GameManager.instance.worldCoinValue -= healthCost;
            healthBar[healthCount].color = Color.yellow;
            healthCount++;
            healthCost += 100;
        }
    }

    public void SpeedUpgrade() {
        // �ӵ� ���׷��̵� 
        if (GameManager.instance.worldCoinValue >= speedCost) {
            if (speedCount > 9) { return; }
            GameManager.instance.worldCoinValue -= speedCost;
            speedBar[speedCount].color = Color.yellow;
            speedCount++;
            speedCost += 100;
        }
    }

    public void PowerUpgrade() {
        // ���ݷ� ���׷��̵� 
        if (GameManager.instance.worldCoinValue >= powerCost) {
            if (powerCount > 9) { return; }
            GameManager.instance.worldCoinValue -= powerCost;
            powerBar[powerCount].color = Color.yellow;
            powerCount++;
            powerCost += 100;
        }
    }
}
