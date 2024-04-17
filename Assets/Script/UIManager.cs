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

    public Image[] healthBar;
    public Image[] speedBar;
    public Image[] powerBar;

    int healthCount = -1;
    int speedCount = -1;
    int powerCount = -1;

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
    }

    void Update() {
        if (gameUI.activeSelf) {
            coinValue.text = GameManager.instance.coinValue.ToString();
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
        Animator playerAnimeLogic = GameManager.instance.playerPrefab.GetComponent<Animator>();
        GameManager.instance.playerPrefab.GetComponent<Player>().enabled = false;
        playerAnimeLogic.enabled = false;
        playerAnimeLogic.SetBool("isJump", false);
        playerAnimeLogic.SetBool("isRunAndJump", false);
    }

    public void ExitUpgradeUI() {
        // ��ȭ UI ���ֱ� 
        statusUpgradeUI.SetActive(false);

        // �÷��̾� �̵� ���� ���� 
        Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        playerLogic.enabled = true;
        playerLogic.isJumpChek = false;
        GameManager.instance.playerPrefab.GetComponent<Animator>().enabled = true;
    }

    public void HealthUpgrade() {
        // ü�� ���׷��̵� 
        if(GameManager.instance.worldCoinValue >= healthCost) {
            healthCount++;
            if(healthCount > 9) { return; }
            GameManager.instance.worldCoinValue -= healthCost;
            healthBar[healthCount].color = Color.yellow;
            healthCost += 100;
        }
    }

    public void SpeedUpgrade() {
        // �ӵ� ���׷��̵� 
        if (GameManager.instance.worldCoinValue >= speedCost) {
            speedCount++;
            if (speedCount > 9) { return; }
            GameManager.instance.worldCoinValue -= speedCost;
            speedBar[speedCount].color = Color.yellow;
            speedCost += 100;
        }
    }

    public void PowerUpgrade() {
        // ���ݷ� ���׷��̵� 
        if (GameManager.instance.worldCoinValue >= powerCost) {
            powerCount++;
            if (powerCount > 9) { return; }
            GameManager.instance.worldCoinValue -= powerCost;
            powerBar[powerCount].color = Color.yellow;
            powerCost += 100;
        }
    }

    public void WeaponNPC() {
        Debug.Log("���⸦ �ϳ� �ݴϴ�.");
    }

    public void ItemNPC() {
        Debug.Log("�������� �ϳ� �ݴϴ�.");
    }
}
