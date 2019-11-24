using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Test_Draw_Line : MonoBehaviour
{
    // Reference
    [Header("Reference Settings")]
    public Material _mat;

    // Twerkable
    [Header("Line Settings")]
    public float _startX;
    public float _endX;
    public float _minY;
    public float _maxY;
    public int _noOfPoints;
    public float _lineHeight;
    public Color _color;

    // Hidden
    [HideInInspector]
    public List<Vector3> ListOfWP;

    // Private
    LineRenderer _lr;

    private void Start()
    {
        _lr = GetComponent<LineRenderer>();
        DrawLine();
    }
    void DrawLine()
    {
        //-- Set Color
        _lr.material = _mat;
        _lr.startColor = _color;
        _lr.endColor = _color;

        //-- Set Height
        _lr.startWidth = _lineHeight;
        _lr.endWidth = _lineHeight;

        //-- Get Width
        float width = (_endX - _startX) / _noOfPoints;

        //-- Set Position
        _lr.positionCount = _noOfPoints;
        float randomY = Random.Range(_minY, _maxY);
        Vector3 currPos = transform.position + new Vector3(_startX - width, randomY, 0);
        ListOfWP.Add(currPos);
        for (int i = 0; i < _noOfPoints; ++i)
        {
            randomY = Random.Range(_minY, _maxY);
            _lr.SetPosition(i, currPos);
            currPos += new Vector3(_startX + i * width, randomY, 0);
            _lr.SetPosition(i+1, currPos);
            ListOfWP.Add(currPos);
        }
    }
}
