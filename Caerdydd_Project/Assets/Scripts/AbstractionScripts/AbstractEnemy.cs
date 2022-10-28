using System.Collections;
using UnityEngine;

public abstract class AbstractEnemy : AbstractBumper, ITriggerActor
{

    protected Rigidbody body;
    protected bool isTrigger = false;
    [SerializeField] protected float rotationSpeed = 10.0f;
    [SerializeField] protected float waitingTimeToTrigger;
    [SerializeField] protected float speed = 0.0f;
    [SerializeField] protected DamageZone damageZone;
    [SerializeField] protected GameObject dragonMesh;
    protected int lastDirection = -1;
    [SerializeField] protected Animator anim;

    protected virtual void Awake()
    {
        anim = dragonMesh.gameObject.GetComponent<Animator>();
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

    private IEnumerator PlayDeathAnimAndDestroy()
    {
        anim.SetTrigger("Death");
        isTrigger = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        Destroy(damageZone);
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

    protected virtual void KillEnemy()
    {
        StartCoroutine(PlayDeathAnimAndDestroy());
    }

    protected virtual void RotateMesh()
    {
        Quaternion toRotation;

        if (lastDirection < 0)
            toRotation = Quaternion.LookRotation(new Vector3(-0.5f, 0f, 0f));
        else
            toRotation = Quaternion.LookRotation(new Vector3(0.5f, 0f, 0f));
        dragonMesh.transform.localRotation = Quaternion.RotateTowards(dragonMesh.transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

}
