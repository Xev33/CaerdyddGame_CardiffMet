using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PopUpBehavior hp1;
    [SerializeField] private PopUpBehavior hp2;
    [SerializeField] private Sprite[] petals = new Sprite[7];
    [SerializeField] private Image daffodilSprite;
    [SerializeField] private PopUpBehavior blackGround;
    [SerializeField] private Animator anim;
    [SerializeField] private float collectibleCountDuration = 3f;
    [SerializeField] private int internPetal = 0;
    [SerializeField] private TMPro.TextMeshProUGUI petalNbr;
    private float timer = 0f;
    private bool isCounterOpen = true;

    private void Awake()
    {

        anim.SetTrigger("Open");
    }

    private void Update()
    {
        if (isCounterOpen == true)
        {
            timer += Time.deltaTime;
            if (timer > collectibleCountDuration)
            {
                anim.SetTrigger("Close");
                timer = 0.0f;
                isCounterOpen = false;
            }
        }
    }

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

    public void CollectPetal()
    {
        timer = 0.0f;
        internPetal++;
        UpdateUI();
        if (isCounterOpen == false)
        {
            isCounterOpen = true;
            anim.SetTrigger("Open");
            if (internPetal >= 6)
                StartCoroutine(CompleteFlower());
            else
                return;
        }
        if (internPetal >= 6)
            StartCoroutine(CompleteFlower());
        else
            anim.SetTrigger("Collect");
    }

    public void UpdateUI()
    {
        daffodilSprite.sprite = petals[internPetal];
        petalNbr.text = Player._instance.collectibleNbr.ToString();
    }

    private IEnumerator CompleteFlower()
    {
        anim.SetTrigger("Complete");
        internPetal = 0;
        Player._instance.Heal();    
        yield return new WaitForSeconds(0.5f);
        UpdateUI();
    }
}
