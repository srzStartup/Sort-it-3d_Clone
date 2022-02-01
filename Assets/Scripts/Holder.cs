using System;
using System.Collections.Generic;

using UnityEngine;

public class Holder
{
    public int order { get; }
    public Queue<Transform> ballQueue { get; private set; }

    public Holder(int order, params Transform[] ballsToQueue)
    {
        this.order = order;
        ballQueue = new Queue<Transform>();

        if (ballsToQueue.Length > 0)
        {
            Array.ForEach(ballsToQueue, ballToQueue => ballQueue.Enqueue(ballToQueue));
        }
    }
}
