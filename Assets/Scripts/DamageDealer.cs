using UnityEngine;
using UnityEngine.Events;

public class DamageDealer : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnDamageDealtEvent;

    public void TriggerDamage()
    {
        OnDamageDealtEvent.Invoke();
    }
}
