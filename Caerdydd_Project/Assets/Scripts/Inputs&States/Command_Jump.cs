public class Command_Jump : XDScript.AbstractCommand
{
    public override void Execute(UnityEngine.GameObject actor)
    {
        Player player = actor.GetComponent<Player>();
        if (player != null)
            player.Jump();
    }
}

/// <summary>
/// Since this action is triggered by the jump button, you can find this class in the same script
/// </summary>
public class Command_Glide : XDScript.AbstractCommand
{
    public override void Execute(UnityEngine.GameObject actor)
    {
        Player player = actor.GetComponent<Player>();
        if (player != null)
            player.Glide();
    }
}

/// <summary>
/// Since this action is triggered by the jump button, you can find this class in the same script
/// </summary>
public class Command_Hover : XDScript.AbstractCommand
{
    public override void Execute(UnityEngine.GameObject actor)
    {
        Player player = actor.GetComponent<Player>();
        if (player != null)
            player.Hover();
    }
}
