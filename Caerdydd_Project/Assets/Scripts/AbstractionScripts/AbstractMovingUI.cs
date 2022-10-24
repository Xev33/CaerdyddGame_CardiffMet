using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XDScript
{
    public enum WindowMovementType
    {
        Right,
        Left,
        Top,
        Bottom,
        Center,
        Custom,
        OwnPosition
    }

    public abstract class AbstractMovingUI : MonoBehaviour
    {
        // Array of all the possible movement according to their WindowMovementType
        public (WindowMovementType type, Vector2 pos)[] m_movementCoordinates;

        protected float m_timeOfTraveling;
        [SerializeField] protected float m_timeOfTravelingOpen;
        public float m_timeOfTravelingClose;
        [SerializeField] protected bool m_shouldStartOpen;
        [SerializeField] protected bool m_shouldGrowUp = false;
        [SerializeField] protected bool m_shouldShrink = false;
        protected bool m_isOpen;
        [HideInInspector]
        public WindowMovementType startPosition;
        [HideInInspector]
        public WindowMovementType endPosition;
        [HideInInspector]
        public RectTransform m_customStartingPos;
        protected Vector3 m_initialScale;
        protected Vector3 m_initialPos;
        [HideInInspector]
        public RectTransform m_customEndPos;
        protected Vector2 m_startPos;
        protected Vector2 m_endPos = new Vector2(Screen.width / 2, Screen.height / 2);
        protected RectTransform m_rectTransform;
        protected bool m_isMoving = false;

        protected virtual void Awake()
        {
            m_rectTransform = this.gameObject.GetComponent<RectTransform>();
            InitCoordinates();
            InitWindow(true, ref m_startPos, startPosition, ref m_customStartingPos);
            if (m_shouldStartOpen == true)
                OpenWindow();
            else
                CloseWindow();
        }


        private void InitCoordinates()
        {
            m_initialScale = this.transform.localScale;
            m_initialPos = m_rectTransform.localPosition;
            m_movementCoordinates = new (WindowMovementType type, Vector2 pos)[]
           {
            (WindowMovementType.Right, new Vector2(m_initialPos.x + (Screen.width * 2f), m_initialPos.y)),
            (WindowMovementType.Left, new Vector2(m_initialPos.x - (Screen.width * 2f), m_initialPos.y)),
            (WindowMovementType.Top, new Vector2(m_initialPos.x, m_initialPos.y + (Screen.height * 2f))),
            (WindowMovementType.Bottom, new Vector2(m_initialPos.x, m_initialPos.y - (Screen.height * 2f))),
            (WindowMovementType.Center, new Vector2(Screen.width / 2, Screen.height / 2)),
            (WindowMovementType.OwnPosition, new Vector2(m_initialPos.x , m_initialPos.y))
           };
        }

        // Link all buttons that should close the window and set the correct movement to the window
        public virtual void InitWindow(bool shouldSetStartPos, ref Vector2 posToChange, WindowMovementType type, ref RectTransform customObj)
        {
            int i = System.Array.FindIndex(m_movementCoordinates, item => item.type == type);

            if (type == WindowMovementType.Custom)
            {
                posToChange = customObj.localPosition;
            }
            else
                posToChange = m_movementCoordinates[i].pos;

            if (shouldSetStartPos == true)
                InitWindow(false, ref m_endPos, endPosition, ref m_customEndPos);
        }

        public virtual void ToggleWindow()
        {
            if (m_isOpen == true)
                CloseWindow();
            if (m_isOpen == false)
                OpenWindow();
        }

        protected virtual void OpenWindow()
        {
            if (m_isMoving == true)
                return;
            m_isMoving = true;
            StartCoroutine(MoveWindow(m_startPos, m_initialPos, false));
        }

        protected virtual void CloseWindow()
        {
            if (m_isMoving == true)
                return;
            m_isMoving = true;
            StartCoroutine(MoveWindow(m_initialPos, m_endPos, true));
        }

        protected virtual void MoveUIToward(WindowMovementType moveType, bool shouldClose)
        {
            if (m_isMoving == true)
                return;
            m_isMoving = true;
            int i = System.Array.FindIndex(m_movementCoordinates, item => item.type == moveType);

            if (shouldClose == false)
                StartCoroutine(MoveWindow(m_movementCoordinates[i].pos, m_initialPos, shouldClose));
            else
                StartCoroutine(MoveWindow(m_initialPos, m_movementCoordinates[i].pos, shouldClose));
        }

        public IEnumerator MoveWindow(Vector3 start, Vector3 end, bool shouldClose)
        {
            float timer = 0f;
            float normalizedValue = 0f;
            Vector3 nullVector = new Vector3(0f, 0f, 0f);
            if (shouldClose == true)
                m_timeOfTraveling = m_timeOfTravelingClose;
            else
                m_timeOfTraveling = m_timeOfTravelingOpen;
            if (m_shouldGrowUp == false)
                m_rectTransform.localScale = m_initialScale;
            while (timer < m_timeOfTraveling)
            {
                timer += Time.deltaTime;
                normalizedValue = timer / m_timeOfTraveling; // We normalize our time for the lerp
                normalizedValue = normalizedValue * normalizedValue * (3f - 2f * normalizedValue); // Calcul for a smooth lerp

                m_rectTransform.localPosition = Vector3.Lerp(start, end, normalizedValue);
                if (m_shouldShrink == true && shouldClose == true) // Scale modification for movement to close
                    m_rectTransform.localScale = Vector3.Lerp(m_initialScale, nullVector, normalizedValue);
                else if (m_shouldGrowUp == true && shouldClose == false) // Scale modification for movement to open
                    m_rectTransform.localScale = Vector3.Lerp(nullVector, m_initialScale, normalizedValue);
                yield return null;
            }
            m_rectTransform.localPosition = end;
            if (shouldClose == true && m_shouldShrink == true)
                m_rectTransform.localScale = nullVector;
            if (shouldClose == true)
                m_isOpen = false;
            else
            {
                m_isOpen = true;
                m_rectTransform.localPosition = m_initialPos;
            }
            m_isMoving = false;
            yield return null;
        }
    }

    // Allows custom editor for AbstractMovingUI classes
#if UNITY_EDITOR
    [CustomEditor(typeof(AbstractMovingUI), true)]
    public class AbstractMovingUIEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            DrawDefaultInspector(); // for other non-HideInInspector fields
            AbstractMovingUI myWindow = target as AbstractMovingUI;

            myWindow.startPosition = (WindowMovementType)EditorGUILayout.EnumPopup("Opening movement comes from", myWindow.startPosition);
            myWindow.endPosition = (WindowMovementType)EditorGUILayout.EnumPopup("Closing movement goes to", myWindow.endPosition);

            // Enable the custom vector 2 in editor if user choose "Custom" as movement type
            if (myWindow.startPosition == WindowMovementType.Custom)
            {
                myWindow.m_customStartingPos = (RectTransform)EditorGUILayout.ObjectField("Custom Starting Position", myWindow.m_customStartingPos, typeof(RectTransform), true);
            }
            if (myWindow.endPosition == WindowMovementType.Custom)
            {
                myWindow.m_customEndPos = (RectTransform)EditorGUILayout.ObjectField("Custom Ending Position", myWindow.m_customEndPos, typeof(RectTransform), true);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Area Of Effect");
            }
        }
    }
#endif
}