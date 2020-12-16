using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GvrFillRoundPointer : MonoBehaviour
{
    public const float RETICLE_MIN_ROUND_Fill = 0f;
    public const float RETICLE_MAX_ROUND_Fill = 1f;

    [Range(-32767, 32767)]
    int reticleSortingOrder = 32767;
    public Material MaterialComp { private get; set; }
    float fill = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Renderer rendererComponent = GetComponent<Renderer>();
        rendererComponent.sortingOrder = reticleSortingOrder;

        MaterialComp = rendererComponent.material;
        CreateReticleVertices();
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void OnPointerHover()
    {
        fill += Time.deltaTime / GvrPointerManager.waitSec;
        if (fill >= RETICLE_MAX_ROUND_Fill)
        {
            fill = RETICLE_MAX_ROUND_Fill;
            GvrPointerManager.pointerState = PointerState.Gaze;
        }

        MaterialComp.SetFloat("_Frac", fill);
    }

    public void OnPointerExit()
    {
        fill = RETICLE_MIN_ROUND_Fill;
        MaterialComp.SetFloat("_Frac", fill);
    }

    private void CreateReticleVertices()
    {
        Mesh mesh = new Mesh();
        Vector2[] uvs = new Vector2[] { new Vector2(0f, 1f), new Vector2(1f, 1f),
        new Vector2(1f, 0f), new Vector2(0f, 0f)};

        #region Vertices
        Vector3[] vertices = new Vector3[] {new Vector3(-1f, 1, -1f), new Vector3(1f, 1, -1f),
        new Vector3(1f, -1, -1f), new Vector3(-1f, -1, -1f)};
        #endregion

        #region Triangles
        int[] triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        #endregion

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateBounds();
        gameObject.AddComponent<MeshFilter>();
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
