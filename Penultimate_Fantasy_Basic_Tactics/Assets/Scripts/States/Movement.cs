using UnityEngine;
using System.Collections;

public class Movement : State
{
    GameController gameCont = GameController.instance;
    TileController tileCont = TileController.instance;
    StateController stateCont = StateController.instance;
    UIController uiCont = UIController.instance;

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
        gameCont.activeCharacterScript.ToggleMovementRange();
    }

    //Hide the unit's movement range
    public override void OnStateExit()
    {
        gameCont.activeCharacterScript.ToggleMovementRange();
    }

    private void MoveTileSelector(int xDelta, int zDelta)
    {
        //If the tile the user is trying to move to exists (is not out of the array bounds)
        if ((tileCont.hoveredXIndex + xDelta >= 0 && tileCont.hoveredXIndex + xDelta < tileCont.GRID_DIMENSION) && 
		    (tileCont.hoveredZIndex + zDelta >= 0 && tileCont.hoveredZIndex + zDelta < tileCont.GRID_DIMENSION))
        {
            //Change the hoveredTile information and move the tile selector
            tileCont.hoveredXIndex += xDelta;
            tileCont.hoveredZIndex += zDelta;
            tileCont.hoveredTile = tileCont.levelMap[tileCont.hoveredXIndex, tileCont.hoveredZIndex];
            tileCont.tileSelector.transform.position = tileCont.levelMap[tileCont.hoveredXIndex, tileCont.hoveredZIndex]
			.GetComponent<TileInformation>().highlightLocation;

            uiCont.SetNewHoveredUnit();
        }
        else
        {
            return;
        }
    }

    private void SelectTile()
    {
        //If the hoveredTile does not have a character on it
        if (!tileCont.hoveredTile.GetComponent<TileInformation>().isOccupied || tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit == gameCont.activeCharacter)
        {
            //If the tile is in range
            if (gameCont.activeCharacterScript.CheckRangeForObject(tileCont.hoveredTile, gameCont.activeCharacterScript.unitClass.GetMoveDistance()))
            {
                OnStateExit();

                //Move to the tile
                GameObject oldTile = tileCont.levelMap[gameCont.activeCharacterScript.xPosition, gameCont.activeCharacterScript.zPosition];

				gameCont.activeCharacterScript.UpdatePosition(tileCont.hoveredXIndex, tileCont.hoveredZIndex, tileCont.hoveredTile);

                oldTile.GetComponent<TileInformation>().UpdateInformation(null);
                tileCont.hoveredTile.GetComponent<TileInformation>().UpdateInformation(gameCont.activeCharacter);

                stateCont.stateList.Push(new MenuSelection(oldTile, tileCont.hoveredTile));
                stateCont.currentState = (State) stateCont.stateList.Peek();
            }
        }

        return;
    }

    //Revert to the unit selection phase
    private void RevertState()
    {
        OnStateExit();

        gameCont.activeCharacter = null;
		gameCont.activeCharacterScript = null;

        stateCont.stateList.Pop();
        stateCont.currentState = (State) stateCont.stateList.Peek();
    }
}
