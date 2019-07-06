//敌人生成、数量的控制器

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour 
{
	public int enemyNums;						//敌人数量
	public int difficulty;						//难度等级，递增
	public bool isModeAntiAI;					//是否为对抗电脑模式，不是则不生成敌人

	private UIContoller uIContoller;			//获取ui脚本，判断是否对抗模式
	private int enemyNumsPre = 5;				//敌人数量
	private GameObject enemyTank;				//生成的敌人
	private Enemy enemy;						//生成的敌人上的Enmey脚本
	private float pos_x;						//x轴坐标
	private float pos_y;						//y轴坐标
	private float rot_z;						//围绕z轴旋转量

	private void Start() 
	{
		uIContoller = GameObject.Find("UIController").GetComponent<UIContoller>();
		enemyTank = (GameObject)Resources.Load("Prefabs/Enemy");
		
		if (uIContoller.isAntiAI)
		{
			//生成敌人
			InstantiateEnemy();
		}
	}

	private void Update() 
	{
		if (enemyNums == 0)
		{
			Tank tank;
			//重新刷新敌人时，弹药恢复
			tank = this.transform.Find("Player").GetComponent<Tank>();
			tank.bulletNum = tank.bulleNumPre;
			tank = this.transform.Find("Wingman").GetComponent<Tank>();
			tank.bulletNum = tank.bulleNumPre;
			//难度增加
			difficulty ++;
			enemyNums = enemyNumsPre;
			InstantiateEnemy();
		}
	}

	/// <summary>
	/// 随机位置生成一个敌人，其难度等级为_difficulty
	/// </summary>
	/// <param name="_difficulty">难度等级，越大越难</param>
	public void InstantiateOneEnemy(int _difficulty)
	{
		//随机生成位置和角度
		pos_x = Random.Range(-14, 14);
		pos_y = Random.Range(-7, 7);
		rot_z = Random.Range(0,360);
		//生成，设置父物体为此脚本挂载物体,设置坦克难度
		GameObject newEnemy = Instantiate(
			enemyTank,
			new Vector3(pos_x, pos_y, -1),
			Quaternion.Euler(0, 0, rot_z)
		);
		newEnemy.transform.SetParent(this.transform);
		enemy = newEnemy.transform.GetComponent<Enemy>();
		enemy.SetLevel(_difficulty);
	}

	/// <summary>
	/// 随机生成enemyNums个敌人，位置随机
	/// 当敌人未减到0，就不生成，直到敌人全部消灭，重新生成
	/// </summary>
	public void InstantiateEnemy()
	{
		for (int i = 0; i < enemyNums; i++)
		{
			InstantiateOneEnemy(difficulty);
		}
	}

}
