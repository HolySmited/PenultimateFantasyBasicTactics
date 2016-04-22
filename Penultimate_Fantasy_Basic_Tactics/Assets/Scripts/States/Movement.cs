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
        GameController.gameCont.activeCharacterScript.ToggleMovementRange();
    }

    //Hide the unit's movement range
    public override void OnStateExit()
    {
        GameController.gameCont.activeCharacterScript.ToggleMovementRange();
    }

    private void MoveTileSelector(int xDelta, int zDelta)
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

    private void SelectTile()
    {
        //If the hoveredTile does not have a character on it
        if (!TileController.tileCont.hoveredTile.GetComponent<TileInformation>().isOccupied || TileController.tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit == GameController.gameCont.activeCharacter)
        {
            //If the tile is in range
            if (GameController.gameCont.activeCharacterScript.CheckRangeForTile(TileController.tileCont.hoveredTile))
            {
                OnStateExit();

                //Move to the tile
                Vector3 oldPosition = GameController.gameCont.activeCharacter.transform.position;
                GameObject oldTile = TileController.tileCont.levelMap[GameController.gameCont.activeCharacterScript.xPosition, GameController.gameCont.activeCharacterScript.zPosition];

				GameController.gameCont.activeCharacterScript.UpdatePosition(TileController.tileCont.hoveredXIndex, TileController.tileCont.hoveredZIndex, TileController.tileCont.hoveredTile);

                oldTile.GetComponent<TileInformation>().UpdateInformation(null);
                TileController.tileCont.hoveredTile.GetComponent<TileInformation>().UpdateInformation(GameController.gameCont.activeCharacter);

                StateController.stateCont.stateList.Push(new MenuSelection(oldPosition, oldTile, TileController.tileCont.hoveredTile));
                StateController.stateCont.currentState = (State) StateController.stateCont.stateList.Peek();
            }
        }

        return;
    }

    //Revert to the unit selection phase
    private void RevertState()
    {
        OnStateExit();

        GameController.gameCont.activeCharacter = null;
		GameController.gameCont.activeCharacterScript = null;

        StateController.stateCont.stateList.Pop();
        StateController.stateCont.currentState = (State) StateController.stateCont.stateList.Peek();
    }
}
