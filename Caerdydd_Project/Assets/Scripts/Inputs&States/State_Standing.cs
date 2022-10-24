using UnityEngine;

public class State_Standing : XDScript.IPlayerState
{
    private bool canJump = true;
    private bool isTimerOn = false;
    private float timer = 0.0f;
    private float timeToJump = 0.2f;

    public void HandleInput(Player player)
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0") && canJump)
        {
            player.jumpState.timer = 0.0f;
            player.jumpState.canGlide = false;
            player.currentState = player.jumpState;
            XDScript.InputHandler._instance._Jump.Execute(player.gameObject);
        }
    }

    public void StateUpdate(Player player)
    {
        XDScript.InputHandler._instance._Move.Execute(player.gameObject);

        if (isTimerOn == true)
        {
            timer += Time.deltaTime;
            if (timer >= timeToJump)
            {
                isTimerOn = false;
                timer = 0.0f;
                canJump = false;
            }
        }

        if (player.IsGrounded() == false)
        {
            isTimerOn = true;
            if (canJump == false)
            {
                player.currentState = player.jumpState;
            }
        }
        else
        {
            isTimerOn = false;
            canJump = true;
            timer = 0.0f;
        }
    }

}

