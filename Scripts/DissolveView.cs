using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DissolveView : MonoBehaviour
{
    [Header("Dissolve Params")]
    [SerializeField] private float dissolveDuration = 1.0f;
    [SerializeField] private string alphaClipProperty = "_AlphaClip";
    [SerializeField] private string distortionProperty = "_Use_Distortion";

    private Material _material;
    private Coroutine _dissolveCoroutine;
    private bool _isDistortionEnabled = true;

    private void Awake()
    {
        var component = GetComponent<Renderer>();
        _material = component.material;
    }

    public void StartDissolve() => PlayDissolve(1f);
    public void StartUndissolve() => PlayDissolve(0f);

    public void ToggleDistortion()
    {
        _isDistortionEnabled = !_isDistortionEnabled;
        _material.SetFloat(distortionProperty, _isDistortionEnabled ? 1f : 0f);
    }

    private void PlayDissolve(float targetValue)
    {
        if (_dissolveCoroutine != null)
            StopCoroutine(_dissolveCoroutine);

        _dissolveCoroutine = StartCoroutine(AnimateDissolve(targetValue));
    }

    private System.Collections.IEnumerator AnimateDissolve(float target)
    {
        float start = _material.GetFloat(alphaClipProperty);
        float time = 0f;

        while (time < dissolveDuration)
        {
            float t = time / dissolveDuration;
            float value = Mathf.Lerp(start, target, t);
            _material.SetFloat(alphaClipProperty, value);

            time += Time.deltaTime;
            yield return null;
        }

        _material.SetFloat(alphaClipProperty, target);
    }
}
