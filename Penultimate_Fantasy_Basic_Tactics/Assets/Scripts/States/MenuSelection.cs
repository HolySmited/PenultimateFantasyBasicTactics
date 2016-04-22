using UnityEngine;
using System.Collections;

public class MenuSelection : State
{
	private UIController UICont;
    private int choice = 0; //Indicates the index of the current choice in the menu
    private const int MAX_CHOICES = 2; //Maximum number of menu choices
	private GameObject[] menuChoices;

    //Stores active character position before movement
    private Vector3 oldPosition;
    private GameObject oldTile;
	private GameObject newTile;

    //Initialize
    public MenuSelection(Vector3 oldPos, GameObject oldTile, GameObject newTile)
    {
        this.oldPosition = oldPos;
        this.oldTile = oldTile;
		this.newTile = newTile;
		this.UICont = GameObject.Find ("Controller").GetComponent<UIController> ();
		this.menuChoices = UICont.GetMenuOptions ();

        OnStateEnter();
    }

    public override void ExecuteCommand(KeyCode inputKey)
    {
        //Call the appropriate function based on the key pressed; if no supported key is pressed, return
        switch (inputKey)
        {
            case KeyCode.DownArrow:
                MoveMenuSelector(1);
                break;
            case KeyCode.UpArrow:
                MoveMenuSelector(-1);
                break;
            case KeyCode.Space:
                SelectChoice(choice);
                break;
            case KeyCode.Backspace:
                RevertState();
                break;
            default:
                return;
        }
    }

    //Show the attack menu and make sure the arrow is in it's original position
    public override void OnStateEnter()
    {
        UIController.uiCont.attackMenu.SetActive(true);
        UIController.uiCont.ResetSelectionArrow();
    }

    //Hide the attack menu
    public override void OnStateExit()
    {
        UIController.uiCont.attackMenu.SetActive(false);
    }

    //Move the arrow up and down on the menu
    private void MoveMenuSelector(int moveDelta)
    {
        choice += moveDelta;

        if (choice >= 0 && choice < MAX_CHOICES)
        {
			UIController.uiCont.selectionArrow.transform.position = new Vector3(UIController.uiCont.selectionArrow.transform.position.x, 
			                                                             menuChoices[choice].transform.position.y, UIController.uiCont.selectionArrow.transform.position.z);
        }
        else
        {
            choice -= moveDelta;
        }
    }

    //Select the choice the arrow is pointing to
    private void SelectChoice(int choiceIndex)
    {
        OnStateExit();

        switch (choiceIndex)
        {
            //Move to attack phase
            case 0:
                StateController.stateCont.stateList.Push(new Attack());
                StateController.stateCont.currentState = (State)StateController.stateCont.stateList.Peek();
                break;
            //Start a new turn
            case 1:
                StateController.stateCont.stateList.Clear();
                StateController.stateCont.stateList.Push(new UnitSelection());
                StateController.stateCont.currentState = (State)StateController.stateCont.stateList.Peek();

				GameController.gameCont.UpdateUnits ();
				GameController.gameCont.activeCharacter = null;
				GameController.gameCont.activeCharacterScript = null;

                break;
        }
    }

    //Revert to the movement phase and reset the unit's position
    private void RevertState()
    {
        OnStateExit();

        newTile.GetComponent<TileInformation>().UpdateInformation(null);
        MoveToTile();

        StateController.stateCont.stateList.Pop();
        StateController.stateCont.currentState = (State)StateController.stateCont.stateList.Peek();

        StateController.stateCont.currentState.OnStateEnter();
    }

    //Reset the unit's position
    private void MoveToTile()
    {
		GameController.gameCont.activeCharacterScript.UpdatePosition (oldTile.GetComponent<TileInformation> ().xIndex, oldTile.GetComponent<TileInformation> ().zIndex, oldTile);

        oldTile.GetComponent<TileInformation>().UpdateInformation(GameController.gameCont.activeCharacter);
    }
}
