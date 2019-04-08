using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollChecker : MonoBehaviour
{

    
    UnitStats thisUnitStats = null;

    private void Start()
    {
        thisUnitStats = GetComponentInParent<UnitStats>();
    }
    private void OnTriggerEnter(Collider other)
    {
        int arrowDmgDealt = 0;
        int coinWorth = 0;
        if (other.tag == "Arrow" && !thisUnitStats.GetIfPlayerUnit())
        {
            if (thisUnitStats.GetHealth() > 0)
            {
                if (tag.StartsWith("B"))
                {
                    coinWorth = CoinManager.GetInstance().GetBodyValue() - CoinManager.GetInstance().GetPlayerUnitKillValue();
                    arrowDmgDealt = FindArrowDamage(false);
                }
                else if (tag.StartsWith("H"))
                {
                    coinWorth = CoinManager.GetInstance().GetHeadValue() - CoinManager.GetInstance().GetPlayerUnitKillValue();
                    arrowDmgDealt = arrowDmgDealt = FindArrowDamage(true);
                }

                thisUnitStats.ChangeHealthByAmount(-arrowDmgDealt);
                if (thisUnitStats.GetHealth() <= 0)
                {
                    UIManager.GetInstance().AddKilledEnemy(); // add a killed enemy to the score
                    CoinManager.GetInstance().ChangeTotalCoinsByAmount(coinWorth);
                }
               
            }
        }
    }
    int FindArrowDamage(bool forHead) {
        int damage = 0;
        if (forHead)
        {
            switch (thisUnitStats.GetRace())
            {
                case UnitStats.UnitRace.Human:
                    damage = ArrowDmgManager.GetInstance().GetHumanHead();
                    break;
                case UnitStats.UnitRace.Elf:
                    damage = ArrowDmgManager.GetInstance().GetElfHead();
                    break;
                case UnitStats.UnitRace.Dwarf:
                    damage = ArrowDmgManager.GetInstance().GetDwarfHead();
                    break;
            }
        }
        else {
            switch (thisUnitStats.GetRace())
            {
                case UnitStats.UnitRace.Human:
                    damage = ArrowDmgManager.GetInstance().GetHumanBody();
                    break;
                case UnitStats.UnitRace.Elf:
                    damage = ArrowDmgManager.GetInstance().GetElfBody();
                    break;
                case UnitStats.UnitRace.Dwarf:
                    damage = ArrowDmgManager.GetInstance().GetDwarfBody();
                    break;
            }
        }
        return damage;
    }
}

