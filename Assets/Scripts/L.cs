using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class L
{

    public static void og(params System.Object[] objects)
    {
        var sb = new System.Text.StringBuilder();
        foreach (var obj in objects)
        {
            sb.AppendFormat("{0} ", obj.ToString());
        }
        Debug.Log(sb.ToString());
    }
}
