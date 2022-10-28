using UnityEngine;

public class WalkingEnemy : AbstractEnemy
{
    void Update()
    {
        if (isTrigger == false)
            return;
        body.velocity = new Vector2(lastDirection * speed, body.velocity.y);
        RotateMesh();
        anim.SetFloat("Move", speed);
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
        lastDirection *= -1;
    }
}