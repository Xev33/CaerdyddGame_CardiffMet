using UnityEngine;
using UnityEngine.SceneManagement;

namespace XDScript
{
    public class LevelObserver : AbstractObserver
    {
        [SerializeField] private AbstractMission mission;
        [SerializeField] private GameObject[] checkPoints;
        [SerializeField] private int currentCheckpoint = 0;

        void Start()
        {
            checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
            Player._instance.gameObject.transform.position = checkPoints[currentCheckpoint].transform.position;
        }
    
        public override void OnNotify(GameObject entity, E_Event eventToTrigger)
        {
            switch (eventToTrigger)
            {
                case E_Event.MISSION_STEP_COMPLETE:
                    mission.LaunchNextEvent();
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
                default:
                    Debug.Log("Nothing happened");
                    break;
            }
        }

        private void DisableAllCheckPoints()
        {
            for (int i = 0; i < checkPoints.Length; i++)
                checkPoints[i].GetComponent<CheckPoint>().isOn = false;
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
