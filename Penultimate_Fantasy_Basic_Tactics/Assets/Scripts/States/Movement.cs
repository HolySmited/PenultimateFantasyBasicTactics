using UnityEngine;
using System.Collections;

public class Movement : State
{
    public Movement()
    {
        OnStateEnter();
    }

    public override void ExecuteCommand(KeyCode inputKey)
    {
        //Call the appropriate function based on the key pressed; if no supported key is pressed, return
        switch (inputKey)
        {
            case KeyCode.LeftArrow:
                MoveTileSelector(-1, 0);
                break;
            case KeyCode.RightArrow:
                MoveTileSelector(1, 0);
                break;
            case KeyCode.DownArrow:
                MoveTileSelector(0, -1);
                break;
            case KeyCode.UpArrow:
                MoveTileSelector(0, 1);
                break;
            case KeyCode.Space:
                SelectTile();
                break;
            case KeyCode.Backspace:
                RevertState();
                break;
            default:
                return;
        }
    }

    //Show the unit's movement range
    public override void OnStateEnter()
    {
        GameController.activeCharacter.GetComponent<UnitBehavior>().ToggleMovementRange();
    }

    //Hide the unit's movement range
    public override void OnStateExit()
    {
        GameController.activeCharacter.GetComponent<UnitBehavior>().ToggleMovementRange();
    }

    private void MoveTileSelector(int xDelta, int zDelta)
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

    private void SelectTile()
    {
        //If the hoveredTile does not have a character on it
        if (!TileController.hoveredTile.GetComponent<TileInformation>().isOccupied)
        {
            //If the tile is in range
            if (GameController.activeCharacter.GetComponent<UnitBehavior>().CheckRangeForTile(TileController.hoveredTile))
            {
                OnStateExit();

                //Move to the tile
                Vector3 oldPosition = GameController.activeCharacter.transform.position;
                GameObject oldTile = GameController.activeCharacter.transform.parent.gameObject;

                GameController.activeCharacter.transform.parent = null;
                GameController.activeCharacter.transform.position = TileController.hoveredTile.GetComponent<TileInformation>().unitLocation;
                GameController.activeCharacter.transform.parent = TileController.hoveredTile.transform;

                oldTile.GetComponent<TileInformation>().UpdateInformation(null);
                TileController.hoveredTile.GetComponent<TileInformation>().UpdateInformation(GameController.activeCharacter);

                StateController.stateList.Push(new MenuSelection(oldPosition, oldTile));
                StateController.currentState = (State) StateController.stateList.Peek();
            }
        }

        return;
    }

    //Revert to the unit selection phase
    private void RevertState()
    {
        OnStateExit();

        GameController.activeCharacter = null;

        StateController.stateList.Pop();
        StateController.currentState = (State) StateController.stateList.Peek();
    }
}
