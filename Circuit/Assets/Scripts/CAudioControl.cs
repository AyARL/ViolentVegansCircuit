using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Circuit;

public class CAudioControl : MonoBehaviour {

    public static Dictionary< string, List< AudioClip > > m_dAudioClipContainer = new Dictionary< string,List< AudioClip > >();

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               AddClipToDictionary
    /////////////////////////////////////////////////////////////////////////////
    public static void AddClipToDictionary( string strAudioName, AudioClip aClip )
    {
        // Check if the audio dictionary contains the provided audio name/clip pairing
        //  and add them if it doesn't.
        if ( m_dAudioClipContainer.ContainsKey( strAudioName ) )
        {
            // Add the clip.
            m_dAudioClipContainer[ strAudioName ].Add( aClip );
        }
        else
        {
            // Create clip list.
            List< AudioClip > liClipList = new List< AudioClip >();

            // Add the clip to the clip list.
            liClipList.Add( aClip );

            // Add the list to the dictionary.
            m_dAudioClipContainer.Add( strAudioName, liClipList );
        }
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               CreateAndPlayAudio
    /////////////////////////////////////////////////////////////////////////////
    public static void CreateAndPlayAudio( string strAudioName, bool bLoop, bool bPlayOnAwake, float fVolume )
    {
        // This function will create a GameObject using the provided parameters and will add an
        //  AudioSource component to it which we will configure to suit our needs. The GameObject
        //  will be destroyed once we're done with it.

        string strFunctionName = "CAudioControl::CreateAndPlayAudio()";

        // Check if we have a collection available for the provided audio name.
        if ( false == m_dAudioClipContainer.ContainsKey( strAudioName ) )
        {
            Debug.LogError( string.Format( "{0} {1} " + CErrorStrings.ERROR_UNRECOGNIZED_NAME, strFunctionName, strAudioName ) );
        }

        // Get a list of audio clips available to us.
        List< AudioClip > liAudioClips = m_dAudioClipContainer[ strAudioName ];

        // Attempt to select a random clip from the list.
        AudioClip aClip = liAudioClips.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

        GameObject goAudioClipObject = new GameObject( strAudioName );

        // Set the position of the object.
        goAudioClipObject.transform.position = Vector3.zero;

        // Add the Audio Source component
        goAudioClipObject.AddComponent< AudioSource >();

        // Retrieve the Audio source component. We will use this reference to set up values, etc.
        AudioSource asSource = goAudioClipObject.GetComponent< AudioSource >();

        // Set up the audio source.
        asSource.playOnAwake = bPlayOnAwake;
        asSource.loop = bLoop;
        asSource.clip = aClip;
        asSource.volume = fVolume;

        // Play the clip and destroy the game object once we're done with it.
        asSource.Play();
        Destroy( goAudioClipObject, aClip.length );
    }
}
