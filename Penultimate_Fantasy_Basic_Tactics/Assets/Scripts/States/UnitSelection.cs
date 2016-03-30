using UnityEngine;
using System.Collections;

/*
* ANALYSIS
* Inherits from State. Indicates that the game is waiting for the player to select an active unit.
* Moves the tile selector based on the arrow key that was pressed, and selects the character being
* hovered over (if there is one) when the space bar is pressed.
*
* IMPLEMENTATION
*   ExecuteCommand() calls the appropriate function based on the pressed key. Left, right, down, and up
*   all call MoveUnitSelector() with x and z coordinate deltas. Space calls SetActiveCharacter().
*
*   MoveUnitSelector() moves the tile selector object based on the deltas provided, as well as change all
*   GameController information related to the hoveredTile.
*
*   SetActiveCharacter() checks to see if hoveredTile has a character on it. If it does, that character
*   is set as the active character.
*/

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

    private void MoveUnitSelector(int xDelta, int zDelta)
    {
        //If the tile the user is trying to move to exists (is not out of the array bounds)
        if ((GameController.hoveredXIndex + xDelta >= 0 && GameController.hoveredXIndex + xDelta < GameController.GRID_DIMENSION) && (GameController.hoveredZIndex + zDelta >= 0 && GameController.hoveredZIndex + zDelta < GameController.GRID_DIMENSION))
        {
            //Change the hoveredTile information and move the tile selector
            GameController.hoveredXIndex += xDelta;
            GameController.hoveredZIndex += zDelta;
            GameController.hoveredTile = GameController.levelMap[GameController.hoveredXIndex, GameController.hoveredZIndex];
            GameController.tileSelector.transform.position = new Vector3(GameController.hoveredTile.transform.position.x, (GameController.hoveredTile.transform.position.y * 2), GameController.hoveredTile.transform.position.z);
        }
        else
        {
            return;
        }
    }

    private void SelectUnit()
    {
        //If the hoveredTile has a character on it (all tiles have 5 children, the outer black lines and the colored center)
        if (GameController.hoveredTile.transform.childCount > 5)
        {
            //Set the active character (there will always be one character per tile, and it will always be the sixth child, which is index 5)
            GameController.SetActiveCharacter(GameController.levelMap[GameController.hoveredXIndex, GameController.hoveredZIndex].transform.GetChild(5).gameObject);
            GameController.currentState = new Movement(); //Change the currentState to movement, since a character was selected
        }

        return;
    }
}