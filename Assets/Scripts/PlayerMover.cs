using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMover : MonoBehaviour
{
    public float rotSpeed;
	public float moveSpeed;
	public float TOW; // Take Off Weight
	public float power;
	float powerDelta;
	public float verticalSpeed;
	public float maxRotSpeed;
	public float maxMoveSpeed;
	public float maxVerticalSpeed;
	Vector3 movingVector;
	
	int STATUS;
	int STATUS_GROUND;
	int STATUS_AIR;
	int STATUS_HOVER;
	int STATUS_FLYING;
	
	void Start () {
		rotSpeed = 0.0f;
		moveSpeed = 0.0f;
		TOW = 100f;
		power = 50f;
		verticalSpeed = 0f;
		//transform.Translate(Vector3.up*0.1f);
		powerDelta = 0f;
		
		STATUS = 1;
		STATUS_GROUND = 0;
		STATUS_HOVER = 1;
		STATUS_AIR = 2;
		STATUS_FLYING = 3;
	}

	void Update()
	{

		// A키는 좌회전, D키는 우회전
		if (Input.GetKey(KeyCode.A))
		{
			rotSpeed += -0.2f;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			rotSpeed += 0.2f;
		}
		else
		{
			if (rotSpeed > 0) rotSpeed += -0.4f;
			else if (rotSpeed < 0) rotSpeed += 0.4f;
		}
		// 지상에서 헬기동체의 Y축 중심 회전
		transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed);

		//UP, DOWN 키를 동시에 누르면 브레이크를 작동한다
		if (Input.GetKey("up") && Input.GetKey("down"))
		{
			if (moveSpeed > 0)
				moveSpeed += -0.01f;
			else if (moveSpeed < 0)
				moveSpeed += 0.01f;
			if (Mathf.Abs(moveSpeed) < 0.05f)
				moveSpeed = 0f;
		}
		else
		{
			// UP 키는 전방속도, DOWN키는 후방속도 조절
			if (Input.GetKey("up"))
			{
				moveSpeed += 0.004f;
			}
			else if (Input.GetKey("down"))
			{
				moveSpeed += -0.004f;
			}
		}
		if (STATUS == STATUS_FLYING && moveSpeed < 0.5f)
		{
			STATUS = STATUS_HOVER;
			print("HOVER");
		}
		else if (STATUS == STATUS_HOVER && moveSpeed > 0.5f)
		{
			STATUS = STATUS_FLYING;
			print("FLYING");
		}
		// W키가 눌리면 동력증가, S키는 동력감소
		if (Input.GetKey(KeyCode.W))
		{
			powerDelta += 0.04f;
			//print ("W, power="+power);
		}
		else if (Input.GetKey(KeyCode.S))
		{
			if (power > 0f)
			{
				powerDelta += -0.04f;
				//print ("S, power="+power);
			}
		}
		else
		{
			powerDelta = 0f;
		}
		power += powerDelta;

		// 동력이 이륙중량을 초과하면 상승시작, 
		if (power > TOW)
		{
			verticalSpeed = power / TOW - 1f;
		}
		else if (power < TOW)
		{
			if (STATUS == STATUS_GROUND)
				verticalSpeed = 0f;
			else if (transform.position.y >= 0)
				verticalSpeed = power / TOW - 1f;
		}

		transform.Translate(Vector3.up * Time.deltaTime * verticalSpeed);

		transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
		setMaxSpeed();
	}
	
	void setMaxSpeed()
    {
		if (rotSpeed > maxRotSpeed) rotSpeed = maxRotSpeed;
		if (moveSpeed > maxMoveSpeed) moveSpeed = maxMoveSpeed;
		if (verticalSpeed > maxVerticalSpeed) verticalSpeed = maxVerticalSpeed;
    }
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "ground")
		{
			STATUS = STATUS_GROUND;
			verticalSpeed = 0f;
			print ("On Ground");
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag=="ground")
		{
			STATUS = STATUS_HOVER;
			print ("HOVER");
		}
	}
}