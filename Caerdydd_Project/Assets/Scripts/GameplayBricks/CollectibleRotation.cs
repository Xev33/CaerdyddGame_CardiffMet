using UnityEngine;

public class CollectibleRotation : MonoBehaviour
{
    public float speed = 200f;

    void Update()
    {
        this.gameObject.transform.Rotate(new Vector3(0, 1 * speed * Time.deltaTime, 0));
    }
}
