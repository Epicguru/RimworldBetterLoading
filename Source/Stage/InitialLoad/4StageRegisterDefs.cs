﻿using System.Linq;
using System.Xml;
using HarmonyLib;
using Verse;

namespace BetterLoading.Stage.InitialLoad
{
    public class StageRegisterDefs : LoadingStage
    {
        private static int _numDefsToRegister = 1;
        private static int _currentDefNum;
        
        public StageRegisterDefs(Harmony instance) : base(instance)
        {
        }

        public override string GetStageName()
        {
            return "Registering Defs";
        }

        public override bool IsCompleted()
        {
            return _currentDefNum == _numDefsToRegister;
        }

        public override void BecomeInactive()
        {
            _numDefsToRegister = 1; //Cannot be zero because we can't return 0 from GetMaxProgress
            _currentDefNum = 0;
        }

        public override string? GetCurrentStepName()
        {
            return null;
        }

        public override int GetCurrentProgress()
        {
            return _currentDefNum;
        }

        public override int GetMaximumProgress()
        {
            return _numDefsToRegister;
        }

        public override void DoPatching(Harmony instance)
        {
            instance.Patch(AccessTools.Method(typeof(LoadedModManager), nameof(LoadedModManager.ParseAndProcessXML)), new HarmonyMethod(typeof(StageRegisterDefs), nameof(PreParseProcXml)));
            instance.Patch(AccessTools.Method(typeof(XmlInheritance), nameof(XmlInheritance.TryRegister)), new HarmonyMethod(typeof(StageRegisterDefs), nameof(PreRegisterDef)));
        }

        public static void PreParseProcXml(XmlDocument xmlDoc)
        {
            _numDefsToRegister = xmlDoc.DocumentElement?.ChildNodes.GetEnumerator().ToIEnumerable<XmlNode>().Count(e => e.NodeType == XmlNodeType.Element) ?? 0;
            _currentDefNum = 0;
        }

        public static void PreRegisterDef()
        {
            if (_currentDefNum >= _numDefsToRegister) return;
            
            _currentDefNum++;
        }
    }
}