using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Controlls information regarding the game in general
public class GameController : MonoBehaviour
{
	public static GameController gameCont;

    public GameObject activeCharacter; //The current selected character
	public UnitBehavior activeCharacterScript;
	public LinkedList<GameObject> blueTeam;
	public LinkedList<GameObject> redTeam;
	public bool isBlueTurn = true;

	public Material blueTeamColor;
	public Material redTeamColor;
	public Material blueUsed;
	public Material redUsed;

	void Awake()
	{
		blueTeam = new LinkedList<GameObject> ();
		redTeam = new LinkedList<GameObject> ();

		if (gameCont == null) {
			gameCont = this;
		} 
		else {
			Destroy(this);
		}

		GameObject[] blueTeamArr = GameObject.FindGameObjectsWithTag ("BlueTeam");
		GameObject[] redTeamArr = GameObject.FindGameObjectsWithTag ("RedTeam");

		foreach (GameObject obj in blueTeamArr) {
			blueTeam.AddLast (obj);
		}

		foreach (GameObject obj in redTeamArr) {
			redTeam.AddLast (obj);
		}
	}

    //Sets the active character with the provided GameObject
    public void SetActiveCharacter(GameObject character)
    {
        activeCharacter = character;
		activeCharacterScript = activeCharacter.GetComponent<UnitBehavior> ();
    }

	public void UpdateUnits() {
		activeCharacterScript.hasUsed = true;

		if (activeCharacterScript.blueTeam) {
			activeCharacter.GetComponent<MeshRenderer> ().material = blueUsed;
		} 
		else {
			activeCharacter.GetComponent<MeshRenderer> ().material = redUsed;
		}

		if (isBlueTurn) {
			foreach (GameObject unit in blueTeam) {
				if (unit.GetComponent<UnitBehavior> ().hasUsed == false) {
					return;
				}
			}

			isBlueTurn = false;
			UIController.uiCont.SwapTurn(isBlueTurn);

			foreach (GameObject unit in blueTeam) {
				unit.GetComponent<MeshRenderer>().material = blueTeamColor;
				unit.GetComponent<UnitBehavior>().hasUsed = false;
			}
		} 
		else {
			foreach (GameObject unit in redTeam) {
				if (unit.GetComponent<UnitBehavior> ().hasUsed == false) {
					return;
				}
			}

			isBlueTurn = true;
			UIController.uiCont.SwapTurn(isBlueTurn);

			foreach (GameObject unit in redTeam) {
				unit.GetComponent<MeshRenderer>().material = redTeamColor;
				unit.GetComponent<UnitBehavior>().hasUsed = false;
			}
		}

		CheckEndGame ();
	}

	private void CheckEndGame() {
		if (redTeam.Count == 0) {
			UIController.uiCont.EndGame(true);
		}
		else if (blueTeam.Count == 0) {
			UIController.uiCont.EndGame(false);
		}
	}
}