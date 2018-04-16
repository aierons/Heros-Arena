using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Evergreen : Hero {

	private string s1name = "Nature's Bounty";
	private string s2name = "Toxic Bloom";
	private string ultName = "Guardian of the Forest";

	private float poison = .65f;

	// Use this for initialization
	void Start () {

		base.Start ();

		EV = 1f;
		ACCb = .90f;
		ACC = 1f;

		ATK = 16;
		DEF = 15;
		DMG = 70;

		HP = 290;
		maxHP = 290;
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
			tman.ultButton.GetComponentInChildren<Text> ().text = ultName + " [5]";
		}

		base.Update ();

	}

	//Natures Bounty: creates "Food" on target space within 1 space. {2BP}
	override public bool Skill1() {
		return true;
	}

	//Toxic Bloom: all enemies within 3 spaces have a chance to be poisoned. {3BP}
	override public bool Skill2() {
		int cost = 3;
		string text = this.tag + ":\n";
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			if (enemiesInRange (3)) {
				List<Hero> enemies = getEnemiesInRange (3);
				foreach (Hero e in enemies) {
					float r = Random.value;
					if (r < .70) {
						e.Poison ();
						text += e.tag + " was poissoned " + "\n";
					}
				}
				tman.msgText.text = text;
				tman.BP -= cost;
				return true;
			}
		} else {
			return false;
		}
	}

	private bool enemiesInRange(int r) {
		List<Hero> enemies = tman.getEnemyTeam();
		List<Vector3> epos = new List<Vector3> ();

		foreach (Hero e in enemies) {
			epos.Add (e.transform.position);
		}

		Vector3 pos = this.transform.position;

		for (int i = 0; i < epos.Count; i++) {
			if ((epos [i].x >= pos.x - r || epos [i].x <= pos.x + r) &&
				(epos [i].y >= pos.y - r || epos [i].y <= pos.x + r)) {
				return true;
			}
		}
		return false;
	}

	private List<Hero> getEnemiesInRange(int r) {
		List<Hero> e = tman.getEnemyTeam ();

		Vector3 pos = this.transform.position;

		for (int i = 0; i < e.Count; i++) {
			Vector3 p = e [i].transform.position;
			if ((p.x >= pos.x - r || p.x <= pos.x + r) &&
				(p.y >= pos.y - r || p.y <= pos.x + r)) {

			} else {
				e.Remove (e [i]);
			}
		}
		return e;
	}

	//Guardian of the Forest: Turns into a giant for 2 turns [takes up 4x4 space] deals extra damage while during duration {5BP}
	public override bool Ult() {
		return true;
	}
}
