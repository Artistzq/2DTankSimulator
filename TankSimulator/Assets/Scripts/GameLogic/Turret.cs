using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	// [Header("炮塔旋转速度(表现为惯性，越小越明显)")]
	// public float rotateSpeed;					//炮塔旋转速度
	[Header("炮塔旋转增量")]
	public float rotateDelta;						//炮塔旋转增量
	[Header("装填时间")]
	public float reloadTime = 3.0f;					//预设装填时间
	public bool canAttack							//属性，是否能攻击,当冷却时间小于0，就可攻击，值为真
	{
		get{return cooldown <= 0.0f;}
	}
	[HideInInspector]			//编辑器中不可见
	public float cooldown;							//冷却时间，每次开火时被置为reloadTime, 当其大于0，每帧减取time.deltatime。

	private GameObject cannonBall;					//炮弹
	private Ctrller ctrller;						//控制者
	private Transform turret;						//物体：炮塔
	private Transform muzzle;						//物体：火焰
	private Quaternion targetRotation;				//炮塔目标角度(四元数表示)
	private float targetAngle;						//炮塔目标角度（度数）
	

	void Start () 
	{
		turret = this.transform.Find("Turret");			//获得炮塔物体
		muzzle = turret.transform.Find("Muzzle");		//获得火焰物体
		cannonBall = (GameObject)Resources.Load("Prefabs/CannonBalls");		//加载炮弹物体
		// this.transform.localRotation = targetRotation;
		ctrller = this.GetComponent<Tank>().ctrller;	//同步炮塔的控制者为坦克控制者
		muzzle.gameObject.SetActive(false);				//将火焰不可见
	}
	
	void Update() 
	{
		if (cooldown > 0)
		{
			cooldown -= Time.deltaTime;
		}
	}

	void FixedUpdate () 
	{
		ctrller = this.GetComponent<Tank>().ctrller;	//同步炮塔的控制者为坦克控制者
		switch (ctrller)
		{
			case Ctrller.player:
				//player开火设定
				// Fire(Input.GetButtonDown("Fire1"));
				Fire(Input.GetKeyDown(KeyCode.K));//按下了K键
				//player转向设定
			 	RotateTurret(Input.GetKey(KeyCode.J), Input.GetKey(KeyCode.L));
			break;

			case Ctrller.wingman:
				//wingman开火
				Fire(Input.GetKeyDown(KeyCode.Keypad2));//按下了小键盘2键
				//wingman转向
			 	RotateTurret(Input.GetKey(KeyCode.Keypad1), Input.GetKey(KeyCode.Keypad3));
			break;
		}
	}

	/// <summary>
	/// 根据bool值（按键输入），判断开火，用于不同的控制对象
	/// </summary>
	/// <param name="input">是否按下了某个按键</param>
	public void Fire(bool input)//开火
	{
		if (input)			//如果按下了对应的按键
		{
			if (canAttack && this.transform.GetComponent<Tank>().bulletNum > 0)		//且装填完毕且有子弹，可以攻击
			{
				cooldown = reloadTime;									//开始装填
				muzzle.gameObject.SetActive(true);						//打开火焰
				
				GameObject newBullet = Instantiate(						//实例化子弹，位置在炮塔中间，旋转量和炮塔一致
					cannonBall, 
					muzzle.position + new Vector3(0,0,-2),
					turret.rotation
				);
				newBullet.transform.SetParent(this.transform);			//为子弹设定父级transform，为Player或者Wingman
				
				// newBullet.transform.localScale = new Vector3(			//为子弹设定相对父级的比例，手动调整得到参数
				// 	this.transform.localScale.x, 
				// 	this.transform.localScale.y, 
				// 	this.transform.localScale.z);
				
				this.transform.GetComponent<Tank>().bulletNum --;		//坦克炮弹减1
				turret.GetComponent<AudioSource>().Play();				//播放发射音效
				Invoke("FireClose", 0.1f);								//延时0.1s关闭火焰
			}
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
			targetRotation = Quaternion.Euler(0, 0, targetAngle);
		}
		// 当input2为真，旋转目标角度targetAngle 减小 rotateDelta的大小
		else if(input2)
		{
			targetAngle -= rotateDelta;
			targetRotation = Quaternion.Euler(0, 0, targetAngle);
		}
		//若不滚动滚轮，目标角度维持不变，不进行角度变化操作
		//else{ }	

		//确定目标角度后，每一帧实现向目标角度（以父物体为参考系）的平滑转向
		RotateTo(targetRotation);
	}

	/// <summary>
	/// 以父级为坐标系，转到_targetRotation角度，即相对坦克物体的偏转角度
	/// </summary>
	/// <param name="_targetRotation"></param>
	public void RotateTo(Quaternion _targetRotation)
	{
		if (Quaternion.Angle(this.transform.localRotation, _targetRotation) != 0 )
		{
			// Debug.Log(Quaternion.Angle(this.transform.localRotation, targetRotation));
			turret.localRotation = Quaternion.Slerp(
				turret.localRotation,
				_targetRotation,
				rotateDelta * Time.deltaTime
			);	
		}
	}

	public void RotateInWorld(Quaternion _targetRotation)
	{
		if (Quaternion.Angle(this.transform.localRotation, _targetRotation) != 0 )
		{
			turret.rotation = Quaternion.Slerp(
				turret.rotation,
				_targetRotation,
				rotateDelta * Time.deltaTime
			);	
		}
	}

	//用于在Invoke中延时调用的无参方法
	//关闭火焰
	private void FireClose()
	{
		muzzle.gameObject.SetActive(false);
	}
}
