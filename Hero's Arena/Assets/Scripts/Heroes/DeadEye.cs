using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadEye : Hero
{

	private string s1name = "Steady Aim";
	private string s2name = "Knee Shot";
	private string ultName = "Mark The Dead Man";

	// Use this for initialization
	public override void Start ()
	{

		base.Start ();

		EV = 1f;
		ACCb = .92f;
		ACC = 1f;

		ATK = 16;
		DEF = 12;
		DMG = 70;

		HP = 220;
		maxHP = 220;
		SPEED = 15;
		maxSPEED = 15;
		wallDMG = 1;
		RNG = 4;
	}
	
	// Update is called once per frame
	public override void Update ()
	{
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [1]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [2]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultName + " [6]";
		}

		base.Update ();
		
	}

	//Steady Hand: Grant self ADV and increase range by 1. {1BP}
	override public bool Skill1 ()
	{
		int cost = 1;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag) {
			if (effects.Contains (Effects.RNGUP) && effects.Contains (Effects.ADV)) {
				tman.msgText.text = "can not currently activate this ability";
				return false;
			} else {
				base.RangeUp ();
				effects.Add (Effects.ADV);
				tman.BP -= cost;
				tman.msgText.text = this.tag + " used Steady Hand, gained advantage and +1 RANGE for next attack.";
				return true;
			}
		} else {
			return false;
		}
	}

	//Knee Shot: next hit stuns target and grants them disadvantage on their next turn. {2BP}
	override public bool Skill2 ()
	{
		
		return true;
	}

	//Mark the Dead Man: choose target, next attack deals very large amount of damage [6BP]
	public override bool Ult ()
	{
		return true;
	}
}
