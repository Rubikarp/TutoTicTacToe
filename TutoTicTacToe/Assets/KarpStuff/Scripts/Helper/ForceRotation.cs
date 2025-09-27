using UnityEngine;

[ExecuteAlways]
public class ForceRotation : MonoBehaviour
{
    public bool globalRotation = true;
    public Quaternion forcedRotation = Quaternion.identity;

    void Update()
    {
        forcedRotation.Normalize();
        if (globalRotation)
        {
            transform.rotation = forcedRotation;
        }
        else
        {
            transform.localRotation = forcedRotation;
        }
    }
}

