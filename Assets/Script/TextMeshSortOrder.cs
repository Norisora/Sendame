using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshSortOrder : MonoBehaviour
{
    [SerializeField]
    MeshRenderer meshRenderer;
    [SerializeField]
    int sortingOrder;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer.sortingOrder = sortingOrder;
    }
}
