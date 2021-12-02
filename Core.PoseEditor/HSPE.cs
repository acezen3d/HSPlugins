﻿using System.Reflection;
using ToolBox;
using UnityEngine;
using UnityEngine.SceneManagement;
#if IPA
using Harmony;
using IllusionPlugin;
#elif BEPINEX
using HarmonyLib;
using BepInEx;
using BepInEx.Configuration;
#endif

namespace HSPE
{
#if BEPINEX
    [BepInPlugin(_guid, _name, _versionNum)]
    [BepInDependency("com.bepis.bepinex.extendedsave")]
#if KOIKATSU
    [BepInProcess("CharaStudio")]
#elif AISHOUJO || HONEYSELECT2
    [BepInProcess("StudioNEOV2")]
#endif
#endif
    internal class HSPE : GenericPlugin
#if HONEYSELECT || PLAYHOME
    , IEnhancedPlugin
#endif
    {
#if HONEYSELECT
        internal const string _name = "HSPE";
        internal const string _guid = "com.joan6694.illusionplugins.poseeditor";
#elif PLAYHOME
        internal const string _name = "PHPE";
        internal const string _guid = "com.joan6694.illusionplugins.poseeditor";
#elif KOIKATSU || SUNSHINE //This must be the same for KK/KKS cross compatibility
        internal const string _name = "KKPE";
        internal const string _guid = "com.joan6694.kkplugins.kkpe";
        internal const int saveVersion = 0;
#elif AISHOUJO
        internal const string _name = "AIPE";
        internal const string _guid = "com.joan6694.illusionplugins.poseeditor";
        internal const int saveVersion = 0;
#elif HONEYSELECT2
        internal const string _name = "HS2PE";
        internal const string _guid = "com.joan6694.illusionplugins.poseeditor";
        internal const int saveVersion = 0;
#endif
        internal const string _versionNum = "2.13.0";

#if IPA
        public override string Name { get { return _name; } }
        public override string Version { get { return _versionNum; } }
#if HONEYSELECT
        public override string[] Filter { get { return new[] {"StudioNEO_32", "StudioNEO_64"}; } }
#elif PLAYHOME
        public override string[] Filter { get { return new[] { "PlayHomeStudio32bit", "PlayHomeStudio64bit" }; } }
#endif
#endif

        internal static ConfigEntry<float> ConfigMainWindowSize { get; private set; }
        internal static ConfigEntry<KeyboardShortcut> ConfigMainWindowShortcut { get; private set; }
        internal static ConfigEntry<bool> ConfigCrotchCorrectionByDefault { get; private set; }
        internal static ConfigEntry<bool> ConfigAnklesCorrectionByDefault { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            ConfigMainWindowSize = Config.Bind("Config", "Main Window Size", 1f);
            ConfigMainWindowShortcut = Config.Bind("Config", "Main Window Shortcut", new KeyboardShortcut(KeyCode.RightControl));
            ConfigCrotchCorrectionByDefault = Config.Bind("Config", "Crotch Correction By Default", false);
            ConfigAnklesCorrectionByDefault = Config.Bind("Config", "AnklesCorrection By Default", false);

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }

#if AISHOUJO || HONEYSELECT2
        protected override void LevelLoaded(Scene scene, LoadSceneMode mode)
        {
            base.LevelLoaded(scene, mode);
            if (mode == LoadSceneMode.Single && scene.name.Equals("Studio"))
                this.gameObject.AddComponent<MainWindow>();
        }
#else
        protected override void LevelLoaded(int level)
        {
            base.LevelLoaded(level);
#if HONEYSELECT
            if (level == 3)
#elif SUNSHINE
            if (level == 2)
#elif KOIKATSU
            if (level == 1)
#elif PLAYHOME
            if (level == 1)
#endif
                gameObject.AddComponent<MainWindow>();
        }
#endif
    }
}