using UnityEngine;
using System.Collections;
using Circuit;
using System;

public class CBallFace : MonoBehaviour {

    private Animator m_anAnimator;
    private GameObject m_goPlayer;
    private CBall.EBallState m_eCurrentFaceState;

	/////////////////////////////////////////////////////////////////////////////
    /// Function:               Start
    /////////////////////////////////////////////////////////////////////////////
	void Start () 
    {
        // We use this variable for error reporting.
        string strFunctionName = "CBallFace::Start()";

        // Set current face state to default.
        m_eCurrentFaceState = CBall.EBallState.STATE_NORMAL;

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
        //transform.position = new Vector3( v3BallPosition.x - fSphereRadius / 2, v3BallPosition.y + fSphereRadius / 2, v3BallPosition.z );
        transform.parent = m_goPlayer.transform;

        // Check the ball state and switch animations accordingly.
        CheckBallState();
	}

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               CheckBallState
    /////////////////////////////////////////////////////////////////////////////
    public void CheckBallState()
    {
        // The functions name, this will be used when reporting an error.
        string strFunctionName = "CBallFace::CheckBallState()";

        // Retrieve the CBall component from the player.
        CBall cBallComponent = m_goPlayer.GetComponent< CBall >();

        // Check if we found the component and report any issues.
        if ( false == cBallComponent )
        {
            Debug.LogError( string.Format("{0} {1} " + CErrorStrings.ERROR_MISSING_COMPONENT, strFunctionName, typeof( CBall ) ) );

            // No point in continuing, exit the function.
            return;
        }

        // Retrieve the current state parameter value
        CBall.EBallState eBallState = cBallComponent.m_eCurrentState;

        // Set the transition
        if ( eBallState != m_eCurrentFaceState )
        {
            m_anAnimator.SetInteger( CAnimatorConstants.ANIMATOR_PARAMETER_BALL_STATE, ( int )eBallState );
            m_eCurrentFaceState = eBallState;
        }

        // Check if the animation has finished playing and switch to idle if it has.
        if ( eBallState != CBall.EBallState.STATE_NORMAL )
        {
            if ( m_anAnimator.GetCurrentAnimatorStateInfo(0).length < m_anAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime )
            {
                m_anAnimator.SetInteger( CAnimatorConstants.ANIMATOR_PARAMETER_BALL_STATE, ( int )CBall.EBallState.STATE_NORMAL );
            }
        }
    }


}
