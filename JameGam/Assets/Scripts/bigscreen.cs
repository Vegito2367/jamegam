using UnityEngine;

public class bigscreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Player_Script playerScript;

    public void afterFadeIn()
    {   
        playerScript.currentCutSceneStart(playerScript.currentAnimation);
    }

    public void afterFadeOut()
    {
        Debug.Log("Fade out completed, ending cutscene");
    }
}
