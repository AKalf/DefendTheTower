using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitBehaviour : MonoBehaviour
{
    
    SquadStats thisSquadStats = null;

    public enum UnitStates { WaitingToEngage, Moving, Engaging, Attacking };
    UnitStates thisUnitState = UnitStates.Moving;
    Animator thisAnimator = null;
    NavMeshAgent thisAgent = null;
    GameObject currentTarget = null; // if in combat state, this will be the target that this unit will fight

    private void Awake()
    {
        thisAnimator = GetComponent<Animator>();
        thisAgent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        thisSquadStats = transform.parent.GetComponent<SquadStats>();
        thisSquadStats.AddUnitToSquad(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Contains all the death functionality
    /// </summary>
    /// <returns></returns>
    IEnumerator  Die() {
        Debug.Log(gameObject.name + " died");
        thisAnimator.SetTrigger("Death");
        transform.parent = null;
        thisSquadStats.RemoveUnitToSquad(this); // remove this unit from the squad   
        if (thisAgent.enabled) {
            thisAgent.SetDestination(transform.position);
        }
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(5); // after 5 seconds
        Destroy(this.gameObject); // destroy this gameobject
    }

    public void SetUnitState(UnitStates state) {

        switch (state) {
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
        }
        //Debug.Log(gameObject.name + " state set to " + state.ToString());
        thisUnitState = state;
    }
    private void MakeTransToWaitingForEngage() {
        thisAnimator.SetTrigger("Idle");

    }
    private void MakeTransToEnaging()
    {
        thisAgent.enabled = true;
        thisAgent.SetDestination(currentTarget.transform.position);
    }
    private void MakeTransToAttack()
    { 
        Debug.Log("Attacking");
        if (this.thisAgent.enabled)
        {
            thisAgent.isStopped = true;
        }
        transform.LookAt(currentTarget.transform.position);
        thisAnimator.SetTrigger("Attacking");
    }
    private void MakeTransToMoving()
    {

        thisAnimator.SetTrigger("Moving");
    }
    /***************/
    #region Animation Events
    public void FootR() {

    }
    public void FootL()
    {

    }
    public void Hit() {

    }
    #endregion
    /***************/
    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log(gameObject.name + " collided with "+ collision.gameObject.name );
        if (currentTarget != null)
        {
            if (collision.transform.gameObject == currentTarget)
            {
                Debug.Log("Attacking 1");
                SetUnitState(UnitStates.Attacking);
            }
        }
        else {
            if (collision.transform.tag == "Unit" && thisUnitState == UnitStates.WaitingToEngage)
            {
                string otherSquadTag = collision.transform.parent.tag;
                if (otherSquadTag.StartsWith("Sq") && otherSquadTag != this.transform.parent.tag)
                {
                    currentTarget = collision.gameObject;
                    SetUnitState(UnitStates.Attacking);
                }
            }
        }
    }
   
    public GameObject GetUnitCurrentTarget() {
        return currentTarget;
    }
    public void SetUnitCurrentTarget(GameObject target)
    {
        currentTarget = target;
    }
}
