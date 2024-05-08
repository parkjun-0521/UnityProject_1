using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float damage;

    public float weaponPower;

    Rigidbody2D rigid;
    Player player;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnEnable() {
        player = GameManager.instance.playerPrefab.GetComponent<Player>();

        // 최종 데미지는 ( 무기공격력 / 10 ) * 플레이어 공격력 
        GameManager.instance.fireBallPrefab.GetComponent<FireBall>().damage = (player.power / 2) + ((player.power / 10) * (player.weaponPower / 10));
        StartCoroutine(Destroy());
    }

    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (player.isPlayerRot)
            rigid.AddForce(Vector2.right * 0.025f, ForceMode2D.Impulse);      
        else 
            rigid.AddForce(Vector2.right * -0.025f, ForceMode2D.Impulse);
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.85f);
        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D( Collider2D collision )
    {
        if (collision.CompareTag("Enemy")) {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
