using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DI.SimpleNote {
#if UNITY_EDITOR
	[CustomEditor(typeof(MonoBehaviour), true)]
	public class SimpleNoteAttributeDrawer : Editor
	{
		MonoBehaviour monoBehaviour;
		string title, note = "\n";
		int index = -1;

		void OnEnable()
		{
			monoBehaviour = (MonoBehaviour)target;
			if (SimpleNoteManager.Instance.getIndexAttributeScriptNote(monoBehaviour.gameObject, monoBehaviour) != -1)
			{
				index = SimpleNoteManager.Instance.getIndexAttributeScriptNote(monoBehaviour.gameObject, monoBehaviour);
				title = SimpleNoteManager.Instance.attScriptNote[index].note.title;
				note = SimpleNoteManager.Instance.attScriptNote[index].note.note;
			}
			else{
				title = "Title Here";
				note = "Note Here";
			}
		}

		void OnDisable()
		{
			if (monoBehaviour == null && index != -1)
			{
				//Delete note
				SimpleNoteManager.Instance.attScriptNote.RemoveAt(index);
			}
		}
	

		public override void OnInspectorGUI()
		{
			if(monoBehaviour == null)
				monoBehaviour = (MonoBehaviour)target;
			SimpleNoteAttribute attribute = (SimpleNoteAttribute)PropertyAttribute.GetCustomAttribute(monoBehaviour.GetType(), typeof(SimpleNoteAttribute));
			if (attribute != null)
			{
				//Check if note stored to manager;
				if (SimpleNoteManager.Instance.getIndexAttributeScriptNote(monoBehaviour.gameObject, monoBehaviour) == -1) {
					if (string.IsNullOrEmpty(attribute.title) && string.IsNullOrEmpty(attribute.note))
						SimpleNoteManager.Instance.attScriptNote.Add(new SimpleNoteManager.AttributeScriptNote(monoBehaviour.gameObject, monoBehaviour, title, note));
					else {
						SimpleNoteManager.Instance.attScriptNote.Add(new SimpleNoteManager.AttributeScriptNote(monoBehaviour.gameObject, monoBehaviour, attribute.title, attribute.note));
						title = attribute.title;
						note = attribute.note;
					}
				}
				if (index == -1)
					index = SimpleNoteManager.Instance.getIndexAttributeScriptNote(monoBehaviour.gameObject, monoBehaviour);

				GUIStyle textField = new GUIStyle(EditorStyles.textField);
				textField.fontStyle = FontStyle.Bold;
				if (GUI.GetNameOfFocusedControl() != "Title" + monoBehaviour.gameObject.GetInstanceID())
					textField.normal = EditorStyles.label.normal;

				EditorGUILayout.BeginHorizontal();
				GUI.SetNextControlName("Title" + monoBehaviour.gameObject.GetInstanceID());
				title = EditorGUILayout.TextField(title, textField);
				if (GUI.GetNameOfFocusedControl() == "Title" + monoBehaviour.gameObject.GetInstanceID())
				{
					if (GUILayout.Button("Save", GUILayout.Height(15), GUILayout.Width(45)))
					{
						//EditorPrefs.SetString(monoBehaviour.GetInstanceID() + "-SimpleNote-Title", title);
						SimpleNoteManager.Instance.attScriptNote[index].note.title = title;
						GUI.FocusControl(null);
					}
				}
				EditorGUILayout.EndHorizontal();

				GUIStyle textArea = new GUIStyle(EditorStyles.textArea);
				textArea.richText = true;
				if (GUI.GetNameOfFocusedControl() != "Note" + monoBehaviour.gameObject.GetInstanceID())
					textArea.normal = EditorStyles.label.normal;

				EditorGUILayout.BeginHorizontal();

				GUI.SetNextControlName("Note" + monoBehaviour.gameObject.GetInstanceID());
				note = EditorGUILayout.TextArea(note, textArea);
				if (GUI.GetNameOfFocusedControl() == "Note" + monoBehaviour.gameObject.GetInstanceID())
				{
					if (GUILayout.Button("Save", GUILayout.Height(15), GUILayout.Width(45)))
					{
						//EditorPrefs.SetString(monoBehaviour.GetInstanceID() + "-SimpleNote-Note", note);
						SimpleNoteManager.Instance.attScriptNote[index].note.note = note;
						GUI.FocusControl(null);
					}
				}

				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
				EditorGUI.DrawRect(GUILayoutUtility.GetLastRect(), SimpleNoteData.Instance.getBgColor1);
			}

			DrawDefaultInspector();
		}
	}

#endif
}
