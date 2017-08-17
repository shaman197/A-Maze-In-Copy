using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanLocalizationHolder : MonoBehaviour {

    public static string LeanLocalizationLanguage = "Language";

    private LeanLocalization ll;

    private void Start()
    {
        if (PlayerPrefs.HasKey(LeanLocalizationLanguage))
        {
            ll = transform.GetComponent<LeanLocalization>();
            ll.SetCurrentLanguage(PlayerPrefs.GetString(LeanLocalizationLanguage));
        }
    }
}
