using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    Player playerLogic;
    UIManager uiManager;
    void Start() {
        playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
        uiManager = UIManager.Instance.GetComponent<UIManager>();
    }

    // 체력 강화 
    public void HealthStatus() {
        if (uiManager.healthCount <= 10) { 
            playerLogic.maxHealth = HealthUp(uiManager.healthCount + 1) * GameManager.instance.player.GetComponent<Player>().health;
            playerLogic.health = playerLogic.maxHealth;
        }
    }

    public float HealthUp( int count ) {
        if (count <= 1) {
            return 1;
        }
        else {
            return 1.2f * HealthUp(count - 1);
        }
    }

    // 이동 속도 강화 
    public void SpeedStatus() {
        playerLogic.upMoveSpeed = SpeedUp(uiManager.speedCount + 1) * GameManager.instance.player.GetComponent<Player>().upMoveSpeed;
        playerLogic.moveSpeed = playerLogic.upMoveSpeed;
    }

    public float SpeedUp( int count ) {
        if (count <= 1) {
            return 1;
        }
        else {
            return 1.05f * SpeedUp(count - 1);
        }
    }

    // 공격력 강화 
    public void PowerStatus() {
        playerLogic.upPower = PowerUp(uiManager.powerCount + 1) * GameManager.instance.player.GetComponent<Player>().upPower;
        playerLogic.power = playerLogic.upPower;
    }

    public float PowerUp( int count ) {
        if (count <= 1) {
            return 1;
        }
        else {
            return 1.15f * PowerUp(count - 1);
        }
    }
}
