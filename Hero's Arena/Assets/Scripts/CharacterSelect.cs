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
			Select ("EverGreen");
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
			current++;
			lastClicked.GetComponentInChildren<Text>().text = "Team 1 Captain";
			tm.GetComponent<TeamManager> ().SetCaptain(GameObject.FindGameObjectWithTag(tag).GetComponent<Hero>());

			break;
		case 1: //2.1
			current++;
			lastClicked.GetComponentInChildren<Text>().text = "Team 2 Captain";
			tm2.GetComponent<TeamManager> ().SetCaptain(GameObject.FindGameObjectWithTag(tag).GetComponent<Hero>());
			break;
		case 2: //1.2
			current++;
			lastClicked.GetComponentInChildren<Text>().text = "Team 1 Member 1";
			tm.GetComponent<TeamManager> ().SetMember1(GameObject.FindGameObjectWithTag(tag).GetComponent<Hero>());
			break;
		case 3: //2.2
			current++;
			lastClicked.GetComponentInChildren<Text>().text = "Team 2 Member 1";
			tm2.GetComponent<TeamManager> ().SetMember1(GameObject.FindGameObjectWithTag(tag).GetComponent<Hero>());
			break;
		case 4: //1.3
			current++;
			lastClicked.GetComponentInChildren<Text>().text = "Team 1 Member 2";
			tm.GetComponent<TeamManager> ().SetMember2(GameObject.FindGameObjectWithTag(tag).GetComponent<Hero>());
			break;
		case 5:
			lastClicked.GetComponentInChildren<Text>().text = "Team 2 Member 2";
			tm2.GetComponent<TeamManager> ().SetMember2 (GameObject.FindGameObjectWithTag(tag).GetComponent<Hero>());
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
