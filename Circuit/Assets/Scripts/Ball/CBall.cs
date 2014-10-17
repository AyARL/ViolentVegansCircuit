using UnityEngine;
using System.Collections;
using Circuit;

public enum EMovementType
{
    MOVEMENT_NONE,
    MOVEMENT_TRANSLATE,
    MOVEMENT_FORCE_IMPULSE,
    MOVEMENT_FORCE_CONSTANT,
}

[ RequireComponent( typeof( Rigidbody ) ) ]
[ RequireComponent( typeof( Animator ) ) ]
public class CBall : MonoBehaviour {

    public enum EBallState
    {
        STATE_NORMAL    = 0,
        STATE_DIZZY     = 1,
        STATE_HAPPY     = 2,
        STATE_UNHAPPY   = 3,
        STATE_HIT       = 4,
    }

    private Vector3 m_v3InitialAccelerometerPosition;

    private int m_iAudioID = 0;

    private bool m_bCanMove = true;
    public bool MovementStatus { get { return m_bCanMove; } private set { m_bCanMove = value; } }

    public EMovementType m_eMovementType;
    public EBallState m_eCurrentState;

    private float m_fSpeed = CConstants.DEFAULT_SPEED;

    private CAudioControl m_cAudioController;

	/////////////////////////////////////////////////////////////////////////////
    /// Function:               Start
    /////////////////////////////////////////////////////////////////////////////
	void Start () 
    {
        string strFunctionName = "CBall::Start()";

        // Find the audio controller game object and report any errors.
        GameObject goAudioController = GameObject.FindGameObjectWithTag( CTags.TAG_AUDIO_CONTROLLER );
        if ( null == goAudioController )
        {
            Debug.LogError( string.Format( "{0} {1} " + CErrorStrings.ERROR_NULL_OBJECT, strFunctionName, typeof( GameObject ) ) );
        }

        // Get the Audio controller component.
        m_cAudioController = goAudioController.GetComponent< CAudioControl >();
        if ( null == m_cAudioController )
        {
            Debug.LogError( string.Format( "{0} {1} " + CErrorStrings.ERROR_MISSING_COMPONENT, strFunctionName, typeof( CAudioControl ) ) );
        }

        // Force the screen to remain in the Landscape left orientation.
        Screen.autorotateToPortrait = false;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Get initial accelorometer values when the user first launches the game.
	    m_v3InitialAccelerometerPosition = Input.acceleration;
	}
	
    /////////////////////////////////////////////////////////////////////////////
    /// Function:               Update
    /////////////////////////////////////////////////////////////////////////////
	void Update () 
    {
        // Check for touch input.
        RunTouchLogic();

        // Detect device type and run locomotion logic.
        RunMovementLogic();

        // Will run state/animation logic depending on current state.
        RunStatesLogic();
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               OnCollisionStay
    /////////////////////////////////////////////////////////////////////////////
    void OnCollisionStay( Collision cCollision )
    {
        // Check if the detected collision is a tile.
        if ( cCollision.gameObject.tag == CTags.TAG_TILE )
        {
            // Send a message to the tile containing the vector3 position.
            cCollision.gameObject.SendMessageUpwards( "OnReceiveMessage", transform.position, SendMessageOptions.DontRequireReceiver );
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               OnCollisionEnter
    /////////////////////////////////////////////////////////////////////////////
    void OnCollisionEnter( Collision cCollision )
    {
        // Error reporting.
        string strFunctionName = "CBall::OnCollisionEnter()";

        // Check if we hit a wall, and shake the camera if we did.
        if ( cCollision.gameObject.tag == CTags.TAG_WALL )
        {
            // Get the main Camera.
            GameObject goMainCamera = Camera.main.gameObject;

            // Retrieve the camera controls so we can make the screen shake.
            CCameraControl cCamControls = goMainCamera.GetComponent< CCameraControl >();
            if ( null == cCamControls )
            {
                // We couldn't find the component, report error and return.
                Debug.LogError( string.Format( "{0} {1} " + CErrorStrings.ERROR_MISSING_COMPONENT, strFunctionName, typeof( CCameraControl ) ) );
                return;
            }

            // Run the effect.
            cCamControls.CurrentEffectType = CCameraControl.EEffectType.EFFECT_SHAKE_CAMERA;

            // Play the wall hit sound.
            if ( true == m_cAudioController.AudioFilesLoaded )
            {
                CAudioControl.CreateAndPlayAudio( CAudio.AUDIO_EFFECT_BALL_WALLHIT, false, true, false, 0.8f );
            }
            else
            {
                // We couldn't find the component, report error and return.
                Debug.LogError( string.Format( "{0} {1} " + CErrorStrings.ERROR_AUDIO_FILES_NOT_LOADED, strFunctionName, typeof( CAudioControl ) ) );
                return;
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               OnCollisionExit
    /////////////////////////////////////////////////////////////////////////////
    void OnCollisionExit( Collision cCollision )
    {
        // Check if the detected collision is a tile.
        if ( cCollision.gameObject.tag == CTags.TAG_TILE )
        {
            // Send a message to the tile containing the vector3 position.
            cCollision.gameObject.SendMessageUpwards( "OnBallExit", transform.position, SendMessageOptions.DontRequireReceiver );
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               RunMovementLogic
    /////////////////////////////////////////////////////////////////////////////
    private void RunMovementLogic()
    {
        // Used when reporting errors.
        string strFunctionName = "CBall::RunMovementLogic()";

        // If the ball is moving, play the rolling sound, else make sure it's not playing.
        if ( Mathf.Abs( rigidbody.velocity.x ) > CAudio.MIN_VELOCITY_MAGNITUDE_ROLLING || Mathf.Abs( rigidbody.velocity.z ) > CAudio.MIN_VELOCITY_MAGNITUDE_ROLLING )
        {
            // SoundIsPlaying will return a list containing the GameObjects with the provided name, 0 = the sound isn't playing.
            if ( m_iAudioID <= 0 )
                m_iAudioID = CAudioControl.CreateAndPlayAudio( CAudio.AUDIO_EFFECT_BALL_ROLLING, true, true, true, 0.1f );
        }
        else
        {
            if ( m_iAudioID > 0 )
            { 
                // The stop sound function will check if the sound is playing before stopping it.
                CAudioControl.StopSound( m_iAudioID, false );
                m_iAudioID = 0;
            }
        }

        // The following variables will be used to calculate velocity.
        float fHorizontalInput = 0;
        float fVerticalInput = 0;

        // Check type of input and act accordingly.
        switch ( SystemInfo.deviceType )
        {
            case DeviceType.Desktop:

                // Set variables to mouse input.
                fHorizontalInput = Input.GetAxis( CConstants.CONTROL_MOUSE_X );
                fVerticalInput = Input.GetAxis( CConstants.CONTROL_MOUSE_Y );

                break;

            case DeviceType.Handheld:

                // As this is a mobile device we need to set values relative to the initial position.
                fHorizontalInput = Input.acceleration.x - m_v3InitialAccelerometerPosition.x;
                fVerticalInput = Input.acceleration.y - m_v3InitialAccelerometerPosition.y;

                break;

            default:

                // Unhandled device type.
                Debug.LogError( string.Format("{0} {1} " + CErrorStrings.ERROR_UNHANDLED_DEVICE , strFunctionName, SystemInfo.deviceType ) );

                break;
        }
	    
        // Create the direction vector and normalize it.
        Vector3 v3Direction = new Vector3( fHorizontalInput, 0, fVerticalInput );
        v3Direction.Normalize();

        if ( v3Direction.magnitude < 0.5f )
        {
            return;
        }

        // Set the movement type accordingly depending on the can move flag.
        if ( false == m_bCanMove )
        {
            m_eMovementType = EMovementType.MOVEMENT_NONE;
        }
        else
        {
            m_eMovementType = EMovementType.MOVEMENT_FORCE_IMPULSE;
        }

        // Move the ball according to the movement type specified - This is only temporary 
        //  as we look into the different movement possibilities until we find one that suits us.
        switch ( m_eMovementType )
        {
            case EMovementType.MOVEMENT_FORCE_CONSTANT:

                // The Default speed is not powerful enough for constant force so we need a booster variable.
                float fSpeedBoost = 50.0f;
                fSpeedBoost *= Time.deltaTime;

                // Apply Force.
                rigidbody.AddForce( ( v3Direction * m_fSpeed ) * fSpeedBoost, ForceMode.Force );

                break;

            case EMovementType.MOVEMENT_FORCE_IMPULSE:

                // Apply impulse force.
                rigidbody.AddForce( v3Direction * m_fSpeed * Time.deltaTime, ForceMode.Impulse );

                break;

            case EMovementType.MOVEMENT_TRANSLATE:

                // Translate the ball's position.
                transform.Translate( v3Direction * m_fSpeed * Time.deltaTime, Space.World );
	
                break;

                case EMovementType.MOVEMENT_NONE:

                // Do nothing.

                break;

            default:

                // Unknown variable, report issue.
                Debug.LogError( string.Format("{0} {1} " + CErrorStrings.ERROR_UNRECOGNIZED_VALUE , strFunctionName, m_eMovementType ) );

                break;
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               RunStatesLogic
    /////////////////////////////////////////////////////////////////////////////
    private void RunStatesLogic()
    {
        // Used when reporting erros.
        string strFunctionName = "CBall::RunStatesLogic()";

        // Apply the correct type of animation depending on current state.
        switch ( m_eCurrentState )
        {
            case EBallState.STATE_DIZZY:
                break;
            case EBallState.STATE_HAPPY:
                break;
            case EBallState.STATE_NORMAL:

                // Run the dizzy animation.
                if ( transform.rigidbody.velocity.x > CConstants.DEFAULT_HIGH_VELOCITY || transform.rigidbody.velocity.z > CConstants.DEFAULT_HIGH_VELOCITY )
                {
                    
                    m_eCurrentState = EBallState.STATE_DIZZY;
                }

                break;
            case EBallState.STATE_UNHAPPY:
                break;
            case EBallState.STATE_HIT:
                break;
            default:
                
                // Unknown variable, report issue.
                Debug.LogError( string.Format("{0} {1} " + CErrorStrings.ERROR_UNRECOGNIZED_VALUE, strFunctionName, m_eCurrentState ) );

                break;
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               RunTouchLogic
    /////////////////////////////////////////////////////////////////////////////
    private void RunTouchLogic()
    {
        // Used when reporting erros.
        string strFunctionName = "CBall::RunTouchLogic()";

        // According to device type, run touch logic.
        switch ( SystemInfo.deviceType )
        {
            case DeviceType.Desktop:

                // If the user clicked the left mouse button, we want the ball to stabilise.
                if ( Input.GetMouseButtonDown( CConstants.CONTROL_MOUSE_LEFT_BUTTON ) )
                {
                    // Invert the can move value.
                    m_bCanMove = !m_bCanMove;
                }

                break;
            case DeviceType.Handheld:

                // Check if there is a touch on the screen.
                if ( Input.touches.Length <= 0 )
                {
                    // There are no touches, do nothing.
                }
                else
                {
                    // Loop through all registered touches and run touch logic.
                    for ( int i = 0; i < Input.touchCount; ++i )
                    {
                        // Verify touch type and act correspondingly
                        switch ( Input.GetTouch( i ).phase )
                        {
                            case TouchPhase.Began:
                                
                                // User is currently touching the screen, stop the ball.
                                m_bCanMove = false;

                                break;
                            case TouchPhase.Ended:

                                // User stopped touching, ball can move again.
                                m_bCanMove = true;

                                break;
                            default:

                                // Do nothing.

                                break;
                        }
                    }
                }

                break;
            default:
                Debug.Log( string.Format("{0} {1} " + CErrorStrings.ERROR_UNHANDLED_DEVICE, strFunctionName, SystemInfo.deviceType ) );
                break;
        }          
    }
}
