using UnityEngine;
using System.Collections;

public class UnitSelection : State
{
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
            default:
                return;
        }
    }

    public override void OnStateEnter()
    {
        ;
    }

    public override void OnStateExit()
    {
        StateController.stateCont.stateList.Push(new Movement());
        StateController.stateCont.currentState = (State) StateController.stateCont.stateList.Peek();
    }

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

    private void SelectUnit()
    {
        //If the hoveredTile has a character on it (all tiles have 5 children, the outer black lines and the colored center)
        if (TileController.tileCont.hoveredTile.GetComponent<TileInformation>().isOccupied)
        {
			if (!TileController.tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit.GetComponent<UnitBehavior>().hasUsed) {
				if ((GameController.gameCont.isBlueTurn && TileController.tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit.GetComponent<UnitBehavior>().blueTeam) ||
				    (!GameController.gameCont.isBlueTurn && !TileController.tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit.GetComponent<UnitBehavior>().blueTeam)) {
		            //Set the active character (there will always be one character per tile, and it will always be the sixth child, which is index 5)
		            GameController.gameCont.SetActiveCharacter(TileController.tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit);
		            OnStateExit();
				}
			}
        }

        return;
    }
}