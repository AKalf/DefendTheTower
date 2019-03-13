using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateDeath : IUnitStates
{
    static UnitStateDeath inst = null;
    public static UnitStateDeath GetInstance()
    {
        if (inst == null)
        {
            inst = new UnitStateDeath();
        }

        return inst;
    }
    private UnitStateDeath()
    {

    }
    /// <summary>
    /// Do: anim, SetState, disable navAgent, dispatch from parent, Destroy collider, remove from squad units and if there is target, set his target to null.
    /// </summary>
    public void OnStateEntry(UnitBehaviour unit)
    {
        unit.GetComponent<Animator>().SetTrigger("Death");
        unit.GetThisUnitSquad().GetComponent<SquadStats>().RemoveUnitFromSquad(unit); // remove this unit from the squad   
        unit.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        unit.transform.parent = null;
        MonoBehaviour.Destroy(unit.GetComponent<Collider>());
        unit.StartCoroutine(unit.OnDeath(5));
    }

    public void OnStateExit(UnitBehaviour unit)
    {
        
    }

    public void OnStateUpdate(UnitBehaviour unit)
    {
       
    }
    
    
}
