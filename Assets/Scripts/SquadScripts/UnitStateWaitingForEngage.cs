using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateWaitingForEngage : IUnitStates
{
    [SerializeField]
    static float distanceToStartAttacking = 1.4f;

    static UnitStateWaitingForEngage inst = null;
    public static UnitStateWaitingForEngage GetInstance()
    {
        if (inst == null)
        {
            inst = new UnitStateWaitingForEngage();
        }

        return inst;
    }
    private UnitStateWaitingForEngage()
    {

    }
    public void OnStateEntry(UnitBehaviour unit)
    {
        unit.GetComponent<Animator>().SetTrigger("Idle");
        unit.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
    }

    public void OnStateExit(UnitBehaviour unit)
    {

    }

    public void OnStateUpdate(UnitBehaviour unit)
    {
        if (unit.GetUnitCurrentTarget() != null)
        {
            UnitBehaviour enemyUnit = unit.GetUnitCurrentTarget().GetComponent<UnitBehaviour>();
            if (enemyUnit != null)
            {
                if (enemyUnit.GetCurrentState() == UnitStateDeath.GetInstance())
                {
                    UnitBehaviour newTarget = unit.GetThisUnitSquad().RequestForUnitTarget();
                    if (newTarget != null)
                    {
                        unit.SetUnitCurrentTarget(newTarget.gameObject);
                        unit.SetUnitState(UnitStateEngaging.GetInstance());
                    }
                    else
                    {
                        unit.SetUnitState(UnitStateFormation.GetInstance());
                    }
                }
                else if (Vector3.Distance(unit.transform.position, unit.GetUnitCurrentTarget().transform.position) <= distanceToStartAttacking)
                {
                    unit.SetUnitState(UnitStateAttacking.GetInstance());
                }
            }
            
        }
        else
        {
            unit.SetUnitState(UnitStateFormation.GetInstance());
        }
    }
}
