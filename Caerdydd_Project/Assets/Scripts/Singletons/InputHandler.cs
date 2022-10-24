using UnityEngine;

namespace XDScript
{
    /// <summary>
    /// Singleton class that will allow binding and calling every input
    /// </summary>
    public class InputHandler : Singleton<InputHandler>
    {
        [HideInInspector] public AbstractCommand _Validate;
        [HideInInspector] public AbstractCommand _Jump;
        [HideInInspector] public AbstractCommand _Glide;
        [HideInInspector] public AbstractCommand _Hover;
        [HideInInspector] public AbstractCommand _Move;

        [HideInInspector] public AbstractCommand _buttonUpArrow;
        [HideInInspector] public AbstractCommand _buttonDownArrow;
        [HideInInspector] public AbstractCommand _buttonRightArrow;
        [HideInInspector] public AbstractCommand _buttonLeftArrow;
        [HideInInspector] public AbstractCommand _buttonSpace;


        private void Start()
        {
            _Jump = new Command_Jump();
            _Glide = new Command_Glide();
            _Hover = new Command_Hover();
            _Move = new Command_Move();

            _buttonDownArrow = _Validate;
            _buttonSpace = _Jump;
        }

        /// <summary>
        /// Check if any input was triggered
        /// </summary>
        public AbstractCommand CheckInputToGetCommand()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                return _buttonUpArrow;
            if (Input.GetKeyDown(KeyCode.DownArrow))
                return _buttonDownArrow;
            if (Input.GetKeyDown(KeyCode.RightArrow))
                return _buttonRightArrow;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                return _buttonLeftArrow;
            if (Input.GetKeyDown(KeyCode.Space))
                return _buttonSpace;
            return null;
        }
    }
}
