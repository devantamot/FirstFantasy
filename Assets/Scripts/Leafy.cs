using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leafy : Enemy
{

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    public Character enemyAIAction(List<Character> heros, List<Character> enemies)
    {
        Debug.Log("Leafy AI Thinking");
        return heros[Random.Range(0, heros.Count)];
    }
}