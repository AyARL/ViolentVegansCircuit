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
public class CBall : MonoBehaviour {

    private Vector3 m_v3InitialAccelerometerPosition;
    public float m_fSpeed = CConstants.DEFAULT_SPEED;
    public EMovementType m_eMovementType;

	// Use this for initialization
	void Start () 
    {
        // Force the screen to remain in the Landscape left orientation.
        Screen.autorotateToPortrait = false;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Get initial accelorometer values when the user first launches the game.
	    m_v3InitialAccelerometerPosition = Input.acceleration;
	}
	
	// Update is called once per frame
	void Update () 
    {
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

                // As this is a movile device we need to set values relative to the initial position.
                fHorizontalInput = Input.acceleration.x - m_v3InitialAccelerometerPosition.x;
                fVerticalInput = Input.acceleration.y - m_v3InitialAccelerometerPosition.y;

                break;

            default:
                break;
        }
	    
        // Create the direction vector and normalize it.
        Vector3 v3Direction = new Vector3( fHorizontalInput, 0, fVerticalInput );
        v3Direction.Normalize();

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
            default:
                break;
        }
    }
}
