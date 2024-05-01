using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SetItem : MonoBehaviour
{
    // 추후 다 완성 이후 개인프로젝트 구간 때 5,6 번 셋트에는 추가 효과 구현 예정 

    // 0번 Set : 공격력(소폭) 속도(대폭) 강화 체력 감소  
    // 1번 Set : 공격력(대폭) 강화 체력 감소
    // 2번 Set : 공격력(소폭) 속도(소폭) 강화 체력(대폭) 증가 
    // 4번 Set : 공격력(대폭) 강화 속도(대폭) 감소 
    // 5번 Set : 속도(대폭) 강화 체력 감소
    // 6번 Set : 공격력(대폭) 강화 체력(대폭) 감소 
    Player playerLogic;

    void Awake() {
        playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();
    }

    void SetReset() {
        playerLogic.itemSetSpeed = 0;
        playerLogic.maxHealth = 0f;
        playerLogic.moveSpeed = 0f;
        playerLogic.power = 0f;
    }

    /*  위에는 이전에 대해 증가하는 셋트 
     * //======================================//
     *  아래는 이전에 대해 증가하지 않는 셋트 
     */

    public void Set_1(int key) {      
        if (key == 5) {
            SetOptionStatus(0.2f, 0.2f, 0.2f);
        }
        if (key == 6) {
            SetOptionStatus(0.1f, 0.3f, 0.2f);
        }
        //======================================//
        if (key == 0) {
            SetOptionStatus(0f, 0f, 0f);
        }
        if (key == 1) {
            SetOptionStatus(0f, 0f, 0f);
        }
        if (key == 2) {
            SetOptionStatus(0f, 0f, 0f);
        }
        if (key == 3) {
            SetOptionStatus(0f, 0f, 0f);
        }
        if (key == 4) {
            SetOptionStatus(0f, 0f, 0f);
        }
        if (playerLogic.health > playerLogic.maxHealth)
            playerLogic.health = playerLogic.maxHealth;
    }
    public void Set_2( int key ) {
        if (key == 0) { 
            SetOptionStatus(0.02f, 0.12f, 0.1f); 
        }
        if (key == 1) { 
            SetOptionStatus(0.02f, 0.05f, 0.15f); 
        }
        if (key == 2) { 
            SetOptionStatus(0.2f, 0.05f, 0.05f); 
        }
        if (key == 3) { 
            SetOptionStatus(0f, 0f, 0.3f); 
        }
        if (key == 4) { 
            SetOptionStatus(0f, 0.2f, 0.2f); 
        }
        //======================================//
        if (key == 5) {
            SetOptionStatus(0.2f, 0.2f, 0.2f);
        }
        if (key == 6) {
            SetOptionStatus(0.1f, 0.3f, 0.2f);
        }
        if (playerLogic.health > playerLogic.maxHealth)
            playerLogic.health = playerLogic.maxHealth;
    }
    public void Set_4( int key ) {
        if (key == 0) { 
            SetOptionStatus(0.05f, 0.135f, 0.125f); 
        }
        if (key == 1) { 
            SetOptionStatus(0.05f, 0.075f, 0.20f); 
        }
        if (key == 2) { 
            SetOptionStatus(0.4f, 0.075f, 0.075f);
        }
        //======================================//
        if (key == 3) {
            SetOptionStatus(0f, 0f, 0.3f);
        }
        if (key == 4) {
            SetOptionStatus(0f, 0.2f, 0.2f);
        }
        if (key == 5) {
            SetOptionStatus(0.2f, 0.2f, 0.2f);
        }
        if (key == 6) {
            SetOptionStatus(0.1f, 0.3f, 0.2f);
        }
        if (playerLogic.health > playerLogic.maxHealth)
            playerLogic.health = playerLogic.maxHealth;
    }
    public void Set_6( int key ) {
        if (key == 0) { 
            SetOptionStatus(0.1f, 0.17f, 0.15f); 
        }
        if (key == 1) { 
            SetOptionStatus(0.13f, 0.1f, 0.30f); 
        }
        //======================================//
        if (key == 2) {
            SetOptionStatus(0.4f, 0.075f, 0.075f);
        }
        if (key == 3) {
            SetOptionStatus(0f, 0f, 0.3f);
        }
        if (key == 4) {
            SetOptionStatus(0f, 0.2f, 0.2f);
        }
        if (key == 5) {
            SetOptionStatus(0.2f, 0.2f, 0.2f);
        }
        if (key == 6) {
            SetOptionStatus(0.1f, 0.3f, 0.2f);
        }
        if (playerLogic.health > playerLogic.maxHealth)
            playerLogic.health = playerLogic.maxHealth;
    }

    void SetOptionStatus(float health, float speed, float power) {
        playerLogic.itemSetSpeed = speed;
        playerLogic.maxHealth *= (1.0f + health);
        playerLogic.moveSpeed *= (1.0f + speed);
        playerLogic.power *= (1.0f + power);
    }
}
