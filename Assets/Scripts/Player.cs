using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Character {

    public Menu cmdPanel;
    public Canvas kanvas;
    public string[] commands = {"Attack", "Magic", "Items"};

    void Awake()
    {
        
        base.MYTYPE = TYPE.PLAYER;
        cmdPanel = Instantiate(cmdPanel);
    }

    public void setShitUp(Canvas canvas) {
        cmdPanel.setUp(canvas, commands.Length, commands);
        //cmdPanel.transform.SetPositionAndRotation(new Vector3(29.325f, 95f, -277.505f), Quaternion.identity);


    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void setPlayerStatus(bool status)
    {
        cmdPanel.setActivity(status);
    }
}
