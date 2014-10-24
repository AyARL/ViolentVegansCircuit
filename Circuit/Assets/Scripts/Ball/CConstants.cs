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

        public const int CONTROL_MOUSE_LEFT_BUTTON = 0;
        public const int CONTROL_MOUSE_RIGHT_BUTTON = 1;

        // Various Default values
        public const float DEFAULT_SPEED = 50.0f;
        public const float MAX_ROTATION_ANGLE = 15.0f;
        public const float DEFAULT_HIGH_VELOCITY = 5.0f;

        public const float CAMERA_EFFECT_INTENSITY = 3.0f;
    }

    public class CTags
    {
        // List of commonly used tags.
        public const string TAG_PLAYER = "Player";
        public const string TAG_TILE = "Tile";
        public const string TAG_WALL = "Wall";
        public const string TAG_AUDIO_CONTROLLER = "AudioController";
        public const string TAG_AUDIO = "Audio";
    }

    public class CNames
    {
        // List of commonly used names.
        public const string NAME_TILE_OBSTACLE = "CircuitTile_Obstacle";
    }

    public class CErrorStrings
    {
        // Declare error messages.
        public const string ERROR_UNHANDLED_DEVICE = "is an unsupported device type";
        public const string ERROR_UNRECOGNIZED_VALUE = "Provided enum value is unrecognized";
        public const string ERROR_NULL_OBJECT = "Failed to find object";
        public const string ERROR_MISSING_COMPONENT = "Missing component";
        public const string ERROR_UNRECOGNIZED_NAME = "Provided name is not handled by current function.";
        public const string ERROR_UNMATCHED_AUDIO_CLIP = "Unable to match provided audio file to available patterns.";
        public const string ERROR_AUDIO_FILES_NOT_LOADED = "Audio Controller has indicated that it hasn't finished loading all audio files.";
        public const string ERROR_AUDIO_FAILED_RELOAD = "Could not load audio resources.";
    }

    public class CAnimatorConstants
    {
        // Ball Face Animation names.
        public const string ANIMATION_BALL_DIZZY = "BallFace_dizzy";
        public const string ANIMATION_BALL_HAPPY = "BallFace_Happy";
        public const string ANIMATION_BALL_HIT = "BallFace_Hit";
        public const string ANIMATION_BALL_IDLE = "BallFace_Idle";
        public const string ANIMATION_BALL_UNHAPPY = "BallFace_Unhappy";

        // Animator parameters
        public const string ANIMATOR_PARAMETER_BALL_STATE = "iState";

        // Obstacle animation triggers.
        public const string ANIMATOR_TRIGGER_FAN_BROKEN = "Broken";
        public const string ANIMATOR_TRIGGER_FAN_DEAD = "Dead";
    }

    public class CAudio
    {
        // Default audio path
        public const string PATH_AUDIO = "./Assets/Audio";

        // Default regex patterns.
        public const string AUDIO_MUSIC = "Music";
        public const string AUDIO_EFFECT_GAMEOVER = "LevelFailed";
        public const string AUDIO_EFFECT_MENU_SELECT = "Menu_Select";
        public const string AUDIO_EFFECT_ELECTRIC_LOOP = "Electricity_Loop";
        public const string AUDIO_EFFECT_BALL_HIT = "Ball_Hit";
        public const string AUDIO_EFFECT_BALL_WALLHIT = "Ball_WallHit";
        public const string AUDIO_EFFECT_BALL_ROLLING = "Ball_Rolling";
        public const string AUDIO_EFFECT_ELECTRIC_JOLT = "Electricity_Jolt";
        public const string AUDIO_EFFECT_CHIP_POWER = "PowerUp";
        public const string AUDIO_EFFECT_LEVEL_COMPLETED = "LevelCompleted";
        public const string AUDIO_EFFECT_STAR_FALL = "StarDing";

        // Valid file extensions.
        public const string FILE_TYPE_MP3 = ".mp3";
        public const string FILE_TYPE_WAV = ".wav";

        // Audio altering variables.
        public const float MIN_VELOCITY_MAGNITUDE_ROLLING = 2.0f; 
        public const float AUDIO_FADE_VARIABLE = 0.3f;
    }

    public class CResourcePacks
    {
        public const string RESOURCE_CONTAINER_AUDIO_OBJECTS = "AudioSettings";
    }
}
