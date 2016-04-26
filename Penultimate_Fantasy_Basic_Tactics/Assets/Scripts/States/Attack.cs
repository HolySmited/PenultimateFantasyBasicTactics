using UnityEngine;
using System.Collections;

//State for when attack has been selected from the action menu
public class Attack : State
{
    GameController gameCont = GameController.instance;
    TileController tileCont = TileController.instance;
    StateController stateCont = StateController.instance;
    UIController uiCont = UIController.instance;

    private bool isSpell;

    public Attack(bool isSpell)
    {
        this.isSpell = isSpell;

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
                if (!isSpell)
                    SelectUnit();
                else
                    SelectTile();
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
        if (isSpell)
        {
            gameCont.activeCharacterScript.ToggleMagicRange();
        }
        else
        {
            gameCont.activeCharacterScript.ToggleAttackRange();
        }        
    }

    //Hide the attack range of the active character
    public override void OnStateExit()
    {
        if (isSpell)
        {
            gameCont.activeCharacterScript.ToggleMagicRange();
        }
        else
        {
            gameCont.activeCharacterScript.ToggleAttackRange();
        }
    }

    //Move the selector
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

    //Select the unit the selector is hovering over
    private void SelectUnit()
    {
        //If the hoveredTile has a character on it
        if (tileCont.hoveredTile.GetComponent<TileInformation>().isOccupied)
        {
            //If the character is in range
            if (gameCont.activeCharacterScript.CheckRangeForObject(tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit, gameCont.activeCharacterScript.unitClass.GetAttackDistance()))
            {
				if ((gameCont.isBlueTurn && !tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit.GetComponent<UnitBehavior>().blueTeam) ||
				    (!gameCont.isBlueTurn && tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit.GetComponent<UnitBehavior>().blueTeam)) {
                    gameCont.activeCharacter.GetComponent<BaseClass>().Attack(tileCont.hoveredTile.GetComponent<TileInformation>().occupyingUnit); //Attack it

                    uiCont.SetNewHoveredUnit();

                    gameCont.CheckEndGame();

                    //Start a new turn
                    stateCont.stateList.Clear();
	                stateCont.stateList.Push(new UnitSelection());
	                stateCont.currentState = (State)stateCont.stateList.Peek();

	                OnStateExit();

					gameCont.UpdateUnits ();
	                gameCont.activeCharacter = null;
					gameCont.activeCharacterScript = null;
				}
            }
        }

        return;
    }

    private void SelectTile()
    {
        //If the tile is in range
        if (gameCont.activeCharacterScript.CheckRangeForObject(tileCont.hoveredTile, gameCont.activeCharacter.GetComponent<Mage>().magicDistance))
        {
            GameObject[] unitsInRange = new GameObject[5];
            int counter = 0;

            foreach (GameObject unit in gameCont.allUnits)
            {
                float distanceFromTile = (unit.transform.position - tileCont.hoveredTile.transform.position).magnitude;

                if (distanceFromTile <= 2.05f)
                {
                    unitsInRange[counter] = unit;
                    counter++;
                }
            }

            gameCont.activeCharacter.GetComponent<Mage>().Attack(unitsInRange); //Attack it

            uiCont.SetNewHoveredUnit();

            gameCont.CheckEndGame();

            stateCont.stateList.Clear();
            stateCont.stateList.Push(new UnitSelection());
            stateCont.currentState = (State)stateCont.stateList.Peek();

            OnStateExit();

            gameCont.UpdateUnits();
            gameCont.activeCharacter = null;
            gameCont.activeCharacterScript = null;
        }
    }

    //Revert to the previous state
    private void RevertState()
    {
        OnStateExit();

        stateCont.stateList.Pop();
        stateCont.currentState = (State)stateCont.stateList.Peek();

        stateCont.currentState.OnStateEnter();
    }
}
