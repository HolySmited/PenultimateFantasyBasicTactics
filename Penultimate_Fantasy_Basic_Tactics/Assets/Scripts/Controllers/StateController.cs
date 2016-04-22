using UnityEngine;
using System.Collections;

//Controlls information regarding the current state of the game
public class StateController : MonoBehaviour
{
	public static StateController stateCont;

    public State currentState; //Stores the current state of the game
    public Stack stateList; //Stores a list of states for the entire unit's turn

	void Awake()
	{
		if (stateCont == null) {
			stateCont = this;
		} 
		else {
			Destroy(this);
		}
	}

    void Start()
    {
        stateList = new Stack(); //Initialize
        stateList.Push(new UnitSelection()); //Start with the unit selection phase

        currentState = (State) stateList.Peek(); //Get the top phase
    }

    void Update()
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
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            currentState.ExecuteCommand(KeyCode.Backspace);
        }
    }
}
