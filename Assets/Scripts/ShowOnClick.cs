using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnClick : MonoBehaviour {

    public void ExpandMenu(GameObject expandedMenu)
    {
        if (!expandedMenu.activeSelf)
            expandedMenu.SetActive(true);
        else
            expandedMenu.SetActive(false);
    }
}

