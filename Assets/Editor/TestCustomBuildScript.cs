using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Pipeline;
using UnityEngine;

public class TestCustomBuildScript : BuildScriptPackedMode
{
    public override bool CanBuildData<T>()
    {
        return typeof(T).IsAssignableFrom(typeof(AddressablesPlayerBuildResult));
    }
    

    protected override TResult BuildDataImplementation<TResult>(AddressablesDataBuilderInput builderInput)
    {
        var groups = builderInput.AddressableSettings.groups;

        var filteredGroups = new List<AddressableAssetGroup>();

        foreach (var group in groups)
        {
            
        }
        
        return base.BuildDataImplementation<TResult>(builderInput);
    }
}
