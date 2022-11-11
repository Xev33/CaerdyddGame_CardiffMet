using UnityEngine;

public class DestroyObjectsZone : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToDestroy;

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            foreach (GameObject actor in objectsToDestroy)
            {
                Destroy(actor);
            }
            Destroy(this.gameObject);
        }
    }
}
