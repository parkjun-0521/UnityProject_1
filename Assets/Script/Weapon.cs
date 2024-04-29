using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponID;

    public float playerHealth;
    public float playerSpeed;
    public float weaponPower;

    bool isPlayerCheck = false;
    bool isWeaponCheck = false;

    public Rigidbody2D rigid;
    public bool itemJump= false;
    void Awake() {
        rigid = GetComponent<Rigidbody2D>();  
    }

    void OnEnable() {
        // 위로 랜덤한 힘을 줘서 튀어오르는 것 같이 보이게 ( 위 + 좌우 ) 
        if (!itemJump) {
            int rendom = Random.Range(0, 2);
            float renPower = Random.Range(0f, 3f);
            Vector2 power = (rendom == 0) ? Vector2.left * renPower : Vector2.right * renPower;
            rigid.AddForce(Vector2.up * 5 + power, ForceMode2D.Impulse);
        }
    }

    void Update() {

        WeaponDrop();

        if (Input.GetKeyDown(KeyCode.F) && isPlayerCheck && !isWeaponCheck) {
            // 무기에 닿았을 때 
            Player playerLogic = GameManager.instance.playerPrefab.GetComponent<Player>();

            playerLogic.weaponID = this.weaponID;

            playerLogic.weaponHealth = this.playerHealth;
            playerLogic.weaponSpeed = this.playerSpeed;
            playerLogic.weaponPower = this.weaponPower;

            playerLogic.maxHealth = playerLogic.upHealth + playerHealth;
            playerLogic.moveSpeed = playerLogic.upMoveSpeed + playerSpeed;

            playerLogic.health = playerLogic.health + playerHealth;
            if (playerLogic.health > playerLogic.maxHealth) {
                playerLogic.health = playerLogic.maxHealth;
            }

            playerLogic.PlayerWeaponChange();
            isWeaponCheck = true;         
            StartCoroutine(WeaponGet());
        }
    }

    IEnumerator WeaponGet() {
        yield return new WaitForSeconds(0.1f);
        isWeaponCheck = false;
        gameObject.SetActive(false);
    }

    void WeaponDrop() {
        if (rigid.velocity.y < 0) {
            Debug.DrawRay(rigid.position, Vector2.down, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(rigid.position, Vector2.down, 0.5f, LayerMask.GetMask("Floor"));
            if (hit.collider != null) {
                if (hit.collider.tag == "Floor") {
                    rigid.velocity = Vector3.zero;
                    rigid.gravityScale = 0;
                }
            }
        }
    }

    void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            isPlayerCheck = true;
        }
    }

    void OnTriggerExit2D( Collider2D collision ) {
        if (collision.CompareTag("Player")) {
            isPlayerCheck = false;
        }
    }
}
