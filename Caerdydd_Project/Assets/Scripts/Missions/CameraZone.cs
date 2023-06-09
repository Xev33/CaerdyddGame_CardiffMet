using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraZone : MonoBehaviour
{
    private GameObject cam;
    private CameraFollow camera;
    [SerializeField] private float movementDuration;
    public bool hasAnchor;
    public bool shouldBlockYAxis;
    public bool shouldBlockXAxis;
    [SerializeField] private float newCamTimeOffSet = 0.2f;
    [HideInInspector] public GameObject anchor;
    [HideInInspector] public bool shouldLookAtPlayer;
    [HideInInspector] public Vector3 newPosition;

    private void Start()
    {
        cam = Player._instance.selfCamAnchor;
        camera = Camera.main.gameObject.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && Player._instance.cameraZone != this)
        {
            Player._instance.cameraZone = this;

            if (shouldBlockYAxis == false)
                camera.yToFollow = cam;
            else
                camera.yToFollow = anchor;
            if (shouldBlockXAxis == false)
                camera.xToFollow = cam;
            else
                camera.xToFollow = anchor;
            if (hasAnchor && anchor != null)
                StartCoroutine(MoveToAnchor());
            else
                StartCoroutine(MoveToNewPosition());
        }
    }

    private IEnumerator MoveToAnchor()
    {
        float timer = 0f;
        float normalizedValue = 0f;
        Vector3 initialPos = cam.gameObject.transform.position;
        Quaternion initialRot = cam.gameObject.transform.rotation;
        float lastOffSet = camera.timeOffSet;

        cam.gameObject.transform.parent = null;
        if (lastOffSet == 0.0f)
            camera.timeOffSet = newCamTimeOffSet;
        while (timer < movementDuration && Player._instance.cameraZone == this)
        {
            timer += Time.deltaTime;
            normalizedValue = timer / movementDuration;
            normalizedValue = normalizedValue * normalizedValue * (3f - 2f * normalizedValue);

            if (lastOffSet != 0.0f)
                camera.timeOffSet = Mathf.Lerp(lastOffSet, newCamTimeOffSet, normalizedValue);
            cam.transform.position = Vector3.Lerp(initialPos, anchor.transform.position, normalizedValue);
            cam.transform.rotation = Quaternion.Lerp(initialRot, anchor.transform.rotation, normalizedValue);

            yield return null;
        }

        camera.timeOffSet = newCamTimeOffSet;
        cam.transform.position = anchor.transform.position;
        cam.transform.rotation = anchor.transform.rotation;
        yield return null;
    }

    private IEnumerator MoveToNewPosition()
    {
        float timer = 0f;
        float normalizedValue = 0f;
        if (Player._instance.hp > 0)
            cam.transform.parent = Player._instance.gameObject.transform;
        Vector3 initialPos = cam.gameObject.transform.localPosition;
        Quaternion initialRot = cam.gameObject.transform.localRotation;
        float lastOffSet = camera.timeOffSet;

        if (lastOffSet == 0.0f)
            camera.timeOffSet = newCamTimeOffSet;
        while (timer < movementDuration && Player._instance.cameraZone == this)
        {
            timer += Time.deltaTime;
            normalizedValue = timer / movementDuration;
            normalizedValue = normalizedValue * normalizedValue * (3f - 2f * normalizedValue);

            if (lastOffSet != 0.0f)
                camera.timeOffSet = Mathf.Lerp(lastOffSet, newCamTimeOffSet, normalizedValue);
            cam.transform.localPosition = Vector3.Lerp(initialPos, newPosition, normalizedValue);
            cam.transform.localRotation = Quaternion.Lerp(initialRot, anchor.transform.localRotation, normalizedValue);

            yield return null;
        }

        camera.timeOffSet = newCamTimeOffSet;
        cam.transform.localPosition = newPosition;
        cam.transform.localRotation = anchor.transform.localRotation;
        yield return null;
    }
}

// Allows custom editor for AbstractMovingUI classes
#if UNITY_EDITOR
[CustomEditor(typeof(CameraZone), true)]
public class CameraZoneEditor : Editor
{
    override public void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields
        CameraZone myCamera = target as CameraZone;


        // Enable the custom vector 2 in editor if user chooses to block an axis/uses an anchor as movement type
        if (myCamera.hasAnchor == true && (myCamera.shouldBlockYAxis == true || myCamera.shouldBlockXAxis == true))
        {
            myCamera.anchor = (GameObject)EditorGUILayout.ObjectField("Anchor object", myCamera.anchor, typeof(GameObject), true);
            myCamera.shouldLookAtPlayer = EditorGUILayout.ToggleLeft("Should look at player", myCamera.shouldLookAtPlayer);
        }
        else if (myCamera.hasAnchor == false && (myCamera.shouldBlockYAxis == true || myCamera.shouldBlockXAxis == true))
        {
            myCamera.anchor = (GameObject)EditorGUILayout.ObjectField("Anchor object", myCamera.anchor, typeof(GameObject), true);
            myCamera.shouldLookAtPlayer = EditorGUILayout.ToggleLeft("Should look at player", myCamera.shouldLookAtPlayer);

            myCamera.newPosition = EditorGUILayout.Vector3Field("New camera position", myCamera.newPosition);
        }
        else if (myCamera.hasAnchor == true && myCamera.shouldBlockYAxis == false)
        {
            myCamera.anchor = (GameObject)EditorGUILayout.ObjectField("Anchor object", myCamera.anchor, typeof(GameObject), true);
            myCamera.shouldLookAtPlayer = EditorGUILayout.ToggleLeft("Should look at player", myCamera.shouldLookAtPlayer);
        }
        else
        {
            myCamera.newPosition = EditorGUILayout.Vector3Field("New camera position", myCamera.newPosition);
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Area Of Effect");
        }
    }
}
#endif
