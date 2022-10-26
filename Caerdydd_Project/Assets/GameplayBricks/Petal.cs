using UnityEngine;

public class Petal : MonoBehaviour
{
    [SerializeField] private float speed = 200f;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Player._instance.CollectPetal();
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Rotate(new Vector3(0, 1 * speed * Time.deltaTime, 0));
    }
}
