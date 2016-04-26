using UnityEngine;
using System.Collections;

public abstract class BaseClass : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int maxMana;
    public int currentMana;

    public int strength;
    public int physDef;
    public int intelligence;
    public int magDef;

    public enum DAMAGE_TYPE {PHYSICAL, MAGICAL};

    public abstract void Attack(GameObject target);
    public abstract void Attack(GameObject[] targets);

    public abstract void TakeDamage(int amount, DAMAGE_TYPE type);

    public abstract float GetMoveDistance();

    public abstract float GetAttackDistance();
}
