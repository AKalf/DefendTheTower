using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles the arrow's behaviour after it had been shot
/// </summary>
public class ArrowBehaviour : MonoBehaviour
{
   // TO DO LIST //
   /*
        - Destroy arrow after time
    */ 

    Rigidbody rb = null;
    Transform arrowPosOnBow = null;
    bool hasShot = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    /// <summary>
    /// add forces to the arrow's rb and removes the parent of the arrow
    /// </summary>
    /// <param name="shootPower"></param>
    /// <param name="upwardsForce" (not used)></param>
    public void ApplyForce(float shootPower, float upwardsForce)
    {
        // add force. The vectors are wrong because our current model has been exported with vectore.up actually being the forward vector
        rb.AddForce(transform.up /* + trans.forward * upwardsForce) */ * shootPower , ForceMode.Impulse);
        // remove parent 
        transform.parent = null;
        hasShot = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Body" || other.tag == "Head")
        {
            rb.constraints = RigidbodyConstraints.FreezeAll; // set kinematic to true to stop movement
            transform.position = other.ClosestPointOnBounds(transform.position);
            transform.parent = other.transform; // so the arrow follows the gameobject that is pinned to
            
            Destroy(transform.GetComponent<Collider>()); // destroy the collider so there are no other interactions
        }        
        else if (other.isTrigger)
        {

        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up * 0.1f, 0.1f);
            rb.constraints = RigidbodyConstraints.FreezeAll; // set kinematic to true to stop movement            
            Destroy(transform.GetComponent<Collider>()); // destroy the collider so there are no other interactions
        }
       
    }
    /// <summary>
    /// set where the arrow should be placed on bow when spawned
    /// </summary>
    /// <param name="arrowPointTrans"></param>
    public void SetArrowPosOnBow(Transform arrowPointTrans) {
        arrowPosOnBow = arrowPointTrans;
    }
}
