using UnityEngine;

public class cutscene : MonoBehaviour
{
    public Player_Script playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void launchSpecialmove()
    {
        playerScript.currentCutSceneEnd();
    }

    // Update is called once per frame

}
