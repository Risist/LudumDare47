using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Effects
{
    [DefaultExecutionOrder(9999)]
    public class BattleUiManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _yourName;
        [SerializeField] private TMP_Text _elementsLeft;
        [SerializeField] private TMP_Text _survive;
        [SerializeField] private TMP_Text _failedToSurvive;
        [SerializeField] private TMP_Text _tryAgain;
        [SerializeField] private TMP_Text _gameWon;
        private string _playerName;


        void Start()
        {
            var playerFish = FindObjectOfType<InputControllerMouseKeyboard>();
            var nameManager = playerFish.transform.parent.GetComponentInChildren<FishNameManager>();
            var yourName = nameManager.FishName;
            _playerName = yourName;

            _yourName.text = $"You are <i><color=green>{yourName}</i>";
        }

        void Update()
        {
            var playerFish = FindObjectOfType<InputControllerMouseKeyboard>();
            bool playerIsAlive = playerFish != null;

            if (!playerIsAlive)
            {
                _yourName.text = $"You <i>were</i> <i><color=green>{_playerName}</i>";
            }

            _elementsLeft.gameObject.SetActive( playerIsAlive);
            _survive.gameObject.SetActive( playerIsAlive);
            _failedToSurvive.gameObject.SetActive( !playerIsAlive);
            _tryAgain.gameObject.SetActive( !playerIsAlive);

            var fishLeft = FindObjectsOfType<FishTrailController>();
            _elementsLeft.text = $"There are <color=red>{fishLeft.Length}</color> fish left";

            if (!playerIsAlive)
            {
                Time.timeScale = 0.25f;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Time.timeScale = 1;
                    SceneManager.LoadScene(this.gameObject.scene.name);
                }
            }

            var gameWon = fishLeft.Length == 1 && playerIsAlive;
            if (gameWon)
            {
                _survive.gameObject.SetActive(!gameWon);
                _gameWon.gameObject.SetActive(gameWon);
                _tryAgain.gameObject.SetActive(gameWon); 
                Time.timeScale = 0.25f;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Time.timeScale = 1;
                    SceneManager.LoadScene(this.gameObject.scene.name);
                }

            }
        }
    }
}
