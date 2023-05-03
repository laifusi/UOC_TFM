using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPoint : MonoBehaviour
{
    [SerializeField] string[] textLines;
    [SerializeField] bool isPreEventStoryPoint;

    private int currentLine;

    public bool IsPreEvent => isPreEventStoryPoint;

    public string GetNextLine()
    {
        currentLine++;
        if (IsDone())
            return null;

        string lineToShow = textLines[currentLine - 1];
        return lineToShow;
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
