using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class BoxColliderInRect : MonoBehaviour
{
    void Update()
    {
        var collider = GetComponent<BoxCollider2D>();
        var rect = GetComponent<RectTransform>();
        collider.size = rect.rect.size;
    }
}
