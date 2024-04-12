using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float damage;

    Rigidbody2D rigid;
    Player player;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GameManager.instance.playerPrefab.GetComponent<Player>();
    }

    void Start()
    {
        Invoke("Destroy", 0.85f);
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

    void Destroy()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D( Collider2D collision )
    {
        if (collision.CompareTag("Enemy")) {
            Destroy(gameObject);
        }
    }
}
