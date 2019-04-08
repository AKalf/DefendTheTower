using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDmgManager : MonoBehaviour
{
    [SerializeField]
    int[] headDamage = new int[3];
    [SerializeField]
    int[] bodyDamage = new int[3];



    static ArrowDmgManager inst = null;


    public static ArrowDmgManager GetInstance() {
        return inst;
    }

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Humans
    public int GetHumanHead() {
        return headDamage[0];
    }
    public int GetHumanBody()
    {
        return bodyDamage[0];
    }
    // Elves
    public int GetElfHead()
    {
        return headDamage[1];
    }
    public int GetElfBody()
    {
        return bodyDamage[1];
    }
    // Dwarves
    public int GetDwarfHead()
    {
        return headDamage[2];
    }
    public int GetDwarfBody()
    {
        return bodyDamage[2];
    }
}
