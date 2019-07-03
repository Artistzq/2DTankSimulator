//皮肤（贴图）

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin : MonoBehaviour {

	public GameObject player;
	public GameObject wingman;

	public SkinClass M99A; 	// ZTZ - 99A 式主战坦克 （默认皮肤）
	public SkinClass M96B;

	// Use this for initialization
	void Start () 
	{	
		player = GameObject.Find("Player");
		player.GetComponent<SpriteRenderer>().sprite = M99A.bodySkin;
		player.transform.Find("Turret").GetComponent<SpriteRenderer>().sprite = M99A.turretSkin;
		wingman = GameObject.Find("Wingman");
		wingman.GetComponent<SpriteRenderer>().sprite = M96B.bodySkin;
		wingman.transform.Find("Turret").GetComponent<SpriteRenderer>().sprite = M96B.turretSkin;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
