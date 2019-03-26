using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollChecker : MonoBehaviour
{
    [SerializeField]
    int arrowDmgDealt = 2;
    UnitStats thisUnitStats = null;

    private void Start()
    {
        thisUnitStats = GetComponentInParent<UnitStats>();
    }
    private void OnTriggerEnter(Collider other)
    {
       
            if (other.tag == "Arrow" && !thisUnitStats.GetIfPlayerUnit())
            {
                if (thisUnitStats.GetHealth() > 0)
                {
                    if (thisUnitStats.GetHealth() - arrowDmgDealt <= 0)
                    {
                        UIManager.GetInstance().AddKilledEnemy(); // add a killed enemy to the score
                        if (tag.StartsWith("H"))
                        {
                            CoinManager.GetInstance().ChangeTotalCoinsByAmount(CoinManager.GetInstance().GetHeadValue() - CoinManager.GetInstance().GetPlayerUnitKillValue());
                        }
                        else if(tag.StartsWith("B"))
                        {
                            CoinManager.GetInstance().ChangeTotalCoinsByAmount(CoinManager.GetInstance().GetBodyValue() - CoinManager.GetInstance().GetPlayerUnitKillValue());
                        }
                    }
                    thisUnitStats.ChangeHealthByAmount(-arrowDmgDealt);
                }
            }
        
    }
}
