using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IronRebel : Hero {

	private string s1name = "Armor Spike";
	private string s2name = "Iron Hide";
	private string ultName = "Panzer Smash";

	private bool spiked;
	private bool armorUp;

	private int armor;

	public override string getHeroText ()
	{
		return tag + " : " + HP + "+" + armor + "/" + maxHP + " SPEED:" + SPEED;
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();
		HP = 300;
		maxHP = 300;
		DEF = 14;
		SPEED = 15;
		maxSPEED = 15;
		ATK = 2;
		DMG = 45;
		wallDMG = 3;
		RNG = 1;
		spiked = false;
		armorUp = false;
		armor = 0;
	}
	
	public override void Update () {
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [2]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [2]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultName + " [4]";
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
			tman.etman.getCurrentHero ().Losehp (15);
			tman.msgText.text = tman.etman.getCurrentHero ().tag + " took 15 damage from Armor Spike";
			spiked = false;
		}

		//Armor
		if (armor > 0 && armor >= loss) {
			armor -= loss;
		} else if (armor > 0 && armor < loss) {
			loss -= armor;
			armor = 0;
			base.Losehp(loss);
		} else {
		base.Losehp (loss);
		}
	}

	//Iron Hide : grant self some temp HP
	override public bool Skill2() {
		int cost = 2;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag) {
			armor = 50;
			tman.msgText.text = this.tag + " has activated Iron Hide";
			tman.BP -= cost;
			return true;
		} else {
			return false;
		}
	}

	//Panzer Smash: next attack deals damage in a 3x4 rectangle space infront of him, goes through walls and destroys walls, all enemies hit are stunned {4BP}
	public override bool Ult() {
		int cost = 4;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			effects.Add (Effects.DOUBLEDMG);
			tman.msgText.text = this.tag + " has activated Panzer Smash";
			tman.BP -= cost;
			return true;
		} else {
			return false;
		}
	}


}
