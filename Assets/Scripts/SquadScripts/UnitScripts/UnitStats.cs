using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{

    [SerializeField]
    int health = 10;
    [SerializeField]
    int damage = 1;

    bool isPlayerUnit = false;
    UnitBehaviour thisUnit = null;
    public enum UnitRace { Human, Elf, Dwarf }
    [SerializeField]
    UnitRace thisUnitRace;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.tag == "Sq1") {
            isPlayerUnit = true;
        }
        thisUnit = this.gameObject.AddComponent<UnitBehaviour>();
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
                if (!GetIfPlayerUnit())
                {
                    CoinManager.GetInstance().ChangeTotalCoinsByAmount(CoinManager.GetInstance().GetPlayerUnitKillValue());
                }
            }
        }
    }
    public int GetDamage() {
        return damage;
    }
    public bool GetIfPlayerUnit() {
        return isPlayerUnit;
    }
    public void SetUnitRace(UnitRace race) {
        thisUnitRace = race;
    }
    public UnitRace GetRace() {
        return thisUnitRace;
    }
}
