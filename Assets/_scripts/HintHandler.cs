using UnityEngine;
using System.Collections;

public class HintHandler : MonoBehaviour {

    public Transform hint;
    public Transform arrowHint;

    private bool hasHints;

    public bool HintsEnabled {
        get {
            return hasHints;
        }
    }

    /// <summary>
    /// public getter to the ienumerator EnableHints
    /// </summary>
    public void StartHint() {
        StartCoroutine(EnableHints());
    }

    /// <summary>
    /// public getter to the ienumerator DisableHints
    /// </summary>
    private void HideHints() {
        if(hasHints)
            StartCoroutine(DisableHints());
    }

    /// <summary>
    /// Stops the animations and disables the hints
    /// </summary>
    public void Stop() {
        StopAllCoroutines();
        hasHints = false;
        hint.localScale = arrowHint.localScale = Vector3.zero;
    }

    /// <summary>
    /// Disables the hints from the screen on a small animation
    /// in which the scale shrinks from 1 to 0
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisableHints() {
        Vector3 target = Vector3.zero;
        hint.localScale = arrowHint.localScale = new Vector3(1f, 1f, 1f);
        while (hint.localScale != target)
        {
            hint.localScale = arrowHint.localScale = Vector3.Lerp(hint.localScale, target, .1f);
            yield return null;
        }
        hasHints = false;
    }

    /// <summary>
    /// Shows the hints on the screen
    /// Plays a small animation in which the scale grows 
    /// from 0 to 1
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableHints() {
        hasHints = true;
        Vector3 target = new Vector3(1f, 1f, 1f);
        hint.localScale = arrowHint.localScale = Vector3.zero;
        while (hint.localScale != target) {
            hint.localScale = arrowHint.localScale = Vector3.Lerp(hint.localScale, target, .1f);
            yield return null;
        }
        Invoke("HideHints", 2f);
    }
}
