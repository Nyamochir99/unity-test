using UnityEngine;

// Controls the bend of every material that uses the "Custom/Curved World Lit" shader.
// Put this on one object in the scene (e.g. an empty "CurvedWorld" object).
[ExecuteAlways]
public class CurvedWorldController : MonoBehaviour
{
    [Tooltip("How much the road bends down in the distance. 0 = flat.")]
    [Range(-10f, 10f)]
    public float curveDown = 2f;

    [Tooltip("How much the road bends left (-) or right (+) in the distance.")]
    [Range(-10f, 10f)]
    public float curveSideways = 0f;

    [Tooltip("Bend starts from this point. Leave empty to use the Main Camera.")]
    public Transform origin;

    private static readonly int CurveOriginId = Shader.PropertyToID("_CurveOrigin");
    private static readonly int CurveDownId = Shader.PropertyToID("_CurveDown");
    private static readonly int CurveSidewaysId = Shader.PropertyToID("_CurveSideways");

    void LateUpdate()
    {
        Transform t = origin;
        if (t == null && Camera.main != null)
            t = Camera.main.transform;

        Vector3 originPos = t != null ? t.position : Vector3.zero;

        Shader.SetGlobalVector(CurveOriginId, originPos);
        Shader.SetGlobalFloat(CurveDownId, curveDown);
        Shader.SetGlobalFloat(CurveSidewaysId, curveSideways);
    }

    void OnDisable()
    {
        Shader.SetGlobalFloat(CurveDownId, 0f);
        Shader.SetGlobalFloat(CurveSidewaysId, 0f);
    }
}
