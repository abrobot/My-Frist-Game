using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class MyMathFuctions
{

    static public int RoundNum(float num, int roundInt) {
        return Mathf.RoundToInt(num / roundInt) * roundInt;
    }

}
