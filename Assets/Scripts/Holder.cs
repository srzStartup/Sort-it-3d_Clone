using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class Holder
{
    public Transform transform { get; }
    public int order { get; }
    public List<Transform> balls { get; private set; }

    public Holder(Transform transform, int order, List<Transform> balls)
    {
        this.order = order;
        this.balls = balls;
    }
}
