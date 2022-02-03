using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : Clickable
{
    public int order { get; set; }
    public List<Transform> balls { get; set; }

    public List<Transform> slots { get; set; }

    public Bounds bounds => transform.GetComponent<Renderer>().bounds;
}
