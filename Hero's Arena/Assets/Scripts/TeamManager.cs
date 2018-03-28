using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
	public GameObject enemyTeam;
	[HideInInspector] public TeamManager etman;

	//Grabs reference to attack button 
	public Button attackButton;
	//Grabs reference to end button
	public Button endTurnButton;

	public Button skill1Button;
	public Button skill2Button;
	public Button ultButton;

	//UI Text to display current hp for each hero
	public Text hpText;

	public Text msgText;
	public Text actText;

	public  GameObject captain;
	public GameObject member1;
	public GameObject member2;

	private Hero hcaptain;
	private Hero hmember1;
	private Hero hmember2;

	public string turn;
	private int turnCount;

	public int BP;
	public int maxBP;

	public List<Hero> getTeam() {
		List<Hero> t = new List<Hero> ();
		t.Add (hcaptain);
		t.Add(hmember1);
		t.Add(hmember2);
		return t;
	}

	public List<Hero> getEnemyTeam() {
		return etman.getTeam ();
	}

	public Hero getCurrentHero() {
		if (turn == captain.tag) {
			return hcaptain;
		} else if (turn == member1.tag) {
			return hmember1;
		} else {
			return hmember2;
		}
	}

	public List<Vector3> getAllyPoses(Hero user) {
		List<Vector3> p = new List<Vector3> ();
		if (user == hcaptain) {
			p.Add (captain.transform.transform.position);
		}
		if (user == hmember1) {
			p.Add (member1.transform.transform.position);
		}
		if (user == hmember2) {
			p.Add (member2.transform.transform.position);
		}
		return p;
	}

	private void changeCurrentHero() {
		if (turn == captain.tag) {
			turn = member1.tag;
		} else if (turn == member1.tag) {
			turn = member2.tag;
		} else {
			turn = captain.tag;
		}
	}

	private string getHeroText(Hero h) {
		return h.tag + " : " + h.getHP () + "/" + h.getMaxHP () + " DEF:" + h.getDEF() + " SPEED:" + h.getSPEED();
	}

	// Use this for initialization
	void Start ()
	{
		turnCount = 0;
		BP = 1;
		maxBP = 1;

		hcaptain = captain.GetComponent<Hero>();
		hmember1 = member1.GetComponent<Hero>();
		hmember2 = member2.GetComponent<Hero>();

		etman = enemyTeam.GetComponent<TeamManager> ();

		turn = captain.tag;

		hpText.text = getHeroText(hcaptain) + "\n" + getHeroText(hmember1) + "\n" + getHeroText(hmember2)
			+ "\n" + BP.ToString() + "/" + maxBP.ToString();
		msgText.text = "";
		actText.text = "";

		attackButton.onClick.AddListener (TriggerAttack);
		endTurnButton.onClick.AddListener (TurnEnd);
		skill1Button.onClick.AddListener (TriggerSkill1);
		skill2Button.onClick.AddListener (TriggerSkill2);
		ultButton.onClick.AddListener (TriggerUlt);
	}
	
	// Update is called once per frame
	void Update () {
		hpText.text = getHeroText(hcaptain) + "\n" + getHeroText(hmember1) + "\n" + getHeroText(hmember2)
			+ "\n" + BP.ToString() + "/" + maxBP.ToString();

		if (GameManager.instance.turn == tag) {
			actText.color = Color.red;
			actText.text = turn;
		} else {
			actText.text = "";
		}

		if (hmember1.getHP () <= 0 && turn == member1.tag) {
			turn = member2.tag;
		}
		if (hmember2.getHP () <= 0 && turn == member2.tag) {
			turn = captain.tag;
		}
	}

	public void CheckIfGameOver() {
		//Check if hp point total is less than or equal to zero.
		if (hcaptain.getHP() <= 0) {
			//Stop the background music.
			SoundManager.instance.musicSource.Stop ();

			//Call the GameOver function of GameManager.
			GameManager.instance.GameOver ();

			this.gameObject.SetActive (false);
		}
	}

	private void TriggerAttack() {
		bool act;
		if (GameManager.instance.turn == tag) {
			if (turn == captain.tag) {
				act = hcaptain.Attack ();
			} else if (turn == member1.tag) {
				act = hmember1.Attack ();
			} else {
				act = hmember2.Attack ();
			}
			if (act) {
				attackButton.interactable = false;
			}
		}
	}


	private void TriggerSkill1() {
		bool act;
		if (GameManager.instance.turn == tag) {
			if (turn == captain.tag) {
				act = hcaptain.Skill1 ();
			} else if (turn == member1.tag) {
				act = hmember1.Skill1 ();
			} else {
				act = hmember2.Skill1 ();
			}
			if (act) {
				skill1Button.interactable = false;
				skill2Button.interactable = false;
			}
		}
	}

	private void TriggerSkill2() {
		bool act;
		if (GameManager.instance.turn == tag) {
			if (turn == captain.tag) {
				act = hcaptain.Skill2 ();
			} else if (turn == member1.tag) {
				act = hmember1.Skill2 ();
			} else {
				act = hmember2.Skill2 ();
			}
			if (act) {
				skill2Button.interactable = false;
				skill1Button.interactable = false;
			}
		}
	}

	private void TriggerUlt() {
		bool act;
		if (GameManager.instance.turn == tag) {
			if (turn == captain.tag) {
				act = hcaptain.Ult ();
			} else if (turn == member1.tag) {
				act = hmember1.Ult ();
			} else {
				act = hmember2.Ult ();
			}
			if (act) {
				ultButton.interactable = false;
			}
		}
	}

	private void TurnEnd() {
		if (GameManager.instance.turn == this.tag) {
			attackButton.interactable = false;
			endTurnButton.interactable = false;
			skill1Button.interactable = false;
			skill2Button.interactable = false;
			ultButton.interactable = false;

			etman.attackButton.interactable = true;
			etman.endTurnButton.interactable = true;
			etman.skill1Button.interactable = true;
			etman.skill2Button.interactable = true;
			etman.ultButton.interactable = true;

			skill1Button.GetComponentInChildren<Text> ().text = "Skill1";
			skill2Button.GetComponentInChildren<Text> ().text = "Skill2";
			ultButton.GetComponentInChildren<Text> ().text = "Ultimate";

			Hero h = getCurrentHero ();
			h.EndTurn ();
			turnCount++;
			if (turnCount % 3 == 0) {
				RoundEnd ();
			}
			GameManager.instance.turn = enemyTeam.tag;
			changeCurrentHero ();
		}
	}

	private void RoundEnd() {
		if (maxBP < 10) {
			maxBP++;
			BP = maxBP;
		}
	}

}

