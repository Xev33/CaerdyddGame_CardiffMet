using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraZone : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float movementDuration;
    public bool hasAnchor;
    [HideInInspector] public GameObject anchor;
    [HideInInspector] public bool shouldLookAtPlayer;
    [HideInInspector] public Vector3 newPosition;
    [HideInInspector] public Quaternion newRotation;

    private void Start()
    {
        cam = Player._instance.selfCamAnchor;
        newRotation.x = newRotation.x / 100;
        newRotation.y = newRotation.y / 100;
        newRotation.z = newRotation.z / 100;
        //newRotation.w = cam.transform.rotation.w;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && Player._instance.cameraZone != this)
        {
            Player._instance.cameraZone = this;
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

        cam.gameObject.transform.parent = null;
        while (timer < movementDuration && Player._instance.cameraZone == this)
        {
            timer += Time.deltaTime;
            normalizedValue = timer / movementDuration;
            normalizedValue = normalizedValue * normalizedValue * (3f - 2f * normalizedValue);

            cam.transform.position = Vector3.Lerp(initialPos, anchor.transform.position, normalizedValue);
            cam.transform.rotation = Quaternion.Lerp(initialRot, anchor.transform.rotation, normalizedValue);

            yield return null;
        }
        yield return null;
    }

    private IEnumerator MoveToNewPosition()
    {
        float timer = 0f;
        float normalizedValue = 0f;
        cam.transform.parent = Player._instance.gameObject.transform;
        Vector3 initialPos = cam.gameObject.transform.localPosition;
        Quaternion initialRot = cam.gameObject.transform.localRotation;

        while (timer < movementDuration && Player._instance.cameraZone == this)
        {
            timer += Time.deltaTime;
            normalizedValue = timer / movementDuration;
            normalizedValue = normalizedValue * normalizedValue * (3f - 2f * normalizedValue);

            cam.transform.localPosition = Vector3.Lerp(initialPos, newPosition, normalizedValue);
            cam.transform.localRotation = Quaternion.Lerp(initialRot, anchor.transform.localRotation, normalizedValue);

            yield return null;
        }
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
        Vector4 vec4 = QuaternionToVector4(myCamera.newRotation);


        // Enable the custom vector 2 in editor if user choose "Custom" as movement type
        if (myCamera.hasAnchor == true)
        {
            myCamera.anchor = (GameObject)EditorGUILayout.ObjectField("Anchor object", myCamera.anchor, typeof(GameObject), true);
            myCamera.shouldLookAtPlayer = EditorGUILayout.ToggleLeft("Should look at player", myCamera.shouldLookAtPlayer);
        }
        else
        {
            myCamera.newPosition = EditorGUILayout.Vector3Field("New camera position", myCamera.newPosition);
            myCamera.newRotation = Vector4ToQuaternion(EditorGUILayout.Vector4Field("New camera rotation", vec4));
        }
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Area Of Effect");
        }
    }

    Quaternion Vector4ToQuaternion(Vector4 v4)
    {
        return new Quaternion(v4.x, v4.y, v4.z, v4.w);
    }

    Vector4 QuaternionToVector4(Quaternion q)
    {
        return new Vector4(q.x, q.y, q.z, q.w);
    }
}
#endif
