
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tank : MonoBehaviour 
{
	// public GameObject tank;
	[Header("控制者")]
	public Ctrller ctrller;		//是否为玩家
	[Header("装甲厚度：x为前甲，y为后甲")]
	public Vector2 health;
	public Vector2 healthPre;	//预设装甲厚度
	[Header("反应装甲起作用的概率")]
	public float armor;
	[Header("各装甲脆弱成度")]
	public Vector2 damage;
	Vector3 movement;
	[Header("移动速度")]
	public float moveSpeed;
	[Header("旋转速度")]
	public Vector3 rotateSpeed = new Vector3(0,0,1);
	// public Vector3 force;
	
	Rigidbody2D rigidbodyTank;			//负责碰撞
	Rigidbody2D rigidbodyFrontAmour;	//前装甲，负责炮弹击中碰撞
	Rigidbody2D rigidbodyBackAmour;		//后装甲

	void Start () 
	{	
		health = healthPre;
		// tank = GameObject.Find("Tank");
		rigidbodyTank = this.transform.GetComponent<Rigidbody2D>();
		this.transform.rotation = Quaternion.Euler(0,0,90);
	}
	
	private void FixedUpdate() 
	{
		// Debug.Log(this.transform.rotation.eulerAngles.z);
		switch (ctrller)
		{
			case Ctrller.player:		//1号玩家坦克操控，键盘+鼠标操控
				//移动设定
				float rotate = -Input.GetAxis("Horizontal1");	//返回-1到1的实数值,水平按键AD代表旋转量
				float move = Input.GetAxis("Vertical1");			//WS代表移动量
				Move(rotate, move);
			break;

			case Ctrller.wingman: 
				float rotate2 = -Input.GetAxis("Horizontal2");	//返回-1到1的实数值,方向键左右代表旋转量
				float move2 = Input.GetAxis("Vertical2");			//方向键上下代表移动量
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

	public void UpdateHealth(bool isFront)
	{
		if (isFront)				//前装甲被击中，health.x减少damage.x血量
		{	
			health = new Vector2(health.x - damage.x, health.y);
		}
		else						//后装甲集中，减少damage.y血量
		{
			health = new Vector2(health.x, health.y - damage.y);
		}
		if (health.x <0 || health.y <0)
		{
			Destroy(this.gameObject);
		}
	}
}
