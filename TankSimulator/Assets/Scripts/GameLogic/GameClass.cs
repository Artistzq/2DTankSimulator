using UnityEngine;

/// <summary>
/// 表示方向的枚举类型
/// </summary>
public enum Direction
{
	forWard,		//向前
	backWard,		//向后
	leftTurn,		//向左转
	rightTurn		//向右转
}

/// <summary>
/// 表示控制器的枚举类型
/// </summary>
public enum Ctrller
{
	enemy,		//敌人（电脑）
	player,		//主玩家（如果只有一个玩家）
	wingman,	//二号坦克
	destroyed	//被摧毁
}