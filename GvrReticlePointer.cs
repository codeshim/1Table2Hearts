using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GvrReticlePointer : MonoBehaviour
{
    // Gaze
    public const float RETICLE_MIN_INNER_ANGLE = 0.1f;
    public const float RETICLE_MIN_OUTER_ANGLE = 0.01f;
    public const float RETICLE_MAX_INNER_ANGLE = 0.5f;
    public const float RETICLE_MAX_OUTER_ANGLE = 0.4f;

    [Range(-32767, 32767)]
    int reticleSortingOrder = 32767;
    int reticleSegments = 20;
    public float reticleGrowthSpeed = 8.0f;

    public Material MaterialComp { private get; set; }
    public float ReticleInnerAngle { get; private set; }
    public float ReticleOuterAngle { get; private set; }
    public float ReticleInnerDiameter { get; private set; }
    public float ReticleOuterDiameter { get; private set; }

    void Awake()
    {
        ReticleInnerAngle = RETICLE_MIN_INNER_ANGLE;
        ReticleOuterAngle = RETICLE_MIN_OUTER_ANGLE;
    }

    // Start is called before the first frame update
    void Start()
    {
        Renderer rendererComponent = GetComponent<Renderer>();
        rendererComponent.sortingOrder = reticleSortingOrder;

        MaterialComp = rendererComponent.material;

        CreateReticleVertices();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDiameters();
    }

    private void CreateReticleVertices()
    {
        Mesh mesh = new Mesh();
        gameObject.AddComponent<MeshFilter>();
        GetComponent<MeshFilter>().mesh = mesh;

        int segments_count = reticleSegments;
        int vertex_count = (segments_count + 1) * 2;

        #region Vertices

        Vector3[] vertices = new Vector3[vertex_count];

        const float kTwoPi = Mathf.PI * 2.0f;
        int vi = 0;
        for (int si = 0; si <= segments_count; ++si)
        {
            float angle = (float)si / (float)segments_count * kTwoPi;

            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);

            vertices[vi++] = new Vector3(x, y, 0.0f);
            vertices[vi++] = new Vector3(x, y, 1.0f);
        }
        #endregion

        #region Triangles
        int indices_count = (segments_count + 1) * 3 * 2;
        int[] indices = new int[indices_count];

        int vert = 0;
        int idx = 0;
        for (int si = 0; si < segments_count; ++si)
        {
            indices[idx++] = vert + 1;
            indices[idx++] = vert;
            indices[idx++] = vert + 2;

            indices[idx++] = vert + 1;
            indices[idx++] = vert + 2;
            indices[idx++] = vert + 3;

            vert += 2;
        }
        #endregion

        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateBounds();
    }

    public void OnPointerEnter()
    {
        ReticleInnerAngle = RETICLE_MAX_INNER_ANGLE;
        ReticleOuterAngle = RETICLE_MAX_OUTER_ANGLE;
    }

    public void OnPointerExit()
    {
        ReticleInnerAngle = RETICLE_MIN_INNER_ANGLE;
        ReticleOuterAngle = RETICLE_MIN_OUTER_ANGLE;
    }

    public void UpdateDiameters()
    {
        if (ReticleInnerAngle < RETICLE_MIN_INNER_ANGLE)
        {
            ReticleInnerAngle = RETICLE_MIN_INNER_ANGLE;
        }

        if (ReticleOuterAngle < RETICLE_MIN_OUTER_ANGLE)
        {
            ReticleOuterAngle = RETICLE_MIN_OUTER_ANGLE;
        }

        ReticleInnerDiameter =
      Mathf.Lerp(ReticleInnerDiameter, ReticleInnerAngle, Time.unscaledDeltaTime * reticleGrowthSpeed);
        ReticleOuterDiameter =
      Mathf.Lerp(ReticleOuterDiameter, ReticleOuterAngle, Time.unscaledDeltaTime * reticleGrowthSpeed);

        MaterialComp.SetFloat("_InnerDiameter", ReticleInnerDiameter);
        MaterialComp.SetFloat("_OuterDiameter", ReticleOuterDiameter);
    }
}
