using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace AddRunicLensToProcChain
{
    internal static class Main
    {
        internal static void MakeRunicLensAddItselfToProcChainMask(ILContext il)
        {
            ILCursor c = new(il);
            // moving before "component2.RunicLensUpdateVariables"
            if (!c.TryGotoNext(MoveType.Before,
                x => x.MatchLdloc(1),
                x => x.MatchLdloc(147),
                x => x.MatchLdarg(1)
            ))
            {
                Log.Error("COULD NOT IL HOOK IL.RoR2.GlobalEventManager.ProcessHitEnemy");
                LogILStuff(il, c);
                return;
            }
            try
            {
                // gearbox added a proctype for runic lens AND they check for it before proccing
                // but they forgot to make runic lens add it's proctype to the procchainmask when it procs?????????????
                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Action<DamageInfo>>((damageInfo) =>
                {
                    damageInfo.procChainMask.AddProc(ProcType.MeteorAttackOnHighDamage);
                });
            }
            catch (Exception e)
            {
                Log.Error($"COULD NOT EMIT INTO IL.RoR2.GlobalEventManager.ProcessHitEnemy DUE TO:\n{e}");
                LogILStuff(il, c);
            }
        }

        internal static void MakeRunicLensUseTheActualProcChainMask(ILContext il)
        {
            ILCursor c = new(il);
            // moving after "procChainMask = default(ProcChainMask)"
            if (!c.TryGotoNext(MoveType.After,
                x => x.MatchDup(),
                x => x.MatchLdflda<BlastAttack>("procChainMask"),
                x => x.MatchInitobj<ProcChainMask>()
            ))
            {
                Log.Error("COULD NOT IL HOOK IL.RoR2.MeteorAttackOnHighDamageBodyBehavior.DetonateRunicLensMeteor");
                LogILStuff(il, c);
                return;
            }
            try
            {
                // why does runic lens reset the procchainmask even though the real one is easily accessible???????
                c.Emit(OpCodes.Dup);
                c.Emit(OpCodes.Ldloc_0);
                c.EmitDelegate<Action<BlastAttack, DamageInfo>>((blastAttack, damageInfo) =>
                {
                    blastAttack.procChainMask = damageInfo.procChainMask;
                });
            }
            catch (Exception e)
            {
                Log.Error($"COULD NOT EMIT INTO IL.RoR2.MeteorAttackOnHighDamageBodyBehavior.DetonateRunicLensMeteor DUE TO:\n{e}");
                LogILStuff(il, c);
            }
        }



        internal static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);
            Log.Warning($"ProcChainMask is {damageInfo.procChainMask}");
        }

        private static void LogILStuff(ILContext il, ILCursor c)
        {
            Log.Warning($"cursor is {c}");
            Log.Warning($"il is {il}");
        }
    }
}