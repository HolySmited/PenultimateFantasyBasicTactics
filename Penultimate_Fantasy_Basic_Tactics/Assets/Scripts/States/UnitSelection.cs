﻿using UnityEngine;
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
        StateController.stateList.Push(new Movement());
        StateController.currentState = (State) StateController.stateList.Peek();
    }

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

    private void SelectUnit()
    {
        //If the hoveredTile has a character on it (all tiles have 5 children, the outer black lines and the colored center)
        if (TileController.hoveredTile.GetComponent<TileInformation>().isOccupied)
        {
            //Set the active character (there will always be one character per tile, and it will always be the sixth child, which is index 5)
            GameController.SetActiveCharacter(TileController.hoveredTile.GetComponent<TileInformation>().occupyingUnit);

            OnStateExit();
        }

        return;
    }
}