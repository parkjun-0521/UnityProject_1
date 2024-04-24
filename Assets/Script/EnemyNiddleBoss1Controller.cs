using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.XR;
using System.Runtime.Serialization;
using UnityEngine.InputSystem.XInput;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyNiddleBoss1Controller : MonoBehaviour
{
	public GameObject[] rayserPos;

    int curPatternCount = 0;
    public int maxFireBall;
    public int piValue;
    public int rand;
    float time;

    public Transform lognAttackPos;

    public GameObject rayser;
	public GameObject healpartical;
    public GameObject enemyFireBall;
    public GameObject longDistanceAttack;

	public Animator anim;
	public Transform particalPos;
    Enemy enemy;

    Rigidbody2D rigid;

	void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

	void Start() {
        enemy = GetComponent<Enemy>();
    }

    void OnEnable() {
        GameObject targetObject = GameObject.Find("RayserSpawn");
        if (targetObject != null) {
            // 대상 오브젝트의 자식 개수만큼 배열 크기를 설정합니다.
            rayserPos = new GameObject[targetObject.transform.childCount];

            // 대상 오브젝트의 각 자식을 배열에 저장합니다.
            for (int i = 0; i < targetObject.transform.childCount; i++) {
                rayserPos[i] = targetObject.transform.GetChild(i).gameObject;
            }
        }
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


	public void FireBall() {
        Debug.Log("중앙 상단으로 이동 해서 아래 방향 => 무작위로 쏘는 파이어볼");
        StartCoroutine(MoveToPosition(new Vector3(-1f, 6f, transform.position.z), enemy.enemySpeed));
        rigid.gravityScale = 0;

        rand = Random.Range(0, 2);
        piValue = 3;

        if(rand == 0) {
            maxFireBall = Random.Range(13,17);
        }
        else {
            maxFireBall = 7;
        }

        Invoke("Fire", 0.5f);


        curPatternCount = 0;
    }

    void Fire() {

        Vector2 dirVec;
        int roundNum;

        if (rand == 0) {
            // 부체꼴로 쏘는 패턴 
            //GameObject enemyFireBallObj = Instantiate(enemyFireBall, transform.position, Quaternion.identity);
            GameObject enemyFireBallObj = GameManager.instance.poolManager.GetObject(7);
            enemyFireBallObj.transform.position = lognAttackPos.position;
            Rigidbody2D rierRigid = enemyFireBallObj.GetComponent<Rigidbody2D>();
            dirVec = new Vector2(Mathf.Sin(Mathf.PI * piValue * curPatternCount / maxFireBall), -1);
            rierRigid.AddForce(dirVec.normalized * 7, ForceMode2D.Impulse);
            time = 0.5f;
        }
        else {
            // 원형으로 쏘는 패턴 
            roundNum = curPatternCount % 2 == 0 ? 13 : 14;
            for (int i = 0; i < roundNum; i++) {
                float angle = 360f * i / roundNum;
                float radians = angle * Mathf.Deg2Rad;
                dirVec = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

                //GameObject enemyFireBallObj = Instantiate(enemyFireBall, transform.position, Quaternion.identity);
                GameObject enemyFireBallObj = GameManager.instance.poolManager.GetObject(7);
                enemyFireBallObj.transform.position = lognAttackPos.position;
                Rigidbody2D rierRigid = enemyFireBallObj.GetComponent<Rigidbody2D>();
                rierRigid.AddForce(dirVec.normalized * 7, ForceMode2D.Impulse);
            }
            time = 1f;
        }

        curPatternCount++;

        if (curPatternCount < maxFireBall)
            Invoke("Fire", time);
        else {
            StartCoroutine(FirdBallGravity(curPatternCount));
        }
    }

    IEnumerator FirdBallGravity(int count) {
		yield return new WaitForSeconds(0.5f);
        rigid.gravityScale = 1;
    }

    public void Sword() {
        Debug.Log("특정 위치 이동 (양쪽 구석으로 이동 ) 검기 날리기");
        bool check;

        int rand = Random.Range(0, 2);
		if(rand == 0) {
            // 왼쪽 
            check = false;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            StartCoroutine(MoveToPosition(new Vector3(-14f, -3f, transform.position.z), enemy.enemySpeed));
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            StartCoroutine(LongAttack(check));
        }
		else if(rand == 1){
            // 오른쪽 
            check = true;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            StartCoroutine(MoveToPosition(new Vector3(16f, -3f, transform.position.z), enemy.enemySpeed));
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            StartCoroutine(LongAttack(check));
        }
    }

    IEnumerator LongAttack(bool check) {
        yield return new WaitForSeconds(0.8f);

        GameObject longAttack = GameManager.instance.poolManager.GetObject(8);
        longAttack.transform.position = lognAttackPos.position;
        Rigidbody2D longRigid = longAttack.GetComponent<Rigidbody2D>();

        if (!check) {
            //GameObject longAttack = Instantiate(longDistanceAttack, lognAttackPos.position, transform.rotation);
            longRigid.AddForce(Vector2.right * 20, ForceMode2D.Impulse);
        }
        else {
            //GameObject longAttack = Instantiate(longDistanceAttack, lognAttackPos.position, transform.rotation);        
            longRigid.AddForce(Vector2.left * 20, ForceMode2D.Impulse);
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPos, float speed ) {
        float t = 0f;
        Vector3 startPos = transform.position;

        while (t < 1) {
            t += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
    }

    public void RayserRain() {
		Debug.Log("왼쪽 끝 부터 레이저 아래에서 위로 발사, 점점 오른쪽으로 옴 레이져 비"); 
        int rand = Random.Range(0, 3);
		switch (rand) {
			case 0:
                StartCoroutine(RayserRandom());
                break;
			case 1:
                StartCoroutine(RayserRight());
                break;
			case 2:
                StartCoroutine(RayserLeft());
                break;
		}
    }

    IEnumerator RayserRandom() {
        for (int i = 0; i < 10; i++) {
            int rand = Random.Range(0, rayserPos.Length);
            //Instantiate(rayser, rayserPos[rand].position, Quaternion.identity);
            GameObject rayser = GameManager.instance.poolManager.GetObject(5);
			rayser.transform.position = rayserPos[rand].transform.position;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator RayserLeft() {
		for(int i = 0; i < rayserPos.Length; i++) {
            //Instantiate(rayser, rayserPos[i].position, Quaternion.identity);
            GameObject rayser = GameManager.instance.poolManager.GetObject(5);
            rayser.transform.position = rayserPos[i].transform.position;
            yield return new WaitForSeconds(0.5f);
        }
	} 
	
	IEnumerator RayserRight() {
        for (int i = rayserPos.Length-1; i >= 0; i--) {		
            //Instantiate(rayser, rayserPos[i].position, Quaternion.identity);
            GameObject rayser = GameManager.instance.poolManager.GetObject(5);
            rayser.transform.position = rayserPos[i].transform.position;
            yield return new WaitForSeconds(0.5f);
        }
	}

    public void Heal() {
        Debug.Log("보스 체력 회복 힐");
		//GameObject heal = Instantiate(healpartical);
        GameObject heal = GameManager.instance.poolManager.GetObject(6);
        heal.transform.position = particalPos.position;
        StartCoroutine(SlowHeal());
    }

    IEnumerator SlowHeal() {
		for(int i = 0; i < 3; i++) {
			if (enemy.enemyHealth < enemy.enemyMaxHealth) {
				enemy.enemyHealth += 50;
				if(enemy.enemyHealth >= enemy.enemyMaxHealth) {
                    enemy.enemyHealth = enemy.enemyMaxHealth;
                }
			}
			Debug.Log(enemy.enemyHealth);
			yield return new WaitForSeconds(1.5f);
		}
    }
}



