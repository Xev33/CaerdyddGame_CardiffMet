using UnityEngine;
using System.Collections;

public class TutorialPopUp : MonoBehaviour
{
    [SerializeField] private PopUpBehavior popUpToOpen;
    [SerializeField] private PopUpController controller;
    [SerializeField] private bool shouldCloseTuto = false;


    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            if (popUpToOpen == null || shouldCloseTuto == true)
                StartCoroutine(TriggerControllerToClose());
            else
            {
                popUpToOpen.OpenPopUp();
                Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator TriggerControllerToClose()
    {
        controller.CloseAllPopUps();
        yield return new WaitForSeconds(1);
        Destroy(controller.gameObject);
        Destroy(this.gameObject);
    }
}
