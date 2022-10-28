using System.Collections;
using UnityEngine;

public abstract class AbstractEnemy : AbstractBumper, ITriggerActor
{
    protected Rigidbody body;
    protected bool isTrigger = false;
    [SerializeField] protected float waitingTimeToTrigger;
    [SerializeField] protected DamageZone damageZone;

    protected virtual void Awake()
    {
        body = this.gameObject.GetComponent<Rigidbody>();
    }

    public void TriggerActor()
    {
        if (waitingTimeToTrigger == 0)
            isTrigger = true;
        else
            StartCoroutine(WaitToTrigger());
    }

    private IEnumerator WaitToTrigger()
    {
        yield return new WaitForSeconds(waitingTimeToTrigger);
        isTrigger = true;
    }

    protected virtual void KillEnemy()
    {
        Destroy(this.gameObject);
    }
}
