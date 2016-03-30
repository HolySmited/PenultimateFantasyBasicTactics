using UnityEngine;
using System.Collections;

/*
* ANALYSIS
* Abstracy class that all game states inherit from.
*
* IMPLEMENTATION
*   ExecuteCommand() accepts a key and acts accordingly, based on the current state
*   and what the key was.
*/

public abstract class State
{
    public abstract void ExecuteCommand(KeyCode inputKey);
}