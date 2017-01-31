using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace DI.SimpleNote {
	#if UNITY_EDITOR
	[InitializeOnLoad]
	public class SimplenoteGameObjectInitializer
	{

		static SimplenoteGameObjectInitializer()
		{
			SceneView.onSceneGUIDelegate += onSceneGUIUpdate;
		}

		private static void onSceneGUIUpdate(SceneView sceneView)
		{
			if (Selection.gameObjects.Length == 1)
			{
				int index = SimpleNoteSO.Instance.getIndexGameObjectNote(Selection.activeGameObject);
				if (index != -1)
				{
					Handles.BeginGUI();
					float height = EditorStyles.textArea.CalcHeight(new GUIContent(SimpleNoteSO.Instance.gameObjectNote[index].note.note), EditorPrefs.GetFloat("NoteWidthSceneView", 300));
					Vector2 startPosition = new Vector2(EditorPrefs.GetFloat("NoteOffsetX", 15), EditorPrefs.GetFloat("NoteOffsetY", 15));
					if ((SimpleNoteData.SimpleNoteSceneViewPosition)EditorPrefs.GetInt("simpleNoteSceneViewPosition", 1) == SimpleNoteData.SimpleNoteSceneViewPosition.BottomLeft) {
						startPosition.y = sceneView.camera.pixelRect.height - EditorGUIUtility.singleLineHeight - EditorPrefs.GetFloat("NoteOffsetY", 15);
						if (!SimpleNoteSO.Instance.gameObjectNote[index].hide)
							startPosition.y -= height;
					}
					
					EditorGUI.DrawRect(new Rect(startPosition.x, startPosition.y, EditorPrefs.GetFloat("NoteWidthSceneView", 300), EditorGUIUtility.singleLineHeight), SimpleNoteData.Instance.getBgColor2);
					if(!SimpleNoteSO.Instance.gameObjectNote[index].hide)
						EditorGUI.DrawRect(new Rect(startPosition.x, startPosition.y + EditorGUIUtility.singleLineHeight, EditorPrefs.GetFloat("NoteWidthSceneView", 300), height), SimpleNoteData.Instance.getBgColor1);
					
					Color oriColor = EditorStyles.label.normal.textColor;
					Color guiBgOriColor = GUI.backgroundColor;
					GUI.backgroundColor = SimpleNoteData.Instance.getBgColor1;
					GUIStyle textField = new GUIStyle(EditorStyles.textField);
					textField.normal = EditorStyles.label.normal;
					textField.focused = EditorStyles.label.normal;
					textField.normal.textColor = SimpleNoteData.Instance.getTextColor;
					textField.focused.textColor = SimpleNoteData.Instance.getTextColor;
					textField.fontStyle = FontStyle.Bold;
					GUI.SetNextControlName("Title-Scene" + Selection.activeGameObject.GetInstanceID());
					Undo.RecordObject(SimpleNoteSO.Instance, "Undo Title");
					SimpleNoteSO.Instance.gameObjectNote[index].note.title = EditorGUI.TextField(new Rect(startPosition.x, startPosition.y, EditorPrefs.GetFloat("NoteWidthSceneView", 300) - 30, EditorGUIUtility.singleLineHeight), SimpleNoteSO.Instance.gameObjectNote[index].note.title, textField);
					textField.fontStyle = FontStyle.Normal;
					EditorStyles.textField.normal.textColor = oriColor;
					EditorStyles.textField.focused.textColor = oriColor;

					if (GUI.Button(new Rect(startPosition.x + EditorPrefs.GetFloat("NoteWidthSceneView", 300) - 30, startPosition.y, 15, 15), "-"))
					{
						SimpleNoteSO.Instance.gameObjectNote[index].hide = !SimpleNoteSO.Instance.gameObjectNote[index].hide;
					}

					if (!SimpleNoteSO.Instance.gameObjectNote[index].hide) {
						GUIStyle textArea = new GUIStyle(EditorStyles.textArea);
						textArea.normal = EditorStyles.label.normal;
						textArea.focused = EditorStyles.label.normal;
						textArea.richText = true;
						textArea.normal.textColor = SimpleNoteData.Instance.getTextColor;
						textArea.focused.textColor = SimpleNoteData.Instance.getTextColor;
						GUI.SetNextControlName("Note-Scene" + Selection.activeGameObject.GetInstanceID());
						Undo.RecordObject(SimpleNoteSO.Instance, "Undo Note");
						SimpleNoteSO.Instance.gameObjectNote[index].note.note = EditorGUI.TextArea(new Rect(startPosition.x, startPosition.y + EditorGUIUtility.singleLineHeight * 1.05f, EditorPrefs.GetFloat("NoteWidthSceneView", 300), height), SimpleNoteSO.Instance.gameObjectNote[index].note.note, textArea);

						textArea.normal.textColor = oriColor;
						textArea.focused.textColor = oriColor;
						textArea.richText = false;
					}
					GUI.backgroundColor = guiBgOriColor;
					EditorUtility.SetDirty(SimpleNoteSO.Instance);
					
					Color guiOriColor = GUI.color;
					GUI.color = SimpleNoteData.Instance.getBgColor1;
					bool lastFocusIsField = (GUI.GetNameOfFocusedControl() == "Note-Scene" + Selection.activeGameObject.GetInstanceID() || GUI.GetNameOfFocusedControl() == "Title-Scene" + Selection.activeGameObject.GetInstanceID());
					if (EditorPrefs.GetBool("Save Note Control Button", true)) {
						if (lastFocusIsField) {
							if (Event.current.isKey && Event.current.keyCode == KeyCode.LeftControl)
								GUI.FocusControl(null);
						}
					}

					if (GUI.Button(new Rect(startPosition.x + EditorPrefs.GetFloat("NoteWidthSceneView", 300) - 15, startPosition.y, 15, 15), "x"))
					{
						Undo.RecordObject(SimpleNoteSO.Instance, "Delete Note");
						SimpleNoteSO.Instance.gameObjectNote.RemoveAt(index);
						EditorUtility.SetDirty(SimpleNoteSO.Instance);
					}
					GUI.color = guiOriColor;

					Handles.EndGUI();
				}
			}
		}

	}
	#endif
}
