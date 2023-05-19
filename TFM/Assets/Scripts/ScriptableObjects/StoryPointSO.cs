using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "new Story Points", menuName = "StoryPoints", order = 2)]
public class StoryPointSO : ScriptableObject
{
    [SerializeField] LocalizedString[] textLines;
    [SerializeField] bool isPreEventStoryPoint;
    [SerializeField] StoryPointSO controllingSP;
    [SerializeField] bool controllerMustHavePlayed;
    [SerializeField] bool isAbilitiesBook;
    [SerializeField] bool isLastSP;
    
    private bool hasPlayed;
    private int currentLine;

    public bool HasPlayed => hasPlayed;
    public bool IsPreEvent => isPreEventStoryPoint;
    public bool IsAbilitiesBook => isAbilitiesBook;
    public bool IsLastSP => isLastSP;

    public void ResetStory()
    {
        hasPlayed = false;
        currentLine = 0;
    }

    public bool ShouldPlay()
    {
        return !hasPlayed && (controllingSP == null || controllerMustHavePlayed == controllingSP.HasPlayed);
    }

    public string GetNextLine()
    {
        currentLine++;
        if (IsDone())
        {
            return null;
        }
        string lineToShow = textLines[currentLine - 1].GetLocalizedString();
        return lineToShow;
    }

    public void MarkPlayed()
    {
        hasPlayed = true;
    }

    public bool IsLastLine()
    {
        return currentLine == textLines.Length;
    }

    public bool IsDone()
    {
        return currentLine >= textLines.Length + 1;
    }
}
