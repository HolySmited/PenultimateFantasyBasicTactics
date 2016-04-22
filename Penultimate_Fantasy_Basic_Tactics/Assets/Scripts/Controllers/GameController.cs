using UnityEngine;
using System.Collections;

//Controlls information regarding the game in general
public class GameController : MonoBehaviour
{
    public static GameObject activeCharacter; //The current selected character

    //Sets the active character with the provided GameObject
    public static void SetActiveCharacter(GameObject character)
    {
        activeCharacter = character;
    }
}
