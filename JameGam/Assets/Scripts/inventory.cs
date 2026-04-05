using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour
{
    Player_Script player;
    public Image inventoryImage;
    int numOrbs=0;
    public Image[] orbSprites; // Array of sprites for different orb counts
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame

    void Start()
    {
        player = FindAnyObjectByType<Player_Script>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !player.isCutSceneActive)
        {
            inventoryImage.gameObject.SetActive(!inventoryImage.gameObject.activeSelf); // Toggle inventory visibility
        }
    }

    public void addOrb()
    {
        orbSprites[numOrbs].enabled = true; // Enable the sprite for the current orb count
        numOrbs++;
    }

    public void resetOrbs()
    {
        for (int i = 0; i < orbSprites.Length; i++)
        {
            orbSprites[i].enabled = false;
        }
        numOrbs = 0; // Reset orb count
    }
}
