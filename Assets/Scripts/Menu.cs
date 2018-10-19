using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private GlowText[] menuItems; // keeps track of each item
    public int numItems; // counts the number of items
    public GlowText textRef; // for the 
    protected RectTransform rectTransform;

    private void Awake()
    {
        setActivity(false);
        rectTransform = GetComponent<RectTransform>();
    }

    /*
     * Adds itself to the Canvas UI
     * Adds the text elemets to the menu
     */
    public void setUp(Canvas canvas, int numI, String[] items)
    {
        this.transform.SetParent(canvas.transform);
        menuItems = new GlowText[numI];
        numItems = numI;
        for(int count = 0; count < numI; count++) {
            menuItems[count] = Instantiate(textRef);
            menuItems[count].transform.SetParent(this.transform);
            menuItems[count].rectTransform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
            menuItems[count].text += items[count]; 
        }

        menuItems[0].updateGlow(true);
        this.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    /*
     * Makes it so the option switches of the desired index. Mainly for CMDMENU
     */ 
    public void optionSwitch(int index, Boolean b)
    {
        if (b)
        {
            menuItems[index].updateGlow(true);
        }
        else {
            menuItems[index].fontStyle = FontStyle.Normal;
            menuItems[index].updateGlow(false);
        }
    }

    /*
     * Mainly for EnemyPanel
     */
    public void removeItem(int index)
    {

    }

    /*
    * Mainly for EnemyPanel
    */
    public void setGlow(bool b)
    {
        for (int count = 0; count < menuItems.Length; count++)
        {
            menuItems[count].setPulse(b);
        }
    }

    /*
     * Used to set the visibility of the menu
     */
    public void setActivity(bool b)
    {       
        this.gameObject.SetActive(b);

    }
}
