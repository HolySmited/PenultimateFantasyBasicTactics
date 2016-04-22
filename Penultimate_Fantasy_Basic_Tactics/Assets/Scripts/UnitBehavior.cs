using UnityEngine;
using System.Collections;

//Behavior for a basic unit
public class UnitBehavior : MonoBehaviour
{
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
}
