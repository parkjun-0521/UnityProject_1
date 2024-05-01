using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SetItem : MonoBehaviour
{
    // ���� �� �ϼ� ���� ����������Ʈ ���� �� 5,6 �� ��Ʈ���� �߰� ȿ�� ���� ���� 

    // 0�� Set : ���ݷ�(����) �ӵ�(����) ��ȭ ü�� ����  
    // 1�� Set : ���ݷ�(����) ��ȭ ü�� ����
    // 2�� Set : ���ݷ�(����) �ӵ�(����) ��ȭ ü��(����) ���� 
    // 4�� Set : ���ݷ�(����) ��ȭ �ӵ�(����) ���� 
    // 5�� Set : �ӵ�(����) ��ȭ ü�� ����
    // 6�� Set : ���ݷ�(����) ��ȭ ü��(����) ���� 
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

    /*  ������ ������ ���� �����ϴ� ��Ʈ 
     * //======================================//
     *  �Ʒ��� ������ ���� �������� �ʴ� ��Ʈ 
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
