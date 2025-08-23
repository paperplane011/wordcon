using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class HoverChecker : MonoBehaviour // checks if cursor is hovered upon this gameobject (canvas)
{

    private EventTrigger _eventTrigger;

    public bool IsCursorHovered { get; private set; }

    private void Awake()
    {
        _eventTrigger = GetComponent<EventTrigger>();
    }

    private void Start()
    {
        InitializePointersBehaviour();
    }


    private void InitializePointersBehaviour()
    {
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();

        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerExitEntry.eventID = EventTriggerType.PointerExit;


        pointerEnterEntry.callback.AddListener((v) => PointerEnterBehaviour());
        pointerExitEntry.callback.AddListener((v) => PointerExitBehaviour());


        _eventTrigger.triggers.Add(pointerEnterEntry);
        _eventTrigger.triggers.Add(pointerExitEntry);
    }

    private void PointerEnterBehaviour()
    {
        IsCursorHovered = true;
    }

    private void PointerExitBehaviour()
    {
        IsCursorHovered = false;
    }

 


}
