using System;
using UnityEditor;
using UnityEngine;

namespace GB.Utils
{
	[SharedBetweenAnimators]
	public class GB_StateTrigger : StateMachineBehaviour {

		public enum Flag
		{
			False,
			True,
			Toggle
		}

		[Serializable]
		public struct Trigger
		{
			public string name;
			public Flag flag;
		}

		[CustomPropertyDrawer(typeof(Trigger))]
		public class TriggerDrawer : PropertyDrawer
		{
			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			{
				EditorGUI.BeginProperty(position, label, property);
				// Draw label
				position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
				// Don't make child fields be indented
				var indent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
				// Calculate rects
				var nameRect = new Rect(position.x, position.y, 80, position.height);
				var flagRect = new Rect(position.x + 80, position.y, 60, position.height);
				// Draw fields - passs GUIContent.none to each so they are drawn without labels
				EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);
				EditorGUI.PropertyField(flagRect, property.FindPropertyRelative("flag"), GUIContent.none);
				
				EditorGUI.indentLevel = indent;
				EditorGUI.EndProperty();
			}
		}
		
		public Trigger[] onEnter;
		public Trigger[] onExit;

		void SwitchTriggers(Animator animator, Trigger[] triggers)
		{
			foreach(var t in triggers)
			{
				switch (t.flag)
				{
					case Flag.False: animator.ResetTrigger(t.name); break;
					case Flag.True: animator.SetTrigger(t.name); break;
					default:
						animator.SetBool(t.name, !animator.GetBool(t.name));
						break;
				}
			}
		}

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			SwitchTriggers(animator, onEnter);
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			SwitchTriggers(animator, onExit);
		}
	}
}


