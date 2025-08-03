using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]  // Works in Editor too
public class CircularLayout : MonoBehaviour
{
    [SerializeField] private float _radius = 2f;  // Circle radius
    [SerializeField] private float _startAngle = 0f;  // Starting angle (degrees)
    [SerializeField] private float _angleStep = 30f;  // Angle between objects (degrees)

    private void Update()
    {
        UpdateLayout();
    }

    public void Set(float radius, float startAngle, float angleStep)
    {
        _radius = radius;
        _startAngle = startAngle;
        _angleStep = angleStep;
    }

    private void UpdateLayout()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy)
                children.Add(child);
        }

        for (int i = 0; i < children.Count; i++)
        {
            float angle = _startAngle + (_angleStep * i);
            float rad = angle * Mathf.Deg2Rad;

            Vector2 pos = new Vector2(
                Mathf.Sin(rad) * _radius,
                Mathf.Cos(rad) * _radius
            );

            children[i].localPosition = pos;
        }
    }

    
}