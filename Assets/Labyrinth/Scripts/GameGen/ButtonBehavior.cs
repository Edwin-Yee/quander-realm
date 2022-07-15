using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Labyrinth 
{ 
    public class ButtonBehavior : MonoBehaviour
    {
        public GameObject winScreen;
        public GameObject gameplayButtons;
        public GameObject gameplayObjects;
        public GameObject[] starsWon;

        public Button[] buttons;

        void Start() {
            if (buttons == null) {
                return;
            }
            /* else if (buttons.Length > 0) {
                for (int i=0; i<15; i++) {
                    if (i < FindObjectOfType<SaveData>().levelUnlocked) {
                        buttons[i].interactable = true;
                    }
                    else {
                        buttons[i].interactable = false;
                    }
                }
            } */
            else if (buttons.Length > 0) {
                for (int i=1; i<=15; i++) {
                    GameObject.Find($"StarMessage{i}").GetComponent<StarMessage>().displayStars();
                }
            }
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                LoadMainMenu();
                // LoadLevelSelectMenu();
            }
        }

        public void LoadLevelSelectMenu() {
            Time.timeScale = 1f;
            SceneManager.LoadScene("LevelSelect");
        }

        public void LoadMainMenu() {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }

        public void Exit() {
            // Save.SaveGame();
            
            Application.Quit();
        }

        public void Win(int goalsCollected) {      
            Save.SaveGame();
    
            winScreen.SetActive(true);
            gameplayButtons.SetActive(false);
            gameplayObjects.SetActive(false);
            starsWon[goalsCollected].SetActive(true);
            Time.timeScale = 0f;
        }

        public void UndoWin(int goalsCollected) {
            winScreen.SetActive(false);
            gameplayButtons.SetActive(true);
            gameplayObjects.SetActive(true);
            starsWon[goalsCollected].SetActive(false);
            Time.timeScale = 1f;
        }

        public void LevelSelect(int sel) {
            string currScene;
            SaveData.Instance.CurrentLevel = sel;

            switch(sel) {
                case < 3:
                    SaveData.Instance.Degree = 0;
                    currScene = "4x4";
                    break;
                case < 5:
                    SaveData.Instance.Degree = 0;
                    currScene = "5x5";
                    break;
                case 5:
                    SaveData.Instance.Degree = 0;
                    currScene = "6x6";
                    break;


                case < 8:
                    SaveData.Instance.Degree = 180;
                    currScene = "4x4";
                    break;
                case < 10:
                    SaveData.Instance.Degree = 180;
                    currScene = "5x5";
                    break;
                case 10:
                    SaveData.Instance.Degree = 180;
                    currScene = "6x6";
                    break;


                case < 13:
                    SaveData.Instance.Degree = 90;
                    currScene = "4x4";
                    break;
                case < 15:
                    SaveData.Instance.Degree = 90;
                    currScene = "5x5";
                    break;
                case 15:
                    SaveData.Instance.Degree = 90;
                    currScene = "6x6";
                    break;
                

                default:
                    currScene = "LevelSelect";
                    break;
            }
            SceneManager.LoadScene(currScene);
        }

        // ~~~~~~~~~~~~~~~ Enabled in Filament Environment ~~~~~~~~~~~~~~~
        
        // public static Action MinigameClosed;

        private void OnEnable() {
            Wrapper.Events.MinigameClosed += DestroyDataObject;
        }
        private void OnDisable(){
            Wrapper.Events.MinigameClosed -= DestroyDataObject;
        }
        private void DestroyDataObject() {
            Time.timeScale = 1f;
            Destroy(GameObject.Find("ProfileData"));
        }

    }
}