using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Controlls information regarding the UI
public class UIController : MonoBehaviour
{
	public static UIController instance;
	
    public GameObject attackMenu; //Menu for selecting attack
    public GameObject magicMenu; //Menu for mages
    public GameObject selectionArrow; //Arrow to indicate selection

    public Text unitStatus;

	public Text turnIndicator;
	public Text gameWin;
	
	public GameObject[] attackMenuOptions;
    public GameObject[] magicMenuOptions;

    private BaseClass hoveredUnit = null;

	void Awake()
	{
		if (instance == null) {
            instance = this;
		} 
		else {
			Destroy (this);
		}
	}

    void Start()
    {
        //Initialize
        attackMenu = GameObject.Find("BasicAttackMenu");
        magicMenu = GameObject.Find("BasicMagicMenu");
        selectionArrow = GameObject.Find("SelectionArrow");

		turnIndicator.text = "Blue Turn";

        attackMenu.SetActive(false); //Hide the menu
        magicMenu.SetActive(false);
        selectionArrow.SetActive(false);
    }

    private void UpdateUnitStatus()
    {
        if (hoveredUnit != null)
            unitStatus.text = "Health: " + hoveredUnit.currentHealth + "/" + hoveredUnit.maxHealth + "\n" + "Mana: " + hoveredUnit.currentMana + "/" + hoveredUnit.maxMana;
        else
            unitStatus.text = "";
    }

    public void SetNewHoveredUnit()
    {
        if (TileController.instance.hoveredTile.GetComponent<TileInformation>().occupyingUnit != null)
            hoveredUnit = TileController.instance.hoveredTile.GetComponent<TileInformation>().occupyingUnit.GetComponent<BaseClass>();
        else
            hoveredUnit = null;

        UpdateUnitStatus();
    }

	public GameObject[] GetAttackMenuOptions()
	{
		return attackMenuOptions;
	}

    public GameObject[] GetMagicMenuOptions()
    {
        return magicMenuOptions;
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
		Destroy (GameController.instance);
		Destroy (TileController.instance);
		StateController.instance.stateList.Clear ();
		Destroy (StateController.instance);
        Destroy(instance);
	}
}
