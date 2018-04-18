using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Evergreen : Hero
{
	public GameObject Food;
	public GameObject Wall;
	private string s1name = "Nature's Bounty";
	private string s2name = "Toxic Bloom";
	private string ultName = "Forest Prison";

	private float poison = .65f;

	// Use this for initialization
	public override void Start ()
	{

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
	public override void Update ()
	{
		if (GameManager.instance.turn == team.tag && tman.turn == this.tag) {
			tman.skill1Button.GetComponentInChildren<Text> ().text = s1name + " [2]";
			tman.skill2Button.GetComponentInChildren<Text> ().text = s2name + " [3]";
			tman.ultButton.GetComponentInChildren<Text> ().text = ultName + " [5]";
		}

		base.Update ();

	}

	//Natures Bounty: creates "Food" items on spaces around her. {3BP}
	override public bool Skill1 ()
	{
		int cost = 2;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			Vector3 pos = transform.position;

			GameObject p1 = Instantiate (Food);
			p1.transform.position = new Vector3(pos.x + 1, pos.y, pos.z);

			GameObject p2 = Instantiate (Food);
			p2.transform.position = new Vector3(pos.x - 1, pos.y, pos.z);

			GameObject p3 = Instantiate (Food);
			p3.transform.position = new Vector3(pos.x, pos.y + 1, pos.z);

			GameObject p4 = Instantiate (Food);
			p4.transform.position = new Vector3(pos.x, pos.y - 1, pos.z);

			tman.msgText.text = this.tag + " used Nature's Bounty"; 
			tman.BP -= cost;
			return true;
		}
		return false;
	}

	/*
	protected override void Skill1Calc() {
		int cost = 2;
		Vector3 tpos = selectedTiletarget.transform.position;
		foreach (GameObject wall in GameObject.FindGameObjectsWithTag("TempWall")) {
			if (wall.transform.position.x == tpos.x &&
			    wall.transform.position.y == tpos.y) {
				tman.msgText.text = "can not target this space";
				return;
			}
		}
		GameObject p = Instantiate (Food);
		p.transform.position = tpos;
		tman.BP -= cost;
		animator.SetTrigger ("ATK");
		targeting = false;
		Destroy (GameObject.Find ("Target"));
	}
*/

	//Toxic Bloom: all enemies within 3 spaces have a chance to be poisoned. {3BP}
	override public bool Skill2 ()
	{
		int cost = 3;
		string text = this.tag + ":\n";
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			if (enemiesInRange (3)) {
				List<Hero> enemies = getEnemiesInRange (3);
				foreach (Hero e in enemies) {
					float r = Random.value;
					if (r < poison) {
						e.Poison ();
						text += e.tag + " was poisoned " + "\n";
					}
				}
				tman.msgText.text = text;
				tman.BP -= cost;
				return true;
			}
		}
		return false;
	}

	private bool enemiesInRange (int r)
	{
		List<Hero> enemies = tman.getEnemyTeam ();
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

	private List<Hero> getEnemiesInRange (int r)
	{
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

	//Forest Prison: surround target with temporary walls  {5BP}
	public override bool Ult ()
	{
		int cost = 5;
		if (tman.BP >= cost && GameManager.instance.turn == team.tag
		    && tman.getCurrentHero ().tag == this.tag) {
			targeting = true;
			targetingType = 3;
			makeEATarget (RNG * 30);
			return true;
		}
		return false;
	}

	protected override void UltCalc() {
		int cost = 5;

		Vector3 tpos = selectedTarget.transform.position;

		GameObject p1 = Instantiate (Wall);
		p1.transform.position = new Vector3(tpos.x + 1, tpos.y, tpos.z);

		GameObject p2 = Instantiate (Wall);
		p2.transform.position = new Vector3(tpos.x - 1, tpos.y, tpos.z);

		GameObject p3 = Instantiate (Wall);
		p3.transform.position = new Vector3(tpos.x, tpos.y + 1, tpos.z);

		GameObject p4 = Instantiate (Wall);
		p4.transform.position = new Vector3(tpos.x, tpos.y - 1, tpos.z);

		tman.BP -= cost;
		targeting = false;
		Destroy (GameObject.Find ("Target"));
	}
}
