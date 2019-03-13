﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UnitStateEngaging : IUnitStates
{
    [SerializeField]
    static float distanceToStartAttacking = 1.2f;

    static UnitStateEngaging inst = null;
    public static UnitStateEngaging GetInstance()
    {
        if (inst == null)
        {
            inst = new UnitStateEngaging();
        }

        return inst;
    }
    private UnitStateEngaging()
    {
    }
    public void OnStateEntry(UnitBehaviour unit)
    {
        unit.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        unit.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(unit.GetUnitCurrentTarget().transform.position);
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
        else {
            unit.SetUnitState(UnitStateFormation.GetInstance());
        }
    }

    
}
