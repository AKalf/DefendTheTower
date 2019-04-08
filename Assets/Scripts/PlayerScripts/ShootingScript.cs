using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    [SerializeField][Tooltip ("The starting force that the arrow will be shooted")]
    float shootPower = 10.0f;
    //[SerializeField]
    //[Tooltip("In order to give a curvy path to the arrow we can apply an upward force when arrow is shot")]
    //float upwardsForce = 0;
    [SerializeField][Tooltip ("how much time until next arrow is spawned")]
    float reloadTime = 5.0f; 
    [SerializeField][Tooltip ("How fast will the shooting power will be increased when holding left click")]
    float pullSpeed = 0.1f;
    [SerializeField][Tooltip("How much extra shooting power can be added when pulling")]
    int maxPullAmount = 10;
    [SerializeField][Tooltip("The prefab to shoot")]
    GameObject arrowPrefab;
    [SerializeField][Tooltip("Where at the bow arrows should spawn")]
    GameObject arrowPoint;
    [SerializeField]
    [Tooltip("Available arrows to shoot")]
    int numberOfArrows = 10;
 

    GameObject arrow; // variable that holds the new created arrows
    AudioSource stringSource = null;
    bool reloaded = false; 
    int pullAmount = 0; // how much is the string of the arrow currently pulled
    float timeSinceShot = 0; // counts the time scince the last shot

    private void Start()
    {
        GUIManager.GetInstance().SetMaxMana(maxPullAmount);
        stringSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (numberOfArrows > 0)
        {
            if (!reloaded)
            {
                ReloadArrow();
            }
            else
            {
                ShootLogic();
            }
        }
       
    }
    /// <summary>
    /// Counts the time until reload is finished and spawns the arrow if enough time passed
    /// </summary>
    void ReloadArrow() {
        timeSinceShot += Time.deltaTime;
        if (timeSinceShot >= reloadTime)
        {
            timeSinceShot = 0;
            reloaded = true;
            SpawnArrow();
        }
    }
    /// <summary>
    /// Create a new arrow at the location of arrowPoint. Set this gameobject as parent of the arrow.
    /// </summary>
    void SpawnArrow() {
        if (reloaded) {
            arrow = Instantiate(arrowPrefab, arrowPoint.transform.position, arrowPoint.transform.rotation);
            arrow.GetComponent<ArrowBehaviour>().SetArrowPosOnBow(arrowPoint.transform);
            arrow.transform.parent = transform.parent;
        }
    }

    void ShootLogic() {
        
        // if pull amount gets more than allowed set it to max
        if (pullAmount > maxPullAmount + 1) {
            pullAmount = maxPullAmount + 1;           
        }
        // if left click is pressed, increase pull amount
        if (Input.GetMouseButton(0) && pullAmount <= maxPullAmount) {
            if (!stringSource.isPlaying) {
                stringSource.Play();
            }
            int newPullAmount = (int)(Time.deltaTime * pullSpeed);            
            pullAmount += (int)newPullAmount;       
        }
        // if left click is releashed, tell the arrow to addforce and start reloading
        if (Input.GetMouseButtonUp(0)) {
            if (stringSource.isPlaying)
            {
                stringSource.Stop();
            }
            arrow.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;          
            arrow.GetComponent<ArrowBehaviour>().ApplyForce(shootPower * ((pullAmount/ maxPullAmount) + 0.01f), 0);
            pullAmount = 0;
            UIManager.GetInstance().AddPassedEnemy();
            numberOfArrows--; 
            reloaded = false;
        }
        GUIManager.GetInstance().InformPlayerManaSlider(pullAmount); // inform the GUIManager to change the U.I element
    }
}
