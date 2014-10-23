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
    private GameObject m_goEffect = null;

    private ParticleSystem[] m_psParticleSystems = null;

    [ SerializeField ]
    private float m_fHealth = 100.0f;

    [ SerializeField ]
    EObstacleState m_eObstacleState = EObstacleState.STATE_NONE;
    public EObstacleState ObstacleState { get { return m_eObstacleState; } private set { m_eObstacleState = value; } }

    private bool m_bCanBeDamaged = true;

	/////////////////////////////////////////////////////////////////////////////
    /// Function:               Start
    /////////////////////////////////////////////////////////////////////////////
	void Start () 
    {
        string strFunctionName = "CObstacle::Start()";

        // Load all particle effects within the Effect game object
        if ( null == m_goEffect )
        {
            Debug.LogError( string.Format( "{0} {1} " + CErrorStrings.ERROR_NULL_OBJECT, strFunctionName, typeof( GameObject ).ToString() ) );
            return;
        }

        m_psParticleSystems = m_goEffect.GetComponentsInChildren< ParticleSystem >();
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

        // Get a handle on the animator.
        Animator anAnimatorHandle = GetComponent< Animator >();
        if ( null == anAnimatorHandle )
        {
            // Holy moly, no animator!!
            Debug.LogError( string.Format( "{0} {1} " + CErrorStrings.ERROR_MISSING_COMPONENT, strFunctionName, typeof( Animator ).ToString() ) );
            return;
        }

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

                // Fan is broken, trigger the broken fan animation.
                anAnimatorHandle.SetTrigger( CAnimatorConstants.ANIMATOR_TRIGGER_FAN_BROKEN );

                break;
            case EObstacleState.STATE_DEAD:

                // Fan is dead, trigger the dead animation.
                anAnimatorHandle.SetTrigger( CAnimatorConstants.ANIMATOR_TRIGGER_FAN_DEAD );

                break;
            default:

                Debug.LogError( string.Format( "{0} {1} " + CErrorStrings.ERROR_UNRECOGNIZED_VALUE, strFunctionName, m_eObstacleState ) );
                return;
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               OnCollisionEnter
    /////////////////////////////////////////////////////////////////////////////
    void OnCollisionEnter( Collision cCollision )
    {
        // Error handling.
        string strFunctionName = "CObstacle::OnCollisionEnter()";

        // Check if the player collided with the obstacle.
        if ( CTags.TAG_PLAYER == cCollision.gameObject.tag )
        {
            // Get a handle on the main camera.
            Camera cMainCamera = Camera.main;

            // Retrieve the camera controls.
            CCameraControl cCamControls = cMainCamera.GetComponent< CCameraControl >();
            if ( null == cCamControls )
            {
                // We failed to get a handle on the controls, report error and return.
                Debug.LogError( string.Format( "{0} {1} " + CErrorStrings.ERROR_MISSING_COMPONENT, strFunctionName, typeof( CCameraControl ).ToString() ) );
                return;
            }

            // Apply the camera effect.
            cCamControls.CurrentEffectType = CCameraControl.EEffectType.EFFECT_SHAKE_CAMERA;

            // Throw some particles around.
            m_goEffect.transform.position = cCollision.contacts[ 0 ].point;
            foreach ( ParticleSystem psParticle in m_psParticleSystems )
            {
                psParticle.Play();
                VibrationManager.Vibrate(20);
            }

            // Play collision sound
            CAudioControl.CreateAndPlayAudio( CAudio.AUDIO_EFFECT_BALL_WALLHIT, false, true, false, 0.5f );

            // Damage the obstacle.
            ApplyDamage();
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               OnCollisionExit
    /////////////////////////////////////////////////////////////////////////////
    private void OnCollisionExit( Collision cCollision )
    {
        // Check if the player is out of range and stop the particle effects.
        if ( cCollision.transform.tag == CTags.TAG_PLAYER )
        {
            foreach (ParticleSystem psParticle in m_psParticleSystems)
            {
                psParticle.Stop();
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               ApplyDamage
    /////////////////////////////////////////////////////////////////////////////
    public void ApplyDamage()
    {
        // If we can't damage the obstacle, return.
        if ( m_bCanBeDamaged != true )
            return;

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

        // Obstacle has been hit and the damage has been applied.
        m_bCanBeDamaged = false;

        // Wait a second and set the can be damaged flag to true;
        StartCoroutine( Wait( 1 ) );
    }

    private IEnumerator Wait( float fTime )
    {
        yield return new WaitForSeconds( fTime );
        m_bCanBeDamaged = true;
        yield return null;
    }

}
