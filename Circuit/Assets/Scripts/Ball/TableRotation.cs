using UnityEngine;
using System.Collections;
using Circuit;

public class TableRotation : MonoBehaviour {

    private Vector3 m_v3InitialAccelerometerPosition;
    private Vector3 m_v3TableCenter;

    private Quaternion m_qInitialTableRotation;

    private Bounds m_sCombinedBounds;

    float m_fSpeed = CConstants.DEFAULT_SPEED;

	// Use this for initialization
	void Start () 
    {
        // Get the initial position of the Accelorometer.
	    m_v3InitialAccelerometerPosition = Input.acceleration;

        // We need to get the combined bounds of this obect ( including the children bounds )
        //  and find the board center.
        m_sCombinedBounds = collider.bounds;

        // Loop through all the inner colliders and retrieve the bounds.
        foreach ( Collider cCollider in GetComponentsInChildren< Collider >() )
        {
            if ( cCollider != collider && cCollider.tag == CTags.TAG_TILE )
                m_sCombinedBounds.Encapsulate( cCollider.bounds );
        }

        // Keep track of the initial rotation.
        m_qInitialTableRotation = transform.rotation;

        // Assign the center.
        m_v3TableCenter = m_sCombinedBounds.center;
	}
	
	// Update is called once per frame
	void Update () 
    {
        string strFunctionName = "TableRotation::Update()";

        float fHorizontalInput = 0;
        float fVerticalInput = 0;

        bool bBoardIsStuck = false;

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
        Vector3 v3Direction = new Vector3( -fVerticalInput, 0, fHorizontalInput );
        v3Direction.Normalize();

        // Get the minimum and maximum rotation angles allowed for the x axis.
        float fMinRotation = m_qInitialTableRotation.eulerAngles.x - CConstants.MAX_ROTATION_ANGLE;
        float fMaxRotation = m_qInitialTableRotation.eulerAngles.x + CConstants.MAX_ROTATION_ANGLE;
        
        // Attempt to predict the change on the X axis.
        float fPredictedNewX = transform.rotation.eulerAngles.x + v3Direction.x;  
        if ( fPredictedNewX > 90 )
            fPredictedNewX -= 360;
        
        // Do not allow the board to rotate beyond the limits we declared above.
        if ( fPredictedNewX > fMaxRotation || fPredictedNewX < fMinRotation )
        {
            bBoardIsStuck = true;
        }

        // Do the same for Z.
        fMinRotation = m_qInitialTableRotation.eulerAngles.z - CConstants.MAX_ROTATION_ANGLE;
        fMaxRotation = m_qInitialTableRotation.eulerAngles.z + CConstants.MAX_ROTATION_ANGLE;

        float fPredictedNewZ = transform.rotation.eulerAngles.z + v3Direction.z;
        if ( fPredictedNewZ > 90 )
            fPredictedNewZ -= 360;

        if ( fPredictedNewZ > fMaxRotation || fPredictedNewZ < fMinRotation )
        {
            bBoardIsStuck = true;
        }

        // Rotate the whole table around the table center pivot point.
        if ( false == bBoardIsStuck )
            transform.RotateAround( m_v3TableCenter, v3Direction, -m_fSpeed * Time.deltaTime );
        
        else
        {
            Quaternion qTargetRotation = Quaternion.LookRotation( m_v3TableCenter - transform.position );
            qTargetRotation.y = 0;
            float fBlendVar = Mathf.Min ( m_fSpeed * Time.deltaTime, 1 );
            transform.rotation = Quaternion.Slerp ( transform.rotation, qTargetRotation, fBlendVar / 40 );
        }
    }
}
