using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject objectToFollow;
    public float timeOffSet;
    private Vector3 velocity;
    public GameObject yToFollow;
    public GameObject xToFollow;

    // Start is called before the first frame update
    void Start()
    {
        objectToFollow = Player._instance.selfCamAnchor;
        yToFollow = objectToFollow;
        xToFollow = objectToFollow;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(xToFollow.transform.position.x, yToFollow.transform.position.y, objectToFollow.transform.position.z);

        this.gameObject.transform.position = Vector3.SmoothDamp(this.transform.position, pos, ref velocity, timeOffSet);
        this.gameObject.transform.rotation = objectToFollow.transform.rotation;
    }
}
