﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using System;
using Circuit;

public class CAudioControl : MonoBehaviour {

    public static Dictionary< string, List< AudioClip > > m_dAudioClipContainer = new Dictionary< string,List< AudioClip > >();

    private List< string > m_liValidExtensions = new List< string > 
    { 
        CAudio.FILE_TYPE_MP3
    };

    private List< string > m_liRegexPatterns = new List< string > 
    { 
        CAudio.AUDIO_EFFECT_BALL_HIT,
        CAudio.AUDIO_EFFECT_BALL_ROLLING,
        CAudio.AUDIO_EFFECT_BALL_WALLHIT,
        CAudio.AUDIO_EFFECT_CHIP_POWER,
        CAudio.AUDIO_EFFECT_ELECTRIC_JOLT,
        CAudio.AUDIO_MUSIC,
        CAudio.AUDIO_EFFECT_MENU_SELECT,
        CAudio.AUDIO_EFFECT_GAMEOVER,
        CAudio.AUDIO_EFFECT_ELECTRIC_LOOP
    };

    private bool m_bAudioFilesLoaded = false;
    public bool AudioFilesLoaded { get { return m_bAudioFilesLoaded; } private set { m_bAudioFilesLoaded = value; }  }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               Awake
    /////////////////////////////////////////////////////////////////////////////
    void Awake()
    {
        // Reload the audio files.
        ReloadSounds();

        // We finished loading the Audio files, set files loaded flag to true.
        AudioFilesLoaded = true;
    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               AddClipToDictionary
    /////////////////////////////////////////////////////////////////////////////
    IEnumerator AddClipToDictionary( string strFilePath )
    {
        // Error reporting.
        string strFunctionName = "CAudioControl::AddClipToDictionary()";

        // Will be used as a dictionary key if an audio file can be matched to a pattern.
        string strDictionaryKey = "";

        // Attempt to get the AudioClip from the provided file path.
        WWW www = new WWW( "file://" + strFilePath );
        AudioClip aClip = www.GetAudioClip( false );

        // Yield if the clip isn't ready yet.
        while( false == aClip.isReadyToPlay )
            yield return www;

        // Set the clip's name.
        aClip.name = Path.GetFileName( strFilePath );

        // Clip is ready, we need to identify it.
        foreach ( string strPattern in m_liRegexPatterns )
        {
            // Attempt to find the pattern within the clip name.
            Regex regex = new Regex( @strPattern );
            Match match = regex.Match( aClip.name );

            if ( true == match.Success )
            {
                // We found a match, set the pattern which should be unique to each
                //  sound type as the dictionary key.
                strDictionaryKey = strPattern;
                break;
            }
        }

        // Check if we have set a dictionary key,
        if ( true == string.IsNullOrEmpty( strDictionaryKey ) )
        {
            // Report that we couldn't find a pattern match.
            Debug.LogError( string.Format( "{0} {1} " + CErrorStrings.ERROR_UNMATCHED_AUDIO_CLIP, strFunctionName, strFilePath ) );

        }

        // Check if the audio dictionary contains the provided audio name/clip pairing
        //  and add them if it doesn't.
        if ( m_dAudioClipContainer.ContainsKey( strDictionaryKey ) )
        {
            // Add the clip.
            m_dAudioClipContainer[ strDictionaryKey ].Add( aClip );
        }
        else
        {
            // Create clip list.
            List< AudioClip > liClipList = new List< AudioClip >();

            // Add the clip to the clip list.
            liClipList.Add( aClip );

            // Add the list to the dictionary.
            m_dAudioClipContainer.Add( strDictionaryKey, liClipList );
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

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               ReloadSounds
    /////////////////////////////////////////////////////////////////////////////
    private void ReloadSounds()
    {
        // This function will clear the existing dictionary and reload all sounds.
        m_dAudioClipContainer.Clear();

        // Get a handle on the Audio directory.
        DirectoryInfo directoryInfo = new DirectoryInfo( CAudio.PATH_AUDIO );

        // Holy crap I'm using a lambda. P.S. We're verifying directory contents for valid audio files.
        FileInfo[] rgFileInfo = directoryInfo.GetFiles().Where( x => IsValidAudioType( Path.GetExtension( x.Name ) ) ).ToArray();
        
        // Loop through the files we found and attempt to load them into the dictionary.
        foreach ( FileInfo fileInfo in rgFileInfo )
        {
            // Attempt to load the clip to dictionary.
            StartCoroutine( AddClipToDictionary( fileInfo.FullName ) );
        }

    }

    /////////////////////////////////////////////////////////////////////////////
    /// Function:               IsValidAudioType
    /////////////////////////////////////////////////////////////////////////////
    private bool IsValidAudioType( string strFile )
    {
        // Will verify if the file provided has the correct extension.
        bool bIsValidAudioType = false;

        if ( true == m_liValidExtensions.Contains( strFile ) )
            bIsValidAudioType = true;

        return bIsValidAudioType;
    }
}
