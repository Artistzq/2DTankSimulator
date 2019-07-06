//Enemy类，继承Tank类，挂载在敌人坦克上，控制enemy坦克的行为

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 继承自Tank类，添加了一些独有的方法，比如炮塔自动控制
/// </summary>
public class Enemy : Tank 
{
	public float rotateEnemy = -1;
	public float moveEnemy = 1;

	public int deviation_max;	//偏差范围，越小，敌人打的越准
	public Vector3 vec;
	public float targetAng; 
	public Quaternion targetRot;
	
	public GameObject player;		//两玩家物体
	public GameObject wingman;
	
	float fireTime = 1;
	private float frequency = 0.7f;		//频率控制器
	private float frequencyCout;	
	public float tar;
	private void Start() 
	{
		tar = Random.Range(0.0f,1.0f);
		fireTime = Random.Range(3, 10);
		switch (ctrller)
		{
			case Ctrller.enemy:
				player = GameObject.Find("GameController/Player");
				wingman = GameObject.Find("GameController/Wingman");
				InvokeRepeating("RandomRotChange", 0.1f, 1);			
				InvokeRepeating("RandomMoveChange", 0.1f, 5f);
				InvokeRepeating("RandomFire", 3f, fireTime);
			break;	
		}
	}

	private void Update() 
	{
		switch (ctrller)
		{
			case Ctrller.enemy:
				Move(rotateEnemy,moveEnemy);		//随机移动
				RandomAim();						//随即瞄准
				this.transform.GetComponent<Turret>().RotateInWorld(targetRot);	//向瞄准目标转向
			break;	
		}
		
	}

	/// <summary>
	/// 产生随机移动量
	/// </summary>
	private void RandomMoveChange()
	{
		frequencyCout = Random.Range(0.0f,1.0f);
		if (frequencyCout > 1-frequency)				//使向前移动（为1）的概率为frequency
		{
			moveEnemy = 1;
		}
		else
		{
			moveEnemy = -1;
		}
	}

	/// <summary>
	/// 产生随机转向量
	/// </summary>
	private void RandomRotChange()
	{
		rotateEnemy = Random.Range(-1,1);
	}

	/// <summary>
	/// 随即开火，无参形式以供延时调用
	/// </summary>
	private void RandomFire()
	{
		this.transform.GetComponent<Turret>().Fire(true);
	}

	/// <summary>
	/// 随机瞄准某个玩家
	/// </summary>
	private void RandomAim()
	{
		if (tar < 0.64)
		{
			AimTo(Ctrller.player);		//0.64的几率瞄准1号玩家
		}
		else
		{
			AimTo(Ctrller.wingman);		//0.36机率瞄准2号玩家
		}

	}

	/// <summary>
	/// 修正敌人的炮塔角度，使其瞄准_ctrller
	/// </summary>
	/// <param name="_ctrller">被瞄准者</param>
	private void AimTo(Ctrller _ctrller)
	{
		int deviation = Random.Range(0, deviation_max);	//获得偏差量
		//目标位置向量减去此敌人位置向量，得到差向量
		switch (_ctrller)
		{
			case Ctrller.player:
				vec = player.transform.position - this.transform.position;
			break;
			case Ctrller.wingman:
				vec = wingman.transform.position - this.transform.position;
			break;
		}
		
		//根据差向量，用反正切函数得到相对位置偏角，再用四元数targetRot表示，再加上偏差量
		targetAng = 180/Mathf.PI*Mathf.Atan2(vec.y, vec.x) + deviation;
		targetRot = Quaternion.Euler(0 ,0, targetAng);
	}

	/// <summary>
	/// 重写Tank类中设置难度方法，主要区别为添加了Enemy类中的炮塔误差
	/// </summary>
	/// <param name="_difficulty"></param>
	public override void SetLevel(int _difficulty)
	{
		difficulty = _difficulty;
		health = healthPre = new Vector2(50 * _difficulty, 25 * _difficulty);			//前装甲50*dif，后装甲20*dif
		bulletSpeed = 4 * _difficulty ;													//子弹速度3 * dif
		// damage = new Vector2(10 * _difficulty, 13 * _difficulty);					//前装甲伤害2*dif，后装甲伤害-dif
		moveSpeed = 0.2f * _difficulty;													//移动速度0.2*dif
		rotateSpeed = new Vector3(0, 0, 0.2f * _difficulty);							//炮塔转速
		deviation_max = 20 - _difficulty;												//炮塔误差，0-20度

		this.transform.GetComponent<Turret>().rotateDelta = 0.2f * _difficulty;
	}

}
