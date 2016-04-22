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
        GameController.activeCharacter.GetComponent<UnitBehavior>().ToggleAttackRange();        
    }

    //Hide the attack range of the active character
    public override void OnStateExit()
    {
        GameController.activeCharacter.GetComponent<UnitBehavior>().ToggleAttackRange();
    }

    //Move the selector
    private void MoveUnitSelector(int xDelta, int zDelta)
    {
        //If the tile the user is trying to move to exists (is not out of the array bounds)
        if ((TileController.hoveredXIndex + xDelta >= 0 && TileController.hoveredXIndex + xDelta < TileController.GRID_DIMENSION) && (TileController.hoveredZIndex + zDelta >= 0 && TileController.hoveredZIndex + zDelta < TileController.GRID_DIMENSION))
        {
            //Change the hoveredTile information and move the tile selector
            TileController.hoveredXIndex += xDelta;
            TileController.hoveredZIndex += zDelta;
            TileController.hoveredTile = TileController.levelMap[TileController.hoveredXIndex, TileController.hoveredZIndex];
            TileController.tileSelector.transform.position = TileController.levelMap[TileController.hoveredXIndex, TileController.hoveredZIndex].GetComponent<TileInformation>().highlightLocation;
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
        if (TileController.hoveredTile.GetComponent<TileInformation>().isOccupied)
        {
            //If the character is in range
            if (GameController.activeCharacter.GetComponent<UnitBehavior>().CheckRangeForEnemy(TileController.hoveredTile.GetComponent<TileInformation>().occupyingUnit))
            {
                Debug.Log("Attack complete"); //Attack it

                //Start a new turn
                StateController.stateList.Clear();
                StateController.stateList.Push(new UnitSelection());
                StateController.currentState = (State)StateController.stateList.Peek();

                OnStateExit();

                GameController.activeCharacter = null;
            }
        }

        return;
    }

    //Revert to the previous state
    private void RevertState()
    {
        OnStateExit();

        StateController.stateList.Pop();
        StateController.currentState = (State)StateController.stateList.Peek();

        StateController.currentState.OnStateEnter();
    }
}
