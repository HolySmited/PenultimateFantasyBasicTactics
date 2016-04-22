using UnityEngine;
using System.Collections;

//Controlls information regarding the UI
public class UIController : MonoBehaviour
{
    public static GameObject attackMenu; //Menu for selecting attack
    public static GameObject selectionArrow; //Arrow to indicate selection
	
	public GameObject[] menuOptions;

    private static Vector3 originalArrowPos; //Original position of the arow

    void Start()
    {
        //Initialize
        attackMenu = GameObject.Find("BasicAttackMenu");
        selectionArrow = GameObject.Find("SelectionArrow");

        originalArrowPos = selectionArrow.transform.position;

        attackMenu.SetActive(false); //Hide the menu
    }

    //Move the arrow back to it's original position
    public static void ResetSelectionArrow()
    {
        selectionArrow.transform.position = originalArrowPos;
    }

	public GameObject[] GetMenuOptions()
	{
		return menuOptions;
	}
}
