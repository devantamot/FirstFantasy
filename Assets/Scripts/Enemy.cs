using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    void Awake()
    {
        base.MYTYPE = TYPE.ENEMY;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    /* 
     * reutnr negative for adding health return plus for subtracting health 
     * Default attack
     */
     public Character enemyAIAction(List<Character> heros, List<Character> enemies)
     {
        Debug.Log("Default Enemy Attack");
        return heros[Random.Range(0, heros.Count)];
     }
    /*
     * MAKE SURE THE LAST ANIMATION IS DONE BEFORE YOU CONTINUE 
     */
}

