using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RadioFreak : Hero {

	private string s1name = "Static";
	private string s2name = "SMPTE";
	private string ultName = "Glitch";

	// Use this for initialization
	void Start () {

		base.Start ();

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
	void Update () {
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [2]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [2]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultName + " [5]";
		}

		base.Update ();

	}

	//Static: target enemy can't use one of their skills (chosen by random) during their next turn {2BP}
	override public bool Skill1() {
		return true;
	}

	//SMPTE: inflict 1 random debuff on target [stun, freeze, DISADV, burn, poison, bleed] {2BP}
	override public bool Skill2() {
		return true;
	}

	//Glitch: all allies get a random buff, all enemies get a random debuff {5BP}
	public override bool Ult() {
		int cost = 5;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag) {
			List<Hero> allies = tman.getTeam ();
			List<Hero> enemies = tman.getEnemyTeam ();


			foreach (Hero a in allies) {
				float r = Random.value;
				if (r <= .25) {
					a.effects.Add (Effects.ADV);
				} else if (r <= .50) {
					a.effects.Add (Effects.RNGUP);
				} else if (r <= .55) {
					a.effects.Add (Effects.DOUBLEDMG);
				}
			}

			foreach (Hero e in enemies) {
				float r = Random.value;
				if (r <= .25) {
					e.Stun ();
				} else if (r <= .50) {
					e.effects.Add (Effects.DADV);
				} else if (r <= .75) {
					e.Poison ();
				}
			}
			tman.BP -= cost;
			return true;
		} else {
			return false;
		}
	}
}
