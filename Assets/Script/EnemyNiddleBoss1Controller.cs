using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyNiddleBoss1Controller : MonoBehaviour
{

	public Animator anim;
	public GameObject healpartical;
	public Transform particalPos;

	Rigidbody2D rigid;

	void Awake() {
        rigid = GetComponent<Rigidbody2D>();

    }

	// Controls facing direction
	public bool facingRight;

	public void Jump()
    {
		anim.SetBool("Jump", true) ;
	}

	public void JumpOff()
    {
		anim.SetBool("Jump", false);
	}

	public void Dead()
	{
		anim.SetBool("Dead" , true);
	}

	public void DeadOff()
	{
		anim.SetBool("Dead", false);
	}
	public void Walk()
	{
		anim.SetBool("Walk" , true);
	}

	public void WalkOff()
	{
		anim.SetBool("Walk", false);
	}
	public void Run()
	{
		anim.SetBool("Run" , true);
	}
	public void RunOff()
	{
		anim.SetBool("Run", false);
	}
	public void Attack()
	{
		anim.SetBool("Attack" , true);
	}
	public void AttackOff()
	{
		anim.SetBool("Attack", false);
	}

	void Update()
	{
		Attack();
    } 

	public void FireBall() {
        Debug.Log("중앙 상단으로 이동 해서 아래 방향 => 무작위로 쏘는 파이어볼");
        // x = -1, y = 5  ( 중앙 
        transform.position = new Vector2(-1f, 5f);
		rigid.gravityScale = 0;

		StartCoroutine(FirdBallGravity());
    }

	IEnumerator FirdBallGravity() {
		yield return new WaitForSeconds(2f);
        rigid.gravityScale = 1;
    }

    public void Sword() {
        Debug.Log("특정 위치 이동 (양쪽 구석으로 이동 ) 검기 날리기");
		int rand = Random.Range(0, 2);
		if(rand == 0) {
			// 왼쪽 
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            transform.position = new Vector2(-13f, -3f);
        }
		else if(rand == 1){
            // 오른쪽 
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            transform.position = new Vector2(16f, -3f);		
        }
        // x = -15, y = -3 ( 왼쪽 끝 
        // x = 16, y = -3 ( 오른쪽 끝 ( y 회전 -180) )
    }
    public void RayserRain() {
        Debug.Log("왼쪽 끝 부터 레이저 아래에서 위로 발사, 점점 오른쪽으로 옴 레이져 비");
        // x = -15, y = -3 ( 왼쪽 끝 
        // x = 16, y = -3 ( 오른쪽 끝
    }
    public void Heal() {
        Debug.Log("보스 체력 회복 힐");
		GameObject heal = Instantiate(healpartical);
		heal.transform.position = particalPos.position;
    }
}



