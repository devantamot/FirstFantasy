using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    private GlowText[] textItems;
    public static int statFields = 3;
    public GlowText textRef;

	// Use this for initialization
	void Start () {
		
	}

    public void setup(Character player)
    {
        textItems = new GlowText[statFields];
        for(int count = 0; count < statFields; count++)
        {
            textItems[count] = Instantiate(textRef);
            textItems[count].transform.SetParent(this.transform);
            textItems[count].rectTransform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
            textItems[count].setPulse(false);
        }

        textItems[0].text = player.charName;
        textItems[1].text = player.currentHealth + "";
        textItems[2].text = player.currentMana + "";

    }

    public void setActive(bool b)
    {
        Debug.Log("CharacterStat, changing current active player!: " + b);
        for (int count = 0; count < statFields; count++)
        {
            textItems[count].updateGlow(b);
        }
    }
}
