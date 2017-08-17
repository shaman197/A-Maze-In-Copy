using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPref : MonoBehaviour
{
    public GameObject ResetButton;

    public void DeleteLocalSave()
    {
        PlayerPrefs.DeleteAll();

        // De playerpref wordt al aangeroepen voor deze knop werkt, deze moet handmatig gedaan worden
        DataHolder.Instance.SetLevelSystemIndexNumber(1);
    }
}
