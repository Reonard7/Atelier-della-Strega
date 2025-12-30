using Events;
using TMPro;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        AlchemyEvents.OnBrewingStarted += OnBrewingStarted;
    }

    private void OnDisable()
    {
        AlchemyEvents.OnBrewingStarted -= OnBrewingStarted;
    }

    private void OnBrewingStarted()
    {
        int number = Random.Range(1, 21);
        _text.text = number.ToString();

        AlchemyEvents.OnDiceCast?.Invoke(number);
    }
}
