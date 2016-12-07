using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GB.EventSystems
{
    [RequireComponent(typeof(EventSystem))]
    public class GB_InputModule : BaseInputModule
    {
        public enum AxisState { Down, Hold, Up }

        static void OnButtonDown(GB_IButtonHandler handler, GB_ButtonEventData data)
        {
            handler.OnButtonDown(data);
        }

        static void OnButtonHold(GB_IButtonHandler handler, GB_ButtonEventData data)
        {
            handler.OnButtonHold(data);
        }

        static void OnButtonUp(GB_IButtonHandler handler, GB_ButtonEventData data)
        {
            handler.OnButtonUp(data);
        }

        public Action<GB_IButtonHandler, GB_ButtonEventData> buttonDownHandler = OnButtonDown;
        public Action<GB_IButtonHandler, GB_ButtonEventData> buttonHoldHandler = OnButtonHold;
        public Action<GB_IButtonHandler, GB_ButtonEventData> buttonUpHandler = OnButtonUp;
        
        [SerializeField] protected bool fireHoldState = false;
        [SerializeField] protected string[] additionalAxis;
        [SerializeField] protected string[] additionalButtons;


        protected readonly Dictionary<string, AxisState> axisStates = new Dictionary<string, AxisState>();

        public override void Process()
        {
            //base.Process();

            GameObject target = eventSystem.currentSelectedGameObject != null ? eventSystem.currentSelectedGameObject : eventSystem.firstSelectedGameObject;

            foreach(string button in additionalButtons)
            {
                if (Input.GetButtonDown(button))
                {
                    Execute(target, buttonDownHandler, new GB_ButtonEventData(eventSystem, button));
                }
                else if (fireHoldState && Input.GetButton(button))
                {
                    Execute(target, buttonHoldHandler, new GB_ButtonEventData(eventSystem, button));
                }
                else if (Input.GetButtonUp(button))
                {
                    Execute(target, buttonUpHandler, new GB_ButtonEventData(eventSystem, button));
                }
            }

            foreach(string axis in additionalAxis)
            {
                if(axisStates.ContainsKey(axis))
                {
                    if (Input.GetAxis(axis) == 0)
                    {
                        if(axisStates[axis] != AxisState.Up)
                        {
                            Execute(target, buttonUpHandler, new GB_ButtonEventData(eventSystem, axis));
                            axisStates[axis] = AxisState.Up;
                        }
                    }
                    else
                    {
                        if(axisStates[axis] == AxisState.Up)
                        {
                            Execute(target, buttonDownHandler, new GB_ButtonEventData(eventSystem, axis));
                            axisStates[axis] = AxisState.Down;
                        }
                        else if(fireHoldState)
                        {
                            Execute(target, buttonHoldHandler, new GB_ButtonEventData(eventSystem, axis));
                            axisStates[axis] = AxisState.Hold;
                        }
                    }
                }
                else
                {
                    axisStates[axis] = AxisState.Up;
                }                
            }
        }

        protected void Execute<T1, T2>(GameObject target, Action<T1, T2> action, T2 data) where T1 : IEventSystemHandler
        {
            if(target != null && action != null)
            {
                T1[] handlers = target.GetComponents<T1>();
                foreach(T1 handler in handlers)
                {
                    action(handler, data);
                }
            }
            else
            {
#if UNITY_EDITOR
				Debug.LogWarning("Target or action is null");
#endif
            }
        }
    }
}
