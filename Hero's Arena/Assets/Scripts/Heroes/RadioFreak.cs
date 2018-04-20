using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RadioFreak : Hero
{

	private string s1name = "Static";
	private string s2name = "SMPTE";
	private string ultName = "Glitch";
	private string info = "RadioFreak: \nStatic: Target enemy can't use one of their skills (selected by random) during their next turn {2BP}\n" +
	                      "SMPTE: Inflict 1 random DISADV on target [stun, freeze, burn, poison, bleed] {2BP}\n" +
	                      "Glitch: All allies get a random ADV, all enemies get a random DISADV {5BP}";

	// Use this for initialization
	public override void StartGame ()
	{

		base.StartGame ();

		EV = 1f;
		ACCb = .90f;
		ACC = 1f;

		ATK = 14;
		DEF = 16;
		DMG = 70;

		HP = 350;
		maxHP = 350;
		SPEED = 15;
		maxSPEED = 15;
		wallDMG = 2;
		RNG = 2;
	}

	// Update is called once per frame
	public override void Update ()
	{
		if (tman != null && GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [2]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [2]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultName + " [5]";
		}

		base.Update ();

	}

	//Static: target enemy can't use one of their skills (chosen by random) during their next turn {2BP}
	override public bool Skill1 ()
	{
		int cost = 2;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag) {
			targeting = true;
			targetingType = 1;
			makeTarget (3);
			return true;
		}
		return false;
	}

	protected override void Skill1Calc() {
		int cost = 2;
		selectedTarget.Static ();
		tman.msgText.text = this.tag + " inflicted " + selectedTarget.tag + " with Static.";
		tman.BP -= cost;
		targeting = false;
		Destroy (GameObject.Find ("Target"));
	}

	//SMPTE: inflict 1 random debuff on target [stun, freeze, DISADV, burn, poison, bleed] {2BP}
	override public bool Skill2 ()
	{
		int cost = 2;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			targeting = true;
			targetingType = 2;
			makeTarget (3);
			return true;
		}
		return false;
	}

	override protected void Skill2Calc ()
	{
		int cost = 2;
		float r = Random.value;
		string type = "";
		if (r <= .25) {
			selectedTarget.Stun ();
			type = "stun";
		} else if (r <= .50) {
			selectedTarget.effects.Add (Effects.DADV);
			type = "disadvantage";
		} else if (r <= .75) {
			selectedTarget.Poison ();
			type = "poison";
		}
		tman.msgText.text = this.tag + " inflicted " + type + " on " + selectedTarget.tag;
		tman.BP -= cost;
		targeting = false;
		Destroy (GameObject.Find ("Target"));
	}

	//Glitch: all allies get a random buff, all enemies get a random debuff {5BP}
	public override bool Ult ()
	{
		int cost = 5;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			List<Hero> allies = tman.getTeam ();
			List<Hero> enemies = tman.getEnemyTeam ();

			string type = "";
			tman.msgText.text = this.tag + "gave "; 
			foreach (Hero a in allies) {
				float r = Random.value;
				if (r <= .25) {
					a.effects.Add (Effects.ADV);
					type = "advantage";
				} else if (r <= .50) {
					a.effects.Add (Effects.RNGUP);
					type = "Range Up";
				} else if (r <= .55) {
					a.effects.Add (Effects.DOUBLEDMG);
					type = "Double Damage";
				}
				tman.msgText.text += a.tag + " " + type + "\n";
			}

			foreach (Hero e in enemies) {
				float r = Random.value;
				if (r <= .25) {
					e.Stun ();
					type = "stun";
				} else if (r <= .50) {
					e.effects.Add (Effects.DADV);
					type = "disadvantage";
				} else if (r <= .75) {
					e.Poison ();
					type = "poison";
				}
				tman.msgText.text += e.tag + " " + type + "\n";
			}
			tman.BP -= cost;
			return true;
		} else {
			return false;
		}
	}

	public override string Info() {
		return info;
	}
}
