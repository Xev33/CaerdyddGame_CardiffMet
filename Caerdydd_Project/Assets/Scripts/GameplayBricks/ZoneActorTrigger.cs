using UnityEngine;

public class ZoneActorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] actors;

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            foreach (GameObject actor in actors)
            {
                if (actor != null)
                    actor.GetComponent<ITriggerActor>().TriggerActor();
            }
            Destroy(this.gameObject);
        }
    }
}
