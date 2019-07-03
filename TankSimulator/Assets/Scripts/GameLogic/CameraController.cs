//控制相机位置，为两个玩家之间的位置
//同时设置相机大小

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject player;
	public GameObject wingman;

	Vector2 pos_1;
	Vector2 pos_2;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (player != null && wingman != null)
		{
			//让相机位置在2辆坦克之间
			pos_1 = new Vector2(player.transform.position.x, player.transform.position.y);
			pos_2 = new Vector2(wingman.transform.position.x, wingman.transform.position.y);
			this.transform.position = new Vector3(
				(pos_1.x + pos_2.x)/2,
				(pos_1.y + pos_2.y)/2,
				this.transform.position.z
			);
		}
			
		// if ( Vector2.Distance(pos_1, pos_2) > )
		// {
			
		// }
	}
}
