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

        [Space]
        public Transform idleCheckpoint;
        public Transform speedUpCheckpoint;
        public float checkpointDistance;

        enum EState
        {
            EIdle,
            ESpeedUp,
            ECombat
        }
        EState currentState = EState.EIdle;

        void InitStateIdle()
        {
            currentState = EState.EIdle;
            _survive.text = "Lick the door by pressing right mouse button";
        }
        void InitStateSpeedUp()
        {
            currentState = EState.ESpeedUp;
            _survive.text = "Left mouse button to glide";
        }
        void InitStateCombat()
        {
            currentState = EState.ECombat;
            _survive.text = "Survive";
        }

        void CheckForStateChange()
        {
            var playerFish = FindObjectOfType<InputControllerMouseKeyboard>();
            if (!playerFish)
                return;

            if (currentState == EState.EIdle)
            {
                float dist = (playerFish.transform.position - idleCheckpoint.position).ToPlane().magnitude;
                if(dist < checkpointDistance)
                {
                    InitStateSpeedUp();
                }
            }else if(currentState == EState.ESpeedUp)
            {
                float dist = (playerFish.transform.position - speedUpCheckpoint.position).ToPlane().magnitude;
                if (dist < checkpointDistance)
                {
                    InitStateCombat();
                }
            }
        }


        void Start()
        {
            var playerFish = FindObjectOfType<InputControllerMouseKeyboard>();
            var nameManager = playerFish.transform.parent.GetComponentInChildren<FishNameManager>();
            var yourName = nameManager.FishName;
            _playerName = yourName;

            _yourName.text = $"You are <i><color=green>{yourName}</i>";

            InitStateIdle();
        }

        void Update()
        {
            CheckForStateChange();

            //if (currentState != EState.ECombat)
            //    return;

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
