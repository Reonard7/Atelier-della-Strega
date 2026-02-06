using TMPro;
using UnityEngine;
using System.Collections;

public class TutorialUI : MonoBehaviour
{
    public TMP_Text tutorialText;

    public void ShowText(string text, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ShowTextRoutine(text, duration));
    }

    private IEnumerator ShowTextRoutine(string text, float duration)
    {
        tutorialText.text = text;
        tutorialText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        tutorialText.gameObject.SetActive(false);
    }
}
