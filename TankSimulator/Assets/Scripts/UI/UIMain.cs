using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 游戏界面的UI控制
/// </summary>
public class UIMain : MonoBehaviour 
{
	public Tank player;
	public Tank wingman;
	public int desNum;


	private Text fruit;		//游戏结果
	private GameObject panel_back;
	private GameObject panel_over;
	private UIContoller uIContoller;


	private void Start()
	{
		uIContoller = GameObject.Find("UIController").GetComponent<UIContoller>();	//获取脚本
		player = GameObject.Find("Player").GetComponent<Tank>();
		wingman = GameObject.Find("Wingman").GetComponent<Tank>();
		panel_back = GameObject.Find("Panel_Back");
		panel_over = GameObject.Find("Panel_Over");
		fruit = panel_over.transform.GetChild(1).GetComponent<Text>();

		panel_back.SetActive(false);
		panel_over.SetActive(false);
	}

	private void Update() 
	{

		//检测重生次数
		//不同游戏模式不同判断方法
		if (uIContoller.isAntiAI)	//如果是双人合作模式
		{
			if (player.isDone && wingman.isDone)		//双人合作模式，两玩家均战败，则游戏结束
			{
				GameOver();
			}
		}
		else
		{
			if (player.isDone || wingman.isDone)		//对战模式，任何一玩家重生用尽，则游戏结束
			{
				GameOver();
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{	
			panel_back.SetActive(true);
		}
	}
	
	/// <summary>
	/// 打开返回
	/// </summary>
	public void ShowBack()
	{
		panel_back.SetActive(true);
	}


	/// <summary>
	/// 关闭返回
	/// </summary>
	public void HideBack()
	{
		panel_back.SetActive(false);
	}

	/// <summary>
	/// 回到主菜单
	/// </summary>
	public void BackToMainMenu()
	{
		SceneManager.LoadScene(0);		//返回主菜单
		Destroy(uIContoller.gameObject);
	}

	public void GameOver()
	{
		if (uIContoller.isAntiAI)			//合作模式
		{
			fruit.text = "你们共击毁了" + desNum.ToString() + "辆坦克（包括友军！）。\n" + 
						 "敌人的最高等级为：" + GameObject.Find("GameController").GetComponent<EnemyController>().difficulty.ToString();
		}
		else
		{
			string temp = null;
			if (player.isDone && !wingman.isDone)	//如果是玩家1耗尽了
			{
				temp = "玩家2（轻型坦克）";
			}
			else if (!player.isDone && wingman.isDone)
			{
				temp = "玩家1（重型坦克）";
			}
			fruit.text = temp + "获胜！\n";
		}

		panel_over.SetActive(true);
	}
}
