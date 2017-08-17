using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour {

	public Themes[] themes;

	[System.Serializable]
	public class Themes {
		public string name;
		public StoryParts[] storyParts;
	}

	[System.Serializable]
	public class StoryParts {
		public string name;
		public Sprite image;
		public string storyPartText;
	}
}