using UnityEngine;

public class UnlockAndLoadNextLevel : MonoBehaviour
{
    [SerializeField] private XDScript.LevelObserver levelObs;

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            levelObs.OnNotify(this.gameObject, XDScript.E_Event.LEVEL_COMPLETE);
            Destroy(this.gameObject);
        }
    }

}
