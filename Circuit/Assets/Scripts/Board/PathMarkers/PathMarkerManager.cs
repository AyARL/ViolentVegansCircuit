using UnityEngine;
using System.Collections;

public class PathMarkerManager : MonoBehaviour
{
    PathMarker marker = null;

    public enum MarkerType { Marker_Start, Marker_Path, Marker_Intersection, Marker_End }

    [SerializeField]
    protected MarkerType markerType = MarkerType.Marker_Path;
    public MarkerType TypeOfMarker { get { return markerType; } }

    public void UpdateToMatchType()
    {
        switch (markerType)
        {
            case MarkerType.Marker_Start:

                if (markerType != null && (marker as StartPathMarker) == null)
                {
                    DestroyImmediate(marker);
                    marker = gameObject.AddComponent<StartPathMarker>();
                }
                break;

            case MarkerType.Marker_Path:

                break;

            case MarkerType.Marker_Intersection:

                break;

            case MarkerType.Marker_End:

                break;
        }
    }
}
