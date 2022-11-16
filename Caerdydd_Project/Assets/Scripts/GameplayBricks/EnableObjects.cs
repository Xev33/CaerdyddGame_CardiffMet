using UnityEngine;

public class EnableObjects : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToEnable;

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            foreach (GameObject actor in objectsToEnable)
            {
                actor.SetActive(true);
            }
        }
    }
}
