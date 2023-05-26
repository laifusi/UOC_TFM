using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetonatorUIController : MonoBehaviour
{
    [SerializeField] Image detonatorPrefab;
    [SerializeField] EventCard eventCard;

    public void UpdateUI()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        foreach(EventOutcome outcome in eventCard.GetOutcomes())
        {
            Image detonationImage = Instantiate(detonatorPrefab, transform);
            if (outcome.detonatorStat == StatType.Power)
                detonationImage.sprite = IconManager.Instance.GetPowerSprite(outcome.powerDetonator);
            else if (outcome.detonatorStat != StatType.Health && outcome.detonatorStat != StatType.Attack)
                detonationImage.sprite = IconManager.Instance.GetStatSprite(outcome.detonatorStat);
            else
                Destroy(detonationImage.gameObject);
        }
    }
}
