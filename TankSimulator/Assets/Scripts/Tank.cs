
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tank : MonoBehaviour 
{
	public float x;
	public float y;
	// public GameObject tank;
	public Rigidbody2D rigidbodyTank;
	public Vector3 movement;
	public float moveSpeed;
	public Vector3 rotateSpeed = new Vector3(0,0,1);
	public Vector3 force;

	// Use this for initialization
	void Start () 
	{
		// tank = GameObject.Find("Tank");
		rigidbodyTank = this.transform.GetComponent<Rigidbody2D>();
	}
	
	private void FixedUpdate() 
	{
		float rotate = -Input.GetAxis("Horizontal");	//返回-1到1的实数值,水平按键AD代表旋转量
		float move = Input.GetAxis("Vertical");			//WS代表移动量

		this.transform.Rotate(0, 0, rotate * rotateSpeed.z);          //旋转
				
		movement = new Vector3(
			move * moveSpeed * (float)Math.Cos(Math.PI * this.transform.rotation.eulerAngles.z / 180),
			move * moveSpeed * (float)Math.Sin(Math.PI * this.transform.rotation.eulerAngles.z / 180),
			0
		);
		// rigidbodyTank.AddForce(new Vector2(movement.x, movement.y));

		rigidbodyTank.velocity = new Vector2(movement.x, movement.y);
		//调试用
		// x = this.transform.rotation.z;
		// x = (float)Math.Cos(Math.PI * this.transform.rotation.eulerAngles.z / 180.0f);
		// y = (float)Math.Sin(Math.PI * this.transform.rotation.eulerAngles.z / 180.0f);
	}
}
