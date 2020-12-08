using System;
using TLDLoader;
using UnityEngine;

namespace HelicopterMod
{
    public class HelicopterMod : Mod
    {
        // The ID of the mod - Should be unique
        public override string ID
        {
            get
            {
                return "Helicopter";
            }
        }

        // The name of the mod that is displayed
        public override string Name
        {
            get
            {
                return "HelicopterVehicle";
            }
        }

        // The name of the author
        public override string Author
        {
            get
            {
                return "InsDel2113";
            }
        }

        // The version of the mod
        public override string Version
        {
            get
            {
                return "1.1";
            }
        }

        public override bool UseAssetsFolder
        {
            get
            {
                return true;
            }
        }

        AssetBundle bundle = null;
        public GameObject obj = null;
        GameObject helicopterObj;

        public override void OnLoad()
        {
            if (bundle == null || obj == null)
            {
                bundle = LoadAssets.LoadBundle(this, "ucak-main1.unity3d");
                obj = bundle.LoadAsset("ucak-main1.prefab") as GameObject;
            }
            if (obj != null)
            {
                helicopterObj = UnityEngine.Object.Instantiate(obj);
                HelicopterScript rb = helicopterObj.AddComponent<HelicopterScript>(); // add helicopter script
                                                                                      // adds all the required stuff for it, just make sure prefab is setup correctly
                                                                                      // TODO: MAKE HELICOPTER SCRIPT MORE DYNAMIC!
                helicopterObj.transform.position = mainscript.M.player.transform.position + (Vector3.back * 5) + (Vector3.up * 5); // at the home, this is just outside the back door!
            }
            else
            {
                Debug.LogError("Failed to load obj/ucak-main1.prefab from AssetBundles - HelicopterMod disabled");
            }
            if (bundle == null)
            {
                Debug.LogError("Failed to load bundle/ucak-main1.unity3d from AssetBundles - HelicopterMod disabled");
            }
        }
    }
}
