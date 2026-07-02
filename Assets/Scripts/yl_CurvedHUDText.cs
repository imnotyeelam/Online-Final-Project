using TMPro;
using UnityEngine;

[ExecuteAlways]
public class yl_CurvedHUDText : MonoBehaviour
{
    [Range(-0.3f, 0.3f)]
    public float curvature = 0.12f;

    TMP_Text textMesh;

    void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    void LateUpdate()
    {
        if (textMesh == null) return;

        textMesh.ForceMeshUpdate();

        var textInfo = textMesh.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            int index = textInfo.characterInfo[i].vertexIndex;
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            Vector3[] vertices =
                textInfo.meshInfo[materialIndex].vertices;

            Vector3 mid =
                (vertices[index] + vertices[index + 2]) / 2;

            float x = mid.x * 0.001f;

            float offset =
                -(x * x) * curvature * 100f;

            for (int j = 0; j < 4; j++)
            {
                vertices[index + j].y += offset;
            }
        }

        textMesh.UpdateVertexData();
    }
}