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

public class Player : MonoBehaviour {

	public GameObject tankGO ;
	public GameObject turretGO;
	public GameObject cannonBallGO;

	public Vehicle tank = new Vehicle();
	public Vehicle turret = new Vehicle();
	public Vehicle cannonBall = new Vehicle();

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			tank.Move(Direction.forWard);
			tankGO.transform.position = new Vector3(
				tank.Position.x,
				tank.Position.y,
				tankGO.transform.position.z
			);
			tankGO.transform.Rotate(
				0, 0, 10
			);

		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			
		}else if (Input.GetKeyDown(KeyCode.S))
		{
			
		}else if (Input.GetKeyDown(KeyCode.D))
		{
			
		}
		
		
	}
}
