//相机控制器，控制相机的大小和位置

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour 
{
	public GameObject player;			//玩家1物体
	public GameObject wingman;			//玩家2物体

	private Vector3 pos_1;						//玩家1的位置
	private Vector3 pos_2;						//玩家2的位置
	private float curCameraSize;				//当前摄像机大小
	private float tarCameraSize;				//目标摄像机大小
	private float bigTargetSize;				//变大的目标
	private float smallTargetSize;				//变小的目标
	private float biggerLine;					//变大界限，超过这个距离就变大
	private float smallerLine;					//超过这个距离就变小

	void Start () 
	{
		curCameraSize = Camera.main.orthographicSize;
		tarCameraSize = curCameraSize;
	}
	
	void FixedUpdate () 
	{
		bigTargetSize = 1.4f * curCameraSize;
		smallTargetSize = 0.9f * curCameraSize;
		biggerLine = 0.9f * curCameraSize;
		smallerLine = 0.4f * curCameraSize;

		if (player != null && wingman != null)
		{
			//让相机位置在2辆坦克之间
			pos_1 = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
			pos_2 = new Vector3(wingman.transform.position.x, wingman.transform.position.y, wingman.transform.position.z);
			this.transform.position = Vector3.Lerp(
				this.transform.position, 				//从当前位置
				new Vector3(							//到两物体中心
					(pos_1.x + pos_2.x)/2,
					(pos_1.y + pos_2.y)/2,
					this.transform.position.z
				),
				Time.deltaTime
				);
		}
	
		if (Math.Abs(pos_1.x - pos_2.x)/4 < smallerLine && 
			Math.Abs(pos_1.y - pos_2.y)/2 < smallerLine)		
		{
			tarCameraSize = smallTargetSize;
		}
		else if (Math.Abs(pos_1.x - pos_2.x)/4 > biggerLine || 
				Math.Abs(pos_1.y - pos_2.y)/2 > biggerLine)
		{
			tarCameraSize = bigTargetSize;
		}
		
		//当实际大小接近目标值时，重设当前大小
		if (Mathf.Abs(tarCameraSize - Camera.main.orthographicSize) < 0.001)				//无限逼近的结果不可能相等，只能差值很小
		{
			curCameraSize = Camera.main.orthographicSize;
		}

		Camera.main.orthographicSize = Mathf.Lerp(
				Camera.main.orthographicSize,
				tarCameraSize,
				Time.deltaTime
			);
	}

}
