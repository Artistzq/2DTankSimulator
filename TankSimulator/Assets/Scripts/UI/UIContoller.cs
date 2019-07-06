using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIContoller : MonoBehaviour 
{
	[HideInInspector]
	public bool isAntiAI;				//是否合作对抗电脑模式
	public bool isMain;					//是否开始界面
	public GameObject panel_intro;
	public GameObject panel_quit;

	private void Start() 
	{
		isMain = true;
		DontDestroyOnLoad(this.transform);
		// panel_intro = GameObject.Find("Panel_Intro");	//手动拖取
		// panel_quit = GameObject.Find("Panel_Quit");
		panel_intro.SetActive(false);
		panel_quit.SetActive(false);
	}

	private void Update() 
	{
		if (isMain)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				panel_quit.SetActive(true);
			}
		}
	}

	public void AntiAI()
	{
		isAntiAI = true;
		isMain = false;
		SceneManager.LoadScene(1);
	}

	public void PlayerFight()
	{
		isAntiAI = false;
		isMain = false;
		SceneManager.LoadScene(1);
	}

	/// <summary>
	/// 打开游戏介绍
	/// </summary>
	public void ShowIntro()
	{
		panel_intro.SetActive(true);
	}

	/// <summary>
	/// 关闭游戏介绍
	/// </summary>
	public void HideIntro()
	{
		panel_intro.SetActive(false);
	}
	
	/// <summary>
	/// 打开退出面板
	/// </summary>
	public void ShowQuit()
	{
		panel_quit.SetActive(true);
	}

	/// <summary>
	/// 关闭退出面板
	/// </summary>
	public void HideQuit()
	{
		panel_quit.SetActive(false);
	}

	/// <summary>
	/// 退出游戏
	/// </summary>
	public void QuitGame()
	{
		Application.Quit();
	}

}
