using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoDetour;
using RoR2;
using MonoDetour.Cil;

namespace AddRunicLensToProcChain
{
    internal static class Main
    {
        internal static void AddRunicLensToProcChainMask(ILManipulationInfo info)
        {
            ILWeaver w = new(info);
            // moving before "characterBody.RunicLensUpdateVariables"
            w.MatchRelaxed(
                x => x.MatchLdloc(out _) && w.SetCurrentTo(x),
                x => x.MatchLdloc(out _),
                x => x.MatchLdarg(out _),
                x => x.MatchLdloc(out _),
                x => x.MatchCallOrCallvirt<CharacterBody>("RunicLensUpdateVariables")
            ).ThrowIfFailure();
            w.InsertBeforeCurrent(
                w.Create(OpCodes.Ldarg_1),
                w.CreateDelegateCall((DamageInfo damageInfo) =>
                {
                    // gearbox added a proctype for runic lens AND they check for it before proccing
                    // but they forgot to make runic lens add it's proctype to the procchainmask when it procs?????????????
                    damageInfo.procChainMask.AddProc(ProcType.MeteorAttackOnHighDamage);
                })
            );
        }


        internal static void UseTheActualProcChainMaskGodDamnit(ILManipulationInfo info)
        {
            ILWeaver w = new(info);
            // moving after "procChainMask = default(ProcChainMask)"
            w.MatchRelaxed(
                x => x.MatchDup(),
                x => x.MatchLdflda<BlastAttack>("procChainMask"),
                x => x.MatchInitobj<ProcChainMask>() && w.SetCurrentTo(x)
            ).ThrowIfFailure();
            w.InsertAfterCurrent(
                w.Create(OpCodes.Dup),
                w.Create(OpCodes.Ldloc_0),
                w.CreateDelegateCall((BlastAttack blastAttack, DamageInfo damageInfo) =>
                {
                    // why does runic lens reset the procchainmask even though the real one is easily accessible???????
                    blastAttack.procChainMask = damageInfo.procChainMask;
                })
            );
        }



        internal static void DebugLogProcChainMask(HealthComponent self, ref DamageInfo damageInfo)
        {
            Log.Warning($"ProcChainMask is {damageInfo.procChainMask}");
        }
    }
}