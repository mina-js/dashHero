using System.Collections.Generic;
public static class EventManager
{
  // Define your events here
  public static event System.Action<string, Dictionary<string, object>> OnEventEmitted;

  // Method to invoke the OnButtonClicked event
  public static void EmitEvent(string eventKey, Dictionary<string, object> data)
  {
    OnEventEmitted?.Invoke(eventKey, data);
  }
}