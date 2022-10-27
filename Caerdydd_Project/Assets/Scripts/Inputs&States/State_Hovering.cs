using UnityEngine;

public class State_Hovering : XDScript.IPlayerState
{
    public bool canGlide = true;
    public float timer = 0.0f;
    private float timeBeforeGlide = 0.75f;

    public void HandleInput(Player player)
    {
        //if (player.hp <= 0)
        //    return;
        //if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && canGlide)
        //{
        //    player.glidingState.canHover = false;
        //    player.currentState = player.glidingState;
        //    player.speed = player.maxSpeed;
        //    timer = 0.0f;
        //    player.LaunchGivenAnimation(AnimationToLaunch.ANIM_GLIDE);
        //}
    }

    public void StateUpdate(Player player)
    {
        if (player.hp <= 0)
            return;
        XDScript.InputHandler._instance._Move.Execute(player.gameObject);

        //if (canGlide == false)
        //{
        //    timer += Time.deltaTime;
        //    if (timer >= timeBeforeGlide)
        //    {
        //        canGlide = true;
        //        timer = 0.0f;
        //        player.LaunchGivenAnimation(AnimationToLaunch.ANIM_JUMP);
        //    }
        //}

        if (player.IsGrounded() == true && player.hp > 0)
        {
            player.speed = player.maxSpeed;
            timer = 0.0f;
            player.currentState = player.standingState;
            //player.LaunchGivenAnimation(AnimationToLaunch.ANIM_GROUNDED);
        }
    }
}
