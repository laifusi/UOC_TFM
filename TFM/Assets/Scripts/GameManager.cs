using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] MapPosition[] mapPositions;
    [SerializeField] MapPosition initialPosition;

    private void Start()
    {
        MapPosition.OnPositionSelected += UpdatePositions;
        UpdatePositions(initialPosition);
    }

    private void UpdatePositions(MapPosition newMapPosition)
    {
        foreach(MapPosition pos in mapPositions)
        {
            pos.IsConnected = false;
            pos.IsCurrent = false;
        }

        newMapPosition.ChangeToCurrent();

        foreach(MapPosition pos in mapPositions)
        {
            pos.UpdatePositionColor();
        }
    }

    private void OnDestroy()
    {
        MapPosition.OnPositionSelected -= UpdatePositions;
    }
}
