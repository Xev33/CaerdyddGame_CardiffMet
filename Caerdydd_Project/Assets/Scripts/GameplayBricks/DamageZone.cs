using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public bool isEnemy = false;

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            Player._instance.TakeHit(1);
        }
    }
}
