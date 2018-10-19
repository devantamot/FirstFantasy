using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    public static int IDN = 0;

    private int ID;

    public int maxHealth;
    public int currentHealth;
    public int maxMana;
    public int currentMana;
    public int strength;
    public int defense;
    public int speed;

    public string charName; //TODO Have the player name these at the beginning of the game

    protected TYPE MYTYPE;


    [HideInInspector] public Animator animatior;

    void Awake()
    {
        ID = IDN;
        IDN++;
    }

    // Use this for initialization
    protected virtual void Start () {
        animatior = GetComponent<Animator>();      

    }

    public Animator getAnimator() {
        return animatior;
    }

    public Vector3 getPosition()
    {
        return this.transform.position;
    }

    public TYPE getType()
    {
        return MYTYPE;
    }

    public int getStrength()
    {
        return strength;
    }

    public int getDefense()
    {
        return defense;
    }

    public void adjustHealth(int adjustment)
    {
        currentHealth += adjustment;
    }

    public void setActive(bool b) {

        if (MYTYPE == TYPE.PLAYER)
        {
            ((Player)this).setPlayerStatus(b);
        }
        else
        {
            //((Enemy)this).setPlayerStatus(true);
        }
    }
}
