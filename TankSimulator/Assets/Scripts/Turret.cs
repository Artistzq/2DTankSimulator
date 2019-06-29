using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	private GameObject cannonBall;
	private Ctrller ctrller;		//控制者
	[Header("炮塔旋转速度(表现为惯性，越小越明显)")]
	public float rotateSpeed;	//炮塔旋转速度
	[Header("炮塔旋转增量")]
	public float rotateDelta;	//炮塔旋转增量
	public List<GameObject> cannonBalls;	//子弹

	private Transform turret;	//物体：炮塔
	private Transform muzzle;	//物体：火焰
	Quaternion targetRotation;	//目标角度(欧拉角表示)
	private float targetAngle;		//目标角度（度数）
	

	void Start () 
	{
		turret = this.transform.Find("Turret");
		muzzle = turret.transform.Find("Muzzle");
		cannonBall = (GameObject)Resources.Load("Prefabs/CannonBalls");
		this.transform.localRotation = targetRotation;
		ctrller = this.GetComponent<Tank>().ctrller;
		muzzle.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		switch (ctrller)
		{
			case Ctrller.player:
				//player转向设定
				RotateTurret(Input.GetAxisRaw("Mouse ScrollWheel") > 0, Input.GetAxisRaw("Mouse ScrollWheel") < 0 );
				//player开火设定
				Fire(Input.GetButtonDown("Fire1"));
			break;

			case Ctrller.wingman:
				//wingman转向
			 	RotateTurret(Input.GetKey(KeyCode.Keypad1), Input.GetKey(KeyCode.Keypad3));
				//wingman开火
				Fire(Input.GetKeyDown(KeyCode.Keypad2));
			break;
		}
		
	}

	/// <summary>
	/// 根据bool值（按键输入），判断开火，用于不同的控制对象
	/// </summary>
	/// <param name="isFire"></param>
	public void Fire(bool isFire)//开火
	{
		if (isFire)
		{
			//开火的瞬间，火焰出现，根据预制体Prefab实例化一个炮弹，并加入列表cannonBalls中
			//炮弹体的世界坐标旋转量和炮塔的世界坐标旋转量一致，位置在炮塔中间
			muzzle.gameObject.SetActive(true);

			GameObject newBullet = Instantiate(
				cannonBall, 
				muzzle.position + new Vector3(0,0,-2),
				turret.rotation
			);
			newBullet.transform.SetParent(this.transform.parent);
			newBullet.transform.localScale = new Vector3(
				0.04f * this.transform.parent.localScale.x, 
				0.04f * this.transform.parent.localScale.y, 
				1 * this.transform.parent.localScale.z);
			cannonBalls.Add(newBullet);

			Invoke("FireClose", 0.1f);
		}
		
	}

	/// <summary>
	/// 根据是否有两个输入值，进行炮塔的旋转
	/// </summary>
	/// <param name="input1"></param>
	/// <param name="input2"></param>
	public void RotateTurret(bool input1, bool input2)
	{
		//改变目标角度
		//当input1为真（player为滚动滚轮，wingman为按下按键），旋转目标角度targetAngle 增加 rotateDelta的大小
		if(input1)
		{
			targetAngle += rotateDelta;
			targetAngle = targetAngle % 360;
			targetRotation = Quaternion.Euler(0, 0, targetAngle);
		}
		// 当input2为真，旋转目标角度targetAngle 增加 rotateDelta的大小
		else if(input2)
		{
			targetAngle -= rotateDelta;
			targetAngle = targetAngle % 360;
			targetRotation = Quaternion.Euler(0, 0, targetAngle);
		}
		//若不滚动滚轮，目标角度维持不变，不进行角度变化操作
		//else{ }	

		//确定目标角度后，每一帧实现向目标角度（以父物体为参考系）的平滑转向
		if (Quaternion.Angle(this.transform.localRotation, targetRotation) != 0 )
		{
			// Debug.Log(Quaternion.Angle(this.transform.localRotation, targetRotation));
			turret.localRotation = Quaternion.Slerp(
				turret.localRotation,
				targetRotation,
				rotateSpeed * Time.deltaTime
			);	
		}
	}

	private void FireClose()
	{
		muzzle.gameObject.SetActive(false);
	}
}
