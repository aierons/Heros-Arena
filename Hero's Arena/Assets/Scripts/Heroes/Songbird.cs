using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Songbird : Hero {

	private string s1name = "Song of Healing";
	private string s2name = "Song of Battle";
	private string ultName = "Song of Valor";
	private string info = "Song of Healing: all allies within 1 space heal 2 hp. {3BP}\n\t" +
	                      "Song of Battle: allies within 1 space get a bonus to ATK and a bonus to DMG {3BP} \n\t" +
	                      "Song of Valor: all allies within 3 spaces gain a large bonus to ATK and a larger bonus to DMG until the end of their next turn. {5 BP}\n\t" +
	                      "Inner Song: Heal a small amount of health to self every other turn.";

	private bool sing;

	// Use this for initialization
	public override void Start () {
		base.Start ();

		EV = 1f;
		ACCb = .90f;
		ACC = 1f;

		ATK = 14;
		DEF = 14;
		DMG = 70;

		HP = 200;
		maxHP = 200;
		SPEED = 15;
		maxSPEED = 15;
		wallDMG = 1;
		RNG = 2;

		sing = false;
	}
	
	// Update is called once per frame
	public override void Update () {
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [3]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [3]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultName + " [7]";
		}
		base.Update ();
	}

	//Song of Healing
	override public bool Skill1() {
		int cost = 3;
		string text = this.tag + ":\n";
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			if (alliesInRange (1) && allyNeedsHealing()) {
				List<Hero> h = getAlliesInRange (1);
				foreach (Hero t in h) {
					t.Heal (30);
					text += t.tag + " was healed 20 HP"  + "\n";
				}
				tman.msgText.text = text;
				tman.BP -= cost;
				return true;
			}
		}
		return false;
	}

	//Song of Battle
	override public bool Skill2() {
		int cost = 3;
		string text = this.tag + ":\n";
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
			&& tman.getCurrentHero ().tag == this.tag) {
			if (alliesInRange (1)) {
				List<Hero> h = getAlliesInRange (1);
				foreach (Hero t in h) {
					t.Boost ();
					text +=  t.tag + " was Boosted " + "\n";
				}
				tman.BP -= cost;
				tman.msgText.text = text;
				return true;
			}
		}
		return false;
	}

	//Song of Valor
	public override bool Ult() {
		int cost = 7;
		string text = this.tag + ":\n";
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			List<Hero> h = getAlliesInRange (1);
			foreach (Hero t in h) {
				t.ValorBoost ();
				text += t.tag + " was Valor Boosted " + "\n";
			}
			tman.msgText.text = text;
			tman.BP -= cost;
			return true;
		}
		return false;
	}

	public override string Info() {
		return info;
	}

	//inner song
	public override void EndTurn() {
		if (sing) {
			Heal (10);
			tman.msgText.text = this.tag + " passive (Inner Song) activated";
			sing = false;
		} else {
			sing = true;
		}

		base.EndTurn ();
	}


	private bool alliesInRange(int r) {
		List<Vector3> p = tman.getAllyPoses (this);
		Vector3 pos = this.transform.position;

		for (int i = 0; i < p.Count; i++) {
			if ((p [i].x >= pos.x - r || p [i].x <= pos.x + r) &&
				(p [i].y >= pos.y - r || p [i].y <= pos.x + r)) {
				return true;
			}
		}
		return false;
	}

	private bool allyNeedsHealing() {
		List<Hero> t = tman.getTeam ();
		t.Remove (this);

		foreach (Hero h in t) {
			if (h.getHP () < h.getMaxHP ()) {
				return true;
			}
		}
		return false;
	}

	private List<Hero> getAlliesInRange(int r) {
		List<Hero> t = tman.getTeam ();
		t.Remove (this);

		Vector3 pos = this.transform.position;

		for (int i = 0; i < t.Count; i++) {
			Vector3 p = t [i].transform.position;
			if ((p.x >= pos.x - r || p.x <= pos.x + r) &&
			    (p.y >= pos.y - r || p.y <= pos.x + r)) {

			} else {
				t.Remove (t [i]);
			}
		}
		return t;
	}
}
