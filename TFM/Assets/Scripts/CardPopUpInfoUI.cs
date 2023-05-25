using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

public class CardPopUpInfoUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] bool stateDependant;
    [SerializeField] GameState stateOfAction;
    [SerializeField] GameObject textBlock;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!stateDependant || GameManager.Instance.CurrentState == stateOfAction)
            textBlock.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!stateDependant || GameManager.Instance.CurrentState == stateOfAction || textBlock.activeSelf == true)
            textBlock.SetActive(false);
    }
}
