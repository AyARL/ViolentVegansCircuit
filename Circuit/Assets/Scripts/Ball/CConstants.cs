using UnityEngine;
using System.Collections;

namespace Circuit
{
    public class CConstants 
    {
        // Control related constants
        public const string CONTROL_AXIS_HORIZONTAL = "Horizontal";
        public const string CONTROL_AXIS_VERTICAL = "Vertical";
        public const string CONTROL_MOUSE_X = "Mouse X";
        public const string CONTROL_MOUSE_Y = "Mouse Y";

        // Various Default values
        public const float DEFAULT_SPEED = 50.0f;

        // Animator parameters
        public const string ANIMATOR_PARAMETER_BALL_STATE = "iState";
    }

    public class CTags
    {
        // List of commonly used tags.
        public const string TAG_PLAYER = "Player";
    }

    public class CErrorStrings
    {
        // Declare error messages.
        public const string ERROR_UNHANDLED_DEVICE = "is an unsupported device type";
        public const string ERROR_UNRECOGNIZED_VALUE = "Provided enum value is unrecognized";
        public const string ERROR_NULL_OBJECT = "Failed to find object";
        public const string ERROR_MISSING_COMPONENT = "Missing component";
    }
}
