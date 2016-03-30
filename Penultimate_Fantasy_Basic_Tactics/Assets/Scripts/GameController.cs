using UnityEngine;
using System.Collections;

/*
* ANALYSIS
* Class in charge of initializing the level and managing the current game state.
*
* IMPLEMENTATION
*   Start() initializes all necessary values.
*
*   Update() checks for input from the user and calls the ExecuteCommand function of the current state, passing
*   in the key that was pressed that frame.
*
*   SetActiveCharacter() takes a GameObject as an argument and sets it as the activeCharacter
*/

public class GameController : MonoBehaviour
{
    public const int GRID_DIMENSION = 11; //Map will always be a sqaure, so this is both the x and z dimensions of the level

    public GameObject[] levelMapOneDimension; //One dimensional array is used so that each grid piece can be dragged into an array in the Inspector
    public static GameObject[,] levelMap; //The actual code uses this 2D array, which is initialized using the 1D array declared above (Unity does not support 2D arrays in the Inspector)
    public static Vector3[,] gridCoordinates; //2D array parallel with levelMap that stores the positions units will be in when occupying a space

    public static GameObject hoveredTile; //The tile currently being hovered over; used to move the tile selector
    public static int hoveredXIndex; //The x index of the tile currently being hovered over in levelMap
    public static int hoveredZIndex; //The y index of the tile currently being hovered over in levelMap
    public static GameObject selectedTile; //The tile currently selected (used for movement and some skill targeting)

    public GameObject hoverIndicator; //Stores the prefab for the hover indicator (used for instantiation only)
    public static GameObject tileSelector; //Stores the instance of the hoverIndicator in the game world
    public GameObject class1; //Prefab for of a character for movement testing (used for instantiation only)

    public static State currentState; //Stores the current state of the game
    public static GameObject activeCharacter; //Stores the active character (currently selected character)
    private GameObject testCharacter; //Stores the instance of the class1 character in the game world (used for movement testing)

	void Start ()
    {
        int counter = 0; //Counter used to iterate through levelMapOneDimension

        levelMap = new GameObject[GRID_DIMENSION, GRID_DIMENSION]; //Initialize the 2D array for the grid objects

        //Loop through the 1D array and use it to populate the 2D array
        for (int i = 0; i < GRID_DIMENSION; i++)
        {
            for (int k = 0; k < GRID_DIMENSION; k++)
            {
                levelMap[i, k] = levelMapOneDimension[counter];
                counter++;
            }
        }

        levelMapOneDimension = null; //The 1D array is no longer necessary

        gridCoordinates = new Vector3[GRID_DIMENSION, GRID_DIMENSION]; //Initialize the grid coordinate array

        hoveredTile = levelMap[GRID_DIMENSION / 2, GRID_DIMENSION / 2]; //Set the initial hoveredTile to the center tile
        hoveredXIndex = GRID_DIMENSION / 2; //Set the x index of the hovered tile
        hoveredZIndex = GRID_DIMENSION / 2; //Set the y index of the hovered tile

        selectedTile = null; //No tile starts out selected

        currentState = new UnitSelection(); //The first state is always selecting a unit

        activeCharacter = null; //No unit starts out selected

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
        testCharacter = Instantiate(class1, gridCoordinates[0, 0], Quaternion.identity) as GameObject; //Instantiate the test character
        testCharacter.transform.SetParent(levelMap[0, 0].transform); //Child the test character
	}
	
	void Update ()
    {
        //If a key was pressed this frame, call the ExecuteCommand() function of the current state, passing in the pressed key

	    if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentState.ExecuteCommand(KeyCode.LeftArrow);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentState.ExecuteCommand(KeyCode.RightArrow);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentState.ExecuteCommand(KeyCode.DownArrow);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentState.ExecuteCommand(KeyCode.UpArrow);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            currentState.ExecuteCommand(KeyCode.Space);
        }
    }

    public static void SetActiveCharacter(GameObject character)
    {
        activeCharacter = character; //Sets the active character with the provided GameObject
    }
}