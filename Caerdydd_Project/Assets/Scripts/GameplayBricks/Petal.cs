using UnityEngine;

public class Petal : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Player._instance.CollectPetal();
            Destroy(this.gameObject);
        }
    }
}
