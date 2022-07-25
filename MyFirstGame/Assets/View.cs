using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class View : MonoBehaviour
{
    static View _currentView;


    public GameObject viewGameObject;
    public bool isOpen = false;

    public Button openButten;
    public Button closeButten;
    public KeyCode toggleViewKey;

    public delegate void on();
    public on onViewOpen;
    public on onViewClose;



    public void Open() {
        currentView = this;
        viewGameObject.SetActive(true);
        isOpen = true;
        if (onViewOpen != null){
            onViewOpen();
        }
    }

    public void Close() {
        viewGameObject.SetActive(false);
        isOpen = false;
        if (onViewClose != null){
            onViewClose();
        }
    }

    

    protected virtual void Start() {
        if (openButten) {
            openButten.onClick.AddListener(() => {
                Open();
            });
        }

        if (closeButten) {
            closeButten.onClick.AddListener(() => {
                Close();
            });
        }
    }

    protected virtual void Update() {
        if (Input.GetKeyDown(toggleViewKey))
        {
            if (isOpen) {
                Close();
            } else {
                Open();
            }
            
        }
    }


    static View currentView {get {return _currentView ;} set {
        if (_currentView){
            _currentView.Close();
        }
        _currentView = value;
    }}

}
