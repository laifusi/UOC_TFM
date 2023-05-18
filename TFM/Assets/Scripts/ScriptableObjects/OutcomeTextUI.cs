using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutcomeTextUI : MonoBehaviour
{
    [SerializeField] TMP_Text textUI;
    [SerializeField] Vector3 offset;

    public void UpdateOutcomeUI(string text, Transform position, bool active)
    {
        textUI.SetText(text);
        transform.position = position.position + offset;
        gameObject.SetActive(active);
    }
}
