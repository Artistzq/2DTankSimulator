using UnityEngine;

public enum Direction
{
	forWard,		//向前
	backWard,		//向后
	leftTurn,		//向左转
	rightTurn		//向右转
}

public enum Ctrller
{
	enemy,		//敌人（电脑）
	player,		//主玩家（如果只有一个玩家）
	wingman		//二号坦克
}

public class PressButton
{
	public KeyCode Forward{get; set;}
	public KeyCode Backward{get; set;}
	public KeyCode Letfturn{get; set;}
	public KeyCode Rightturn{get; set;}
	public KeyCode Reload{get; set;}


}

public class PlayerController : PressButton
{
	public string Fire;
	public PlayerController()
	{
		Forward = KeyCode.W;
		Backward = KeyCode.S;
		Letfturn = KeyCode.A;
		Rightturn = KeyCode.D;
		Reload = KeyCode.R;
	}
}

public class WingmanController : PressButton
{
	
}

public enum bullet
{
	enemy,
	player,
	wingman
}

//皮肤类
[System.Serializable]
public class SkinClass
{
	public Sprite bodySkin;		//车体皮肤
	public Sprite turretSkin;	//炮塔皮肤
}