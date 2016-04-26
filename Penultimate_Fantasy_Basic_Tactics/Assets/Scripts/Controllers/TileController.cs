using UnityEngine;
using System.Collections;

//Controlls information regarding the map
public class TileController : MonoBehaviour
{
	public static TileController instance;

    public int GRID_DIMENSION = 11; //Map will always be a sqaure, so this is both the x and z dimensions of the level

    public GameObject[] levelMapOneDimension; //One dimensional array is used so that each grid piece can be dragged into an array in the Inspector
    public GameObject[,] levelMap; //The actual code uses this 2D array, which is initialized using the 1D array declared above (Unity does not support 2D arrays in the Inspector)
    public Vector3[,] gridCoordinates; //2D array parallel with levelMap that stores the positions units will be in when occupying a space

    public GameObject hoveredTile; //The tile currently being hovered over; used to move the tile selector
    public int hoveredXIndex; //The x index of the tile currently being hovered over in levelMap
    public int hoveredZIndex; //The y index of the tile currently being hovered over in levelMap
    public GameObject selectedTile; //The tile currently selected (used for movement and some skill targeting)

    public GameObject hoverIndicator; //Stores the prefab for the hover indicator (used for instantiation only)
    public GameObject tileSelector; //Stores the instance of the hoverIndicator in the game world

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public void Initialize()
    {
        int counter = 0; //Counter used to iterate through levelMapOneDimension

        levelMap = new GameObject[GRID_DIMENSION, GRID_DIMENSION]; //Initialize the 2D array for the grid objects

        //Loop through the 1D array and use it to populate the 2D array
        for (int i = 0; i < GRID_DIMENSION; i++)
        {
            for (int k = 0; k < GRID_DIMENSION; k++)
            {
                levelMap[i, k] = levelMapOneDimension[counter];
				levelMap[i, k].GetComponent<TileInformation>().xIndex = i;
				levelMap[i, k].GetComponent<TileInformation>().zIndex = k;
                counter++;
            }
        }

        levelMapOneDimension = null; //The 1D array is no longer necessary

        gridCoordinates = new Vector3[GRID_DIMENSION, GRID_DIMENSION]; //Initialize the grid coordinate array

        hoveredTile = levelMap[GRID_DIMENSION / 2, GRID_DIMENSION / 2]; //Set the initial hoveredTile to the center tile
        hoveredXIndex = GRID_DIMENSION / 2; //Set the x index of the hovered tile
        hoveredZIndex = GRID_DIMENSION / 2; //Set the y index of the hovered tile

        selectedTile = null; //No tile starts out selected

        //Populate the gridCoordinates array with possible unit locations
        for (int i = 0; i < GRID_DIMENSION; i++)
        {
            int xCoordinate = -5;

            for (int k = 0; k < GRID_DIMENSION; k++)
            {
                int zCoordinate = -5;

                gridCoordinates[i, k] = new Vector3(xCoordinate, (levelMap[i, k].transform.position.y * 2) + 1, zCoordinate);
                zCoordinate++;
            }

            xCoordinate++;
        }

        //Instantiate the tile selector based on the currently hovered tile
        tileSelector = Instantiate(hoverIndicator, new Vector3(hoveredTile.transform.position.x, (hoveredTile.transform.position.y * 2), hoveredTile.transform.position.z), Quaternion.identity) as GameObject;
    }
}
