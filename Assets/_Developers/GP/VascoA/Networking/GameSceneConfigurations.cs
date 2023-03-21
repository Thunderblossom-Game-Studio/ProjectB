using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneConfigurations : MonoBehaviour
{
    /// <summary>
    ///
    /// </summary>
    [Tooltip("Physics mode to use in your game. This is required with scene stacking while using physics.")]
    [SerializeField]
    private LocalPhysicsMode _physicsMode = LocalPhysicsMode.Physics3D;
    /// <summary>
    /// Physics mode to use in your game. This is required with scene stacking while using physics.
    /// See SimulatePhysics script within demo game.
    /// https://docs.unity3d.com/ScriptReference/SceneManagement.LocalPhysicsMode.html
    /// </summary>
    public virtual LocalPhysicsMode PhysicsMode { get { return _physicsMode; } }
    /// <summary>
    /// Additive scenes to load in addition to mainScene.
    /// </summary>
    [Tooltip("Additive scenes to load in addition to mainScene.")]
    [SerializeField]
    private Object[] _scenes = new Object[0];
    /// <summary>
    /// String variant for loading in builds.
    /// </summary>
    [SerializeField, HideInInspector]
    private string[] _sceneNames = new string[0];


    private void OnValidate()
    {
        List<string> additives = new List<string>();
        if (_scenes != null)
        {
            foreach (Object item in _scenes)
            {
                if (item != null)
                    additives.Add(item.name);
            }
        }
        _sceneNames = additives.ToArray();
    }

    /// <summary>
    /// Gets scenes to load when a player starts a room.
    /// </summary>
    public virtual string[] GetGameScenes()
    {
        return _sceneNames;
    }
}
