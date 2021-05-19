using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class UIButtonAnimationComponent : UIElementComponent
{
    public float SelectSize=1.3f;

    [Header("Activarion Parameters")] 
    public float ActivationTime=0.2f;
    public AnimationCurve ActivationCurve= AnimationCurve.Linear(0,0,1,1);

    [Header("Desactivation Parameters")] 
    public float DesactivationTime=0.1f;
    public AnimationCurve DesactibvationCurve= AnimationCurve.Linear(0,0,1,1);
    

    private Button _button;
    private EventTrigger _eventTrigger;
    public void Awake()
    {
        _eventTrigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data)=>{Activate();});
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((data)=>{Desactivat();});
         _eventTrigger.triggers.Add(entry);
         _eventTrigger.triggers.Add(entry2);
         if (GetComponent<Button>()) _button = GetComponent<Button>();

    }

    public override void Activate()
    {
        if(_button!=null)if(!_button.interactable) return;
        transform.DOPause();
        transform.DOScale(Vector3.one * SelectSize, ActivationTime).SetEase(ActivationCurve).SetUpdate(true);
        Debug.Log("EnterUI");
    }

    public override void Desactivat()
    {
        if(_button!=null)if(!_button.interactable) return;
        transform.DOPause();
        transform.DOScale(Vector3.one , DesactivationTime).SetEase(DesactibvationCurve).SetUpdate(true);
        Debug.Log("ExiteUI");
    }
}
