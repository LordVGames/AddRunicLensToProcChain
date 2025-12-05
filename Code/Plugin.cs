using System;
using BepInEx;
using MonoDetour;
using MonoDetour.HookGen;
using RoR2;

namespace AddRunicLensToProcChain
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "LordVGames";
        public const string PluginName = "AddRunicLensToProcChain";
        public const string PluginVersion = "1.0.4";

        public void Awake()
        {
            Log.Init(Logger);
            MonoDetourManager.InvokeHookInitializers(typeof(Plugin).Assembly);
        }

#if DEBUG
        [MonoDetourTargets(typeof(HealthComponent))]
#endif
        [MonoDetourTargets(typeof(GlobalEventManager))]
        [MonoDetourTargets(typeof(MeteorAttackOnHighDamageBodyBehavior))]
        private static class Hooks
        {
            [MonoDetourHookInitialize]
            private static void Setup()
            {
#if DEBUG
                Mdh.RoR2.HealthComponent.TakeDamage.Postfix(Main.DebugLogProcChainMask);
#endif
                Mdh.RoR2.GlobalEventManager.ProcessHitEnemy.ILHook(Main.AddRunicLensToProcChainMask);
                Mdh.RoR2.MeteorAttackOnHighDamageBodyBehavior.DetonateRunicLensMeteor.ILHook(Main.UseTheActualProcChainMaskGodDamnit);
            }
        }
    }
}