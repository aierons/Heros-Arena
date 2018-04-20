using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterSelect : MonoBehaviour {

	private List<GameObject> characters;
	private int characterIndex = 0;
	private int current = 0; 
	private Button lastClicked;
	private string tag;
	public Text info;

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

	/*
	 * 0 = pendragon
	 * 1 = iron rebel
	 * 2 = songbird
	 * 
	 */

	private UnityAction select0;
	private UnityAction select1;
	private UnityAction select2;

	/*
	void Awake () {
		characters = new List<GameObject> ();	
		foreach (Transform t in transform) {
			characters.Add (t.gameObject);
			t.gameObject.SetActive (false);
		}
		characters [characterIndex].SetActive (true);

	}
	*/

	void Start () {
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

	}

	public void Select(string index) {
		/*
		if (index == characterIndex || index < 0 || index >= characters.Count) {
			return; 
		}
*/

	//	characters [characterIndex].SetActive (false);
		//characterIndex = index;
		tag = index; 
		info.text = GameObject.FindGameObjectWithTag (tag).GetComponent<Hero> ().Info ();
		//characters [characterIndex].SetActive (true);

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
		case "EverGreen": 
			lastClicked = selectEverGreen;
			break;
		case "RadioFreak": 
			lastClicked = selectRadioFreak;
			break;
		case "DeadEye": 
			lastClicked = selectDeadEye;
			break;
		default:
			return;
		}
	}

	public void NextCharacter() {
		GameObject tm = GameObject.FindGameObjectWithTag ("Team1");
		GameObject tm2 = GameObject.FindGameObjectWithTag ("Team2");

		switch (current) {
		case 0: //1.1
			//lastClicked.GetComponentInChildren<Text> ().text = "Team 1 Captain";
			tm.GetComponent<TeamManager> ().SetCaptain (GameObject.FindGameObjectWithTag (tag), 1);

			Animator anim = t1capAnim.GetComponent<Animator> ();
			SpriteRenderer sprite = t1capAnim.GetComponent<SpriteRenderer> ();
			anim.runtimeAnimatorController = GameObject.FindGameObjectWithTag (tag).GetComponent<Animator> ().runtimeAnimatorController;
			sprite = GameObject.FindGameObjectWithTag (tag).GetComponent<SpriteRenderer> ();

			lastClicked.interactable = false;
			current++;

			break;
		case 1: //2.1
			//lastClicked.GetComponentInChildren<Text>().text = "Team 2 Captain";
			tm2.GetComponent<TeamManager> ().SetCaptain(GameObject.FindGameObjectWithTag(tag),  2);

			Animator anim0 = t2capAnim.GetComponent<Animator> ();
			SpriteRenderer sprite0 = t2capAnim.GetComponent<SpriteRenderer> ();
			anim0.runtimeAnimatorController = GameObject.FindGameObjectWithTag (tag).GetComponent<Animator> ().runtimeAnimatorController;
			sprite0 = GameObject.FindGameObjectWithTag (tag).GetComponent<SpriteRenderer> ();

			lastClicked.interactable = false;
			current++;

			break;
		case 2: //1.2			
			//lastClicked.GetComponentInChildren<Text>().text = "Team 1 Member 1";
			tm.GetComponent<TeamManager> ().SetMember1(GameObject.FindGameObjectWithTag(tag), 1);

			Animator anim1 = t1m1Anim.GetComponent<Animator> ();
			SpriteRenderer sprite1 = t1m1Anim.GetComponent<SpriteRenderer> ();
			anim1.runtimeAnimatorController = GameObject.FindGameObjectWithTag (tag).GetComponent<Animator> ().runtimeAnimatorController;
			sprite1 = GameObject.FindGameObjectWithTag (tag).GetComponent<SpriteRenderer> ();

			lastClicked.interactable = false;
			current++;

			break;
		case 3: //2.2
			//team 2 member 1 is getting set as team 1 member 3 
			//team 1 member 3 is getting set as team 2 member 1 
			//lastClicked.GetComponentInChildren<Text>().text = "Team 2 Member 1";
			tm2.GetComponent<TeamManager> ().SetMember1(GameObject.FindGameObjectWithTag(tag), 2);

			Animator anim2 = t2m1Anim.GetComponent<Animator> ();
			SpriteRenderer sprite2 = t2m1Anim.GetComponent<SpriteRenderer> ();
			anim2.runtimeAnimatorController = GameObject.FindGameObjectWithTag (tag).GetComponent<Animator> ().runtimeAnimatorController;
			sprite2 = GameObject.FindGameObjectWithTag (tag).GetComponent<SpriteRenderer> ();

			lastClicked.interactable = false;
			current++;

			break;
		case 4: //1.3
			//lastClicked.GetComponentInChildren<Text>().text = "Team 1 Member 2";
			tm.GetComponent<TeamManager> ().SetMember2(GameObject.FindGameObjectWithTag(tag), 1);

			Animator anim3 = t1m2Anim.GetComponent<Animator> ();
			SpriteRenderer sprite3 = t1m2Anim.GetComponent<SpriteRenderer> ();
			anim3.runtimeAnimatorController = GameObject.FindGameObjectWithTag (tag).GetComponent<Animator> ().runtimeAnimatorController;
			sprite3 = GameObject.FindGameObjectWithTag (tag).GetComponent<SpriteRenderer> ();

			lastClicked.interactable = false;
			current++;

			break;
		case 5:
			//lastClicked.GetComponentInChildren<Text>().text = "Team 2 Member 2";
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

	/*
	public void PrevCharacter() {
		
	}


	public void Done() {
		gameObject.find ("characters...").GetComponent<CharacterSelect>().characterIndex;
		//record that
	}

	//positions for characters 
	*/
}
