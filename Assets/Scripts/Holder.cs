using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Holder : MonoBehaviour, IPointerClickHandler
{
    private HolderEventSystem _holderEventChannel;
    private float _duration = .3f;

    public int order { get; set; }
    public List<Transform> slots { get; set; }
    // key: slotIndex, value: ball
    private Dictionary<int, Transform> balls;

    public void SetHolderEventChannel(HolderEventSystem channel)
    {
        _holderEventChannel = channel;
    }

    public Bounds bounds => transform.GetComponent<Renderer>().bounds;

    public void BallsToSlots(List<Transform> ballsToAdd)
    {
        balls = ballsToAdd.ToDictionary(ballToAdd =>
        {
            int slotIndex = ballsToAdd.IndexOf(ballToAdd);
            ballToAdd.parent = slots[slotIndex];
            ballToAdd.localPosition = Vector3.zero;

            return slotIndex;
        });
    }

    public bool hasAvailableSlot
    {
        get
        {
            return slots.Find(slot =>
            {
                return slot.GetComponentsInChildren<Transform>()
                    .ToList()
                    .FindAll(child => !child.Equals(slot))
                    .Count == 0;
            }) != null;
        }
    }
    
    private Transform availableSlot
    {
        get
        {
            return slots.FindAll(slot =>
            {
                return slot.GetComponentsInChildren<Transform>()
                    .ToList()
                    .FindAll(child => !child.Equals(slot))
                    .Count == 0;
            }).First();
        }
    }

    public bool isEmpty
    {
        get
        {
            return slots.TrueForAll(slot =>
            {
                return slot.GetComponentsInChildren<Transform>()
                    .ToList()
                    .FindAll(child => !child.Equals(slot))
                    .Count == 0;
            });
        }
    }

    public Transform LastBall => balls[balls.Keys.Max()];

    public void Add(Transform ballToAdd)
    {
        int slotIndex = slots.IndexOf(availableSlot);

        ballToAdd.parent = null;
        ballToAdd.parent = availableSlot;

        Vector3 targetPosition = new Vector3(ballToAdd.parent.position.x, ballToAdd.position.y, ballToAdd.parent.position.z);
        ballToAdd.DOMove(targetPosition, _duration)
            .OnComplete(() => ballToAdd.DOLocalMove(Vector3.zero, _duration));

        balls.Add(slotIndex, ballToAdd);

        if (slots.Count == balls.Count)
        {
            Color color = ballToAdd.GetComponent<Renderer>().material.color;

            bool isFinish = balls.Values
                .ToList()
                .TrueForAll(ball => ball.GetComponent<Renderer>().material.color.Equals(color));

            if (isFinish)
                _holderEventChannel.RaiseHolderCompletedEvent(this);
        }
    }

    // need to check if isEmpty
    public Transform Pop()
    {
        int index = balls.Keys.Max();
        Transform poppedBall = balls[index];
        poppedBall.parent = null;
        balls.Remove(index);

        return poppedBall;
    }

    public void Popup(float popupHeight)
    {
        int ballIndex = balls.Keys.Max();
        Transform ball = balls[ballIndex];

        ball.DOMoveY(GetAbsolutePopupHeight(popupHeight), _duration);
    }

    public float GetAbsolutePopupHeight(float popupHeight)
    {
        return bounds.size.y + popupHeight;
    }

    public void BackDown()
    {
        Transform ball = balls[balls.Keys.Max()];
        ball.DOMoveY(ball.position.y + 1.5f, _duration)
            .OnComplete(() => ball.DOLocalMoveY(.0f, _duration));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _holderEventChannel.RaiseHolderClickedEvent(this);
    }
}
