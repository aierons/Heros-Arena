using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Medico : Hero {

	private string s1name = "Injection";
	private string s2name = "Patch Up";
	private string ultname = "Elixir";
	private string passname = "This Might Sting A Bit";

	private float poison = .65f;


	// Use this for initialization
	void Start () {
		base.Start ();

		EV = 1f;
		ACCb = .90f;
		ACC = 1f;

		ATK = 15;
		DEF = 13;
		DMG = 75;

		HP = 250;
		maxHP = 250;
		SPEED = 15;
		maxSPEED = 15;
		wallDMG = 1;
		RNG = 1;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [2]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [3]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultname + " [5]";
		}

		base.Update ();
		
	}

	//Injection: if target is enemy inflict poison, if ally heal a medium amount of hp (can target self)
	override public bool Skill1() {
		return true;
	}

	//Patch: remove all effects from  target (good and bad)
	override public bool Skill2() {
		return true;
	}

	//Elixir: for a few turns target ally gets large buff to all stats.
	public override bool Ult() {
		return true;
	}

	//This might sting a bit :  can target allies with attack to heal a small
}
