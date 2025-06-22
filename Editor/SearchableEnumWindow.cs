using UnityEditor;
using UnityEngine;

namespace JuiceProtocol.BeyondTooling
{
	internal sealed class SearchableEnumWindow : EditorWindow
	{
		private const float ButtonHeight = 20f;
		private const float SearchBarHeight = 22f;
		private const string SearchBarControlName = "SearchBar";
		private const string EnumButtonControlName = "EnumButton_";

		private SerializedProperty _property;
		private GUIStyle _enumButtonStyle;
		private string _searchString = "";
		private Vector2 _scrollPosition;
		private string[] _enumNames;
		private int _visibleEnumsCount;

		private int _focusedElementIndex = -1;

		public static void Init( Rect rect, SerializedProperty property )
		{
			SearchableEnumWindow window = CreateInstance<SearchableEnumWindow>();

			Vector2 screenPos = GUIUtility.GUIToScreenPoint( new Vector2( rect.x, rect.y + rect.height ) );
			window.position = new Rect( screenPos, new Vector2( rect.width, rect.height ) );

			window.Initialize( property );
			window.ShowPopup();
		}

		private void Initialize( SerializedProperty property )
		{
			_property = property;
			_enumNames = _property.enumDisplayNames;

			_enumButtonStyle = new GUIStyle( EditorStyles.toolbarButton );
			_enumButtonStyle.alignment = TextAnchor.MiddleLeft;
			_enumButtonStyle.focused.textColor = Color.white;
		}

		private void OnLostFocus()
		{
			Close();
		}

		private void OnGUI()
		{
			DrawSearchBar();

			_scrollPosition = GUILayout.BeginScrollView( _scrollPosition );
			_visibleEnumsCount = 0;
			for( int i = 0; i < _enumNames.Length; i++ )
			{
				if( !string.IsNullOrEmpty( _searchString ) && !_enumNames[i].ToLower().Contains( _searchString.ToLower() ) )
					continue;

				DrawEnumButton( i, _visibleEnumsCount );
				_visibleEnumsCount++;
			}

			Rect mainWindow = EditorGUIUtility.GetMainWindowPosition();
			Rect rect = position;
			float remainingWindowHeight = mainWindow.height - ( position.y - mainWindow.position.y );
			rect.height = Mathf.Min( _visibleEnumsCount * ButtonHeight + SearchBarHeight, remainingWindowHeight );
			position = rect;

			GUILayout.EndScrollView();

			HandleNavigation();
		}
		private void DrawSearchBar()
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label( "Search:", GUILayout.Width( 50 ) );
			GUI.SetNextControlName( SearchBarControlName );
			_searchString = GUILayout.TextField( _searchString );
			GUILayout.EndHorizontal();
		}

		private void DrawEnumButton( int enumIndex, int displayIndex )
		{
			GUILayout.BeginHorizontal();

			GUI.SetNextControlName( EnumButtonControlName + displayIndex );
			if( GUILayout.Button(
				   ( _property.enumValueIndex == enumIndex ? "\u2713  " : "     " ) + _enumNames[enumIndex],
				   _enumButtonStyle,
				   GUILayout.ExpandWidth( true )
			   ) )
			{
				_property.enumValueIndex = enumIndex;
				_property.serializedObject.ApplyModifiedProperties();
				Close();
			}

			GUILayout.EndHorizontal();
		}

		private void HandleNavigation()
		{
			Event e = Event.current;

			if( e.type == EventType.KeyUp && e.keyCode == KeyCode.Escape )
			{
				Close();
				e.Use();
				return;
			}

			if( GUI.GetNameOfFocusedControl() == SearchBarControlName )
				_focusedElementIndex = -1;

			if( e.type == EventType.KeyUp && e.keyCode == KeyCode.DownArrow )
			{
				_focusedElementIndex++;
				if( _focusedElementIndex > _visibleEnumsCount )
					_focusedElementIndex = _visibleEnumsCount;
				e.Use();
				UpdateFocusedElement( _focusedElementIndex );
			}
			else if( e.type == EventType.KeyUp && e.keyCode == KeyCode.UpArrow )
			{
				_focusedElementIndex--;
				if( _focusedElementIndex < -1 )
					_focusedElementIndex = -1;
				e.Use();
				UpdateFocusedElement( _focusedElementIndex );
			}
		}

		private void UpdateFocusedElement( int focusedElementIndex )
		{
			if( focusedElementIndex == -1 )
				GUI.FocusControl( SearchBarControlName );
			else if( focusedElementIndex < _visibleEnumsCount )
				GUI.FocusControl( EnumButtonControlName + focusedElementIndex );
		}
	}
}