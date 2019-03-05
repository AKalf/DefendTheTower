using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitBehaviour : MonoBehaviour
{
    [SerializeField]
    SquadStats thisSquadStats = null;

    public enum UnitStates { Idle, Moving, Attacking };
    UnitStates thisUnitState = UnitStates.Moving;
    Animator thisAnimator = null;


    private void Awake()
    {
        thisAnimator = GetComponent<Animator>();
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

    IEnumerator  Die() {
        Debug.Log("unit dies");
        thisSquadStats.RemoveUnitToSquad(this);
        transform.parent = null;
        thisAnimator.SetTrigger("Death");
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

    public void SetUnitState(UnitStates state) {
        switch (state) {
            case UnitStates.Idle:
                MakeTransToIdle();
                break;
            case UnitStates.Moving:
                MakeTransToMoving();
                break;
                case UnitStates.Attacking:
                MakeTransToAttack();
                break;
                //    MakeTransToAttack();
                //      or
                //    MakeTransToEnaging
                //break;
        }
        thisUnitState = state;
    }
    private void MakeTransToIdle() {
        thisAnimator.SetTrigger("Idle");
    }
    private void MakeTransToEngage()
    {
        // configure units 
        // Set destination to each target unit
    }
    private void MakeTransToAttack()
    { 
        Debug.Log("Attacking");
        thisAnimator.SetTrigger("Attacking");
    }
    private void MakeTransToMoving()
    {
        thisAnimator.SetTrigger("Moving");
    }
    public void FootR() {

    }
    public void FootL()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Tower")
        {
            Debug.Log("Attacking 1");
            SetUnitState(UnitStates.Attacking);
        }
    }
}
