using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureStatsScript : MonoBehaviour
{
    
    [SerializeField]
    int maxHealth = 100;
    [SerializeField]
    int currentHealth = 0;
    int currentLevel = 0;
    // int maxLevel
    // AudioSource

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        
        if (tag.EndsWith("te")) {
            GUIManager.GetInstance().SetPlayerMaxHealth(maxHealth);
            GUIManager.GetInstance().InformPlayerHPSlider(currentHealth);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // check if health <= 0 and to call EndGame function
    public void ChangeHealthByAmount(int amount)
    {
        currentHealth += amount;
        GUIManager.GetInstance().InformPlayerHPSlider(currentHealth);
        if (currentLevel <= 0) {
            Debug.Log("You lost");

        }
       
        // call U.I manager to change the GUI with the new health of the tower (when we have GUI)
    }
    // GetCurrentHealth

    // LevelUp (int levelToGo)
        // TO-DO: change models, stats, etc (step for future)
}
