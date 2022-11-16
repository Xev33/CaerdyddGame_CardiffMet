using UnityEngine;

public class State_Disable : XDScript.IPlayerState
{
    public void HandleInput(Player player)
    {
         XDScript.InputHandler._instance._Move.Execute(player.gameObject);
    }

    public void StateUpdate(Player player)
    {
    }
}
