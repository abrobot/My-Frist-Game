using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class MyMathFuctions
{

    static public int RoundNum(float num, int roundInt) {
        return Mathf.RoundToInt(num / roundInt) * roundInt;
    }

    public static bool CheckEven(int num) {
        if (num % 2 == 0) {
            return true;
        } else {
            return false;
        }
    }

}
