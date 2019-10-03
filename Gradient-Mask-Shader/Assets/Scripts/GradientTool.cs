using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GradientTool : MonoBehaviour
{
    public Vector3 StartPt { get { return _startPt; } set { _startPt = value; } }
    [SerializeField]
    private Vector3 _startPt;
    public Vector3 EndPt { get { return _endPt; } set { _endPt = value; } }
    [SerializeField]
    private Vector3 _endPt;

    public Transform ProjectionTransform { get { return _projectionDummy.transform; } }
    [SerializeField]
    private GameObject _projectionDummy;

    [SerializeField]
    private bool _initialized = false;


    [SerializeField]
    private Vector3 _cachedStartPt;
    [SerializeField]
    private Vector3 _cachedEndPt;
    [SerializeField]
    private Vector3 _cachedTransformPos;
    [SerializeField]
    public Matrix4x4 gradientMatrix;

    public Material _material;
    private Renderer _renderer;
    private Vector3 _rendererBounds, _gizmoPos;

    void OnDrawGizmos()
    {
        _gizmoPos = transform.position + new Vector3(0.0f, _rendererBounds.y * 0.5f, 0.0f);

        if (_gizmoPos != null)
        {
            Gizmos.DrawIcon(_gizmoPos + new Vector3(0.0f, 1.0f, 0.0f), "GradientTool.png");
        }
    }
    private void Awake()
    {       
        Initialize();
    }

    private void Update()
    {
        //Need to refactor somehow
        _material.SetMatrix("_GradientMatrix", gradientMatrix);

        if (_initialized == true && transform.position != _cachedTransformPos)
        {
            TransformChanged();
        }

        if (_cachedStartPt != _startPt || _cachedEndPt != _endPt)
        {
            CachePositions();
            UpdateProjection();
        }
    }

    public void Initialize()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
        {
            _rendererBounds = _renderer.bounds.size;
            _material = _renderer.sharedMaterial;
        }
        else
            _gizmoPos = transform.position;

        if (_initialized == true)
            return;

        else {
            _startPt = transform.position + new Vector3(_rendererBounds.x * 0.5f + _rendererBounds.y * 0.3f, 0.0f, 0.0f);
            _endPt = transform.position - new Vector3(_rendererBounds.x * 0.5f + _rendererBounds.y * 0.3f, 0.0f, 0.0f);

            if (_projectionDummy != null) {
                DestroyImmediate(_projectionDummy);
            }            
            _projectionDummy = new GameObject(transform.name + "Projection transform");
            _projectionDummy.hideFlags = HideFlags.HideInHierarchy;
            UpdateProjection();
            CachePositions();
        }

        _initialized = true;
        CachePositions();
    }

    void CachePositions()
    {
        _cachedTransformPos = transform.position;
        _cachedStartPt = _startPt;
        _cachedEndPt = _endPt;
    }

    void UpdateProjection()
    {
        //Set position of dummy right between gradient points
        float pointsDistance = Vector3.Distance(_endPt,_startPt);
        _projectionDummy.transform.position = _endPt;
        _projectionDummy.transform.localScale = new Vector3(pointsDistance, pointsDistance, pointsDistance);
        
        //Set X-axis look at start gradient point
        _projectionDummy.transform.LookAt(_startPt, Vector3.up);
        _projectionDummy.transform.rotation *= Quaternion.Euler(new Vector3(0, -90, 0));
        
        //Update material variables
        if (_projectionDummy != null)
        {
             gradientMatrix = _projectionDummy.transform.worldToLocalMatrix;

            _material.SetVector("_ProjectionRow0", gradientMatrix.GetRow(0));
            _material.SetVector("_ProjectionRow1", gradientMatrix.GetRow(1));
            _material.SetVector("_ProjectionRow2", gradientMatrix.GetRow(2));
            _material.SetVector("_ProjectionRow3", gradientMatrix.GetRow(3));
        }
    }

    private void TransformChanged()
    {
        Vector3 offset = transform.position - _cachedTransformPos;
        Matrix4x4 mat = Matrix4x4.Translate(offset);
        Vector3 newStartPos = mat.MultiplyPoint3x4(_startPt);
        Vector3 newEndPos = mat.MultiplyPoint3x4(_endPt);
        _startPt = newStartPos;
        _endPt = newEndPos;

        CachePositions();
        UpdateProjection();
    }
    public void Reset()
    {
        _initialized = false;
        Initialize();
    }
        
}
