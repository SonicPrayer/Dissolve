using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveView : MonoBehaviour
{
    [Header("Dissolve Params")]
    [SerializeField] private float dissolveDuration = 1.0f;
    [SerializeField] private string alphaClipProperty = "_AlphaClip";
    [SerializeField] private string distortionProperty = "_Use_Distortion";

    [Header("Target Renderers")]
    [SerializeField] private List<Renderer> targetRenderers = new();

    private List<Material> _materials = new();
    private Coroutine _dissolveCoroutine;
    private bool _isDistortionEnabled = true;

    private void Awake()
    {
        _materials.Clear();

        foreach (var renderer in targetRenderers)
        {
            if (renderer == null) continue;

            foreach (var mat in renderer.materials) // Instance materials
            {
                _materials.Add(mat);
            }
        }
    }

    public void StartDissolve() => PlayDissolve(1f);
    public void StartUndissolve() => PlayDissolve(0f);

    public void ToggleDistortion()
    {
        _isDistortionEnabled = !_isDistortionEnabled;

        foreach (var mat in _materials)
        {
            if (mat.HasProperty(distortionProperty))
                mat.SetFloat(distortionProperty, _isDistortionEnabled ? 1f : 0f);
        }
    }

    private void PlayDissolve(float targetValue)
    {
        if (_dissolveCoroutine != null)
            StopCoroutine(_dissolveCoroutine);

        _dissolveCoroutine = StartCoroutine(AnimateDissolve(targetValue));
    }

    private IEnumerator AnimateDissolve(float target)
    {
        if (_materials.Count == 0) yield break;

        float start = _materials[0].GetFloat(alphaClipProperty);
        float time = 0f;

        while (time < dissolveDuration)
        {
            float t = time / dissolveDuration;
            float value = Mathf.Lerp(start, target, t);

            foreach (var mat in _materials)
            {
                if (mat.HasProperty(alphaClipProperty))
                    mat.SetFloat(alphaClipProperty, value);
            }

            time += Time.deltaTime;
            yield return null;
        }

        foreach (var mat in _materials)
        {
            if (mat.HasProperty(alphaClipProperty))
                mat.SetFloat(alphaClipProperty, target);
        }
    }
}
