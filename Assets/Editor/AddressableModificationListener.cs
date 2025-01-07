using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class AddressableModificationListener
{
    [InitializeOnLoadMethod]
    private static void RegisterGlobalModificationListener()
    {
        AddressableAssetSettings.OnModificationGlobal += OnGlobalModification;
    }

    private static void OnGlobalModification(AddressableAssetSettings settings, AddressableAssetSettings.ModificationEvent evt, object data)
    {
        Debug.Log($"{settings.name} {evt}");
    }
}