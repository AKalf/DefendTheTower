using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatesManager : MonoBehaviour
{
    static UnitStatesManager inst = null;
    private UnitStateIdle idleStateInst = null;

    private UnitStateMoving movingState = null;

    private UnitStateEngaging engagingStateInst = null;

    private UnitStateWaitingForEngage waitingStateInst = null;

    private UnitStateAttacking attackingStateInst = null;

    private UnitStateFormation formationStateInst = null;

    private UnitStateDeath deathStateInst = null;

    static Dictionary<IUnitStates, List<IUnitStates>> statesAvailTrans = new Dictionary<IUnitStates, List<IUnitStates>>();

    private void Awake()
    {
        if (inst == null) {
            inst = this;

            statesAvailTrans.Add(UnitStateIdle.GetInstance(), new List<IUnitStates>() { UnitStateMoving.GetInstance(), UnitStateDeath.GetInstance() });
            statesAvailTrans.Add(UnitStateMoving.GetInstance(), new List<IUnitStates>() { UnitStateEngaging.GetInstance(), UnitStateWaitingForEngage.GetInstance(), UnitStateDeath.GetInstance() });
            statesAvailTrans.Add(UnitStateEngaging.GetInstance(), new List<IUnitStates>() { UnitStateAttacking.GetInstance(), UnitStateFormation.GetInstance(), UnitStateEngaging.GetInstance(), UnitStateDeath.GetInstance() });
            statesAvailTrans.Add(UnitStateWaitingForEngage.GetInstance(), new List<IUnitStates>() { UnitStateAttacking.GetInstance(), UnitStateFormation.GetInstance(), UnitStateEngaging.GetInstance(), UnitStateDeath.GetInstance() });
            statesAvailTrans.Add(UnitStateAttacking.GetInstance(), new List<IUnitStates>() {UnitStateFormation.GetInstance(), UnitStateEngaging.GetInstance(), UnitStateDeath.GetInstance() });
            statesAvailTrans.Add(UnitStateFormation.GetInstance(), new List<IUnitStates>() { UnitStateIdle.GetInstance(), UnitStateDeath.GetInstance()});
            statesAvailTrans.Add(UnitStateDeath.GetInstance(), new List<IUnitStates>());
        }
    }
    public static UnitStatesManager GetInstance() {
        return inst;
    }
    public static void AddNewStateWithNeigbours(IUnitStates newState, List<IUnitStates> neighbours) {
        if (statesAvailTrans.ContainsKey(newState) == false) {
            if (neighbours == null) {
                neighbours = new List<IUnitStates>();
            }
            statesAvailTrans.Add(newState, neighbours);
        }
    }
    public static void AddNewNeighbour(IUnitStates fromState, IUnitStates toState) {
        if (statesAvailTrans.ContainsKey(fromState)) {
            if (statesAvailTrans[fromState].Contains(toState) == false) {
                statesAvailTrans[fromState].Add(toState);
            }
        }
    }
    public static void AddNewNeighbours(IUnitStates fromState, List<IUnitStates> toStates) {
        if (statesAvailTrans.ContainsKey(fromState))
        {
            foreach (IUnitStates state in toStates)
            {
                if (statesAvailTrans[fromState].Contains(state) == false)
                {
                    statesAvailTrans[fromState].Add(state);
                }
            }
        }
    }
    public static Dictionary<IUnitStates, List<IUnitStates>> GetStatesAvailTrans() {
        return statesAvailTrans;
    }
    /// <summary>
    /// Make transition to other state. If possible return true, trigger OnStateExit() and OnStateEntry()
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="stateToGo"></param>
    /// <returns></returns>
    public static bool GetIfUnitTransitionPossible(UnitBehaviour unit, IUnitStates stateToGo) {
        if (statesAvailTrans.ContainsKey(unit.GetCurrentState())) {
            if (statesAvailTrans[unit.GetCurrentState()].Contains(stateToGo)) {
               
                return true;
            }
        }
        return false;
    }
}
