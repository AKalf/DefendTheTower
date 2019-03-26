using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    [SerializeField]
    Text enemiesKilledText = null;
    [SerializeField]
    Text arrowsShotText = null;

    int enemiesKilled = 0;
    int arrowsShot = 0;


    static UIManager thisInstance = null;
    // Start is called before the first frame update
    private void Awake()
    {
        if (thisInstance == null)
        {
            thisInstance = this;
        }
        else {
            Destroy(this);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static UIManager GetInstance() {
        return thisInstance;
    }
    public void AddKilledEnemy()
    {
        enemiesKilled++;
        enemiesKilledText.text = "Enemies killed:" + enemiesKilled.ToString();
    }
    public void AddPassedEnemy()
    {
        arrowsShot++;
        arrowsShotText.text = "Arrows shot: " + arrowsShot.ToString();
    }
}
