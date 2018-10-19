using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private Transform transform;
    // Use this for initialization
    void Awake()
    {
        transform = GetComponent<Transform>();
    }

    public void updatePosition(Character g) {
        transform.position = new Vector3(g.transform.position.x - g.GetComponent<SpriteRenderer>().bounds.size.x/3, g.transform.position.y, 0f);
    }

    public void rescale(float scale)
    {
        transform.localScale.Set(scale, 0, 0);
    }

    public void relocate(float xdir, float ydir)
    {
        transform.Translate(xdir, ydir, 0);
    }

    public void reposition(Vector3 p)
    {   
        transform.position = p;
    }

    public void selfDestruct()
    {
        Destroy(this.gameObject);
    }

    public void setActivity(bool b)
    {
        this.gameObject.SetActive(b);
    }
}
