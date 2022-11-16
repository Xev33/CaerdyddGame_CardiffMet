using UnityEngine;
using UnityEngine.SceneManagement;

namespace XDScript
{
    public class LevelObserver : AbstractObserver
    {
        [SerializeField] private AbstractMission mission;
        [SerializeField] private GameObject[] checkPoints;
        [SerializeField] private Gem[] gems = new Gem[10];
        private Gem[] gemsError; // Is use in Start function to check if ther is not enough (or too many) gems in the level
        private int[] gemsFounded = new int[10];
        [SerializeField] private int currentCheckpoint = 0;
        [SerializeField] private string levelName;
        [SerializeField] private PlayerUI canvasUi;
        [SerializeField] private int nextLevelIndex;

        void Start()
        {
            if (levelName == null)
                Debug.Log(levelName);
            gemsError = FindObjectsOfType<Gem>();
            if (gemsError.Length != 10)
                Debug.Log(gemsError[gemsError.Length]);
            for (int i = 0; i < gemsFounded.Length; i++)
            {
                string name = levelName + i;
                gemsFounded[i] = PlayerPrefs.GetInt(name, 0);
                gems[i].UpdateMaterial(gemsFounded[i]);
            }
            currentCheckpoint = PlayerPrefs.GetInt("lastCheckPoint", 0);
            if (checkPoints.Length > 0)
                Player._instance.gameObject.transform.position = checkPoints[currentCheckpoint].transform.position;
            canvasUi.CollectGems(GetNumberOfGem());
        }
    
        public override void OnNotify(GameObject entity, E_Event eventToTrigger)
        {
            switch (eventToTrigger)
            {
                case E_Event.MISSION_STEP_COMPLETE:
                    mission.LaunchNextEvent();
                    break;
                case E_Event.GEM_FOUNDED:
                    NewGemFounded(entity.GetComponent<Gem>().gemID);
                    break;
                case E_Event.PLAYER_DIES:
                    if (entity.tag == "Player")
                        ReloadScene();
                    break;
                case E_Event.CHECKPOINT_REACHED:
                    if (entity.tag == "CheckPoint")
                    {
                        DisableAllCheckPoints();
                        currentCheckpoint = FindCheckPointIndex(ref entity);
                    }
                    break;
                case E_Event.LEVEL_COMPLETE:
                    CompleteLevel();
                    break;
                default:
                    Debug.Log("Nothing happened");
                    break;
            }
        }

        private void DisableAllCheckPoints()
        {
            for (int i = 0; i < checkPoints.Length; i++)
                checkPoints[i].GetComponent<CheckPoint>().TurnOff();
        }

        private int FindCheckPointIndex(ref GameObject entity)
        {
            for (int i = 0; i < checkPoints.Length; i++)
            {
                if (checkPoints[i] == entity)
                    return i;
            }
            return 0;
        }

        private void ReloadScene()
        {
            PlayerPrefs.SetInt("lastCheckPoint", currentCheckpoint);
            PlayerPrefs.SetInt("collectibleNumber", Player._instance.collectibleNbr);

            SaveGame();
            if (checkPoints.Length > 0)
                Player._instance.gameObject.transform.position = checkPoints[currentCheckpoint].transform.position;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void SaveGame()
        {
            for (int i = 0; i < gemsFounded.Length; i++)
            {
                string name = levelName + i;
                PlayerPrefs.SetInt(name, gemsFounded[i]);
            }
            PlayerPrefs.SetInt(name, GetNumberOfGem());
        }

        private void NewGemFounded(int index)
        {
            if (gemsFounded[index] == 1)
                return;

            gemsFounded[index] = 1;
            canvasUi.CollectGems(GetNumberOfGem());
        }

        private int GetNumberOfGem()
        {
            int gemNbr = 0;

            for (int i = 0; i < gemsFounded.Length; i++)
                gemNbr += gemsFounded[i];
            return gemNbr;
        }

        private void CompleteLevel()
        {
            string nxtLvl = "IsLevel" + (nextLevelIndex + 1) + "Unlock";
            PlayerPrefs.SetInt("lastCheckPoint", 0);
            PlayerPrefs.SetInt("collectibleNumber", 0);
            PlayerPrefs.SetInt(nxtLvl, 1);
            SaveGame();
            canvasUi.OpenLevelCompleteUI(nextLevelIndex);
        }
    }
}
