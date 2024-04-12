using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header ("Move")]
    public float moveSpeed;

    [Header ("Jump")]
    public float jumpPower;
    public float maxJumpSpeed;
    public int jumpCount;
    bool isJumpChek = false;

    [Header ("Attack")]
    public float maxAttackDelay;
    float curAttackDelay;
    public Transform attackArea;
    public bool isPlayerRot;

    Rigidbody2D rigid;
    Animator anime;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
    }

    void Update()
    {
        PlayerMove();
        PlayerJump();
        PlayerAttack();
        PlayerDash();
    }

    void PlayerMove(){
        Vector3 movePosition = Vector3.zero;

        float h = Input.GetAxisRaw("Horizontal");
        if(h < 0){
            movePosition = Vector3.left;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            anime.SetBool("isRun", true);
        }
        else if (h == 0){
            movePosition = Vector3.zero;
            anime.SetBool("isRun", false);
        }
        else {
            movePosition = Vector3.right;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            anime.SetBool("isRun", true);
        }
        transform.position += movePosition * moveSpeed * Time.deltaTime;

        
    }

    void PlayerJump(){
        
        if(Input.GetKeyDown(KeyCode.X) && !isJumpChek){
            anime.SetBool("isRunAndJump", true);
            anime.SetBool("isJump", true);
            jumpCount++;
            if(jumpCount == 2){
                isJumpChek = true;
                jumpCount = 0;
            }
            if(Mathf.Abs(rigid.velocity.y) < maxJumpSpeed){
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }
        }

        if (rigid.velocity.y < 0){
            Debug.DrawRay(rigid.position, Vector2.down, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("Floor"));
            if (hit.collider != null)
            {
                if(hit.collider.tag == "Floor"){
                    isJumpChek = false;
                    anime.SetBool("isRunAndJump", false);
                    anime.SetBool("isJump", false);
                    anime.SetBool("isDash", false);
                }
            }   
        }
    }

    void PlayerAttack(){
        curAttackDelay += Time.deltaTime; 

        if(Input.GetKeyDown(KeyCode.Z) && maxAttackDelay < curAttackDelay){
            anime.SetTrigger("attack");
            if(transform.rotation == Quaternion.Euler(0f, 0f, 0f)) 
                 isPlayerRot = true;
            else
                 isPlayerRot= false;

            Instantiate(GameManager.instance.fireBallPrefab, attackArea.position, Quaternion.identity);
            moveSpeed = 2f;
            curAttackDelay = 0;
            Invoke("MoveSet", 0.5f);
        }
    }

    void MoveSet(){
        moveSpeed = 5f;
    }

    void PlayerDash(){
        if(Input.GetKeyDown(KeyCode.C)){
            anime.SetBool("isDash", true);
            moveSpeed = 30;
            Invoke("StopDashAnime", 0.15f); 
        }
    }

    void StopDashAnime(){
        moveSpeed = 5;
        anime.SetBool("isDash", false);
    }

    void OnCollisionEnter2D( Collision2D collision )
    {
        if (collision.collider.CompareTag("Enemy")) {
            anime.SetTrigger("hurt");
        }
    }

    void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.CompareTag("EndPoint")) {
            GameManager.instance.enemyCount--;
            collision.gameObject.SetActive(false);
        }
    }

}
