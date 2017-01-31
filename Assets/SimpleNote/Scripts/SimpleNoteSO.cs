using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DI.SimpleNote
{
	public class SimpleNoteSO : ScriptableObject
	{
		public static SimpleNoteSO Instance { get { return GetInstance(); } }
		public bool Hide { get; set; }
		static SimpleNoteSO GetInstance()
		{
			SimpleNoteSO _instance = FindObjectOfType<SimpleNoteSO>();
			if (_instance == null)
			{
				//OLD
				//GameObject instance = new GameObject("NoteManager");
				//instance.hideFlags = HideFlags.HideInHierarchy;
				//_instance = instance.AddComponent<SimpleNoteManager>();
				_instance = ScriptableObject.CreateInstance<SimpleNoteSO>();
			}
			if (!_instance._oldImplemantationChecked) {
				GameObject[] go = FindObjectsOfType<GameObject>();
				foreach (GameObject g in go)
				{
					Component[] components = g.GetComponents<Component>();
					for (int i = 0; i < components.Length; i++)
					{
						if (components[i] == null && g.hideFlags == HideFlags.HideInHierarchy && g.name == "NoteManager")
						{
							string s = g.name;
							Transform t = g.transform;
							while (t.parent != null)
							{
								s = t.parent.name + "/" + s;
								t = t.parent;
							}
							Debug.Log(s + " : is old method. Now fixed..");
							DestroyImmediate(g);
						}
					}
				}
			}
				_instance._oldImplemantationChecked = false;
			return _instance;
		}

		private bool _oldImplemantationChecked = false;

		[System.Serializable]
		public class GameObjectNote
		{
			public GameObject gameObject;
			public Note note = new Note();
			public bool hide = false;

			public GameObjectNote(GameObject gameObject)
			{
				this.gameObject = gameObject;
			}
			public GameObjectNote(GameObject gameObject, string title, string note)
			{
				this.gameObject = gameObject;
				this.note.title = title;
				this.note.note = note;
			}
			public GameObjectNote(GameObject gameObject, string note)
			{
				this.gameObject = gameObject;
				this.note.note = note;
			}
		}

		[System.Serializable]
		public class AttributeScriptNote
		{
			public GameObject gameObject;
			public MonoBehaviour script;
			public Note note = new Note();
			public bool hide = false;
			public AttributeScriptNote(GameObject gameObject, MonoBehaviour script)
			{
				this.gameObject = gameObject;
				this.script = script;
			}

			public AttributeScriptNote(GameObject gameObject, MonoBehaviour script, string title, string note)
			{
				this.gameObject = gameObject;
				this.script = script;
				this.note.title = title;
				this.note.note = note;
			}
			public AttributeScriptNote(GameObject gameObject, string note)
			{
				this.gameObject = gameObject;
				this.note.note = note;
			}
		}

		public int getIndexGameObjectNote(GameObject go)
		{
			for (int i = 0; i < gameObjectNote.Count; i++)
			{
				if (gameObjectNote[i].gameObject == go)
					return i;
			}
			return -1;
		}

		public int getIndexAttributeScriptNote(GameObject go, MonoBehaviour script)
		{
			for (int i = 0; i < attScriptNote.Count; i++)
			{
				if (attScriptNote[i].gameObject == go && attScriptNote[i].script == script)
					return i;
			}
			return -1;
		}

		public List<GameObjectNote> gameObjectNote = new List<GameObjectNote>();
		public List<AttributeScriptNote> attScriptNote = new List<AttributeScriptNote>();
	}

#if UNITY_EDITOR
	public class SimpleNoteManagerMenu
	{

		[MenuItem("GameObject/SimpleNote/Add or Remove Note", priority = 0)]
		public static void AddRemoveNote()
		{
			if (!Selection.activeObject)
				Debug.Log("No Game Object Selected");
			else
			{
				foreach (GameObject obj in Selection.gameObjects)
				{
					if (SimpleNoteSO.Instance.getIndexGameObjectNote(obj) != -1)
						SimpleNoteSO.Instance.gameObjectNote.RemoveAt(SimpleNoteSO.Instance.getIndexGameObjectNote(obj));
					else
					{
						SimpleNoteSO.Instance.gameObjectNote.Add(new SimpleNoteSO.GameObjectNote(obj, obj.name, "Note"));
					}
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
#else
					UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
#endif
					EditorUtility.SetDirty(SimpleNoteSO.Instance);
				}
			}
		}
	}
#endif


}
