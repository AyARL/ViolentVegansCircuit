using UnityEngine;
using System.Collections;
using Circuit;

public class CBallFace : MonoBehaviour {

    private Animator m_anAnimator;
    private GameObject m_goPlayer;

	/////////////////////////////////////////////////////////////////////////////
    /// Function:               Start
    /////////////////////////////////////////////////////////////////////////////
	void Start () 
    {
        // We use this variable for error reporting.
        string strFunctionName = "CBallFace::Start()";

        // Get a handle on the ball game object.
	    m_goPlayer = GameObject.FindGameObjectWithTag( CTags.TAG_PLAYER );

        // Report if we couldn't find the player
        if ( false == m_goPlayer )
        {
            Debug.LogError( string.Format("{0} {1} " + CErrorStrings.ERROR_NULL_OBJECT, strFunctionName, CTags.TAG_PLAYER ) );
        }

        // Try to get a handle on the animator.
        m_anAnimator = GetComponent< Animator >();

        // Report if the animator component is missing.
        if ( false == m_anAnimator )
        {
            Debug.LogError( string.Format("{0} {1} " + CErrorStrings.ERROR_MISSING_COMPONENT, strFunctionName, typeof( Animator ) ) );
        }
	}
	
	/////////////////////////////////////////////////////////////////////////////
    /// Function:               Update
    /////////////////////////////////////////////////////////////////////////////
	void Update () 
    {
        // Get the player position and the sphere collider from the player game object.
        //  We will use these to position the face slightly above the ball itself.
	    Vector3 v3BallPosition = m_goPlayer.transform.position;
        SphereCollider cSphereCollider = m_goPlayer.GetComponent< SphereCollider >();

        // Get the sphere's radius
        float fSphereRadius = Mathf.Max( cSphereCollider.transform.lossyScale.x, cSphereCollider.transform.lossyScale.y, cSphereCollider.transform.lossyScale.z ) * cSphereCollider.radius;

        // Set the face position.
        transform.position = new Vector3( v3BallPosition.x - fSphereRadius / 2, v3BallPosition.y + fSphereRadius / 2, v3BallPosition.z );

        //if ( Random.Range( 0, 100 ) == 0 )
        //{
        //    SetTransition( 1 );
        //}
	}

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               SetTransition
    /////////////////////////////////////////////////////////////////////////////
    public void SetTransition( int iState )
    {
        // Retrieve the current state parameter value
        int iCurrentValue = m_anAnimator.GetInteger( CConstants.ANIMATOR_PARAMETER_BALL_STATE );

        // Set the transition
        if ( iCurrentValue != iState )
        {
            m_anAnimator.SetInteger( CConstants.ANIMATOR_PARAMETER_BALL_STATE, iState );
        }
    }


}
