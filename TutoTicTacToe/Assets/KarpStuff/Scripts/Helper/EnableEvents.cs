using UnityEngine.Events;
using UnityEngine;

public class EnableEvents : MonoBehaviour
{
    public UnityEvent onEnable;
    public UnityEvent onDisable;

    private void OnEnable() => onEnable?.Invoke();
    private void OnDisable() => onDisable?.Invoke();
}

