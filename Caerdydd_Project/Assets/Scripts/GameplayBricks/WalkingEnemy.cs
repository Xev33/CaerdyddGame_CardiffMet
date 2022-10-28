using UnityEngine;

public class WalkingEnemy : AbstractEnemy
{
    private int direction = 1;
    [SerializeField] private float speed;

    void Update()
    {
        if (isTrigger == false)
            return;
        body.velocity = new Vector2(direction * speed, body.velocity.y);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "Player")
        {
            if (player.currentState != player.standingState && Player._instance.hp > 0 && player.isInvicible == false)
            {
                KillEnemy();
            }
        }
        else if (other.tag == "KillZone")
        {
            KillEnemy();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        direction *= -1;
    }

    protected override void KillEnemy()
    {
        Destroy(damageZone);
        base.KillEnemy();
    }
}
