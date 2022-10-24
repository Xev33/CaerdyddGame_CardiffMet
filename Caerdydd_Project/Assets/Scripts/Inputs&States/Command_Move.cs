public class Command_Move : XDScript.AbstractCommand
{
    public override void Execute(UnityEngine.GameObject actor)
    {
        Player player = actor.GetComponent<Player>();
        if (player != null)
            player.Move();
    }
}
