using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Button3D : MonoBehaviour
{
    [SerializeField] private GameObject onMeLight;

    private void OnDisable()
    {
        onMeLight.SetActive(false);
    }

    protected virtual void Start()
    {
        onMeLight.SetActive(false);
    }

    private void OnMouseEnter()
    {
        onMeLight.SetActive(true);
    }

    private void OnMouseExit()
    {
        onMeLight.SetActive(false);
    }

    private void OnMouseDown()
    {

    }

    private void OnMouseUp()
    {
        
    }

    private void OnMouseUpAsButton()
    {
        OnClick();
    }

    public abstract void OnClick();    

}