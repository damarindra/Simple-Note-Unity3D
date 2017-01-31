﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DI.SimpleNote {
	public class SimpleNoteManager : MonoBehaviour {
		public static SimpleNoteManager Instance { get { return GetInstance();  } }
		public bool Hide { get; set; }
		static SimpleNoteManager GetInstance() {
			SimpleNoteManager _instance = FindObjectOfType<SimpleNoteManager>();
			if (_instance == null) {
				GameObject instance = new GameObject("NoteManager");
				instance.hideFlags = HideFlags.HideInHierarchy;
				_instance = instance.AddComponent<SimpleNoteManager>();
			}
			return _instance;
		}
		
		[System.Serializable]
		public class GameObjectNote {
			public GameObject gameObject;
			public Note note = new Note();
			public bool hide = false;

			public GameObjectNote(GameObject gameObject) {
				this.gameObject = gameObject;
			}
			public GameObjectNote(GameObject gameObject, string title, string note) {
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
	public class SimpleNoteManagerMenu {

		[MenuItem("GameObject/SimpleNote/Add or Remove Note", priority = 0)]
		public static void AddRemoveNote()
		{
			if (!Selection.activeObject)
				Debug.Log("No Game Object Selected");
			else
			{
				foreach (GameObject obj in Selection.gameObjects)
				{
					if (SimpleNoteManager.Instance.getIndexGameObjectNote(obj) != -1)
						SimpleNoteManager.Instance.gameObjectNote.RemoveAt(SimpleNoteManager.Instance.getIndexGameObjectNote(obj));
					else
					{
						SimpleNoteManager.Instance.gameObjectNote.Add(new SimpleNoteManager.GameObjectNote(obj, obj.name, "Note"));
					}
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
#else
					UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
#endif
					EditorUtility.SetDirty(SimpleNoteManager.Instance);
				}
			}
		}
	}
#endif


}
