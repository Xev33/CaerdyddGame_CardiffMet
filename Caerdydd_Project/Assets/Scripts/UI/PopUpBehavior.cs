using XDScript;

/// <summary>
/// Class that adds an in and out transition movement to a UI object with a RectTransform 
/// </summary>
public class PopUpBehavior : AbstractMovingUI
{
    public void OpenPopUp()
    {
        OpenWindow();
    }

    public void ClosePopUp()
    {
        CloseWindow();
    }

    public void MovePopUpToward(WindowMovementType moveType, bool shouldClose)
    {
        MoveUIToward(moveType, shouldClose);
    }
}
