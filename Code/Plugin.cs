using BepInEx;
using RoR2;

namespace AddRunicLensToProcChain
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "LordVGames";
        public const string PluginName = "AddRunicLensToProcChain";
        public const string PluginVersion = "1.0.0";
        public void Awake()
        {
            Log.Init(Logger);
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += Main.AddRunicLensToProcChainMask;
            IL.RoR2.MeteorAttackOnHighDamageBodyBehavior.DetonateRunicLensMeteor += Main.UseTheActualProcChainMaskGodDamnit;
#if DEBUG
            On.RoR2.HealthComponent.TakeDamage += Main.HealthComponent_TakeDamage;
#endif
        }
    }
}