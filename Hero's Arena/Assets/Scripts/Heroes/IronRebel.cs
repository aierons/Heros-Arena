using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IronRebel : Hero {

	private string s1name = "Armor Spike";
	private string s2name = "Armor Up";
	private string s2name2 = "Armor Down";
	private string ultName = "Heavy Panzer";

	private bool spiked;
	private bool armorUp;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		HP = 7;
		maxHP = 7;
		DEF = 14;
		SPEED = 5;
		maxSPEED = 5;
		ATK = 2;
		DMG = 2;
		wallDMG = 3;
		RNG = 1;
		spiked = false;
		armorUp = false;
	}
	
	public override void Update () {
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [2]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [1]";
			if (armorUp) {
				tman.skill2Button.GetComponentInChildren<Text> ().text = s2name2 + " [1]";
			}
			tman.ultButton.GetComponentInChildren<Text> ().text = ultName + " [4]";
		} 

		if (armorUp) {
			DEF = 15;
			maxSPEED = 3;
		} else {
			DEF = 14;
			maxSPEED = 5;
		}

		base.Update ();
	}

	//Armor Spike : next time an enemy deals damage to you they take 2 damage
	override public bool Skill1() {
		int cost = 2;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag && !spiked) {
			spiked = true;
			tman.msgText.text = this.tag + " has activated Armor Spike";
			tman.BP -= cost;
			return true;
		} else {
			return false;
		}
	}

	public override void Losehp (int loss) {
		if (spiked) {
			tman.etman.getCurrentHero ().Losehp (1);
			tman.msgText.text = tman.etman.getCurrentHero ().tag + " took 2 damage from Armor Spike";
			spiked = false;
		}

		//Unbreakable
		if (HP > 1 && HP - loss <= 0) {
			HP = 1;
			tman.msgText.text = this.tag + " passive (Unbreakable) activated";
		} else {
			base.Losehp (loss);
		}
	}

	//Iron Defense : Raise Def by 2 until next turn
	override public bool Skill2() {
		int cost = 1;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag) {
			armorUp = !armorUp;
			if (armorUp) {
				tman.msgText.text = this.tag + " has activated Armor up";
			} else {
				tman.msgText.text = this.tag + " has activated Armor down";
			}
			tman.BP -= cost;
			return true;
		} else {
			return false;
		}
	}

	public override bool Ult() {
		int cost = 4;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			effects.Add (Effects.DOUBLEDMG);
			tman.msgText.text = this.tag + " has activated Heavy Panzer";
			tman.BP -= cost;
			return true;
		} else {
			return false;
		}
	}


}
