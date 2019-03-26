using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateAttacking : IUnitStates
{

    static UnitStateAttacking inst = null;
    public static UnitStateAttacking GetInstance()
    {
        if (inst == null)
        {
            inst = new UnitStateAttacking();
        }

        return inst;
    }
    private UnitStateAttacking()
    {

    }
    public void OnStateEntry(UnitBehaviour unit)
    {
        unit.transform.LookAt(unit.GetComponent<UnityEngine.AI.NavMeshAgent>().destination);
        unit.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        unit.GetComponent<Animator>().SetBool("Attacking", true);
    }

    public void OnStateExit(UnitBehaviour unit)
    {
        unit.GetComponent<Animator>().SetBool("Attacking", false);
    }

    public void OnStateUpdate(UnitBehaviour unit)
    {
        UnitBehaviour enemyUnit = unit.GetUnitCurrentTarget().GetComponent<UnitBehaviour>();
        if (enemyUnit != null) {
            if (enemyUnit.GetCurrentState() == UnitStateDeath.GetInstance()) {
                UnitBehaviour newTarget = unit.GetThisUnitSquad().RequestForUnitTarget();
                if (newTarget != null)
                {
                    unit.SetUnitCurrentTarget(newTarget.gameObject);
                    unit.SetUnitState(UnitStateEngaging.GetInstance());
                }
                else {
                    unit.SetUnitState(UnitStateFormation.GetInstance());
                }
            }
        }
    }

}
