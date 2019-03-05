using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    [SerializeField]
    float shootPower = 10.0f;
    //[SerializeField]
    //[Tooltip("In order to give a curvy path to the arrow we can apply an upward force when arrow is shot")]
    //float upwardsForce = 0;
    [SerializeField]
    float reloadTime = 5.0f;
    [SerializeField]
    float pullSpeed = 0.1f;
    [SerializeField]
    float maxPullAmount = 10;
    [SerializeField]
    GameObject arrowPrefab;
    [SerializeField]
    GameObject arrowPoint;
    [SerializeField]
    int numberOfArrows = 10;
 

    GameObject arrow;
    bool reloaded = false;
    float pullAmount = 0;
    float timeSinceShot = 0;

    private void Update()
    {
        
        if (!reloaded)
        {
            timeSinceShot += Time.deltaTime;
            if (timeSinceShot >= reloadTime)
            {
                timeSinceShot = 0;
                reloaded = true;
                SpawnArrow();
            }
        }
        else {
            ShootLogic();
        }
       
    }
    void SpawnArrow() {
        if (reloaded) {
            arrow = Instantiate(arrowPrefab, arrowPoint.transform.position, arrowPoint.transform.rotation);
            arrow.GetComponent<ArrowBehaviour>().SetArrowPosOnBow(arrowPoint.transform);
            arrow.transform.parent = transform.parent;
        }
    }

    void ShootLogic() {
        if (numberOfArrows > 0) {
            if (pullAmount > maxPullAmount) {
                pullAmount = maxPullAmount;
                UIManager.GetInstance().SetPullAmountText(pullAmount);
            }

            if (Input.GetMouseButton(0) && pullAmount < maxPullAmount) {

                pullAmount += Time.deltaTime * pullSpeed;
                UIManager.GetInstance().SetPullAmountText(pullAmount);
            }
            if (Input.GetMouseButtonUp(0)) {
                arrow.GetComponent<Rigidbody>().isKinematic = false;
               
                arrow.GetComponent<ArrowBehaviour>().ApplyForce(shootPower * ((pullAmount/ maxPullAmount) + 0.05f), 0);
                pullAmount = 0;
                pullAmount += Time.deltaTime * pullSpeed;
                numberOfArrows--;
                reloaded = false;
            }
            
        }

    }
}
