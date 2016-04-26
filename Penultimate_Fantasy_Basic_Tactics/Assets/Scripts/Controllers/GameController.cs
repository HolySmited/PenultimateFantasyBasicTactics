using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Controlls information regarding the game in general
public class GameController : MonoBehaviour
{
	public static GameController instance;

    public GameObject activeCharacter; //The current selected character
	public UnitBehavior activeCharacterScript;
	public LinkedList<GameObject> blueTeam;
	public LinkedList<GameObject> redTeam;
	public bool isBlueTurn = true;

	public Material blueTeamColor;
	public Material redTeamColor;
	public Material blueUsed;
	public Material redUsed;

    public LinkedList<GameObject> allUnits;

	void Awake()
	{
		if (instance == null) {
            instance = this;
		} 
		else {
			Destroy(this);
		}

        blueTeam = new LinkedList<GameObject>();
        redTeam = new LinkedList<GameObject>();

        GameObject[] blueTeamArr = GameObject.FindGameObjectsWithTag ("BlueTeam");
		GameObject[] redTeamArr = GameObject.FindGameObjectsWithTag ("RedTeam");

		foreach (GameObject obj in blueTeamArr) {
			blueTeam.AddLast (obj);
		}

		foreach (GameObject obj in redTeamArr) {
			redTeam.AddLast (obj);
		}

        allUnits = new LinkedList<GameObject>(blueTeam);

        foreach (GameObject unit in redTeam)
        {
            allUnits.AddLast(unit);
        }
    }

    void Start()
    {
        TileController.instance.Initialize();

        foreach (GameObject tile in TileController.instance.levelMap)
        {
            tile.GetComponent<TileInformation>().Initialize();
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
			UIController.instance.SwapTurn(isBlueTurn);

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
			UIController.instance.SwapTurn(isBlueTurn);

			foreach (GameObject unit in redTeam) {
				unit.GetComponent<MeshRenderer>().material = redTeamColor;
				unit.GetComponent<UnitBehavior>().hasUsed = false;
			}
		}
	}

	public void CheckEndGame() {
		if (redTeam.Count == 0) {
            UIController.instance.EndGame(true);
		}
		else if (blueTeam.Count == 0) {
            UIController.instance.EndGame(false);
		}
	}
}