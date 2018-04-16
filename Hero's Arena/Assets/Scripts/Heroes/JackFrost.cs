using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JackFrost : Hero {
	private string s1name = "Arctic Cliff";
	private string s2name = "Frostbite";
	private string ultname = "Zeroth Hour";
	private string passname = "Permafrost";

	private float freeze = .65f;

	// Use this for initialization
	public override void Start () {
		base.Start ();

		EV = 1f;
		ACCb = .90f;
		ACC = 1f;

		ATK = 16;
		DEF = 15;
		DMG = 85;

		HP = 295;
		maxHP = 295;
		SPEED = 15;
		maxSPEED = 15;
		wallDMG = 1;
		RNG = 2;
	}

	// Update is called once per frame
	public override void Update () {
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [3]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [2]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultname + " [4]";
		}
		base.Update ();
	}

	//Arctic Cliff: create a wall covering 3 spaces within 5 spaces (lasts 2 turns or till canceled).
	override public bool Skill1() {
		return true;
	}

	//Frostbite: next attack has a chance of freezing the target
	override public bool Skill2() {
		return true;
	}

	//Zeroth hour: for the next 2 turns frostbite costs 0BP and Permafrost has chance to freeze targets in range.
	public override bool Ult() {
		return true;
	}

	//Permafrost: when target ends turn within 1 space their speed is reduced on their next turn
}
