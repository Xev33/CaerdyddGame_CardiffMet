using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PopUpBehavior hp1;
    [SerializeField] private PopUpBehavior hp2;
    [SerializeField] private PopUpBehavior gemsNumber;
    [SerializeField] private PopUpBehavior lifeDaffodil;
    [SerializeField] private Sprite[] petals = new Sprite[7];
    [SerializeField] private Image daffodilSprite;
    [SerializeField] private PopUpBehavior blackGround;
    [SerializeField] private Animator anim;
    [SerializeField] private Animator animGem;
    [SerializeField] private float collectibleCountDuration = 3f;
    [SerializeField] private float gemCountDuration = 5f;
    [SerializeField] private int internPetal = 0;
    [SerializeField] private TMPro.TextMeshProUGUI petalNbr;
    [SerializeField] private TMPro.TextMeshProUGUI gemNbrTxt;
    [SerializeField] private TMPro.TextMeshProUGUI gemNbrTxt2;
    [SerializeField] private Color txtGoldenColor;
    private float timer = 0f;
    private float timerGem = 0f;
    private bool isCounterOpen = true;
    private bool isGemOpen = true;

    private void Awake()
    {
        anim.SetTrigger("Open");
        animGem.SetTrigger("Open");
        lifeDaffodil.gameObject.GetComponent<Image>().enabled = false;
        StartCoroutine(EnableDaffodilSprite());
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
        if (isGemOpen == true)
        {
            timerGem += Time.deltaTime;
            if (timerGem > gemCountDuration)
            {
                gemsNumber.ClosePopUp();
                timer = 0.0f;
                isGemOpen = false;
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
        {
            lifeDaffodil.ClosePopUp();
            hp1.OpenPopUp();
        }
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

    public void CollectGems(int gemNbr)
    {
        timerGem = 0.0f;
        if (isGemOpen == false)
        {
            isGemOpen = true;
            gemsNumber.OpenPopUp();
            gemNbrTxt.text = gemNbr.ToString();
            if (gemNbr >= 10)
                StartCoroutine(CompleteGem());
            else
                return;
        }
        if (gemNbr >= 10)
            StartCoroutine(CompleteGem());
        else
            animGem.SetTrigger("Collect");
            gemNbrTxt.text = gemNbr.ToString();
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

    private IEnumerator EnableDaffodilSprite()
    {
        yield return new WaitForSeconds(2f);
        lifeDaffodil.gameObject.GetComponent<Image>().enabled = true;
    }

    private IEnumerator CompleteGem()
    {
        float timer = 0.0f;
        float duration = 3.0f;
        float lastTime = 0.2f;
        bool isGold = true;
        animGem.SetTrigger("CompleteGems");
        gemNbrTxt.color = txtGoldenColor;
        gemNbrTxt2.color = txtGoldenColor;
        
        while (timer < duration)
        {
            timer += Time.deltaTime;
            if (timer > lastTime)
            {
                lastTime += 0.5f;
                if (isGold == true)
                {
                    isGold = false;
                    gemNbrTxt.color = Color.black;
                    gemNbrTxt2.color = Color.black;
                }
                else
                {
                    isGold = true;
                    gemNbrTxt.color = txtGoldenColor;
                    gemNbrTxt2.color = txtGoldenColor;
                }
            }
            yield return null;
        }
        isGold = true;
        gemNbrTxt.color = txtGoldenColor;
        gemNbrTxt2.color = txtGoldenColor;
        yield return null;
    }
}
