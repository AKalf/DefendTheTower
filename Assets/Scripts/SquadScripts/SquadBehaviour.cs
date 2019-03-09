using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SquadBehaviour : MonoBehaviour
{
    NavMeshAgent thisAgent = null;
    Vector3 targetLanePos = Vector3.zero;
    SquadStats thisSquadStats = null;
    [SerializeField]
    SquadBehaviour enemyTargetedSquad = null;
    public enum SquadStates { Moving, Engaging, WaitingToEngage, Disengage }
    [SerializeField]
    SquadStates currentState = SquadStates.Moving;
    bool init = false;
    private void Awake()
    {
        thisAgent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        thisSquadStats = GetComponent<SquadStats>();
        init = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (init) {

            
            SetSquadState(SquadStates.Moving);
            init = false;
        }
        if (currentState == SquadStates.Disengage) {         
            bool isAUnitNotIdle = false;
            foreach (UnitBehaviour unit in GetSquadUnits()) {
                if (unit.GetCurrentState() != UnitBehaviour.UnitStates.Idle) {
                    isAUnitNotIdle = true;
                }
            }
            if (!isAUnitNotIdle) {
                SetSquadState(SquadStates.Moving);
            }
        }
        else if (currentState == SquadStates.Moving && Vector3.Distance(transform.position, thisAgent.destination) < 0.2f){
            SetSquadState(SquadStates.WaitingToEngage);
        }
    }
    /// <summary>
    /// Called by spawn manager when spawned, to set lane path.  
    /// </summary>
    /// <param name="pos"></param>
    public void SetTargetLane(Vector3 pos) {
        targetLanePos = pos;
        thisAgent = GetComponent<NavMeshAgent>();
        thisAgent.SetDestination(targetLanePos);
        init = true;
    }


    private void SetSquadState(SquadStates state) {

        switch (state) {
            case SquadStates.WaitingToEngage:
                thisAgent.isStopped = true;
                foreach (UnitBehaviour unit in GetSquadUnits()) {
                    unit.SetUnitState(this.gameObject, UnitBehaviour.UnitStates.WaitingToEngage);
                   
                }
                break;
            case SquadStates.Moving:
                thisAgent.isStopped = false;
                foreach (UnitBehaviour unit in GetSquadUnits())
                {
                    unit.SetUnitState(this.gameObject, UnitBehaviour.UnitStates.Moving);
                }  
                break;
            case SquadStates.Engaging:
                
                foreach (UnitBehaviour unit in GetSquadUnits())
                {
                    unit.SetUnitState(this.gameObject, UnitBehaviour.UnitStates.Engaging);
                }
                thisAgent.isStopped = true;
                break;
            case SquadStates.Disengage:
                foreach (UnitBehaviour unit in GetSquadUnits())
                {
                    unit.SetUnitState(this.gameObject, UnitBehaviour.UnitStates.Formation);
                }
                enemyTargetedSquad = null;
                break;

        }
        currentState = state;
    }
    public SquadStates GetSquadState() {
        return currentState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("Sq") && other.tag != this.tag) {
            if (GetSquadState() == SquadStates.Moving)
            {
                SquadBehaviour enemySquad = other.GetComponent<SquadBehaviour>();
                if (enemySquad.GetSquadState() == SquadStates.Moving && enemySquad != enemyTargetedSquad)
                {
                    enemyTargetedSquad = enemySquad;
                    Debug.Log(gameObject.name + ": EnemySquad collided");
                    // if this is Enemy squad 
                    if (this.tag == "Sq2")
                    {
                        if (enemySquad.GetSquadUnits().Count <= this.GetSquadUnits().Count)
                        {
                            SetSquadTargets(enemySquad.GetSquadUnits());
                            SetSquadState(SquadStates.Engaging);
                            enemySquad.SetSquadState(SquadStates.WaitingToEngage);

                        }
                    }
                    else if (this.tag == "Sq1")
                    {
                        if (this.GetSquadUnits().Count > enemySquad.GetSquadUnits().Count)
                        {
                            SetSquadTargets(enemySquad.GetSquadUnits());
                            SetSquadState(SquadStates.Engaging);
                            enemySquad.SetSquadState(SquadStates.WaitingToEngage);
                        }

                    }
                }
            }
        }
    }
    /// <summary>
    /// Get all the units that belong to this squad.
    /// </summary>
    /// <returns></returns>
    public List<UnitBehaviour> GetSquadUnits() {
        return thisSquadStats.GetSquadUnits();
    }
    /***************/
   
    #region Targeting
    /***************/
    /// <summary>
    /// Set a gameobject as a target for this squad's units
    /// </summary>
    /// <param name="target"></param>
    public void SetSquadTarget(GameObject target) {       
        // for each unit in squad, active the navMeshAgen Comp. and set the unit's target
        foreach (UnitBehaviour unit in GetSquadUnits()) {
            NavMeshAgent unitNavMesh = unit.GetComponent<NavMeshAgent>();
            if (unitNavMesh != null)
            {
                unit.SetUnitCurrentTarget(target);                          
            }
        }
        SetSquadState(SquadStates.Engaging);
    }
    /// <summary>
    /// Set multiple gameobject targets for this squad. Each unit will be assigned to a target. If there are more units than targets, a target will be assigned to multiple units.
    /// </summary>
    /// <param name="targets"></param>
    public void SetSquadTargets(List<GameObject> targets)
    {
        int index = 0;
        // for each unit in the squad
        foreach (UnitBehaviour unit in GetSquadUnits())
        {
            // Get NavMeshAgent comp.
            NavMeshAgent unitNavMesh = unit.GetComponent<NavMeshAgent>();
            // if exists
            if (unitNavMesh != null)
            {               
                unit.SetUnitCurrentTarget(targets[index]);
                unitNavMesh.enabled = true;
                index++;
            }
            // if index is bigger or equal to the number of targets, reset and count from start (thus targets will be assigned to multiple units) 
            if (index >= targets.Count) {
                index = 0;
            }
        }
        SetSquadState(SquadStates.Engaging);
    }
    /// <summary>
    /// Set multiple units as targets for this squad's units.
    /// </summary>
    /// <param name="targets"></param>
    public void SetSquadTargets(List<UnitBehaviour> targets)
    {
        int index = 0;
        foreach (UnitBehaviour unit in GetSquadUnits())
        {
            NavMeshAgent unitNavMesh = unit.GetComponent<NavMeshAgent>();
            if (unitNavMesh != null)
            {              
                unit.SetUnitCurrentTarget(targets[index].gameObject);
                unitNavMesh.enabled = true;
                index++;
            }
            if (index >= targets.Count)
            {
                index = 0;
            }
        }
        SetSquadState(SquadStates.Engaging);
    }
    #endregion
    /***************/
    /// <summary>
    /// Returns an alive unit from the enemy squad currently engaged.
    /// </summary>
    /// <returns></returns>
    public UnitBehaviour RequestForUnitTarget() {
        if (enemyTargetedSquad != null)
        {
            if (enemyTargetedSquad.GetSquadUnits().Count > 0)
            {
                return enemyTargetedSquad.GetSquadUnits()[0];
            }
            else {
                SetSquadState(SquadStates.Disengage);
            }
        }
        else {
            SetSquadState(SquadStates.Disengage);
        }
        return null;
    }
}
