using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using JetBrains.Annotations;
using Modding;
//using SFCore.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

namespace NegativeNail
{
    public class NegativeNail: Mod, ITogglableMod
    {
        public override string GetVersion() => "1.0";
        public List<string> recentlyHit = new List<string>();
        public override void Initialize()
        {
            ModHooks.SlashHitHook += ModHooks_SlashHitHook;
            ModHooks.GetPlayerIntHook += ModHooks_GetPlayerIntHook;
        }

        private void ModHooks_SlashHitHook(Collider2D otherCollider, GameObject slash)
        {
            if (otherCollider.gameObject.GetComponent<HealthManager>() && !recentlyHit.Contains(otherCollider.gameObject.name))
            {
                HealthManager enemy = otherCollider.gameObject.GetComponent<HealthManager>();
                int b = PlayerData.instance.nailDamage + Mathf.RoundToInt((float)PlayerData.instance.maxHealth / 4);
                b += PlayerData.instance.fireballLevel;
                b += PlayerData.instance.quakeLevel;
                b += PlayerData.instance.screamLevel;
                if (PlayerData.instance.equippedCharm_19)
                {
                    b += 1;
                }
                if (PlayerData.instance.equippedCharm_25)
                {
                    b -= 2;
                }
                if (PlayerData.instance.equippedCharm_6&&PlayerData.instance.health == 1)
                {
                    b -= 2;
                }
                b = Mathf.RoundToInt((float)b / 2);
                enemy.hp += b;
                Log(b);
                recentlyHit.Add(otherCollider.gameObject.name);
                GameManager.instance.StartCoroutine(Wait(otherCollider.gameObject.name));
            }
        }

        IEnumerator Wait(string s)
        {
            yield return new WaitForSeconds(0.2f);
            recentlyHit.Remove(s);
        }

        private int ModHooks_GetPlayerIntHook(string name, int orig)
        {
            if(name =="fireballLevel" && orig == 0)
            {
                return 1;
            }
            else
            {
                return orig;
            }
        }

        public void Unload()
        {
            ModHooks.SlashHitHook += ModHooks_SlashHitHook;
            ModHooks.GetPlayerIntHook -= ModHooks_GetPlayerIntHook;
        }
    }
}
