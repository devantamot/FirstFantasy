using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManger : MonoBehaviour
{
    public static int scale = 50; // Helps with the sizing of the game

    private List<Character> players; // list of players
    private List<Character> enemies; // list of enemies 

    private CharacterNode battleQueue; // list of player teams
    private CharacterNode battleQueueController; // a pointer to person whos turn it is

    //Battle Characters
    public Monk monk;
    public Swordsmen swordsmen;
    public Leafy leafy;

    // A cursor for the Heros
    public Cursor heroCursor;
    private float heroCursorDelay;
    private float lastCursorMove;

    private TYPE turn;

    enum Command { ATTACK, MAGIC, ITEMS, NONE }; // Commands that players can do

    enum Direction { UP, DOWN, LEFT, RIGHT }; //Used for going up down left right when selecting targets or when selecting menu items

    private int currentHeroOptionIndex; //The index selected (for the cursor)
    private Command selectedCommand; // used when the player has selected something with a multi tier menu option. 
                                     // used for update control.
    private Command inputWaiter; // will wait for the desired command input module to return a selection/action
    
    // A cursor for the enemies 
    public Cursor targetCursor;
    private float targetCursorDelay;
    //private float lastCursorMove;
    private List<Character> targetList;
    private Character dummyChar;
    private int targetSelectorIndex;

    public Canvas canvas; // Canvas for UI elements

    public PlayerPanel plyPanel; // Used to keep track of the players 
    public EnemyPanel enemyPanel;

    void Awake()
    {
        Debug.Log("Game Starting");

        players = new List<Character>();
        enemies = new List<Character>();
        battleQueue = null;

        /* starting up the battle heros*/
        Debug.Log("Setting up Characters");
        monk = Instantiate(monk, new Vector3(4.25f * scale, 1f * scale, 0), Quaternion.identity);
        swordsmen = Instantiate(swordsmen, new Vector3(4.5f * scale, 0f, 0), Quaternion.identity);

        leafy = Instantiate(leafy, new Vector3(-4f * scale, 0f, 0), Quaternion.identity);

        players.Add(monk);
        players.Add(swordsmen);

        enemies.Add(leafy);

        addToQueue(monk);
        addToQueue(swordsmen);
        addToQueue(leafy);

        //points the queue pointer to the first on in the queue
        battleQueueController = battleQueue;

        //setting up the cursor
        Debug.Log("Setting up hero cursor");
        heroCursorDelay = 0.25f;
        lastCursorMove = Time.time;
        currentHeroOptionIndex = 0;

        //setting up the enemy cursor
        Debug.Log("Setting up the enemy cursor");
        targetCursorDelay = 0.25f;
        lastCursorMove = Time.time;

        //sets the first person in the queue as the person with the first turn
        battleQueueController.getCharacterRef().setActive(true);
        turn = battleQueueController.getCharacterRef().getType();

        // Menu Set up 
        plyPanel = Instantiate(plyPanel);
        plyPanel.setUp(canvas, players.Count, players);
        plyPanel.setActivity(true);

        enemyPanel = Instantiate(enemyPanel);
        string[] enemyListNames = new string[enemies.Count];

        for(int count = 0; count < enemies.Count; count++)
        {
            enemyListNames[count] = enemies[count].charName;
        }
        
        enemyPanel.setUp(canvas, enemies.Count, enemyListNames);
        enemyPanel.setActivity(true);
        enemyPanel.setGlow(false);
        
        monk.setShitUp(canvas);
        swordsmen.setShitUp(canvas);

        // It it's the players turn first, the cursor will point to the player
        if (turn == TYPE.PLAYER)
        {
            setUpCursor(battleQueueController.getCharacterRef(), true, 1.5f * scale, 0.5f * scale);
            plyPanel.changeCurrentActivePlayer(players.IndexOf(battleQueueController.getCharacterRef()), true);
        }

        setUpCursor(enemies[0], false, 1.5f * scale, 0.5f * scale);//sets up the target cursor temporarily
        targetCursor.setActivity(false);

        selectedCommand = Command.NONE;
        inputWaiter = Command.NONE;
    }

    // Update is called once per frame
    // TODO Implement when game is over
    //
    void Update()
    {
        /*once the character says they are over, it's myTurn is false,
        * it's turnOver is reset and the battleQueueController does to the next person
        * in the queue
        */
        if(turn == TYPE.PLAYER)
        {
            Debug.Log("Players Turn");
            if (inputWaiter == Command.NONE)
            {
                Command currentUserInput = cmdInputCheck(Time.time); // gets current time
                if (!heroCursor.gameObject.active)
                {
                    heroCursor.gameObject.SetActive(true);
                }

                if (currentUserInput != Command.NONE)
                {
                    if (currentUserInput == Command.ATTACK)
                    {
                        //battleQueueController.getCharacterRef().getAnimator().SetTrigger("attack");
                        inputWaiter = Command.ATTACK;
                        targetList = enemies;
                        targetSelectorIndex = 0;
                        targetCursor.setActivity(true);
                        //targetCursor.reposition(targetList[targetSelectorIndex].getPosition());
                        targetCursor.updatePosition(targetList[targetSelectorIndex]);
                        //targetCursor.relocate(1.5f * scale, 0.5f * scale);
                        Debug.Log("Calling for Attack Input");
                    }
                    else if ((currentUserInput == Command.MAGIC))
                    {
                        inputWaiter = Command.MAGIC;
                        Debug.Log("Calling for Magic Input");

                    }
                    else if ((currentUserInput == Command.ITEMS))
                    {
                        inputWaiter = Command.ITEMS;
                        Debug.Log("Calling for Items Input");
                    }



                }
            }
            else if (inputWaiter == Command.ATTACK)
            {
                Debug.Log("Waiting for Attack Input");
                dummyChar = targetSelection(Time.time);
                if (dummyChar != null)
                {
                    //Debug.Log("Got Target");
                    registerAttack(battleQueueController.getCharacterRef(), dummyChar);
                    turnOver();
                    inputWaiter = Command.NONE;
                }
                
            }
            else if (inputWaiter == Command.MAGIC)
            {
                Debug.Log("Waiting for Magic Input");
                inputWaiter = Command.NONE;
            }
            else if(inputWaiter == Command.ITEMS)
            {
                Debug.Log("Waiting for Item Input");
                inputWaiter = Command.NONE;
            }

        }
        else if(turn == TYPE.ENEMY)
        { 
            Debug.Log("Enemy Turn");
            if (heroCursor.gameObject.active) {
                heroCursor.gameObject.SetActive(false);  
            }
            registerAttack(battleQueueController.getCharacterRef(), ((Enemy)battleQueueController.getCharacterRef()).enemyAIAction(players, enemies));
            turnOver();
        }
    }

    private void registerAttack(Character attacker, Character target)
    {
        attacker.getAnimator().SetTrigger("attack");
        target.adjustHealth(target.getDefense() + (-1 * attacker.getStrength()));
        target.getAnimator().SetTrigger("hit");
    }

    /**
    * Disables the current turn player and moves on to the next
    */
    private void turnOver()
    {

        Debug.Log("Disabling " + battleQueueController.getCharacterRef().charName);

        if (turn == TYPE.PLAYER)
        {
            plyPanel.changeCurrentActivePlayer(players.IndexOf(battleQueueController.getCharacterRef()), false);
        }

        battleQueueController.getCharacterRef().setActive(false);
        battleQueueController = battleQueueController.getNext();
        battleQueueController.getCharacterRef().setActive(true);

        Debug.Log("Enabling " + battleQueueController.getCharacterRef().charName);

        turn = battleQueueController.getCharacterRef().getType();

        if (turn == TYPE.PLAYER)
        {
            heroCursor.reposition(battleQueueController.getCharacterRef().getPosition());
            heroCursor.relocate(1.5f * scale, 0.5f * scale);
            plyPanel.changeCurrentActivePlayer(players.IndexOf(battleQueueController.getCharacterRef()), true);
        } else if(turn == TYPE.ENEMY)
        {
            heroCursor.gameObject.SetActive(false);
        }
    }

    /*
     * Checks the input from the user on the cmd panel. 
     */
    private Command cmdInputCheck(float currentTime)
    {
        //determine if it is time for the cursor to be moved
        if ((currentTime - lastCursorMove) > heroCursorDelay)
        {
            //TODO Make vert global so i don't have to do this shit again
            Player current = (Player)battleQueueController.getCharacterRef();
            int vert = (int)Input.GetAxisRaw("Vertical");//gets up and down input for the battle

            if (vert != 0)
            {
                lastCursorMove = Time.time;
                //moving cursor down
                if (vert < 0)
                {
                    current.cmdPanel.optionSwitch(currentHeroOptionIndex, false);
                    currentHeroOptionIndex++;
                    if (currentHeroOptionIndex >= current.cmdPanel.numItems)
                    {
                        currentHeroOptionIndex = 0;
                    }
                }
                //moving cursor up
                else if (vert > 0)
                {
                    current.cmdPanel.optionSwitch(currentHeroOptionIndex, false);
                    currentHeroOptionIndex--;
                    if (currentHeroOptionIndex < 0)
                    {
                        currentHeroOptionIndex = current.cmdPanel.numItems - 1;
                    }
                }
                current.cmdPanel.optionSwitch(currentHeroOptionIndex, true);
            }
            else
            {
                /*
                 * If the user hits the action command button, then it will 
                 * get whatever option is selctedand return it.  
                 */
                if (Input.GetKey(KeyCode.J))
                {
                    lastCursorMove = Time.time;

                    if (currentHeroOptionIndex == (int)Command.ATTACK)
                    {
                        Debug.Log("Selcted Attack");
                        return Command.ATTACK;
                    }
                    else if (currentHeroOptionIndex == (int)Command.MAGIC)
                    {
                        Debug.Log("Selected Magic");
                        return Command.MAGIC;
                    }
                    if (currentHeroOptionIndex == (int)Command.ITEMS)
                    {
                        Debug.Log("Selected Items");
                        return Command.ITEMS;
                    }
                }
            }
        }

        return Command.NONE;
    }

    /*
     * Will go through enemy list or hero list (Depending on if the user will hit the left or right button)
     * and will return the character selected if the user hits the action button.
     * It will return null if the user changes their mind and hits the back button. 
     */
    private Character targetSelection(float currentTime)
    {
        Debug.Log("In targetSelection");
        if ((currentTime - lastCursorMove) > targetCursorDelay)
        {
            int vert = (int)Input.GetAxisRaw("Vertical");//gets up and down input for the battle
            int horz = (int)Input.GetAxisRaw("Horizontal");//gets left and right input for the battle
            if (vert != 0)
            {
                Debug.Log("Moved Vert In targetSelection");
                lastCursorMove = Time.time;
                //moving cursor down
                if (vert < 0)
                {
                    targetSelectorIndex++;
                    if (targetSelectorIndex >= targetList.Count)
                    {
                        targetSelectorIndex = 0;
                    }
                }
                //moving cursor up
                else if (vert > 0)
                {
                    targetSelectorIndex--;
                    if (targetSelectorIndex < 0)
                    {
                        targetSelectorIndex = targetList.Count - 1;
                    }
                }
                //targetCursor.reposition(targetList[targetSelectorIndex].getPosition());
                targetCursor.updatePosition(targetList[targetSelectorIndex]);
            }
            else if (horz != 0)
            {
                Debug.Log("Moved Horz In targetSelection");
                lastCursorMove = Time.time;

                if (horz != 0)
                {
                    if (targetList == enemies)
                    {
                        Debug.Log("Moved Target Cursor to Heros");
                        targetList = players;
                    }
                    else
                    {
                        Debug.Log("Moved Target Cursor to Enemies");
                        targetList = enemies;
                    }
                    // TODO make sure that the person doesn't select someone who is dead. Add this later 
                    targetSelectorIndex = 0;
                    //targetCursor.reposition(targetList[targetSelectorIndex].getPosition());
                    targetCursor.updatePosition(targetList[targetSelectorIndex]);
                }
            }
            else
            {
                Debug.Log("Action Keypress In targetSelection");
                if (Input.GetKey(KeyCode.J))
                {
                    lastCursorMove = Time.time;
                    Debug.Log("Target Selection: " + targetList[targetSelectorIndex].name);
                    //turnOver(); Should probably have this elsewhere 
                    targetCursor.setActivity(false);
                    return targetList[targetSelectorIndex];
                }
                else if (Input.GetKey(KeyCode.K))
                {
                    targetCursor.setActivity(false);
                    inputWaiter = Command.NONE;
                }
            }
        }
        //TODO remember to call turn over at the end of the players turn. 
        return null;
    }

    /**
    * Sets up the cursor in the desried position on character c
    */
    private void setUpCursor(Character c, bool isHero, float xdir, float ydir)
    {
        if (isHero)
        {
            heroCursor = Instantiate(heroCursor, c.getPosition(), Quaternion.identity);
            heroCursor.relocate(xdir, ydir);
            heroCursor.gameObject.name = "Hero Cursor";
        }
        else
        {
            targetCursor = Instantiate(targetCursor, c.getPosition(), Quaternion.identity);
            targetCursor.transform.Rotate(0, 180f, 0, Space.Self);
            targetCursor.gameObject.name = "Target Cursor";
        }
        
    }

    //inner class used as the character queue
    public class CharacterNode {

        private CharacterNode next;
        private Character characterRef;

        public CharacterNode(Character c, CharacterNode next) {
            characterRef = c;
            this.next = next;

        }

        public Character getCharacterRef() {
            return characterRef;
        }

        public CharacterNode getNext() {
            return next;
        }

        public void setNextRef(CharacterNode newRef) {
            next = newRef;
        }

        public bool equals(CharacterNode c) {
            if(c.getCharacterRef() == this.characterRef)
                return true;
            return false;
        }
    }

    private void addToQueue(Character c) {

        if (battleQueue == null)
        {
            battleQueue = new CharacterNode(c, battleQueue);
            battleQueue.setNextRef(battleQueue);
        }
        else
        {
            CharacterNode current = battleQueue;

            //keep traversing as long as c.speed is greater than the one next to it and
            //the next node is not the head node
            while (c.speed >= current.getCharacterRef().speed && !battleQueue.equals(current.getNext())) {
                current = current.getNext();
            }

            current.setNextRef(new CharacterNode(c, current.getNext()));
        }
    }

    /*TODO create a circular linked list implementation that has each node a character and it is sorted by speed stats to determine
     who will go into the list first. Then have a pointer node that will transfer from each node and that will be the turn
     node. This is much more effecient and is a hell of a lot more organized.*/
}
