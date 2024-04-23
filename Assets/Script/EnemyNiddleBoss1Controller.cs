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
        Debug.Log("�߾� ������� �̵� �ؼ� �Ʒ� ���� => �������� ��� ���̾");
        // x = -1, y = 5  ( �߾� 
        transform.position = new Vector2(-1f, 5f);
		rigid.gravityScale = 0;

		StartCoroutine(FirdBallGravity());
    }

	IEnumerator FirdBallGravity() {
		yield return new WaitForSeconds(2f);
        rigid.gravityScale = 1;
    }

    public void Sword() {
        Debug.Log("Ư�� ��ġ �̵� (���� �������� �̵� ) �˱� ������");
		int rand = Random.Range(0, 2);
		if(rand == 0) {
			// ���� 
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            transform.position = new Vector2(-13f, -3f);
        }
		else if(rand == 1){
            // ������ 
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            transform.position = new Vector2(16f, -3f);		
        }
        // x = -15, y = -3 ( ���� �� 
        // x = 16, y = -3 ( ������ �� ( y ȸ�� -180) )
    }
    public void RayserRain() {
        Debug.Log("���� �� ���� ������ �Ʒ����� ���� �߻�, ���� ���������� �� ������ ��");
        // x = -15, y = -3 ( ���� �� 
        // x = 16, y = -3 ( ������ ��
    }
    public void Heal() {
        Debug.Log("���� ü�� ȸ�� ��");
		GameObject heal = Instantiate(healpartical);
		heal.transform.position = particalPos.position;
    }
}



