using Components;
using UnityEngine;

namespace Controllers
{
    public class GameStateDataController : MonoBehaviour
    {
        public GameState State;

        public void Awake()
        {
            State.IsNetworkMatch = false;
            State.IsPlayerHasAuthority = false;

            if(State.IsLoaded)
            {
                Save();
            }
            else
            {
                Load();
            }
        }

        public void OnApplicationQuit()
        {
            Save();
        }

        public void Load()
        {
            State.BestScore = PlayerPrefs.GetInt(nameof(State.BestScore), 0);
            State.HardMode = PlayerPrefs.GetInt(nameof(State.HardMode), 0) == 1;
            State.SendRate = PlayerPrefs.GetFloat(nameof(State.SendRate), State.SendRate);
            State.IsLoaded = true;
        }

        public void Save()
        {
            PlayerPrefs.SetInt(nameof(State.BestScore), State.BestScore);
            PlayerPrefs.SetInt(nameof(State.HardMode), State.HardMode ? 1 : 0);
            PlayerPrefs.SetFloat(nameof(State.SendRate), State.SendRate);
            PlayerPrefs.Save();
        }
    }
}