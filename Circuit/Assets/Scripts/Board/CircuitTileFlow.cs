using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CircuitTileFlow : MonoBehaviour
{
    [SerializeField]
    PathMarker entryMarker = null;

    public enum EntryType { Entry_Invalid, Entry_Flow, Entry_Connector }
    public EntryType TypeOfEntry { get; private set; }

    public enum ExitType { Exit_Invalid, Exit_Flow, Exit_Connector, Exit_Terminator }
    public ExitType TypeOfExit { get; private set; }

    private void Reset()
    {
        Initialise();
    }

    public void Initialise()
    {
        entryMarker = gameObject.GetComponentInChildren<StartPathMarker>();
        if(entryMarker != null)
        {
            TypeOfEntry = EntryType.Entry_Flow;
        }
        else
        {
            entryMarker = gameObject.GetComponentInChildren<InConnectorPathMarker>();
            if (entryMarker != null)
            {
                TypeOfEntry = EntryType.Entry_Connector;
            }
            else
            {
                TypeOfEntry = EntryType.Entry_Invalid;
                Debug.LogError(string.Format("Tile {0} has no StartPathMarker or InConnectorMarker", gameObject.name));
                return;
            }
        }


        PathMarker end = gameObject.GetComponentInChildren<EndPathMarker>();
        if (end != null)
        {
            TypeOfExit = ExitType.Exit_Flow;
        }
        else
        {
            end = gameObject.GetComponentInChildren<OutConnectorPathMarker>();
            if (end != null)
            {
                TypeOfExit = ExitType.Exit_Connector;
            }
            else
            {
                TypeOfExit = ExitType.Exit_Invalid;
                Debug.LogError(string.Format("Tile {0} has no EndPathMarker or OutConnectorMarker", gameObject.name));
                return;
            }
        }
    }
}
