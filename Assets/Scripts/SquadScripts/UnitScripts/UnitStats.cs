using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{

    [SerializeField]
    int health = 10;
    [SerializeField]
    int damage = 1;

    UnitBehaviour thisUnit = null;
    // Start is called before the first frame update
    void Start()
    {
        thisUnit = GetComponent<UnitBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetHealth() {
        return health;
    }
    public void ChangeHealthByAmount(int amount) {
        if (health > 0)
        {
            health += amount;
            if (health <= 0)
            {
                thisUnit.SetUnitState(UnitStateDeath.GetInstance());
            }
        }
    }
    public int GetDamage() {
        return damage;
    }
}
