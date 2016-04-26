using UnityEngine;
using System.Collections;

public class Mage : BaseClass
{
    //In-world distances
    public float moveDistance;
    public float attackDistance;
    public float magicDistance;

    GameController gameCont;

    void Start()
    {
        moveDistance = 1.25f;
        attackDistance = 1.25f;
        magicDistance = 3.05f;

        gameCont = GameController.instance;
    }

    public override void Attack(GameObject target)
    {
        target.GetComponent<BaseClass>().TakeDamage(strength, DAMAGE_TYPE.PHYSICAL);
    }

    public override void Attack(GameObject[] targets)
    {
        foreach (GameObject target in targets)
        {
            if (target != null)
                target.GetComponent<BaseClass>().TakeDamage(intelligence, DAMAGE_TYPE.MAGICAL);
        }

        currentMana -= 10;
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
