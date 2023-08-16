/*
Description: Enum used to keep track of all the different
settings in the game.
Creator: Alvaro Chavez Mixco
Creation Date: Saturday, January 28, 2017
*/
public enum ESettingsOptions
{
    //Gameplay settings
    ShowAimingAids,
    ShowPlatform,
    InputSensitivity,//NonVR only
    InvertYAxis,//NonVR only

    //Sound settings
    MainVolumePercent,
    SoundEffectsVolumePercent,
    MenuSoundsVolumePercent,
    MusicVolumePercent,

    //Graphics settings
    QualityLevel,
    BrightnessPercent,
    GammaPercent,
    ScreenResolution,//Non VR Only
    DrawDistance
};

/*
Description: Enum used to easily identify the different quality levels supported
by the game
Creator: Alvaro Chavez Mixco
Creation Date: Saturday, January 28, 2017
*/
public enum EQualityLevels
{
    Fastest = 0,
    Fast,
    Simple,
    Good,
    Beautiful,
    Fantastic
};

/*
Description: Enum used to easily identify the screen resolutions supported
by the game
Creator: Alvaro Chavez Mixco
Creation Date: Saturday, January 28, 2017
Extra Notes: This only works for nonVR.
*/
public enum EScreenResolutions
{
    resoltuion_1280x720 = 0,
    resolution_1280x1024,
    resolution_1366x768,
    resolution_1920x1080,
    resolution_1920x1200
}