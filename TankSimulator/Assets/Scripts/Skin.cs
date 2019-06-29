//皮肤（贴图）

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin : MonoBehaviour {

	public GameObject tank;
	public GameObject turret;
	public GameObject cannonballs;

	public SkinClass M99A; 	// ZTZ - 99A 式主战坦克 （默认皮肤）

	// Use this for initialization
	void Start () 
	{	
		tank = GameObject.Find("Player");
		turret = GameObject.Find("Player/Turret");
		tank.GetComponent<SpriteRenderer>().sprite = M99A.bodySkin;
		turret.GetComponent<SpriteRenderer>().sprite = M99A.turretSkin;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
