/** 
 *负责人:
 *版本:
 *提交日期:
 *功能描述:  
 *修改记录: 
*/  

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	public GameObject turret;	//物体：炮塔
	public float rotateSpeed;	//炮塔旋转速度
	public float rotateDelta;	//炮塔旋转增量
	
	Quaternion targetRotate;	//目标角度(欧拉角表示)
	float targetAngle = 0;		//目标角度（度数）

	void Start () 
	{
		turret = GameObject.Find("Tank/Turret");
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//改变目标角度
		//当滚轮值大于0，旋转目标角度targetAngle 增加 rotateDelta的大小
		if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
		{
			targetAngle += rotateDelta;
			targetRotate = Quaternion.Euler(0, 0, targetAngle);
		}
		//当滚轮值小于0，旋转目标角度targetAngle 减少 rotateDelta的大小
		else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0 )
		{
			targetAngle += rotateDelta;
			targetRotate = Quaternion.Euler(0, 0, targetAngle);
		}

		//若不滚动滚轮，目标角度维持不变，不进行操作

		//每一帧实现向目标角度（以父物体为参考系）的平滑转向
		this.transform.localRotation = Quaternion.Slerp(
			this.transform.localRotation,
			targetRotate,
			rotateSpeed * Time.deltaTime
		);
		
		
	}

	// public void test()
	// {
		
	// 		targetRotate = Quaternion.Euler(0, 0, 90);
	// 		this.transform.rotation = Quaternion.Slerp(
	// 			this.transform.rotation,
	// 			targetRotate,
	// 			rotateSpeed * Time.deltaTime
	// 		);
	// }
}
