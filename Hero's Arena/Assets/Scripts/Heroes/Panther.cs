using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panther : Hero {

	private string s1name = "Agile";
	private string s2name = "Scratch Dash";
	private string ultName = "Catastrophe";

	// Use this for initialization
	public override void Start () {
		base.Start ();

		EV = 1f;
		ACCb = .91f;
		ACC = 1f;

		ATK = 16;
		DEF = 15;
		DMG = 80;

		HP = 245;
		maxHP = 245;
		SPEED = 15;
		maxSPEED = 15;
		wallDMG = 2;
		RNG = 1;
	}

	// Update is called once per frame
	public override void Update () {
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [1]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [1]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultName + " [5]";
		}
		base.Update ();
	}
}
