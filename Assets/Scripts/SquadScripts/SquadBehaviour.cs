using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadBehaviour : MonoBehaviour
{
    
   
    SquadStats thisSquadStats = null;
    [SerializeField]
    SquadBehaviour enemyTargetedSquad = null;
    public enum SquadStates { Moving, Engaging}
    [SerializeField]
    SquadStates currentState = SquadStates.Moving;
    
    bool init = false;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        thisSquadStats = GetComponent<SquadStats>();
        StartCoroutine(SetStateWithDelay(1.5f));
        init = true;
    }

    // Update is called once per frame
    void Update()
    {

        
        if (currentState == SquadStates.Engaging) {         
            bool isAUnitNotIdle = false;
            foreach (UnitBehaviour unit in GetSquadUnits()) {
                if (unit.GetCurrentState() != UnitStateIdle.GetInstance()) {
                    isAUnitNotIdle = true;
                }
            }
            if (!isAUnitNotIdle) {
                SetSquadState(this, SquadStates.Moving);
            }
        }
       
    }
    /// <summary>
    /// Called by spawn manager when spawned, to set lane path.  
    /// </summary>
    /// <param name="pos"></param>
    

    /// <summary>
    /// Set the squad's state which also sets the state for each unit of this squad
    /// </summary>
    /// <param name="who"></param>
    /// <param name="state"></param>
    private void SetSquadState(SquadBehaviour who, SquadStates state) {

        switch (state) {           
            case SquadStates.Moving:                
                foreach (UnitBehaviour unit in GetSquadUnits())
                {
                    unit.transform.LookAt(thisSquadStats.GetTargetLane());
                    unit.SetUnitState(UnitStateMoving.GetInstance());
                    
                }
                GetComponent<Collider>().enabled = true;
                thisSquadStats.GetAgent().enabled = true;
                thisSquadStats.GetAgent().SetDestination(thisSquadStats.GetTargetLane());
                break;
            case SquadStates.Engaging:
                thisSquadStats.GetAgent().enabled = false;
                GetComponent<Collider>().enabled = false;
                foreach (UnitBehaviour unit in GetSquadUnits())
                {
                    unit.SetUnitState(UnitStateEngaging.GetInstance());
                }
                
                break;
        }
        currentState = state;
    }
    public SquadStates GetSquadState() {
        return currentState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("Sq") && other.tag != this.tag)
        {
            if (GetSquadState() == SquadStates.Moving)
            {
                SquadBehaviour enemySquad = other.GetComponent<SquadBehaviour>();
                if (enemySquad.GetSquadState() == SquadStates.Moving && enemySquad != enemyTargetedSquad)
                {
                    enemyTargetedSquad = enemySquad;

                    Debug.Log(gameObject.name + ": EnemySquad collided");
                    if (this.tag == "Sq2")
                    {
                      
                        SetSquadTargets(enemySquad.GetSquadUnits());
                        SetSquadState(this, SquadStates.Engaging);
                        enemySquad.SetEnemySquad(this);
                        enemySquad.SetSquadState(this, SquadStates.Engaging);

                       
                    }
                    
                }
            }
        }
        else if (other.tag.StartsWith("pG") && this.tag.EndsWith("2"))
        {
            SetSquadTarget(other.gameObject);
            SetSquadState(this, SquadStates.Engaging);
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
            unit.SetUnitCurrentTarget(target);                                  
        }
        
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
            UnityEngine.AI.NavMeshAgent unitNavMesh = unit.GetComponent<UnityEngine.AI.NavMeshAgent>();
            // if exists
            if (unitNavMesh != null)
            {
                unit.SetUnitCurrentTarget(targets[index]);
                index++;
            }
            // if index is bigger or equal to the number of targets, reset and count from start (thus targets will be assigned to multiple units) 
            if (index >= targets.Count)
            {
                index = 0;
            }
        }

    }
    /// <summary>
    /// Set multiple units as targets for this squad's units.
    /// </summary>
    /// <param name="targets"></param>
    public void SetSquadTargets(List<UnitBehaviour> targets)
    {
        int index = 0;
        if (GetSquadUnits().Count >= targets.Count)
        {
            foreach (UnitBehaviour unit in GetSquadUnits())
            {
                UnityEngine.AI.NavMeshAgent unitNavMesh = unit.GetComponent<UnityEngine.AI.NavMeshAgent>();
                if (unitNavMesh != null)
                {
                    unit.SetUnitCurrentTarget(targets[index].gameObject);
                    targets[index].SetUnitCurrentTarget(unit.gameObject);
                    index++;
                }
                if (index >= targets.Count)
                {
                    index = 0;
                }
            }
        }
        else
        {
            bool allEnemyUnitsHaveTarget = false;
            foreach (UnitBehaviour unit in targets)
            {
                UnityEngine.AI.NavMeshAgent unitNavMesh = unit.GetComponent<UnityEngine.AI.NavMeshAgent>();

                if (unitNavMesh != null)
                {
                    unit.SetUnitCurrentTarget(GetSquadUnits()[index].gameObject);
                    if (!allEnemyUnitsHaveTarget)
                    {
                        GetSquadUnits()[index].SetUnitCurrentTarget(unit.gameObject);
                    }
                    index++;
                }
                if (index >= GetSquadUnits().Count)
                {
                    allEnemyUnitsHaveTarget = true;
                    index = 0;
                }
            }
        }

    }
    public SquadBehaviour GetEnemyTargetedSquad() {
        return enemyTargetedSquad;
    }
    public void SetEnemySquad(SquadBehaviour squad) {
        enemyTargetedSquad = squad;
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
                UnitBehaviour enemyUnit = enemyTargetedSquad.GetSquadUnits()[0];
                if (enemyUnit.GetCurrentState() != UnitStateDeath.GetInstance()) {
                    return enemyUnit;
                }
            }
            
        }
        
        return null;
    }
    IEnumerator SetStateWithDelay(float delay) {
        yield return new WaitForSeconds(delay);
        transform.LookAt(thisSquadStats.GetTargetLane());
        SetSquadState(this, SquadStates.Moving);
       
    }
}
