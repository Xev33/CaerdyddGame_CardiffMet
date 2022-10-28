using UnityEngine;

public class ZoneActorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] actors;

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("Player enter a ZoneActorTrigger with: " + actors.Length + " actors in it");
            foreach (GameObject actor in actors)
            {
                if (actor != null)
                    actor.GetComponent<ITriggerActor>().TriggerActor();
            }
            Destroy(this.gameObject);
        }
    }
}
