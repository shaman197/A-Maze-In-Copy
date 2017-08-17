using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactController : MonoBehaviour {

    public static string MainkeyName = "Fact";

    public int amountOfFacts = 2;

    private LeanLocalizedTextArgs llta;

	private void Start () {
        llta = GetComponent<LeanLocalizedTextArgs>();

        llta.PhraseName = MainkeyName + Random.Range(1, amountOfFacts + 1);
    }
}
