using UnityEngine;
using System.Collections;

//Behavior for a basic unit
public class UnitBehavior : MonoBehaviour
{
    GameController gameCont;

	public int xPosition;
	public int zPosition;

	public bool blueTeam;
	public bool hasUsed = false;

	public GameObject occupiedTile;

    public BaseClass unitClass;

    //Ranges
    private GameObject moveRange;
    private GameObject attackRange;
    private GameObject magicRange;

    //Initialize
    void Start()
    {
        gameCont = GameController.instance;

        moveRange = gameObject.transform.GetChild(0).gameObject;
        attackRange = gameObject.transform.GetChild(1).gameObject;

        unitClass = GetComponent<BaseClass>();

        if (unitClass.GetType() == typeof(Mage))
        {
            magicRange = gameObject.transform.GetChild(2).gameObject;
        }

        if (gameObject.tag == "BlueTeam") {
			blueTeam = true;
		} 
		else {
			blueTeam = false;
		}
    }

    //Toggles movement range tiles on the game world
    public void ToggleMovementRange()
    {
        for (int i = 0; i < moveRange.transform.childCount; i++)
        {
            Vector3 indicatorPos = moveRange.transform.GetChild(i).position;

            if ((indicatorPos.x >= -5 && indicatorPos.x <= 5) && (indicatorPos.z >= -5 && indicatorPos.z <= 5))
            {
                moveRange.transform.GetChild(i).gameObject.SetActive(!moveRange.transform.GetChild(i).gameObject.activeSelf);
            }
        }
    }

    //Toggles attack range tiles on the game world
    public void ToggleAttackRange()
    {
        for (int i = 0; i < attackRange.transform.childCount; i++)
        {
            Vector3 indicatorPos = attackRange.transform.GetChild(i).position;

            if ((indicatorPos.x >= -5 && indicatorPos.x <= 5) && (indicatorPos.z >= -5 && indicatorPos.z <= 5))
            {
                attackRange.transform.GetChild(i).gameObject.SetActive(!attackRange.transform.GetChild(i).gameObject.activeSelf);
            }
        }
    }

    public void ToggleMagicRange()
    {
        for (int i = 0; i < magicRange.transform.childCount; i++)
        {
            Vector3 indicatorPos = magicRange.transform.GetChild(i).position;

            if ((indicatorPos.x >= -5 && indicatorPos.x <= 5) && (indicatorPos.z >= -5 && indicatorPos.z <= 5))
            {
                magicRange.transform.GetChild(i).gameObject.SetActive(!magicRange.transform.GetChild(i).gameObject.activeSelf);
            }
        }
    }

    //Checks to see if an object (tile, enemy, ally, etc.) is within movement range
    public bool CheckRangeForObject(GameObject obj, float range)
    {
        Vector3 vectorToTarget = obj.transform.position - gameObject.transform.position;
        float distanceToTarget = vectorToTarget.magnitude;

        if (distanceToTarget <= range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

	public void UpdatePosition(int newX, int newZ, GameObject newTile) {
		gameCont.activeCharacter.transform.position = newTile.GetComponent<TileInformation> ().unitLocation;
		xPosition = newX;
		zPosition = newZ;

		occupiedTile = newTile;
	}
}
