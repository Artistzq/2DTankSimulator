
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
	public Vector2 healthPre;	//预设装甲厚度
	[HideInInspector]
	public Vector2 health;		//实时装甲厚度，不在编辑器显示

	[Header("炮弹数量")]
	public int bulleNumPre;		//预设炮弹数量
	[HideInInspector]
	public int bulletNum;		//实时炮弹
	[Header("炮弹飞行速度")]
	public float bulletSpeed;
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
	
	Rigidbody2D rigidbodyTank;			//负责碰撞,移动
	public GameObject destroyedTank;
	public GameObject destroyedVehicle;
	Ctrller tmp ;
	void Start () 
	{	
		health = healthPre;
		rigidbodyTank = this.transform.GetComponent<Rigidbody2D>();
		tmp = ctrller;
		destroyedVehicle = GameObject.Find("GameController/DestroyedVehicle");
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
		if (ctrller != Ctrller.destroyed)
		{
			if (isFront)				//前装甲被击中，health.x减少damage.x血量
			{	
				health = new Vector2(health.x - damage.x, health.y);
			}
			else						//后装甲集中，减少damage.y血量
			{
				health = new Vector2(health.x, health.y - damage.y);
			}

			if (health.x <0 || health.y <0)	//前后装甲任意一个损坏之后，不销毁车辆，冻结其位置和旋转，5秒后在原处生成一个损坏的车辆，原来的车辆恢复。
			{
				health = new Vector2(0, 0);
				ctrller = Ctrller.destroyed;					//取消对该脚本物体的控制
				this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;//冻结该脚本物体
				Invoke("ChangeTank", 5);					//5秒后生成损坏车辆，并解冻复位
				
			}
		}
		
	}

	//坦克被摧毁后，5秒后再次生成
	void ChangeTank()
	{
		ctrller = tmp;
		switch (ctrller)
		{
			case Ctrller.player:
				destroyedTank = (GameObject)Resources.Load("Prefabs/Player");
				CloneATank();
				ctrller = Ctrller.player;
				break;
			case Ctrller.wingman:
				destroyedTank = (GameObject)Resources.Load("Prefabs/Wingman");
				CloneATank();
				ctrller = Ctrller.wingman;
			break;
		}	
		this.transform.position = new Vector3(0, 0, this.transform.position.z);		//位置恢复
		this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;//解冻
		health = healthPre;				//血量回复
		bulletNum = bulleNumPre;		//弹药恢复
	}

	void CloneATank()
	{
		GameObject newtank;
		newtank = Instantiate(destroyedTank, this.transform.position,this.transform.rotation);
		newtank.transform.SetParent(destroyedVehicle.transform);
		newtank.GetComponent<Tank>().ctrller = Ctrller.destroyed;
		newtank.transform.GetChild(0).transform.rotation = this.transform.GetChild(0).transform.rotation;
		newtank.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
	}

}
