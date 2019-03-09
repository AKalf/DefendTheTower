using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitBehaviour : MonoBehaviour
{
    
    SquadStats thisSquadStats = null;
    SquadBehaviour thisSquad = null;
    UnitStats thisUnitStats = null;

    public enum UnitStates { WaitingToEngage, Moving, Engaging, Attacking, Disengaged, Formation, Idle, Death };
    [SerializeField]
    UnitStates thisUnitState = UnitStates.Moving;
    Animator thisAnimator = null;
    NavMeshAgent thisAgent = null;
    /// <summary>
    /// The initial relative position to the parent.
    /// </summary>
    Transform initTransform = null;
    [SerializeField]
    GameObject currentTarget = null; // if in combat state, this will be the target that this unit will fight
    NavMeshPath pathToCurrentTarget = null;

    float timeDoingFormation = 0.0f;
    private void Awake()
    {
        thisAnimator = GetComponent<Animator>();
        thisAgent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        thisUnitStats = GetComponent<UnitStats>();
        thisSquadStats = transform.parent.GetComponent<SquadStats>();
        thisSquadStats.AddUnitToSquad(this);
        thisSquad = transform.parent.GetComponent<SquadBehaviour>();
        GameObject initialTransform = new GameObject();
        initialTransform.transform.position = transform.position;
        initialTransform.transform.parent = transform.parent;
        initTransform = initialTransform.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (thisUnitState == UnitStates.Formation) {
            CheckIfFormationPosReached();
        }
        else if (thisUnitState == UnitStates.Attacking && currentTarget == null) {
            CheckForAwkardState();
        }
        if (currentTarget != null)
        {
            if (currentTarget.GetComponent<UnitBehaviour>() != null)
            {
                if (currentTarget.GetComponent<UnitBehaviour>().GetCurrentState() == UnitStates.Death)
                {
                    MakeTransToDisengaged();
                }
            }
        }
        
    }/// <summary>
     /// Check if formation position reached, disable navAgen and set state to Idle.
     /// </summary>
    private void CheckIfFormationPosReached() {
        if (Vector3.Distance(transform.localPosition, initTransform.localPosition) <= 0.22f && thisAgent.enabled)
        {
            thisAgent.enabled = false;
            transform.LookAt(thisSquad.GetComponent<NavMeshAgent>().destination);
            Debug.Log(transform.name + " reached init pos");
            timeDoingFormation = 0;
            SetUnitState(this.gameObject, UnitStates.Idle);
        }
        else if (timeDoingFormation > 1.2f)
        {
            thisAgent.enabled = false;
            transform.LookAt(thisSquad.GetComponent<NavMeshAgent>().destination);
            Debug.Log(transform.name + " reached init pos");
            SetUnitState(this.gameObject, UnitStates.Idle);
            timeDoingFormation = 0;

        }
        else {
            timeDoingFormation += Time.deltaTime;
        }
    }
    /// <summary>
    /// In case the unit does not know what to do... (usually when attacking and target is dead).
    /// </summary>
    private void CheckForAwkardState() {
        thisAnimator.StopPlayback();
        SquadBehaviour.SquadStates thisSquadState = thisSquad.GetSquadState();
        switch (thisSquadState) {
            case SquadBehaviour.SquadStates.Engaging:
                MakeTransToDisengaged();
                break;
            case SquadBehaviour.SquadStates.Moving:
                SetUnitState(this.gameObject, UnitStates.Moving);
                break;
            case SquadBehaviour.SquadStates.Disengage:
                SetUnitCurrentTarget(null);
                SetUnitState(this.gameObject, UnitStates.Formation);
                break;
            case SquadBehaviour.SquadStates.WaitingToEngage:
                MakeTransToDisengaged();
                break;

        }
        
    }
    /// <summary>
    /// Do: anim, SetState, disable navAgent, dispatch from parent, Destroy collider, remove from squad units and if there is target, set his target to null.
    /// </summary>
    public void Die() {
        if (this != null)
        {
            SetUnitState(this.gameObject, UnitStates.Death);
            thisAnimator.SetTrigger("Death");
            thisSquadStats.RemoveUnitFromSquad(this); // remove this unit from the squad   
            if (thisAgent.enabled)
            {
                thisAgent.enabled = false;
            }
            if (currentTarget != null)
            {
                if (currentTarget.GetComponent<UnitBehaviour>() != null)
                {
                    currentTarget.GetComponent<UnitBehaviour>().SetUnitCurrentTarget(null);
                    SetUnitCurrentTarget(null);
                }
            }
            transform.parent = null;
            Destroy(GetComponent<Collider>());
            
            StartCoroutine(OnDeath(5));
        }
    }
   /// <summary>
   /// If there is an enemy target unit, it informs it that it is dead. Same for enemy squad.
   /// It is coroutine.
   /// </summary>
   /// <param name="timeOffset">offset unit death event triggers</param>
   /// <returns></returns>
    private IEnumerator  OnDeath(float timeDelay) {
        yield return new WaitForSeconds(5); // after 5 seconds
        if (this != null)
        {
            Destroy(this.gameObject); // destroy this gameobject
        }
    }

    public void SetUnitState(GameObject calledBy, UnitStates state) {

        switch (state) {
            case UnitStates.Idle:
                MakeTransToIdle();
                break;
            case UnitStates.WaitingToEngage:
                MakeTransToWaitingForEngage();
                break;
            case UnitStates.Moving:
                MakeTransToMoving();
                break;
            case UnitStates.Attacking:
                MakeTransToAttack();
                break;
            case UnitStates.Engaging:
                MakeTransToEnaging();
                break;
            case UnitStates.Disengaged:
                MakeTransToDisengaged();
                break;
            case UnitStates.Formation:
                MakeTransToFormation();
                break;
        }
        Debug.Log(calledBy.name + " set " + gameObject.name + " state set to " + state.ToString());
        thisUnitState = state;
    }
    private void MakeTransToIdle() {
        thisAnimator.SetTrigger("Idle");
    }
    private void MakeTransToWaitingForEngage() {
        thisAnimator.SetTrigger("Idle");
    }
    private void MakeTransToEnaging()
    {
        thisAgent.enabled = true;
        thisAgent.SetPath(pathToCurrentTarget);
    }
    private void MakeTransToAttack()
    { 
        Debug.Log("Attacking");
        if (this.thisAgent.enabled)
        {
            thisAgent.enabled = false;
        }
        transform.LookAt(currentTarget.transform.position);
        thisAnimator.SetTrigger("Attacking");
    }
    private void MakeTransToMoving()
    {
        thisAnimator.SetTrigger("Moving");
    }
    private void MakeTransToDisengaged() {
        SetUnitCurrentTarget(null);
        UnitBehaviour unit = thisSquad.RequestForUnitTarget();
        thisAnimator.SetTrigger("Moving");
        if (unit == null) {
            Debug.Log("unit IS " + unit);
            SetUnitState(this.gameObject, UnitStates.Formation);
        }
        else {
            SetUnitCurrentTarget(unit.gameObject);
            SetUnitState(this.gameObject, UnitStates.Engaging);
        }
    }
    private void MakeTransToFormation() {
        thisAnimator.SetTrigger("Moving");
        thisAgent.enabled = true;
        thisAgent.SetDestination(initTransform.position);
    }
    /***************/
    #region Animation Events
    public void FootR() {

    }
    public void FootL()
    {

    }
    public void Hit() {
        if (currentTarget != null) {
            UnitStats enemyUnitStats = currentTarget.GetComponent<UnitStats>();
            if (enemyUnitStats != null) {
                enemyUnitStats.ChangeHealthByAmount(-thisUnitStats.GetDamage());
            }
        }
    }
    #endregion
    /***************/
    private void OnTriggerEnter(Collider collision)
    {
        if (thisUnitState == UnitStates.Engaging || thisUnitState == UnitStates.WaitingToEngage)
        {
            //Debug.Log(gameObject.name + " collided with "+ collision.gameObject.name );
            if (currentTarget != null)
            {
                if (collision.transform.gameObject == currentTarget )
                {
                    SetUnitState(this.gameObject, UnitStates.Attacking);
                }
            }
            else
            {
                if (collision.transform.tag == "Unit")
                {

                    if (collision.transform.parent.tag.StartsWith("Sq") && collision.transform.parent.tag != transform.parent.tag)
                    {
                        currentTarget = collision.gameObject;
                        SetUnitState(this.gameObject, UnitStates.Attacking);
                    }
                }
            }
        }
        
    }
    
    public UnitStates GetCurrentState() {
        return thisUnitState;
    }
    public GameObject GetUnitCurrentTarget() {
        return currentTarget;
    }
    public void SetUnitCurrentTarget(GameObject target)
    {
        if (target != null) {
            pathToCurrentTarget = new NavMeshPath();
            currentTarget = target;
            thisAgent.enabled = true;
            thisAgent.CalculatePath(currentTarget.transform.position, pathToCurrentTarget);
            thisAgent.enabled = false;
        }
        else {
            currentTarget = null;
            pathToCurrentTarget = null;
        }
    }
}
