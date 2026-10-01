using UnityEngine;

// 4×4 행렬로 다이아몬드를 x축 방향으로 기울이기
[ExecuteAlways]
[RequireComponent(typeof(DiamondMesh))]
public class S09_ShearRaw : MonoBehaviour
{
    [SerializeField] float k = 0.6f; // (2 + 1) ÷ 5

    DiamondMesh diamondMesh;

    void OnEnable()
    {
        diamondMesh = GetComponent<DiamondMesh>();
    }
}
