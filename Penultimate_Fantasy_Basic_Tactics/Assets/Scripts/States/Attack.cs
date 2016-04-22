using UnityEngine;
using System.Collections;

//State for when attack has been selected from the action menu
public class Attack : State
{
    public Attack()
    {
        OnStateEnter();
    }

    public override void ExecuteCommand(KeyCode inputKey)
    {
        //Call the appropriate function based on the key pressed; if no supported key is pressed, return
        switch (inputKey)
        {
            case KeyCode.LeftArrow:
                MoveUnitSelector(-1, 0);
                break;
            case KeyCode.RightArrow:
                MoveUnitSelector(1, 0);
                break;
            case KeyCode.DownArrow:
                MoveUnitSelector(0, -1);
                break;
            case KeyCode.UpArrow:
                MoveUnitSelector(0, 1);
                break;
            case KeyCode.Space:
                SelectUnit();
                break;
            case KeyCode.Backspace:
                RevertState();
                break;
            default:
                return;
        }
    }

    //Show the attack range of the active character
    public override void OnStateEnter()
    {
        GameController.gameCont.activeCharacterScript.ToggleAttackRange();        
    }

    //Hide the attack range of the active character
    public override void OnStateExit()
    {
        GameController.gameCont.activeCharacterScript.ToggleAttackRange();
    }

    //Move the selector
    private void MoveUnitSelector(int xDelta, int zDelta)
    {
        //If the tile the user is trying to move to exists (is not out of the array bounds)
        if ((TileController.tileCont.hoveredXIndex + xDelta >= 0 && TileController.tileCont.hoveredXIndex + xDelta < TileController.tileCont.GRID_DIMENSION) && 
		    (TileController.tileCont.hoveredZIndex + zDelta >= 0 && TileController.tileCont.hoveredZIndex + zDelta < TileController.tileCont.GRID_DIMENSION))
        {
            //Change the hoveredTile information and move the tile selector
            TileController.tileCont.hoveredXIndex += xDelta;
            TileController.tileCont.hoveredZIndex += zDelta;
            TileController.tileCont.hoveredTile = TileController.tileCont.levelMap[TileController.tileCont.hoveredXIndex, TileController.tileCont.hoveredZIndex];
            TileController.tileCont.tileSelector.transform.position = TileController.tileCont.levelMap[TileController.tileCont.hoveredXIndex, TileController.tileCont.hoveredZIndex]
			.GetComponent<TileInformation>().highlightLocation;
        }
        else
        {
            return;
        }
    }

    //Select the unit the selector is hovering over
    private void SelectUnit()
    {
        //If the hoveredTile has a character on it
        if (TileController.tileCont.hoveredTile.GetComponent<TileInformation>().isOccupied)
        {
            //If the character is in range
            if (GameController.gameCont.activeCharacterScript.CheckRangeForEnemy(TileController.tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit))
            {
				if ((GameController.gameCont.isBlueTurn && !TileController.tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit.GetComponent<UnitBehavior>().blueTeam) ||
				    (!GameController.gameCont.isBlueTurn && TileController.tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit.GetComponent<UnitBehavior>().blueTeam)) {
					TileController.tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit.GetComponent<UnitBehavior>()
						.TakeDamage(GameController.gameCont.activeCharacterScript.damage); //Attack it

	                //Start a new turn
	                StateController.stateCont.stateList.Clear();
	                StateController.stateCont.stateList.Push(new UnitSelection());
	                StateController.stateCont.currentState = (State)StateController.stateCont.stateList.Peek();

	                OnStateExit();

					GameController.gameCont.UpdateUnits ();
	                GameController.gameCont.activeCharacter = null;
					GameController.gameCont.activeCharacterScript = null;
				}
            }
        }

        return;
    }

    //Revert to the previous state
    private void RevertState()
    {
        OnStateExit();

        StateController.stateCont.stateList.Pop();
        StateController.stateCont.currentState = (State)StateController.stateCont.stateList.Peek();

        StateController.stateCont.currentState.OnStateEnter();
    }
}
