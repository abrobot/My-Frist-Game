using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewWithMouse : View
{
    public PlayerUI playerUI;

    protected override void Start() {
        base.Start();
        onViewOpen += () => { playerUI.firstPerson = false;};
        onViewClose += () => { playerUI.firstPerson = true;};
    }

    protected override void Update() {
        base.Update();
    }
    
}
