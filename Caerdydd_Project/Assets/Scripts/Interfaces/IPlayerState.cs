namespace XDScript
{
    public interface IPlayerState
    {
        public void HandleInput(Player player);
        public void StateUpdate(Player player);
    }
}
