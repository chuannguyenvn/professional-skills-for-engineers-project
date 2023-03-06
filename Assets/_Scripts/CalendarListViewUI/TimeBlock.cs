using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeBlock<T> : MonoBehaviour
{
    public abstract void Init(T info);

}
