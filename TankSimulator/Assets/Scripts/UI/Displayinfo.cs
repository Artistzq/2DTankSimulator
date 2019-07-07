//显示坦克的各项信息, 此脚本挂载在UI的坦克信息面板上

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Displayinfo : MonoBehaviour 
{

	[Header("坦克")]
	public GameObject Vehicle;				//载具，手动拖动, 存有载具的数据的脚本，几乎都存在此物体上,如下
	private Tank tank;						//Tank脚本，存有坦克的一些数据（装甲厚度，移动速度等）
	private Turret turret;					//Turret脚本，存有炮塔的一些数据（冷却时间，转速等）

	// public GameObject Player;				//palyer坦克物体
	// public GameObject Wingman;				//wingman坦克物体

	// public GameObject reloadPanel_1;		//player装填时间显示面板
	// public GameObject reloadPanel_2;		//wingman装填时间显示面板

	void Start () 
	{
		tank = Vehicle.GetComponent<Tank>();
		turret = Vehicle.GetComponent<Turret>();
	}
	
	void Update () 
	{
		//显示的信息

		//第一个字物体：装填状态
		//图像填充程度随炮塔冷却而变化
		//turret.cooldown / turret.reloadTime保证最大值为1
		this.transform.GetChild(0).GetComponent<Image>().fillAmount = 1 - Math.Abs(turret.cooldown / turret.reloadTime);

		//第二个物体，前装甲
		this.transform.GetChild(1).GetComponent<Image>().fillAmount = tank.health.x / tank.healthPre.x;
		
		//第三个物体，后装甲
		this.transform.GetChild(2).GetComponent<Image>().fillAmount = tank.health.y / tank.healthPre.y;

		//第四个物体，数字显示前后装甲
		this.transform.GetChild(3).GetComponent<Text>().text = "前装甲完整度：" + tank.health.x.ToString() + " 后装甲完整度：" + tank.health.y;

		//第五个物体，数字显示装填时间
		this.transform.GetChild(4).GetComponent<Text>().text = "装填时间：" + turret.reloadTime.ToString() + "秒";

		//第六个物体，数字炮弹速度
		this.transform.GetChild(5).GetComponent<Text>().text = "炮弹速度：" + (tank.bulletSpeed * 50).ToString() + "米/秒";

		//第七个物体（信息），数字炮弹数量
		this.transform.GetChild(6).GetComponent<Text>().text = tank.bulletNum.ToString();

		//第八个物体（信息），炮塔转速
		this.transform.GetChild(7).GetComponent<Text>().text = "炮塔转速" + (35 * turret.rotateDelta).ToString() + "度/秒";

		//第九个物体（信息），坦克移速
		this.transform.GetChild(8).GetComponent<Text>().text = "坦克移速等级：" + (tank.moveSpeed * 10).ToString();

		//第十个物体（信息），坦克转速
		this.transform.GetChild(9).GetComponent<Text>().text = "坦克转速等级："	+ (tank.rotateSpeed.z * 10).ToString();
		
		//第十一个物体（信息），剩余重生次数
		this.transform.GetChild(10).GetComponent<Text>().text = "剩余：" + tank.remainChance.ToString() + "次重生";

	}
}
