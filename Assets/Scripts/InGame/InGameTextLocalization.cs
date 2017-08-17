using UnityEngine;
using UnityEngine.UI;

public class InGameTextLocalization : MonoBehaviour {

    private Text usingText;
    private TextMesh realText;

    void Start () {
        usingText = GetComponent<Text>();
        realText = GetComponent<TextMesh>();

        realText.text = usingText.text;
	}
	
}
