using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterSelect : MonoBehaviour
{
	public Text info;

	private int current = 0;
	private Button lastClicked;
	private string tag;

	public GameObject t1capAnim;
	public GameObject t1m1Anim;
	public GameObject t1m2Anim;
	public GameObject t2capAnim;
	public GameObject t2m1Anim;
	public GameObject t2m2Anim;

	/*
	 * current starts from characters1.1
	 * -> characters2.1
	 * 1.2
	 * 2.2
	 * 1.3
	 * 2.3
	 */

	// buttons
	public Button nextButton;
	public Button readyButton;
	public Button selectPendragon;
	public Button selectIronRebel;
	public Button selectSongbird;
	public Button selectFerocity;
	public Button selectJackFrost;
	public Button selectMedico;
	public Button selectEverGreen;
	public Button selectRadioFreak;
	public Button selectDeadEye;
	public Button selectOxenfree;
	public Button selectPanther;

	/*
	 * 0 = pendragon
	 * 1 = iron rebel
	 * 2 = songbird
	 * 
	 */

	void Start ()
	{
		nextButton.onClick.AddListener (NextCharacter);
		selectPendragon.onClick.AddListener (delegate {
			Select ("Pendragon");
		});
		selectIronRebel.onClick.AddListener (delegate {
			Select ("IronRebel");
		});
		selectSongbird.onClick.AddListener (delegate {
			Select ("Songbird");
		});
		selectFerocity.onClick.AddListener (delegate {
			Select ("Ferocity");
		});
		selectJackFrost.onClick.AddListener (delegate {
			Select ("JackFrost");
		});
		selectMedico.onClick.AddListener (delegate {
			Select ("Medico");
		});
		selectEverGreen.onClick.AddListener (delegate {
			Select ("Evergreen");
		});
		selectRadioFreak.onClick.AddListener (delegate {
			Select ("RadioFreak");
		});
		selectDeadEye.onClick.AddListener (delegate {
			Select ("DeadEye");
		});
		selectOxenfree.onClick.AddListener (delegate {
			Select ("Oxenfree");
		});
		selectPanther.onClick.AddListener (delegate {
			Select ("Panther");
		});

	}

	public void Select (string index)
	{

		tag = index; 
		info.text = GameObject.FindGameObjectWithTag (tag).GetComponent<Hero> ().Info ();

		switch (tag) {
		case "Pendragon": 
			lastClicked = selectPendragon;
			break;
		case "IronRebel":
			lastClicked = selectIronRebel;
			break;
		case "Songbird":
			lastClicked = selectSongbird;
			break;
		case "Ferocity":
			lastClicked = selectFerocity;
			break;
		case "JackFrost": 
			lastClicked = selectJackFrost;
			break;
		case "Medico":
			lastClicked = selectMedico;
			break;
		case "Evergreen": 
			lastClicked = selectEverGreen;
			break;
		case "RadioFreak": 
			lastClicked = selectRadioFreak;
			break;
		case "DeadEye": 
			lastClicked = selectDeadEye;
			break;
		case "Oxenfree":
			lastClicked = selectOxenfree;
			break;
		case "Panther":
			lastClicked = selectPanther;
			break;
		default:
			return;
		}
	}

	public void NextCharacter ()
	{
		GameObject tm = GameObject.FindGameObjectWithTag ("Team1");
		GameObject tm2 = GameObject.FindGameObjectWithTag ("Team2");

		switch (current) {
		case 0: //1.1
			tm.GetComponent<TeamManager> ().SetCaptain (GameObject.FindGameObjectWithTag (tag), 1);

			Animator anim = t1capAnim.GetComponent<Animator> ();
			SpriteRenderer sprite = t1capAnim.GetComponent<SpriteRenderer> ();
			anim.runtimeAnimatorController = GameObject.FindGameObjectWithTag (tag).GetComponent<Animator> ().runtimeAnimatorController;
			sprite = GameObject.FindGameObjectWithTag (tag).GetComponent<SpriteRenderer> ();

			lastClicked.interactable = false;
			current++;

			break;
		case 1: //2.1
			tm2.GetComponent<TeamManager> ().SetCaptain (GameObject.FindGameObjectWithTag (tag), 2);

			Animator anim0 = t2capAnim.GetComponent<Animator> ();
			SpriteRenderer sprite0 = t2capAnim.GetComponent<SpriteRenderer> ();
			anim0.runtimeAnimatorController = GameObject.FindGameObjectWithTag (tag).GetComponent<Animator> ().runtimeAnimatorController;
			sprite0 = GameObject.FindGameObjectWithTag (tag).GetComponent<SpriteRenderer> ();

			lastClicked.interactable = false;
			current++;

			break;
		case 2: //1.2			
			tm.GetComponent<TeamManager> ().SetMember1 (GameObject.FindGameObjectWithTag (tag), 1);

			Animator anim1 = t1m1Anim.GetComponent<Animator> ();
			SpriteRenderer sprite1 = t1m1Anim.GetComponent<SpriteRenderer> ();
			anim1.runtimeAnimatorController = GameObject.FindGameObjectWithTag (tag).GetComponent<Animator> ().runtimeAnimatorController;
			sprite1 = GameObject.FindGameObjectWithTag (tag).GetComponent<SpriteRenderer> ();

			lastClicked.interactable = false;
			current++;

			break;
		case 3: //2.2
			tm2.GetComponent<TeamManager> ().SetMember1 (GameObject.FindGameObjectWithTag (tag), 2);

			Animator anim2 = t2m1Anim.GetComponent<Animator> ();
			SpriteRenderer sprite2 = t2m1Anim.GetComponent<SpriteRenderer> ();
			anim2.runtimeAnimatorController = GameObject.FindGameObjectWithTag (tag).GetComponent<Animator> ().runtimeAnimatorController;
			sprite2 = GameObject.FindGameObjectWithTag (tag).GetComponent<SpriteRenderer> ();

			lastClicked.interactable = false;
			current++;

			break;
		case 4: //1.3
			tm.GetComponent<TeamManager> ().SetMember2 (GameObject.FindGameObjectWithTag (tag), 1);

			Animator anim3 = t1m2Anim.GetComponent<Animator> ();
			SpriteRenderer sprite3 = t1m2Anim.GetComponent<SpriteRenderer> ();
			anim3.runtimeAnimatorController = GameObject.FindGameObjectWithTag (tag).GetComponent<Animator> ().runtimeAnimatorController;
			sprite3 = GameObject.FindGameObjectWithTag (tag).GetComponent<SpriteRenderer> ();

			lastClicked.interactable = false;
			current++;

			break;
		case 5:
			tm2.GetComponent<TeamManager> ().SetMember2 (GameObject.FindGameObjectWithTag (tag), 2);

			Animator anim4 = t2m2Anim.GetComponent<Animator> ();
			SpriteRenderer sprite4 = t2m2Anim.GetComponent<SpriteRenderer> ();
			anim4.runtimeAnimatorController = GameObject.FindGameObjectWithTag (tag).GetComponent<Animator> ().runtimeAnimatorController;
			sprite4 = GameObject.FindGameObjectWithTag (tag).GetComponent<SpriteRenderer> ();

			lastClicked.interactable = false;
			current++;

			break;
		default:
			return;
		}
	}

	public void ResetScene() {
		GameObject tm = GameObject.FindGameObjectWithTag ("Team1");
		GameObject tm2 = GameObject.FindGameObjectWithTag ("Team2");

		current = 0;

		selectPendragon.interactable = true;
		selectIronRebel.interactable = true;
		selectSongbird.interactable = true;
		selectFerocity.interactable = true;
		selectJackFrost.interactable = true;
		selectMedico.interactable = true;
		selectEverGreen.interactable = true;
		selectRadioFreak.interactable = true;
		selectDeadEye.interactable = true;
		selectOxenfree.interactable = true;
		selectPanther.interactable = true;

		tm.GetComponent<TeamManager> ().ResetGame ();
		tm2.GetComponent<TeamManager> ().ResetGame ();

		//reset character anims 
		t1capAnim.GetComponent<Animator>().runtimeAnimatorController = null;
		t1capAnim.GetComponent<SpriteRenderer> ().sprite = null;

		t1m1Anim.GetComponent<Animator>().runtimeAnimatorController = null;
		t1m1Anim.GetComponent<SpriteRenderer> ().sprite = null;

		t1m2Anim.GetComponent<Animator>().runtimeAnimatorController = null;
		t1m2Anim.GetComponent<SpriteRenderer> ().sprite = null;

		t2capAnim.GetComponent<Animator>().runtimeAnimatorController = null;
		t2capAnim.GetComponent<SpriteRenderer> ().sprite = null;

		t2m1Anim.GetComponent<Animator>().runtimeAnimatorController = null;
		t2m1Anim.GetComponent<SpriteRenderer> ().sprite = null;

		t2m2Anim.GetComponent<Animator>().runtimeAnimatorController = null;
		t2m2Anim.GetComponent<SpriteRenderer> ().sprite = null;
	}
}
