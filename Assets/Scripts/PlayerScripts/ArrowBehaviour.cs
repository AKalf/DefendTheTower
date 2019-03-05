using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
   
   
    Rigidbody rb = null;
    Transform arrowPosOnBow = null;
    bool hasShot = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasShot)
        {
            SpinObjectInAir();
        }

        
    }
    public void ApplyForce(float shootPower, float upwardsForce)
    {
        rb.AddForce(transform.up /* + trans.forward * upwardsForce) */ * shootPower , ForceMode.Impulse);
        transform.parent = null;
        hasShot = true;
    }
    void SpinObjectInAir()
    {
        //float xVelocity = rb.velocity.x;
        //float yVelocity = rb.velocity.y;
        //float zVelocity = rb.velocity.z;
        //float combinedVelocity = Mathf.Sqrt(xVelocity * xVelocity + zVelocity * zVelocity);
        //float fallAngle = (-1 * Mathf.Atan2(yVelocity, combinedVelocity) * 180 / Mathf.PI);

        //transform.eulerAngles = new Vector3(fallAngle, transform.eulerAngles.y, transform.eulerAngles.z);
       // Quaternion newQuat = new Quaternion(transform.rotation.x + 0.1f, transform.rotation.y, transform.rotation.z, transform.rotation.w);
       // transform.rotation = Quaternion.Slerp(transform.rotation, newQuat, Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Squad") {
            return;
        }
        rb.isKinematic = true;
        if (other.transform.tag == "Unit")
        {
            Debug.Log("collided with unit");
            UIManager.GetInstance().AddKilledEnemy();
            other.GetComponent<UnitBehaviour>().StartCoroutine("Die");
        }
        Destroy(transform.GetComponent<Collider>());
    }
    public void SetArrowPosOnBow(Transform arrowPointTrans) {
        arrowPosOnBow = arrowPointTrans;
    }
}
