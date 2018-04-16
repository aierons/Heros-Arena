using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ferocity : Hero {

	private string s1name = "Explosion";
	private string s2name = "Ignition";
	private string ultname = "Hellfire";
	private string passname = "Immolate";

	private float burn = .65f;

	// Use this for initialization
	public override void Start () {
		base.Start ();

		EV = 1f;
		ACCb = .90f;
		ACC = 1f;

		ATK = 17;
		DEF = 14;
		DMG = 90;

		HP = 280;
		maxHP = 280;
		SPEED = 15;
		maxSPEED = 15;
		wallDMG = 2;
		RNG = 3;
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

	//Explosion: next attack deals splash damage to spaces above and below target.
	override public bool Skill1() {
		return true;
	}

	//Ignition: next attack has increased range and inflicts burn on enemy
	override public bool Skill2() {
		return true;
	}

	//Hellfire: for the next two turns DMG is increased and all abilities have a large chance of inflicting burn on targets.
	public override bool Ult() {
		return true;
	}

	//Immolate: all direct attacks (basically the chosen target)
}
