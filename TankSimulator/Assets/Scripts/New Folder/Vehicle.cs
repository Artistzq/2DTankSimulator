// /** 
//  *负责人:
//  *版本:
//  *提交日期:
//  *功能描述:  
//  *修改记录: 
// */  

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// public enum Direction
// {
// 	forWard,		//向前
// 	backWard,		//向后
// 	leftTurn,		//向左转
// 	rightTurn		//向右转
// }

// public class Vehicle : MonoBehaviour {
// 	public Vector2 Position {set; get;}	//位置
// 	public float Angle {get; set;}		//偏转角度
// 	public float Distance {get; set;}	//每次移动的距离
    
// 	public void Move(Direction direction)	//根据方向更新位置和角度
// 	{
// 		UpdatePos(direction);				//更新位置
// 		UpdateAngle(direction);				//更新角度
// 	}
	
// 	/// <summary>
// 	/// 更新位置
// 	/// </summary>
// 	/// <param name="direction">方向</param>
// 	protected void UpdatePos(Direction direction)
// 	{
// 		switch (direction)
// 		{
// 			case Direction.forWard: //向前移动时，根据载具的角度Angle，使载具的位置向Angle所指方向移动
// 				Position = new Vector2(
// 					Position.x + Distance * (float)Math.Cos(Angle),
// 					Position.y + Distance * (float)Math.Sin(Angle)
// 				);
// 			break;
// 			case Direction.backWard://向后移动时，根据载具的角度Angle，使载具的位置向Angle所指反方向移动
// 				Position = new Vector2(
// 					Position.x - Distance * (float)Math.Cos(Angle),
// 					Position.y - Distance * (float)Math.Sin(Angle)
// 				);
// 			break;
// 		}
// 	}

// 	protected void UpdateAngle(Direction direction)
// 	{
// 		switch (direction)
// 		{
// 			case Direction.leftTurn: //角度加 1度，再转换为弧度制
// 				Angle = Angle + (float)Math.PI / 180;
// 			break;
// 			case Direction.rightTurn://角度减 1度，再转换为弧度制
// 				Angle = Angle - (float)Math.PI / 180;
// 			break;
// 		}
// 	}

// }
