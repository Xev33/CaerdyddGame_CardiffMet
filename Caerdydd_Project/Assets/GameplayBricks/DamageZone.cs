using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            Player._instance.TakeHit(1);
        }
    }
}
