using UnityEngine;
using System.Collections;

/*
* ANALYSIS
* Abstracy class that all game states inherit from.
*
* IMPLEMENTATION
*   ExecuteCommand() accepts a key and acts accordingly, based on the current state
*   and what the key was.
*   OnStateEnter() is called upon entering a state
*   OnStateExit() is called upon exiting a state
*/

public abstract class State
{
    public abstract void ExecuteCommand(KeyCode inputKey);

    public abstract void OnStateEnter();

    public abstract void OnStateExit();
}