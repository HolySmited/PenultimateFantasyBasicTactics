using UnityEngine;
using System.Collections;

//Stores information about a tile to be accessed by other scripts
public class TileInformation : MonoBehaviour
{
    GameController gameCont;

    public Vector3 unitLocation;
    public Vector3 highlightLocation;

    public bool isOccupied = false;
	public GameObject occupyingUnit = null;

	public int xIndex;
	public int zIndex;

    public void Initialize()
    {
        gameCont = GameController.instance;

        unitLocation = gameObject.transform.position + new Vector3(0, 0.5f, 0);
        highlightLocation = gameObject.transform.position + new Vector3(0, 0f, 0);

		foreach (GameObject unit in gameCont.blueTeam) {
			if ((unit.GetComponent<UnitBehavior>().xPosition == xIndex) && (unit.GetComponent<UnitBehavior>().zPosition == zIndex)) {
				isOccupied = true;
				occupyingUnit = unit;

				break;
			}
		}

		foreach (GameObject unit in gameCont.redTeam) {
			if ((unit.GetComponent<UnitBehavior>().xPosition == xIndex) && (unit.GetComponent<UnitBehavior>().zPosition == zIndex)) {
				isOccupied = true;
				occupyingUnit = unit;
				
				break;
			}
		}
    }

    //Updates the tile's information
    public void UpdateInformation(GameObject newOccupyingUnit)
    {
        isOccupied = !isOccupied;

        if (isOccupied)
        {
            occupyingUnit = newOccupyingUnit;
        }
        else
        {
            occupyingUnit = null;
        }
    }
}
