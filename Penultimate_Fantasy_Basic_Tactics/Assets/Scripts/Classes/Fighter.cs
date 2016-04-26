using UnityEngine;
using System.Collections;

public class Fighter : BaseClass
{
    //In-world distances
    public float moveDistance;
    public float attackDistance;

    GameController gameCont;

    void Start()
    {
        moveDistance = 2.25f;
        attackDistance = 1.25f;

        gameCont = GameController.instance;
    }

    public override void Attack(GameObject target)
    {
        target.GetComponent<BaseClass>().TakeDamage(strength, DAMAGE_TYPE.PHYSICAL);
    }

    public override void Attack(GameObject[] targets)
    {
        ;
    }

    public override void TakeDamage(int amount, DAMAGE_TYPE type)
    {
        if (type == DAMAGE_TYPE.PHYSICAL)
        {
            amount -= physDef;
        }
        else if (type == DAMAGE_TYPE.MAGICAL)
        {
            amount -= magDef;
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            gameObject.GetComponent<UnitBehavior>().occupiedTile.GetComponent<TileInformation>().isOccupied = false;

            if (gameObject.GetComponent<UnitBehavior>().blueTeam)
            {
                gameCont.blueTeam.Remove(gameObject);
            }
            else {
                gameCont.redTeam.Remove(gameObject);
            }

            gameCont.allUnits.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public override float GetMoveDistance()
    {
        return moveDistance;
    }

    public override float GetAttackDistance()
    {
        return attackDistance;
    }
}
