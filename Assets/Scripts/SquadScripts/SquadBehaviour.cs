using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SquadBehaviour : MonoBehaviour
{
    NavMeshAgent thisAgent = null;
    Vector3 targetLanePos = Vector3.zero;
    SquadStats thisSquadStats = null;
    SquadBehaviour enemyTargetedSquad = null;

    public enum SquadStates { Moving, Engaging, WaitingToEngage }
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
            /****************  TO-DO  ******************/

            // If onMoving state
            // play moving animation
            // play moving sounds
            // ...
            // Else if OnAttacking state
            // Attack method
            // attack anim
            // attack sounds
            // ...
            // Else if OnWaitingForEnemySquadToApproach state
            // play idle anim OR taunt anim if we have
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
                foreach (UnitBehaviour unit in GetSquadUnits()) {
                    unit.SetUnitState(UnitBehaviour.UnitStates.WaitingToEngage);
                    thisAgent.isStopped = true;
                }
                break;
            case SquadStates.Moving:
                foreach (UnitBehaviour unit in GetSquadUnits())
                {
                    unit.SetUnitState(UnitBehaviour.UnitStates.Moving);
                }
                break;
            case SquadStates.Engaging:
                thisAgent.isStopped = true;
                foreach (UnitBehaviour unit in GetSquadUnits())
                {
                    unit.SetUnitState(UnitBehaviour.UnitStates.Engaging);
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
        if (other.tag.StartsWith("Sq") && other.tag != this.tag) {
            if (GetSquadState() == SquadStates.Moving)
            {
                SquadBehaviour enemySquad = other.GetComponent<SquadBehaviour>();
                if (enemySquad.GetSquadState() == SquadStates.Moving)
                {
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

    public List<UnitBehaviour> GetSquadUnits() {
        return thisSquadStats.GetSquadUnits();
    }

    public void SetSquadTarget(GameObject target) {
        thisAgent.isStopped = true;
        foreach (UnitBehaviour unit in GetSquadUnits()) {
            NavMeshAgent unitNavMesh = unit.GetComponent<NavMeshAgent>();
            if (unitNavMesh != null)
            {
                unitNavMesh.enabled = true;
                unit.SetUnitCurrentTarget(target);  
            }
        }
        SetSquadState(SquadStates.Engaging);
    }
    public void SetSquadTargets(List<GameObject> targets)
    {
        thisAgent.isStopped = true;
        int index = 0;
        foreach (UnitBehaviour unit in GetSquadUnits())
        {
            NavMeshAgent unitNavMesh = unit.GetComponent<NavMeshAgent>();
            if (unitNavMesh != null)
            {
                unit.SetUnitCurrentTarget(targets[index]);               
                index++;
            }
            if (index >= targets.Count) {
                index = 0;
            }
        }
        SetSquadState(SquadStates.Engaging);
    }
    public void SetSquadTargets(List<UnitBehaviour> targets)
    {
        thisAgent.isStopped = true;
        int index = 0;
        foreach (UnitBehaviour unit in GetSquadUnits())
        {
            NavMeshAgent unitNavMesh = unit.GetComponent<NavMeshAgent>();
            if (unitNavMesh != null)
            {              
                unit.SetUnitCurrentTarget(targets[index].gameObject);
               
                index++;
            }
            if (index >= targets.Count)
            {
                index = 0;
            }
        }
        SetSquadState(SquadStates.Engaging);
    }

}
