using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ListData<T>
{
    public List<T> list;

    public ListData() { }

    public ListData(List<T> list)
    {
        this.list = list;
    }
}