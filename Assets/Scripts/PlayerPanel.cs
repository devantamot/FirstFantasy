using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanel : Menu
{
    private CharacterStats[] characterStats;
    public CharacterStats charStatsRef;



    public void setUp(Canvas canvas, int numI, List<Character> players)
    {
        Debug.Log("Setting up Player Panel");
        this.transform.SetParent(canvas.transform);
        characterStats = new CharacterStats[numI];
        numItems = numI;
        for (int count = 0; count < numI; count++) {
            characterStats[count] = Instantiate(charStatsRef);
            characterStats[count].transform.SetParent(this.transform);
            characterStats[count].transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
            characterStats[count].setup(players[count]);
        }
        //this.rectTransform.position.Set(-191.27f, 95f, -109.7089f);
        Debug.Log(rectTransform.anchoredPosition);
        rectTransform.anchoredPosition = new Vector2(-225.61f, 95f);
        //this.rectTransform.pivot.Set(-191.27f, 95f);

        Debug.Log(rectTransform.anchoredPosition);

        //this.transform.SetPositionAndRotation(new Vector3(-188.95f, -289f, -109.708f), Quaternion.identity);
        this.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void changeCurrentActivePlayer(int index, bool b)
    {
        Debug.Log("PLayerPanel Changing current active player");
        characterStats[index].setActive(b);
    }
}
