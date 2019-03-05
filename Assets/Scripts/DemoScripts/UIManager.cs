using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    [SerializeField]
    Text pullAmountText;

    [SerializeField]
    Text enemiesKilledText = null;
    [SerializeField]
    Text enemiesPassedText = null;

    int enemiesKilled = 0;
    int enemiesPassed = 0;


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
    public void SetPullAmountText(float amount) {
        pullAmountText.text = "Pull amount" + amount.ToString();
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
        enemiesPassed++;
        enemiesPassedText.text = "Enemies passed:" + enemiesPassed.ToString();
    }
}
