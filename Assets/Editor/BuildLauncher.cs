using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

#if UNITY_EDITOR

internal class BuildLauncher
{
    public static string build_script = "Assets/AddressableAssetsData/DataBuilders/BuildScriptPackedMode.asset";
    public static string settings_asset = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";
    public static string profile_name = "Default";

    private static AddressableAssetSettings _Settings;

    static void _GetSettingsObject(string settingsAsset)
    {
        _Settings = AssetDatabase.LoadAssetAtPath<ScriptableObject>(settings_asset) as AddressableAssetSettings;
        if (_Settings == null)
        {
            Debug.LogError("Settings asset not found at " + settings_asset);
        }
    }

    static void _SetProfile(string profile)
    {
        string profileId = _Settings.profileSettings.GetProfileId(profile);
        if (string.IsNullOrEmpty(profileId))
        {
            Debug.LogError("Profile not found: " + profile);
            return;
        }else
        {
            _Settings.activeProfileId = profileId;            
        }
    }

    static void _SetBuilder(IDataBuilder builder)
    {
        int index = _Settings.DataBuilders.IndexOf((ScriptableObject)builder);
        if (index > 0)
        {
            _Settings.ActivePlayerDataBuilderIndex = index;
        }
        else
        {
            Debug.LogError("Builder not found: " + builder.Name);
        }
    }

    static bool _BuildAddressableContent()
    {
        AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
        bool success = string.IsNullOrEmpty(result.Error);

        if (!success)
        {
            Debug.LogError("Addressables build failed: " + result.Error);
        }

        return success;
    }

    [MenuItem("Build/Build Addressables only")]
    static bool _BuildAddressables()
    {
        _GetSettingsObject(settings_asset);
        _SetProfile(profile_name);
        IDataBuilder builderScript = AssetDatabase.LoadAssetAtPath<ScriptableObject>(build_script) as IDataBuilder;
        
        if (builderScript == null)
        {
            Debug.LogError("Build script not found at " + build_script);
            return false;
        }

        _SetBuilder(builderScript);
        return _BuildAddressableContent();
    }
    
    
    [MenuItem("Build/Build Addressables and Player")]
    static void _BuildAddresablesAndPlayer()
    {
        if (_BuildAddressables())
        {
            BuildPlayerOptions options = new BuildPlayerOptions();

            BuildPlayerOptions playerSettings = BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(options);

            BuildPipeline.BuildPlayer(playerSettings);
        }
    }
}

#endif
    
 
