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
	public Button infoButton; 

	//UI Text to display current hp for each hero
	public Text hpText;

	public Text msgText;
	public Text actText;
	public Text infoText;

	public GameObject captain;
	public GameObject member1;
	public GameObject member2;

	private Hero hcaptain;
	private Hero hmember1;
	private Hero hmember2;

	public string turn;
	private int turnCount;

	public int BP;
	public int maxBP;

	public bool start = false;

	public List<Hero> getTeam() {
		List<Hero> t = new List<Hero> ();
		t.Add(hcaptain);
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
		

	// Use this for initialization
	public void StartGame ()
	{
		if (member2 != null) {
			turnCount = 0;
			BP = 1;
			maxBP = 1;

			/*
		hcaptain = captain.GetComponent<Hero> ();
		hmember1 = member1.GetComponent<Hero>();
		hmember2 = member2.GetComponent<Hero>();
		*/

			etman = enemyTeam.GetComponent<TeamManager> ();

			turn = captain.tag;

			hpText.text = hcaptain.getHeroText () + "\n" + hmember1.getHeroText () + "\n" + hmember2.getHeroText ()
			+ "\n" + BP.ToString () + "/" + maxBP.ToString ();
			infoText.text = "";
			msgText.text = "";
			actText.text = "";
		
			attackButton.onClick.AddListener (TriggerAttack);
			endTurnButton.onClick.AddListener (TurnEnd);
			skill1Button.onClick.AddListener (TriggerSkill1);
			skill2Button.onClick.AddListener (TriggerSkill2);
			ultButton.onClick.AddListener (TriggerUlt);
			infoButton.onClick.AddListener (TriggerInfo);

			start = true;

			captain.GetComponent<Hero> ().StartGame ();
			member1.GetComponent<Hero> ().StartGame ();
			member2.GetComponent<Hero> ().StartGame ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (start) {
			hpText.text = hcaptain.getHeroText () + "\n" + hmember1.getHeroText () + "\n" + hmember2.getHeroText ()
			+ "\n" + BP.ToString () + "/" + maxBP.ToString ();

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
	}

	public void SetCaptain(GameObject cap, int team) {
		captain = cap;
		//turn = captain.tag;
		hcaptain = cap.GetComponent<Hero> ();
		captain.GetComponent<SpriteRenderer> ().enabled = true;
		captain.GetComponent<BoxCollider2D> ().enabled = true;
		captain.GetComponent<Hero> ().enabled = true;

		if (team == 1) {
			captain.GetComponent<Hero> ().setTeam (team);
			captain.transform.position = new Vector3 (0, 0, 0);
		} else {
			captain.GetComponent<Hero> ().setTeam (team);
			captain.transform.position = new Vector3 (13, 10, 0);
		}
	}

	public void SetMember1(GameObject member, int team) {
		member1 = member;
		hmember1 = member.GetComponent<Hero> ();
		member1.GetComponent<SpriteRenderer> ().enabled = true;
		member1.GetComponent<BoxCollider2D> ().enabled = true;
		member1.GetComponent<Hero> ().enabled = true;

		if (team == 1) {
			member1.GetComponent<Hero> ().setTeam (team);
			member1.transform.position = new Vector3 (2, 0, 0);
		} else {
			member1.GetComponent<Hero> ().setTeam (team);
			member1.transform.position = new Vector3 (11, 10, 0);
		}
	}

	public void SetMember2(GameObject member, int team) {
		member2 = member;
		hmember2 = member.GetComponent<Hero> ();
		member2.GetComponent<SpriteRenderer> ().enabled = true;
		member2.GetComponent<BoxCollider2D> ().enabled = true;
		member2.GetComponent<Hero> ().enabled = true;

		if (team == 1) {
			member2.GetComponent<Hero> ().setTeam (team);
			member2.transform.position = new Vector3 (0, 2, 0);
		} else {
			member2.GetComponent<Hero> ().setTeam (team);	
			member2.transform.position = new Vector3 (13, 8, 0);
		}
	}
		
	public void CheckIfGameOver() {
		//Check if hp point total is less than or equal to zero.
		if (hcaptain.getHP() <= 0) {
			GameManager.instance.Reset ();

			//Stop the background music.
			/*
			SoundManager.instance.musicSource.Stop ();

			//Call the GameOver function of GameManager.
			GameManager.instance.GameOver ();

			this.gameObject.SetActive (false);
*/
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

	private void TriggerInfo() {
		if (GameManager.instance.turn == tag) {
			if (turn == captain.tag || turn == member1.tag || turn == member2.tag) {
				infoText.text = hcaptain.Info () + '\n' + hmember1.Info () + '\n' + hmember2.Info ();
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
			infoButton.interactable = false;

			etman.attackButton.interactable = true;
			etman.endTurnButton.interactable = true;
			etman.skill1Button.interactable = true;
			etman.skill2Button.interactable = true;
			etman.ultButton.interactable = true;
			etman.infoButton.interactable = true;

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

	public void ResetGame() {
		captain.GetComponent<SpriteRenderer> ().enabled = false;
		captain.GetComponent<BoxCollider2D> ().enabled = false;
		captain.GetComponent<Hero> ().ResetHero ();
		captain.GetComponent<Hero> ().enabled = false; 
		captain = null;

		member1.GetComponent<SpriteRenderer> ().enabled = false;
		member1.GetComponent<BoxCollider2D> ().enabled = false;
		member1.GetComponent<Hero> ().ResetHero ();
		member1.GetComponent<Hero> ().enabled = false; 
		member1 = null;

		member2.GetComponent<SpriteRenderer> ().enabled = false;
		member2.GetComponent<BoxCollider2D> ().enabled = false;
		member2.GetComponent<Hero> ().ResetHero ();
		member2.GetComponent<Hero> ().enabled = false; 
		member2 = null;

		start = false;
	}
}

