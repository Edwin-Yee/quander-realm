using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnButton : MonoBehaviour
{
    public GameObject startPanel, recipePanel;

    public void ReturnToStartPage()
    {
        startPanel.SetActive(true);
        recipePanel.SetActive(false);
    }
}
