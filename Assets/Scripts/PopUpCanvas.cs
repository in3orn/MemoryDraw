using UnityEngine;
using System.Collections;
using Dev.Krk.MemoryDraw.Common.Animations;

[RequireComponent(typeof(CanvasGroup))]
public class PopUpCanvas : MonoBehaviour
{
    [SerializeField]
    private float showDuration = 0.5f;

    [SerializeField]
    private float showDelay;

    [SerializeField]
    private float hideDuration = 0.5f;

    [SerializeField]
    private float hideDelay;

    private CanvasGroup canvasGroup;

    private ShowAnimator[] animators;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        animators = GetComponentsInChildren<ShowAnimator>();
    }

    public void Show()
    {
        canvasGroup.blocksRaycasts = true;
        //StartCoroutine(SetOpacity(canvasGroup.alpha, 1f, showDuration, showDelay));

        foreach (var animator in animators)
            animator.Show();
    }

    public void Hide()
    {
        canvasGroup.blocksRaycasts = false;
        //StartCoroutine(SetOpacity(canvasGroup.alpha, 0f, hideDuration, hideDelay));

        foreach (var animator in animators)
            animator.Hide();
    }

    public void ShowImmediately()
    {
        canvasGroup.blocksRaycasts = true;
        //canvasGroup.alpha = 1f;
    }

    public void HideImmediately()
    {
        canvasGroup.blocksRaycasts = false;
        //canvasGroup.alpha = 0f;
    }

    private IEnumerator SetOpacity(float from, float to, float duration, float delay)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        float t = 0f;
        while (true)
        {
            t += Time.deltaTime / duration;
            canvasGroup.alpha = Mathf.Lerp(from, to, t);

            if (t >= 1) break;
            yield return null;
        }
    }
}
