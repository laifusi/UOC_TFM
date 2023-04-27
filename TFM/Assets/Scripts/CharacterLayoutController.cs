using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLayoutController : MonoBehaviour
{
    [SerializeField] GameState state;
    [SerializeField] bool needsRearranging;
    [SerializeField] GridLayoutData twoCharLayout;
    [SerializeField] GridLayoutData threeCharLayout;
    [SerializeField] GridLayoutData fourCharLayout;

    private List<Transform> characters = new List<Transform>();
    private GridLayoutGroup layout;
    private RectTransform rectTransform;

    public bool NeedsRearranging => needsRearranging;
    public GameState State => state;

    private void Awake()
    {
        layout = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void RearrangeLayout(Transform newChar)
    {
        characters.Add(newChar);
        
        if(characters.Count <= 2)
        {
            UpdateLayout(twoCharLayout);
        }
        else
        {
            UpdateLayout(fourCharLayout);
        }
    }

    private void UpdateLayout(GridLayoutData layoutData)
    {
        layout.cellSize = new Vector3(layoutData.cellWidth, layoutData.cellHeight);
        layout.constraintCount = layoutData.columnCount;
        rectTransform.sizeDelta = new Vector2(layoutData.rectWidth, rectTransform.sizeDelta.y);
    }
}

[Serializable]
public struct GridLayoutData
{
    public int cellWidth;
    public int cellHeight;
    public int columnCount;
    public float rectWidth;
}
