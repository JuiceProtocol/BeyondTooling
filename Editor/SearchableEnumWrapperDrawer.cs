using UnityEditor;
using UnityEngine;

namespace JuiceProtocol.BeyondTooling
{
	[CustomPropertyDrawer(typeof(SearchableEnumWrapper<> ))]
	public class SearchableEnumWrapperDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty valueProp = property.FindPropertyRelative("value");
			bool showLabel = property.name != "Key";
			EditorGUI.PropertyField(position, valueProp, showLabel ? label : GUIContent.none);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("value"), GUIContent.none, true);
		}
	}
}