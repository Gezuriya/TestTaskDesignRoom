using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsController : MonoBehaviour
{
    private Vector3 _mouseOffset;
    Vector3 _previousPos, _nextPos;
    LayerMask _layerToIgnore;
    bool _canGo, _canBack;

    private void Start()
    {
        _layerToIgnore = 3;
    }
    private void OnMouseDown()
    {
        _previousPos = transform.position;
        gameObject.layer = 3;    
        _mouseOffset = gameObject.transform.position - GetMouseWorld();
        _canGo = false;
        _canBack = false;
    }

    private Vector3 GetMouseWorld()
    {
        Vector3 _mousePoint = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(_mousePoint);
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorld() + _mouseOffset;
    }

    private void OnMouseUp()
    {
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(_ray, out hit, 100f, _layerToIgnore))
        {
            if (hit.collider.tag == "CanBePlace" || hit.collider.tag == "Place")
            {
                GameObject _hitted = hit.collider.gameObject;
                if (_hitted.transform.childCount == 1)
                {
                    if (gameObject.name == "Prop5")
                    {
                        Interract(_hitted);
                    }
                    else if (gameObject.name == "Prop4")
                    {
                        if (_hitted.name == "Prop2" || _hitted.name == "Prop1" || _hitted.name == "Table" || _hitted.name == "Box" || _hitted.name == "Prop3" || _hitted.name == "Chair")
                            Interract(_hitted);
                        else if (_hitted.name == "Wood")
                        {
                            if (transform.rotation.y != 90)
                                transform.rotation = Quaternion.Euler(0, 90, 0);
                            Interract(_hitted);
                        }
                        else
                            ReturnToPlace();
                    }
                    else if (gameObject.name == "Prop3")
                    {
                        if (_hitted.name == "Prop2" || _hitted.name == "Prop1" || _hitted.name == "Chair" || _hitted.name == "Table" || _hitted.name == "Box")
                            Interract(_hitted);
                        else
                            ReturnToPlace();
                    }
                    else
                    {
                        if (_hitted.name == "Table" || _hitted.name == "Prop1" || _hitted.name == "Prop2")
                            Interract(_hitted);
                        else if (_hitted.name == "Chair")
                        {
                            if (transform.rotation.y != 90)
                                transform.rotation = Quaternion.Euler(0, 90, 0);
                            Interract(_hitted);
                        }
                        else
                            ReturnToPlace();
                    }
                }
                else
                    ReturnToPlace();
            }
            else
                ReturnToPlace();
        }
        gameObject.layer = 0;

    }

    void ReturnToPlace()
    {
        
        _canBack = true;
    }

    void Interract(GameObject _collisionObj)
    {
        _nextPos = _collisionObj.transform.GetChild(0).position;
        transform.SetParent(_collisionObj.transform);
        _canGo = true;
    }

    private void Update()
    {
        if(_canGo)
            transform.position = Vector3.MoveTowards(transform.position, _nextPos, 0.1f);
        if(_canBack)
            transform.position = Vector3.MoveTowards(transform.position, _previousPos, 0.1f);
        if(transform.position == _nextPos)
        {
            _canGo = false;
        }
        if(transform.position == _previousPos)
        {
            _canBack = false; 
        }
    }
}
