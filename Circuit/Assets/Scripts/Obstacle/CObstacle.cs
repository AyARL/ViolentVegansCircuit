using UnityEngine;
using System.Collections;
using Circuit;

[ RequireComponent( typeof( Animator ) ) ]
public class CObstacle : MonoBehaviour {

    public enum EObstacleState
    {
        STATE_NONE,
        STATE_NORMAL,
        STATE_INJURED,
        STATE_DEAD,
    };

    [ SerializeField ]
    private float m_fHealth = 100.0f;

    [ SerializeField ]
    EObstacleState m_eObstacleState = EObstacleState.STATE_NONE;
    public EObstacleState ObstacleState { get { return m_eObstacleState; } private set { m_eObstacleState = value; } }

	/////////////////////////////////////////////////////////////////////////////
    /// Function:               Start
    /////////////////////////////////////////////////////////////////////////////
	void Start () 
    {
	}
	
	/////////////////////////////////////////////////////////////////////////////
    /// Function:               Update
    /////////////////////////////////////////////////////////////////////////////
	void Update () 
    {
	    // Run the states logic.
        CheckStates();
	}

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               CheckStates
    /////////////////////////////////////////////////////////////////////////////
    private void CheckStates()
    {
        // Error reporting.
        string strFunctionName = "CObstacle::CheckStates()";

        // Check if we need to switch states.
        if ( m_fHealth > 0.0f && m_fHealth <= 50.0f )
        {
            // Switch to injured.
            m_eObstacleState = EObstacleState.STATE_INJURED;
        }
                
        else if ( m_fHealth <= 0.0f )
        {
            m_eObstacleState = EObstacleState.STATE_DEAD;
        }

        // Apply correct behaviour according to current state.
        switch ( m_eObstacleState )
        {
            case EObstacleState.STATE_NORMAL:

                // Run normal animation.

                break;
            case EObstacleState.STATE_INJURED:

                // Check if we need to switch state.

                break;
            case EObstacleState.STATE_DEAD:

                // Obstacle is dead, do nothing.

                break;
            default:

                Debug.LogError( string.Format( "{0} {1} " + CErrorStrings.ERROR_UNRECOGNIZED_VALUE, strFunctionName, m_eObstacleState ) );
                return;
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               ReceiveDamage
    /////////////////////////////////////////////////////////////////////////////
    public void ApplyDamage()
    {
        // Get a random variable.
        int iRandomVariable = Random.Range( (int)0, (int)1 );

        if ( iRandomVariable == 0 )
        {
            // Take down half of the obstacle's health.
            m_fHealth -= ( m_fHealth / 2 ) - 1;
        }
        else if ( iRandomVariable == 1 )
        {
            // Take down its whole health
            m_fHealth -= m_fHealth;
        }
    }

}
