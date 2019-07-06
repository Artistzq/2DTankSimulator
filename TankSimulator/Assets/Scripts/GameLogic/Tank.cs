
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour 
{
	// public GameObject tank;
	[Header("控制者")]
	public Ctrller ctrller;		//是否为玩家

	[Header("装甲厚度：x为前甲，y为后甲")]
	public Vector2 healthPre;	//预设装甲厚度

	// [HideInInspector]
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

	[Header("移动速度")]
	public float moveSpeed;

	[Header("旋转速度")]
	public Vector3 rotateSpeed = new Vector3(0,0,1);

	// public Vector3 force;
	[Header("难度等级")]
	public int difficulty = 1;
	
	[Header("剩余机会")]
	public int remainChance;

	public bool isDone = false;			//玩家1是否用完重生
	Rigidbody2D rigidbodyTank;			//负责碰撞,移动
	Vector3 movement;
	public GameObject destroyedTank;
	public GameObject destroyedVehicle;
	Ctrller tmp ;
	private AudioSource audioMove;

	private void Awake() 
	{
		health = healthPre;
		bulletNum = bulleNumPre;
		remainChance = 2;
		rigidbodyTank = this.transform.GetComponent<Rigidbody2D>();
		audioMove = this.transform.GetComponent<AudioSource>();
		tmp = ctrller;
		destroyedVehicle = GameObject.Find("GameController/DestroyedVehicle");
	}
	
	private void FixedUpdate() 
	{
		audioMove.pitch = 1;
		switch (ctrller)
		{
			case Ctrller.player:		//1号玩家坦克操控，键盘+鼠标操控
				//移动设定
				float rotate = -Input.GetAxis("Horizontal1");	//返回-1到1的实数值,水平按键AD代表旋转量
				float move = Input.GetAxis("Vertical1");			//WS代表移动量
				Move(rotate, move);
				if (rotate != 0 || move != 0)
				{
					audioMove.pitch = 3;											//播放移动音效（音调加高）
				}
			break;

			case Ctrller.wingman: 
				float rotate2 = -Input.GetAxis("Horizontal2");		//返回-1到1的实数值,方向键左右代表旋转量
				float move2 = Input.GetAxis("Vertical2");			//方向键上下代表移动量
				Move(rotate2, move2);
				if (rotate2 != 0 || move2 != 0)
				{
					audioMove.pitch = 2.4f	;											//播放移动音效（音调加高）
				}
			break;
		}	
	}

	/// <summary>
	/// 根据旋转量和移动量移动坦克
	/// </summary>
	/// <param name="_rotate">旋转量，1到-1，对应左、右</param>
	/// <param name="_move">移动量，1到-1，对应前、后</param>
	protected void Move(float _rotate, float _move)
	{
		this.transform.Rotate(0, 0, _rotate * rotateSpeed.z);	//简单旋转				
		movement = new Vector3(
			_move * moveSpeed * Mathf.Cos(Mathf.PI * this.transform.rotation.eulerAngles.z / 180),
			_move * moveSpeed * Mathf.Sin(Mathf.PI * this.transform.rotation.eulerAngles.z / 180),
			0
		);
		rigidbodyTank.velocity = new Vector2(movement.x, movement.y);
	}

	/// <summary>
	/// 根据被击中位置，减少血量
	/// </summary>
	/// <param name="isFront">是否为前装甲中弹</param>
	public void UpdateHealth(bool isFront)
	{
		if (ctrller != Ctrller.destroyed)		//没有被摧毁的情况下
		{
			if (isFront)						//前装甲被击中，health.x减少damage.x血量
			{	
				health = new Vector2(health.x - damage.x, health.y);
			}
			else								//后装甲被击中，health.y减少damage.y血量
			{
				health = new Vector2(health.x, health.y - damage.y);
			}

			if (health.x <0 || health.y <0)		//前后装甲任意一个损坏之后，不销毁车辆，冻结其位置和旋转，5秒后在原处生成一个损坏的车辆，原来的车辆恢复。
			{
				health = new Vector2(0, 0);							//装甲值清零
				ctrller = Ctrller.destroyed;						//取消对该脚本物体的控制
				remainChance -- ;									//剩余机会减1
				if (remainChance < 0)
				{
					isDone = true;
				}
				this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;//冻结该脚本物体
				Invoke("DepolyADestroyedTank", 5);					//5秒后生成损坏车辆，并解冻复位
			}
		}
		
	}

	/// <summary>
	/// 根据数值_difficulty设置坦克的装甲，攻击，速度等,
	/// 或用作升级玩家
	/// </summary>
	/// <param name="_difficulty"></param>
	public virtual void SetLevel(int _difficulty)
	{
		difficulty = _difficulty;
		health = healthPre = new Vector2(50 * _difficulty, 25 * _difficulty);			//前装甲50*dif，后装甲20*dif
		bulletSpeed = 4 * _difficulty ;													//子弹速度3 * dif
		// damage = new Vector2(10 * _difficulty, 13 * _difficulty);						//前装甲伤害2*dif，后装甲伤害-dif
		moveSpeed = 0.2f * _difficulty;													//移动速度0.2*dif
		rotateSpeed = new Vector3(0, 0, 0.2f * _difficulty);
		bulletNum = bulleNumPre = _difficulty + 10;
		this.transform.GetComponent<Turret>().rotateDelta = 0.2f * _difficulty;
	}
	
	/// <summary>
	/// 生成一个损毁的坦克，生成位置和状态与此脚本物体一致
	/// </summary>
	void DepolyADestroyedTank()
	{
		ctrller = tmp;
		float x = Random.Range(-30.0f, 30);
		float y = Random.Range(-15.0f ,15);
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
			case Ctrller.enemy:
				GameObject GameController = GameObject.Find("GameController");
				GameController.GetComponent<EnemyController>().enemyNums --;
				destroyedTank = (GameObject)Resources.Load("Prefabs/enemy");
				CloneATank();
				Destroy(this.gameObject);										//敌人血量为0时，复制新坦克到原位置，原坦克销毁
			break;
		}	
		this.transform.position = new Vector3(x, y, this.transform.position.z);		//位置恢复
		this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;//解冻
		health = healthPre;				//血量回复
		bulletNum = bulleNumPre;		//弹药恢复
	}

	/// <summary>
	/// 克隆一个新坦克，此方法在DepolyADestroyedTank方法中调用
	/// </summary>
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
