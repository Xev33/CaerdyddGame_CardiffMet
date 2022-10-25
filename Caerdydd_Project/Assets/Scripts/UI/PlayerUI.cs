using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PopUpBehavior hp1;
    [SerializeField] private PopUpBehavior hp2;
    [SerializeField] private PopUpBehavior[] petals = new PopUpBehavior[6];
    [SerializeField] private PopUpBehavior daffodil;
    [SerializeField] private PopUpBehavior blackGround;

    public void TakeDamage(int hpLeft)
    {
        if (hpLeft == 1)
            hp1.ClosePopUp();
        else
        {
            hp2.ClosePopUp();
        }
    }

    public void GetHealed(int newhp)
    {
        if (newhp == 2)
            hp1.OpenPopUp();
        else
            hp2.OpenPopUp();
    }

    public void OpenBlackGround()
    {
        blackGround.OpenPopUp();
    }

    public void CloseBlackGround()
    {
        blackGround.ClosePopUp();
    }
}
