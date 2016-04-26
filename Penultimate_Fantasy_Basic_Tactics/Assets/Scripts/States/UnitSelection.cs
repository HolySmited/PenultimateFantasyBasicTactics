using UnityEngine;
using System.Collections;

public class UnitSelection : State
{
    TileController tileCont = TileController.instance;
    GameController gameCont = GameController.instance;
    StateController stateCont = StateController.instance;
    UIController uiCont = UIController.instance;

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
        stateCont.stateList.Push(new Movement());
        stateCont.currentState = (State) stateCont.stateList.Peek();
    }

    private void MoveUnitSelector(int xDelta, int zDelta)
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

    private void SelectUnit()
    {
        //If the hoveredTile has a character on it (all tiles have 5 children, the outer black lines and the colored center)
        if (tileCont.hoveredTile.GetComponent<TileInformation>().isOccupied)
        {
			if (!tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit.GetComponent<UnitBehavior>().hasUsed) {
                if ((gameCont.isBlueTurn && tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit.GetComponent<UnitBehavior>().blueTeam) ||
				    (!gameCont.isBlueTurn && !tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit.GetComponent<UnitBehavior>().blueTeam)) {
                    //Set the active character (there will always be one character per tile, and it will always be the sixth child, which is index 5)
                    gameCont.SetActiveCharacter(tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit);
		            OnStateExit();
				}
			}
        }

        return;
    }
}