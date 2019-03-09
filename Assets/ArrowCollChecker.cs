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
        if (other.tag == "Arrow") {
            if (thisUnitStats.GetHealth() - arrowDmgDealt <= 0) {
                UIManager.GetInstance().AddKilledEnemy(); // add a killed enemy to the score
            }
            thisUnitStats.ChangeHealthByAmount(-arrowDmgDealt);

        }
    }
}
