//炮弹的控制

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cannonball : MonoBehaviour {

	public float flySpeed;
	private Vector3 movement;
	private GameObject tank;	
	Rigidbody2D rigidbodyBullet;
	
	private void Start() 
	{
		rigidbodyBullet = this.transform.GetComponent<Rigidbody2D>();
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
