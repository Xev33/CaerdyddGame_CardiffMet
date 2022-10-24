using UnityEngine;
using System.Collections;

/// <summary>
/// Class that gather all PopUpBehavior children to control them
/// </summary>
public class PopUpController : MonoBehaviour
{
    [SerializeField] private PopUpBehavior[] m_allPopUps;
    private float maxCloseTravelingtime;

    void Awake()
    {
        maxCloseTravelingtime = 0f;
        m_allPopUps = this.GetComponentsInChildren<PopUpBehavior>(true);
        foreach (PopUpBehavior item in m_allPopUps)
            if (item.m_timeOfTravelingClose > maxCloseTravelingtime)
                maxCloseTravelingtime = item.m_timeOfTravelingClose;
    }

    public void OpenAllPopUps()
    {
        foreach (PopUpBehavior item in m_allPopUps)
            item.OpenPopUp();
    }

    public void CloseAllPopUps()
    {
        foreach (PopUpBehavior item in m_allPopUps)
            item.ClosePopUp();
    }

    public void ToggleAllPopUps()
    {
        foreach (PopUpBehavior item in m_allPopUps)
            item.ToggleWindow();
    }

    public void MoveCanvasToward(XDScript.WindowMovementType moveType, bool shouldClose)
    {
        foreach (PopUpBehavior item in m_allPopUps)
            item.MovePopUpToward(moveType, shouldClose);
    }

    public void DisableCanvas()
    {
        StartCoroutine(DisableCoroutine());
    }

    private IEnumerator DisableCoroutine()
    {
        yield return new WaitForSeconds(maxCloseTravelingtime + 0.1f);
        this.gameObject.SetActive(false);
    }
}
