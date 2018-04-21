using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadEye : Hero
{

	private string s1name = "Steady Aim";
	private string s2name = "Twin Bullets";
	private string ultName = "For Whom the Bell Toles";
	private string info = "DeadEye: \nSteady Hand: Grants self ADV and increases range by 1 {1BP}\n" +
	                      "Twin Shot: Inflict DISADV on self and make two consecutive attacks against a target in range {2BP}\n" +
	                      "The Bell Toles Thy Name: Target any enemy no matter how far away and deal a large amount of damage {6BP}";

	// Use this for initialization
	public override void StartGame ()
	{
		base.StartGame ();

		EV = 1f;
		ACCb = .92f;
		ACC = 1f;

		ATK = 16;
		DEF = 12;
		DMG = 75;

		HP = 220;
		maxHP = 220;
		SPEED = 6;
		maxSPEED = 6;
		wallDMG = 1;
		RNG = 4;
	}
	
	// Update is called once per frame
	public override void Update ()
	{
		if (tman != null && GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [1]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [3]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultName + " [6]";

			if (!tman.attackButton.interactable) {
				tman.skill2Button.interactable = false;
			}
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
				if (!effects.Contains (Effects.RNGUP)) {
					RangeUp ();
				}
				if (!effects.Contains (Effects.ADV)) {
					effects.Add (Effects.ADV);
				}
				tman.BP -= cost;
				tman.msgText.text = this.tag + " used Steady Hand, gained advantage and +1 RANGE for next attack.";
				return true;
			}
		} else {
			return false;
		}
	}

	//Twin Bullets: make two attacks on a target with disadvantage {3BP}
	override public bool Skill2 ()
	{
		int cost = 3;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			if (!tman.attackButton.interactable) {
				tman.msgText.text = "can not currently use this ability";
				return false;
			}
			targeting = true;
			targetingType = 2;
			makeTarget (RNG);
			return true;
		}
		return false;
	}

	override protected void Skill2Calc() {
		int cost = 3;
		int loss1 = 0;
		tman.msgText.text = "";
		for (int i = 0; i < 2; i++) {
			
			if (!effects.Contains(Effects.DADV)) {
				effects.Add (Effects.DADV);
			}

			if (isHit (selectedTarget)) {
				loss1 = getDamage (selectedTarget.getDEF());
				selectedTarget.Losehp (loss1);
				tman.msgText.text += this.tag + " landed a hit on " + selectedTarget.tag + " dealt " + loss1 + " damage\n";
				animator.SetTrigger ("ATK");
			} else {
				tman.msgText.text += this.tag + " missed a hit on " + selectedTarget.tag + "\n";
			}
		}

		tman.BP -= cost;
		tman.attackButton.interactable = false;
		targeting = false;
		Destroy (GameObject.Find ("Target"));
	}

	//For Whom the Bell Toles: deal large amount of damage to one target no matter how far away they are [6BP]
	public override bool Ult ()
	{
		int cost = 6;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			targeting = true;
			targetingType = 3;
			makeTarget (RNG*25);
			return true;
		}
		return false;
	}

	protected override void UltCalc() {
		int cost = 6;
		int loss = 0;

		if (!effects.Contains (Effects.DOUBLEDMG)) {
			effects.Add (Effects.DOUBLEDMG);
		}
		loss = getDamage (selectedTarget.getDEF());
		selectedTarget.Losehp (loss);
		tman.msgText.text = " The bell toles " + selectedTarget.tag +"'s name " + this.tag + " dealt " + loss + " damage";
		animator.SetTrigger ("ATK");

		tman.BP -= cost;
		targeting = false;
		Destroy (GameObject.Find ("Target"));
	}

	public override string Info() {
		return info;
	}
}
