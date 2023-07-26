using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnVisual;
    [SerializeField] private GameObject fryingParticles;

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventHandler e)
    {
        stoveOnVisual.SetActive(e.isOn);
        fryingParticles.SetActive(e.isOn);
    }

    private void TurnOn()
    {
        stoveOnVisual.SetActive(true);
    }

    private void TurnOff()
    {
        stoveOnVisual.SetActive(false);
    }
}
