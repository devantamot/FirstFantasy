using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GlowText : Text {

    private GlowText glowText;
    private Boolean shouldPulse;
    private Boolean shouldGlow;
    private Boolean isGlowing;

    private float timelapsed;
    public float waitTime = 500000.0f;

	// Use this for initialization
	void Awake () {
        glowText = this.GetComponent<GlowText>();
        shouldGlow = false;
        isGlowing = false;
        timelapsed = Time.time;
        shouldPulse = true;
	}

    public void updateGlow(Boolean b) {
        shouldGlow = b;
        if (shouldGlow)
        {
            glowText.fontSize += 1;
            glowText.fontStyle = FontStyle.Normal;
        }
        else
        {
            glowText.fontSize -= 1;
            glowText.fontStyle = FontStyle.Normal;
        }
    }

    public void setPulse(bool b)
    {
        shouldPulse = b;
    }

	// Update is called once per frame
	void Update () {

        if (shouldPulse)
        {
            float currentTime = Time.time;
            //Debug.Log(currentTime - timelapsed);
            if ((currentTime - timelapsed > 0.5f) && shouldGlow)
            {
                Debug.Log("meep");
                timelapsed = Time.time;
                if (!isGlowing)
                {
                    glowText.fontStyle = FontStyle.Bold;
                    glowText.fontSize += 1;
                    isGlowing = true;
                }
                else
                {
                    glowText.fontStyle = FontStyle.Normal;
                    glowText.fontSize -= 1;
                    isGlowing = false;
                }

            }
        }
	}
}
