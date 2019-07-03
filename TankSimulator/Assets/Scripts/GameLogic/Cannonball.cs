//炮弹的控制

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cannonball : MonoBehaviour {
	private float flySpeed;
	private Vector3 movement;
	public Tank tank;	
	Rigidbody2D rigidbodyBullet;
	
	private void Start() 
	{
		rigidbodyBullet = this.transform.GetComponent<Rigidbody2D>();
		tank = this.transform.parent.gameObject.GetComponent<Tank>();				//从该脚本挂载的物体（子弹）的父物体（Player或Wingman，两玩家之一）获取tank脚本

		flySpeed = tank.bulletSpeed;									//从Tank脚本得到炮弹速度
		Invoke("BallDestroy", 2);										//该脚本挂载的物体（即炮弹）实例化2秒后，销毁物体
	}

	private void Update() 
	{
		BulletMove();
	}

	public void BulletMove()
	{
		movement = new Vector3(
			flySpeed * (float)Math.Cos(Math.PI * this.transform.rotation.eulerAngles.z / 180),
			flySpeed * (float)Math.Sin(Math.PI * this.transform.rotation.eulerAngles.z / 180),
			0
		);
		rigidbodyBullet.velocity = new Vector2(movement.x, movement.y);
	}

	private void OnCollisionEnter2D(Collision2D other) 
	{
		if (other.collider.tag == "FrontArmor")
		{
			Debug.Log("击中前装甲");
			other.gameObject.GetComponent<Tank>().UpdateHealth(true);
		}
		else if (other.collider.tag == "BackArmor")
		{
			Debug.Log("击中后装甲");
			other.gameObject.GetComponent<Tank>().UpdateHealth(false);
		}

		BallDestroy();									//碰撞后立即销毁该脚本物体，即炮弹
	}

	private void BallDestroy()
	{
		Destroy(this.gameObject);
	}
}
