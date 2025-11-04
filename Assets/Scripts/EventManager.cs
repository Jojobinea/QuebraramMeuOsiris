public static class EventManager
{
    public delegate void OnPhaseIsRotating();
    public static OnPhaseIsRotating OnPhaseIsRotatingEvent;

    public static void OnPhaseIsRotatingTrigger()
    {
        OnPhaseIsRotatingEvent?.Invoke();
    }


    public delegate void OnPhaseFinishedRotating();
    public static OnPhaseFinishedRotating onPhaseFinishedRotatingEvent;

    public static void OnPhaseFinishedRotatingTrigger()
    {
        onPhaseFinishedRotatingEvent?.Invoke();
    }


    public delegate void OnKeyCollected();
    public static OnKeyCollected onKeyCollectedEvent;

    public static void OnKeyCollectedTrigger()
    {
        onKeyCollectedEvent?.Invoke();
    }


    public delegate void OnGameLoose();
    public static OnGameLoose onGameLooseEvent;

    public static void OnGameLooseTrigger()
    {
        onGameLooseEvent?.Invoke();
    }


    public delegate void OnGameWin();
    public static OnGameWin onGameWinEvent;

    public static void OnGameWinTrigger()
    {
        onGameWinEvent?.Invoke();
    }

    public delegate void OnReachedDoor();
    public static OnReachedDoor onReachedDoorEvent;

    public static void OnReachedDoorTrigger()
    {
        onReachedDoorEvent?.Invoke();
    }
}
