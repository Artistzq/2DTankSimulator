
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tank : MonoBehaviour 
{
	// public GameObject tank;
	[Header("控制者")]
	public Ctrller ctrller;		//是否为玩家
	public float Health = 100;
	Vector3 movement;
	[Header("移动速度")]
	public float moveSpeed;
	[Header("旋转速度")]
	public Vector3 rotateSpeed = new Vector3(0,0,1);
	// public Vector3 force;
	
	Rigidbody2D rigidbodyTank;
	void Start () 
	{
		// tank = GameObject.Find("Tank");
		rigidbodyTank = this.transform.GetComponent<Rigidbody2D>();
	}
	
	private void FixedUpdate() 
	{
		switch (ctrller)
		{
			case Ctrller.player:		//1号玩家坦克操控，键盘+鼠标操控
				//移动设定
				float rotate = -Input.GetAxis("Horizontal1");	//返回-1到1的实数值,水平按键AD代表旋转量
				float move = Input.GetAxis("Vertical1");			//WS代表移动量
				Move(rotate, move);
			break;

			case Ctrller.wingman: 
				float rotate2 = -Input.GetAxis("Horizontal2");	//返回-1到1的实数值,水平按键AD代表旋转量
				float move2 = Input.GetAxis("Vertical2");			//WS代表移动量
				Move(rotate2, move2);
			break;
		}	
	}

	private void Move(float _rotate, float _move)
	{
		this.transform.Rotate(0, 0, _rotate * rotateSpeed.z);	//简单旋转				
		movement = new Vector3(
			_move * moveSpeed * (float)Math.Cos(Math.PI * this.transform.rotation.eulerAngles.z / 180),
			_move * moveSpeed * (float)Math.Sin(Math.PI * this.transform.rotation.eulerAngles.z / 180),
			0
		);
		rigidbodyTank.velocity = new Vector2(movement.x, movement.y);
	}

	public void UpdateHealth()
	{
		Health -= 34;
		if (Health < 0)
		{
			Destroy(this.gameObject);
		}
	}
}
