using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class CustomGameObject
{

    public static GameObject MakeGameObject(Type[] components = default(Type[]), string name = "GameObject", Vector3 position = default(Vector3), GameObject parent = null) {
        
        GameObject newGameObject = new GameObject(name, components);
        newGameObject.transform.position = position;

        if (parent) {
            newGameObject.transform.parent = parent.transform;
        }

        return newGameObject;
    }

    public static GameObject MakeGameObject(Type[] components = default(Type[]), string name = "GameObject", Vector3 position = default(Vector3)) {
        
        GameObject newGameObject = new GameObject(name, components);
        newGameObject.transform.position = position;

        return newGameObject;
    }

        public static GameObject MakeGameObject(string name = "GameObject", Vector3 position = default(Vector3), GameObject parent = null) {
        
        GameObject newGameObject = new GameObject(name);
        newGameObject.transform.position = position;

        if (parent) {
            newGameObject.transform.parent = parent.transform;
        }

        return newGameObject;
    }

    public static GameObject MakeGameObject(string name = "GameObject", Vector3 position = default(Vector3)) {
        
        GameObject newGameObject = new GameObject(name);
        newGameObject.transform.position = position;

        return newGameObject;
    }

    
    public static GameObject MakeGameObject(string name = "GameObject") {
        
        GameObject newGameObject = new GameObject(name);
        return newGameObject;
    }

        public static GameObject MakeGameObject() {
        
        GameObject newGameObject = new GameObject();


        return newGameObject;
    }

    
    
}
