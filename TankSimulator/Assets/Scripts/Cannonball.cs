//炮弹的控制

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cannonball : MonoBehaviour {

	public float flySpeed;
	public Vector3 movement;

	Rigidbody2D rigidbodyBullet;
	
	private void Start() 
	{
		rigidbodyBullet = this.transform.GetComponent<Rigidbody2D>();
	}

	private void Update() {
		movement = new Vector3(
			flySpeed * (float)Math.Cos(Math.PI * this.transform.rotation.eulerAngles.z / 180),
			flySpeed * (float)Math.Sin(Math.PI * this.transform.rotation.eulerAngles.z / 180),
			0
		);
		rigidbodyBullet.velocity = new Vector2(movement.x, movement.y);

		// if ((this.transform.position - ))//超出相机范围，销毁
		// {
			
		// }
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
	//分三部分，炮弹的角度，炮弹的移动，以及炮弹的发射
	//角度和移动类似车体和炮塔的操作，但无需平滑移动等操作

	private void OnCollisionEnter2D(Collision2D other) 
	{
		this.GetComponent<AudioSource>().Play();
		other.gameObject.GetComponent<Tank>().UpdateHealth();
		Destroy(this.gameObject);
	}
}
