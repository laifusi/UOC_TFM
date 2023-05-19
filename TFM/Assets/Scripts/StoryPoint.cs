using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPoint : MonoBehaviour
{
    [SerializeField] StoryPointSO storyPoint;

    public bool IsPreEvent => storyPoint.IsPreEvent;
    public bool ShouldPlay => storyPoint.ShouldPlay();
    public bool IsBook => storyPoint.IsAbilitiesBook;
    public bool IsLastSP => storyPoint.IsLastSP;

    private void Awake()
    {
        storyPoint.ResetStory();
    }

    public string GetNextLine()
    {
        return storyPoint.GetNextLine();
    }
    
    public void MarkPlayed()
    {
        storyPoint.MarkPlayed();
    }

    public bool IsLastLine()
    {
        return storyPoint.IsLastLine();
    }

    public bool IsDone()
    {
        return storyPoint.IsDone();
    }
}
