using UnityEngine;
using System.Collections;

public class MenuSelection : State
{
    UIController uiCont = UIController.instance;
    StateController stateCont = StateController.instance;
    GameController gameCont = GameController.instance;

    private int choice = 0; //Indicates the index of the current choice in the menu
	private GameObject[] menuChoices;

    //Stores active character position before movement
    private GameObject oldTile;
	private GameObject newTile;

    //Initialize
    public MenuSelection(GameObject oldTile, GameObject newTile)
    {
        this.oldTile = oldTile;
		this.newTile = newTile;

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
        if (gameCont.activeCharacterScript.unitClass.GetType() == typeof(Mage))
        {
            uiCont.magicMenu.SetActive(true);
            menuChoices = uiCont.magicMenuOptions;
        }
        else
        {
            uiCont.attackMenu.SetActive(true);
            menuChoices = uiCont.attackMenuOptions;
        }

        uiCont.selectionArrow.SetActive(true);
        uiCont.selectionArrow.transform.position = new Vector3(uiCont.selectionArrow.transform.position.x, menuChoices[choice].transform.position.y, uiCont.selectionArrow.transform.position.z);
    }

    //Hide the attack menu
    public override void OnStateExit()
    {
        uiCont.attackMenu.SetActive(false);
        uiCont.magicMenu.SetActive(false);
        uiCont.selectionArrow.SetActive(false);
    }

    //Move the arrow up and down on the menu
    private void MoveMenuSelector(int moveDelta)
    {
        choice += moveDelta;

        if (choice >= 0 && choice < menuChoices.Length)
        {
			uiCont.selectionArrow.transform.position = new Vector3(uiCont.selectionArrow.transform.position.x, menuChoices[choice].transform.position.y, uiCont.selectionArrow.transform.position.z);
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

        switch (menuChoices[choiceIndex].name)
        {
            //Move to attack phase
            case "AttackOption":
                stateCont.stateList.Push(new Attack(false));
                stateCont.currentState = (State)stateCont.stateList.Peek();
                break;
            case "MagicOption":
                stateCont.stateList.Push(new Attack(true));
                stateCont.currentState = (State)stateCont.stateList.Peek();
                break;
            //Start a new turn
            case "EndTurnOption":
                stateCont.stateList.Clear();
                stateCont.stateList.Push(new UnitSelection());
                stateCont.currentState = (State)stateCont.stateList.Peek();

				gameCont.UpdateUnits ();
				gameCont.activeCharacter = null;
				gameCont.activeCharacterScript = null;

                break;
        }
    }

    //Revert to the movement phase and reset the unit's position
    private void RevertState()
    {
        OnStateExit();

        newTile.GetComponent<TileInformation>().UpdateInformation(null);
        MoveToTile();

        stateCont.stateList.Pop();
        stateCont.currentState = (State)stateCont.stateList.Peek();

        stateCont.currentState.OnStateEnter();
    }

    //Reset the unit's position
    private void MoveToTile()
    {
		gameCont.activeCharacterScript.UpdatePosition (oldTile.GetComponent<TileInformation> ().xIndex, oldTile.GetComponent<TileInformation> ().zIndex, oldTile);

        oldTile.GetComponent<TileInformation>().UpdateInformation(gameCont.activeCharacter);
    }
}
