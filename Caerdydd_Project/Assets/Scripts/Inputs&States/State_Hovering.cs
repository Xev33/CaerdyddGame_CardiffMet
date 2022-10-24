using UnityEngine;

public class State_Hovering : XDScript.IPlayerState
{
    public bool canGlide = true;
    public float timer = 0.0f;
    private float timeBeforeGlide = 0.75f;

    public void HandleInput(Player player)
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && canGlide)
        {
            player.glidingState.canHover = false;
            player.currentState = player.glidingState;
            player.speed *= player.hoveringSpeedDivider;
            timer = 0.0f;
        }
    }

    public void StateUpdate(Player player)
    {
        XDScript.InputHandler._instance._Move.Execute(player.gameObject);

        if (canGlide == false)
        {
            timer += Time.deltaTime;
            if (timer >= timeBeforeGlide)
            {
                canGlide = true;
                timer = 0.0f;
            }
        }

        if (player.IsGrounded() == true)
        {
            player.speed *= player.hoveringSpeedDivider;
            timer = 0.0f;
            player.currentState = player.standingState;
        }
    }
}
