using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : GSystem
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _zDistanceFromTarget;
    [SerializeField] private float _yDistanceFromTarget;

    private Transform _cameraTarget;
    public override void InitializeSystem()
    {

    }

    public void FollowTarget(Transform target)
    {
        _cameraTarget = target;
    }

    public void LookAt(Vector3 position)
    {
        _camera.transform.forward = (position - _camera.transform.position).normalized;
    }

    public override void UpdateSystem()
    {
        if (_cameraTarget != null)
        {
            _camera.transform.position = _cameraTarget.position + (Vector3.back * _zDistanceFromTarget) + (Vector3.up * _yDistanceFromTarget);
            LookAt(_cameraTarget.position);
        }
    }
}
