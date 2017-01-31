using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DI.SimpleNote
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class SimpleNoteAttribute : PropertyAttribute
	{

		public SimpleNoteAttribute() {
		}


	}

}
