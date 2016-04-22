using UnityEngine;
using System.Collections;

//Behavior for a basic unit
public class UnitBehavior : MonoBehaviour
{
	public int xPosition;
	public int zPosition;

	public bool blueTeam;
	public int health = 2;
	public int damage = 1;
	public bool hasUsed = false;

	public GameObject occupiedTile;

    //Ranges
    private GameObject moveRange;
    private GameObject attackRange;

    //In-world distances
    private float moveDistance = 2.25f;
    private float attackDistance = 1f;

    //Initialize
    void Start()
    {
        moveRange = gameObject.transform.GetChild(0).gameObject;
        attackRange = gameObject.transform.GetChild(1).gameObject;

		if (gameObject.tag == "BlueTeam") {
			blueTeam = true;
		} 
		else {
			blueTeam = false;
		}

		occupiedTile = TileController.tileCont.levelMap [xPosition, zPosition];
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

    //Checks to see if a tile is within movement range
    public bool CheckRangeForTile(GameObject tile)
    {
        Vector3 vectorToTarget = tile.transform.position - gameObject.transform.position;
        float distanceToTarget = vectorToTarget.magnitude;

        if (distanceToTarget <= moveDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Checks to see if an enemy is within attack range
    public bool CheckRangeForEnemy(GameObject enemy)
    {
        Vector3 vectorToTarget = enemy.transform.position - gameObject.transform.position;
        float distanceToTarget = vectorToTarget.magnitude;

        if (distanceToTarget <= attackDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

	public void TakeDamage (int damageAmount) {
		health -= damageAmount;

		Debug.Log ("New health: " + health);

		if (health <= 0) {
			Debug.Log ("Unit has died!");

			occupiedTile.GetComponent<TileInformation>().isOccupied = false;

			if (blueTeam) {
				GameController.gameCont.blueTeam.Remove(gameObject);
			}
			else {
				GameController.gameCont.redTeam.Remove(gameObject);
			}

			Destroy (gameObject);
		}
	}

	public void UpdatePosition(int newX, int newZ, GameObject newTile) {
		GameController.gameCont.activeCharacter.transform.position = newTile.GetComponent<TileInformation> ().unitLocation;
		xPosition = newX;
		zPosition = newZ;

		occupiedTile = newTile;
	}
}
