using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Controlls information regarding the UI
public class UIController : MonoBehaviour
{
	public static UIController uiCont;
	
    public GameObject attackMenu; //Menu for selecting attack
    public GameObject selectionArrow; //Arrow to indicate selection

	public Text turnIndicator;
	public Text gameWin;
	
	public GameObject[] menuOptions;

    private Vector3 originalArrowPos; //Original position of the arow

	void Awake()
	{
		if (uiCont == null) {
			uiCont = this;
		} 
		else {
			Destroy (this);
		}
	}

    void Start()
    {
        //Initialize
        attackMenu = GameObject.Find("BasicAttackMenu");
        selectionArrow = GameObject.Find("SelectionArrow");

        originalArrowPos = selectionArrow.transform.position;
		turnIndicator.text = "Blue Turn";

        attackMenu.SetActive(false); //Hide the menu
    }

    //Move the arrow back to it's original position
    public void ResetSelectionArrow()
    {
        selectionArrow.transform.position = originalArrowPos;
    }

	public GameObject[] GetMenuOptions()
	{
		return menuOptions;
	}

	public void SwapTurn(bool isBlueTurn) {
		if (isBlueTurn) {
			turnIndicator.text = "Blue Turn";
		} 
		else {
			turnIndicator.text = "Red Turn";
		}
	}

	public void EndGame(bool isBlueWin) {
		if (isBlueWin) {
			gameWin.color = Color.blue;
			gameWin.text = "Blue Team Wins!";
		} 
		else {
			gameWin.color = Color.red;
			gameWin.text = "Red Team Wins!";
		}

		Destroy (turnIndicator);
		Destroy (GameController.gameCont);
		Destroy (TileController.tileCont);
		StateController.stateCont.stateList.Clear ();
		Destroy (StateController.stateCont);
	}
}
