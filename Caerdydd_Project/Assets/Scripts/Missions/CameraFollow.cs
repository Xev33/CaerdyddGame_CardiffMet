using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject objectToFollow;
    public float timeOffSet;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        objectToFollow = Player._instance.selfCamAnchor;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = Vector3.SmoothDamp(this.transform.position, objectToFollow.transform.position, ref velocity, timeOffSet);
        this.gameObject.transform.rotation = Quaternion.Slerp(this.transform.rotation, objectToFollow.transform.rotation, timeOffSet);
    }
}
