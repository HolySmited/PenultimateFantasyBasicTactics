using UnityEngine;
using System.Collections;

//Stores information about a tile to be accessed by other scripts
public class TileInformation : MonoBehaviour
{
    public Vector3 unitLocation;
    public Vector3 highlightLocation;

    public bool isOccupied;
    public GameObject occupyingUnit;

    void Start()
    {
        unitLocation = gameObject.transform.position + new Vector3(0, 0.5f, 0);
        highlightLocation = gameObject.transform.position + new Vector3(0, 0f, 0);

        if (gameObject.transform.childCount > 5)
        {
            isOccupied = true;
            occupyingUnit = gameObject.transform.GetChild(5).gameObject;
        }
        else
        {
            isOccupied = false;
            occupyingUnit = null;
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
