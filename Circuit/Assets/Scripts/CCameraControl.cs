using UnityEngine;
using System.Collections;
using Circuit;

public class CCameraControl : MonoBehaviour {

    public enum EEffectType
    {
        EFFECT_NONE,
        EFFECT_SHAKE_CAMERA,
    };

    // For error reporting.
    private string m_strScriptName = "CCameraControl::";

    // Current effect type.
    private EEffectType m_eCurrentType = EEffectType.EFFECT_NONE;
    public EEffectType CurrentEffectType { get { return m_eCurrentType; } set { m_eCurrentType = value; } }

    // The camera's initial position.
    private Vector3 m_v3InitialPosition;

    // Boolean that indicates if a coroutine is running or not.
    private bool m_bEffectIsRunning = false;

	/////////////////////////////////////////////////////////////////////////////
    /// Function:               Start
    /////////////////////////////////////////////////////////////////////////////
	void Start () 
    {
	    // Set the initial position, we will use this to put the camera back into place.
        m_v3InitialPosition = transform.position;
	}
	
	/////////////////////////////////////////////////////////////////////////////
    /// Function:               Update
    /////////////////////////////////////////////////////////////////////////////
	void Update () 
    {
        // An effect is already running, return.
        if ( m_bEffectIsRunning )
            return;

        // Run the coroutine.
        StartCoroutine( RunEffect() );	
	}

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               RunEffect
    /////////////////////////////////////////////////////////////////////////////
    public IEnumerator RunEffect()
    {
        // Error handling
        string strFunctionName = m_strScriptName + "RunEffect()";

        m_bEffectIsRunning = true;

        // Run effect according to current effect type.
        switch ( m_eCurrentType )
        {
            case EEffectType.EFFECT_NONE:

                // Do nothing.

                break;
            case EEffectType.EFFECT_SHAKE_CAMERA:

                // Create a target vector for the MoveObject coroutine.
                Vector3 v3TargetVec = new Vector3( Random.Range( this.transform.position.x - CConstants.CAMERA_EFFECT_INTENSITY, this.transform.position.x + CConstants.CAMERA_EFFECT_INTENSITY ),
                                                   Random.Range( this.transform.position.y - CConstants.CAMERA_EFFECT_INTENSITY, this.transform.position.y + CConstants.CAMERA_EFFECT_INTENSITY ), 
                                                   Random.Range( this.transform.position.z - CConstants.CAMERA_EFFECT_INTENSITY, this.transform.position.z + CConstants.CAMERA_EFFECT_INTENSITY ) );

                // Tilt the camera slightly towards the target Vector.
                yield return StartCoroutine( MoveObject( this.transform, v3TargetVec, 0.1f ) );
                yield return new WaitForSeconds( 0.1f );

                // Move the camera back.
                yield return StartCoroutine( MoveObject( this.transform, m_v3InitialPosition, 0.1f ) );

                // Return to the normal state.
                m_eCurrentType = EEffectType.EFFECT_NONE;

                break;
            default:

                // Invalid value has been provided.
                Debug.LogError( string.Format( "{0} {1} " + CErrorStrings.ERROR_UNRECOGNIZED_VALUE, strFunctionName, m_eCurrentType ) );

                break;
        }

        // Return to the normal
        m_eCurrentType = EEffectType.EFFECT_NONE;

        m_bEffectIsRunning = false;
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               MoveObject
    /////////////////////////////////////////////////////////////////////////////
    private IEnumerator MoveObject( Transform trObjectToMove, Vector3 v3TargetPosition, float fTime )
    {
        // This function will move the provided object towards a target destination in the provided amount of time.

        // Declare crap.
        float fTimePassed = 0.0f;
        Vector3 v3OriginalPos = trObjectToMove.position;

        // Keep moving the object until the time provided has passed.
        while ( fTimePassed < 1.0f )
        {
            trObjectToMove.position = Vector3.Lerp( v3OriginalPos, v3TargetPosition, fTimePassed );
            fTimePassed += Time.deltaTime / fTime;
            yield return null;
        }
    }
}
