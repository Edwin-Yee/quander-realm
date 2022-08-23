using System;
//using Wrapper;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// Script adapted from: https://pressstart.vip/tutorials/2019/06/1/96/level-selector.html

namespace QueueBits
{
    public class LevelSelect : MonoBehaviour
    {
        public GameObject levelHolder;
        public GameObject iconStar0;
        public GameObject iconStar1;
        public GameObject iconStar2;
        public GameObject iconStar3;
        public GameObject thisCanvas;
        public Vector2 iconSpacing;
        private Rect panelDimensions;
        private Rect iconDimensions;
        private int amountPerPage;
        private int currentLevelCount;

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Load();

            if (GameManager.saveData.dialogueSystem[0])
            {
                Wrapper.Events.StartDialogueSequence?.Invoke("QB_Intro");
                GameManager.saveData.dialogueSystem[0] = false;
                GameManager.Save();
            }

            panelDimensions = levelHolder.GetComponent<RectTransform>().rect;
            iconDimensions = iconStar3.GetComponent<RectTransform>().rect;
            int maxInARow = Mathf.FloorToInt((panelDimensions.width + iconSpacing.x) / (iconDimensions.width + iconSpacing.x));
            int maxInACol = Mathf.FloorToInt((panelDimensions.height + iconSpacing.y) / (iconDimensions.height + iconSpacing.y));
            amountPerPage = maxInARow * maxInACol;
            int totalPages = Mathf.CeilToInt((float)(GameManager.saveData.starSystem.Length - 1)/ amountPerPage);
            LoadPanels(totalPages);
        }

        void LoadPanels(int numberOfPanels)
        {
            GameObject panelClone = Instantiate(levelHolder) as GameObject;

            for (int i = 1; i <= numberOfPanels; i++)
            {
                GameObject panel = Instantiate(panelClone) as GameObject;
                panel.transform.SetParent(thisCanvas.transform, false);
                panel.transform.SetParent(levelHolder.transform);
                panel.name = "Page-" + i;
                panel.GetComponent<RectTransform>().localPosition = new Vector2(panelDimensions.width * (i - 1), 0);
                SetUpGrid(panel);
                int numberOfIcons = i == numberOfPanels ? (GameManager.saveData.starSystem.Length - 1) - currentLevelCount : amountPerPage;
                LoadIcons(numberOfIcons, panel);
            }
            Destroy(panelClone);
        }
        void SetUpGrid(GameObject panel)
        {
            GridLayoutGroup grid = panel.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(iconDimensions.width, iconDimensions.height);
            grid.childAlignment = TextAnchor.MiddleCenter;
            grid.spacing = iconSpacing;
        }
        void LoadIcons(int numberOfIcons, GameObject parentObject)
        {
            for (int i = 1; i <= numberOfIcons; i++)
            {
                currentLevelCount++;
                int currentLevelStar = GameManager.saveData.starSystem[currentLevelCount];
                GameObject icon;
                if (currentLevelStar == 3)
                {
                    icon = Instantiate(iconStar3) as GameObject;
                }
                else if (currentLevelStar == 2)
                {
                    icon = Instantiate(iconStar2) as GameObject;
                }
                else if (currentLevelStar == 1)
                {
                    icon = Instantiate(iconStar1) as GameObject;
                }
                else
                {
                    icon = Instantiate(iconStar0) as GameObject;
                }
                icon.transform.SetParent(thisCanvas.transform, false);
                icon.transform.SetParent(parentObject.transform);
                icon.name = "Level" + i;
                icon.GetComponentInChildren<TextMeshProUGUI>().SetText("Level " + currentLevelCount);
                string chosenLevel = "Level"+i;
                icon.GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene(chosenLevel); });
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}