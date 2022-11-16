using UnityEngine;

public class State_Gliding : XDScript.IPlayerState
{
    public bool canHover = true;
    public float timer = 0.0f;
    private float timeBeforeGlide = 0.5f;

    public void HandleInput(Player player)
    {
        if (player.hp <= 0)
            return;
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && canHover)
        {
            player.currentState = player.hoveringState;
            player.speed = player.maxSpeed / player.hoveringSpeedDivider;
            timer = 0.0f;
            player.hoveringState.canGlide = false;
            player.hoveringState.timer = 0.0f;
            XDScript.InputHandler._instance._Hover.Execute(player.gameObject);
        }
    }

    public void StateUpdate(Player player)
    {
        if (player.hp <= 0)
            return;
        XDScript.InputHandler._instance._Glide.Execute(player.gameObject);

        if (canHover == false)
        {
            timer += Time.deltaTime;
            if (timer >= timeBeforeGlide)
            {
                canHover = true;
                timer = 0.0f;
            }
        }

        if (player.IsGrounded() == true)
        {
            player.glidingState.timer = 0.0f;
            player.currentState = player.standingState;
            //player.LaunchGivenAnimation(AnimationToLaunch.ANIM_GROUNDED);
        }
    }

}
