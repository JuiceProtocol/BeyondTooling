using UnityEditor;
using UnityEngine;

namespace JuiceProtocol.BeyondTooling
{
	[CustomPropertyDrawer( typeof( SearchableEnumAttribute ) )]
	internal sealed class SearchableEnumDrawer : PropertyDrawer
	{
		private const string DropDownControlName = "DropDownControl";

		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			if( property.propertyType != SerializedPropertyType.Enum )
			{
				EditorGUILayout.PropertyField( property, label );
				return;
			}

			EditorGUI.BeginProperty( position, label, property );
			position = EditorGUI.PrefixLabel( position, label );

			GUIContent guiContent = new( property.enumDisplayNames[property.enumValueIndex] );
			GUI.SetNextControlName( DropDownControlName );
			bool dropdownButton = EditorGUI.DropdownButton( position, guiContent, FocusType.Keyboard );

			Event e = Event.current;
			if( GUI.GetNameOfFocusedControl() == DropDownControlName )
				if( e.type == EventType.KeyDown && e.keyCode is KeyCode.Return or KeyCode.KeypadEnter )
					dropdownButton = true;

			if( dropdownButton )
				SearchableEnumWindow.Init( position, property );

			EditorGUI.EndProperty();
		}
	}
}