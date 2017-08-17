using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningSign : MonoBehaviour {

    public Image warningIcon;
    public float timeBetween = 0.5f;

    //repeatEachCycle should always be even, otherwise always visable
    private int repeatEachCycle = 2;
    private bool visable;
    private bool isPlaying;

    private IEnumerator Blinking()
    {
        AudioController.Instance.PlayHunterWarningSound();

        for (int x = 0; x < repeatEachCycle; x++)
        {
            if (visable)
            {
                warningIcon.color = new Color(warningIcon.color.r, warningIcon.color.g, warningIcon.color.b, 0);
            }
            else
            {
                warningIcon.color = new Color(warningIcon.color.r, warningIcon.color.g, warningIcon.color.b, 1);
            }

            visable = !visable;

            yield return new WaitForSecondsRealtime(timeBetween);
        }

        AudioController.Instance.StopHunterWarningSound();
        isPlaying = false;
    }

    public void StartWarning()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            StartCoroutine(Blinking());
        }
    }
}
