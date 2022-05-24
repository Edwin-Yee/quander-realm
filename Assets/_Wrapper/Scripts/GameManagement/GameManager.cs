using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using System;

namespace Wrapper
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get { return _instance; } }
        private static GameManager _instance;

        private void Awake()
        {
            InitSingleton();
            FirebaseCall();
        }

        private void OnEnable()
        {
            Events.OpenMinigame += OpenMinigame;
        }

        private void OnDisable()
        {
            Events.OpenMinigame -= OpenMinigame;
        }

        private void OpenMinigame(Minigame minigame)
        {
            SceneManager.LoadScene(minigame.StartScene);
        }

        private void FirebaseCall()
        {
            SaveManager saveManager = new SaveManager();
            UserSave data = new UserSave("Rob", "CU_02");

            saveManager.InitFirebase();
            saveManager.Save(data);
        }

        #region Helpers

        private void InitSingleton()
        {
            DontDestroyOnLoad(this);

            if (_instance != null && _instance != this)
                Destroy(this.gameObject);
            else
                _instance = this;
        }

        #endregion
    }
}
