using Harmony;
using ModFramework;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedFilterMenu {
    namespace Patches {
        [HarmonyPatch(typeof(PlayerController), "OnPrefabInit")]
        public static class PlayerController_OnPrefabInit {
            public static void Postfix(PlayerController __instance) {
                List<InterfaceTool> interfaceTools = new List<InterfaceTool>(__instance.tools);

                GameObject afmDigTool = new GameObject(AdvancedFiltrationAssets.ADVANCEDFILTERMENU_CANCEL_TOOLNAME, typeof(AdvancedFilterMenuCancelTool));
                afmDigTool.transform.SetParent(__instance.gameObject.transform);
                afmDigTool.gameObject.SetActive(true);
                afmDigTool.gameObject.SetActive(false);

                interfaceTools.Add(afmDigTool.GetComponent<InterfaceTool>());

                GameObject afmDeconstructTool = new GameObject(AdvancedFiltrationAssets.ADVANCEDFILTERMENU_DECONSTRUCT_TOOLNAME, typeof(AdvancedFilterMenuDeconstructTool));
                afmDeconstructTool.transform.SetParent(__instance.gameObject.transform);
                afmDeconstructTool.gameObject.SetActive(true);
                afmDeconstructTool.gameObject.SetActive(false);

                interfaceTools.Add(afmDeconstructTool.GetComponent<InterfaceTool>());

                GameObject afmPrioritizeTool = new GameObject(AdvancedFiltrationAssets.ADVANCEDFILTERMENU_PRIORITIZE_TOOLNAME, typeof(AdvancedFilterMenuPrioritizeTool));
                afmPrioritizeTool.transform.SetParent(__instance.gameObject.transform);
                afmPrioritizeTool.gameObject.SetActive(true);
                afmPrioritizeTool.gameObject.SetActive(false);

                interfaceTools.Add(afmPrioritizeTool.GetComponent<InterfaceTool>());

                GameObject afmEmptyPipeTool = new GameObject(AdvancedFiltrationAssets.ADVANCEDFILTERMENU_EMPTYPIPE_TOOLNAME, typeof(AdvancedFilterMenuEmptyPipeTool));
                afmEmptyPipeTool.transform.SetParent(__instance.gameObject.transform);
                afmEmptyPipeTool.gameObject.SetActive(true);
                afmEmptyPipeTool.gameObject.SetActive(false);

                interfaceTools.Add(afmEmptyPipeTool.GetComponent<InterfaceTool>());

                __instance.tools = interfaceTools.ToArray();

                AdvancedFiltrationAssets.ADVANCEDFILTERMENU_CANCEL_TOOLCOLLECTION = ToolMenu.CreateToolCollection(
                    STRINGS.UI.TOOLS.CANCEL.NAME,
                    "icon_action_cancel",
                    Action.BuildingCancel,
                    AdvancedFiltrationAssets.ADVANCEDFILTERMENU_CANCEL_TOOLNAME,
                    STRINGS.UI.TOOLTIPS.CANCELBUTTON,
                    true
                );

                AdvancedFiltrationAssets.ADVANCEDFILTERMENU_DECONSTRUCT_TOOLCOLLECTION = ToolMenu.CreateToolCollection(
                    STRINGS.UI.TOOLS.DECONSTRUCT.NAME,
                    "icon_action_deconstruct",
                    Action.BuildingDeconstruct,
                    AdvancedFiltrationAssets.ADVANCEDFILTERMENU_DECONSTRUCT_TOOLNAME,
                    STRINGS.UI.TOOLTIPS.DECONSTRUCTBUTTON,
                    true
                );

                AdvancedFiltrationAssets.ADVANCEDFILTERMENU_PRIORITIZE_TOOLCOLLECTION = ToolMenu.CreateToolCollection(
                    STRINGS.UI.TOOLS.PRIORITIZE.NAME,
                    "icon_action_prioritize",
                    Action.Prioritize,
                    AdvancedFiltrationAssets.ADVANCEDFILTERMENU_PRIORITIZE_TOOLNAME,
                    STRINGS.UI.TOOLTIPS.PRIORITIZEBUTTON,
                    true
                );

                AdvancedFiltrationAssets.ADVANCEDFILTERMENU_EMPTYPIPE_TOOLCOLLECTION = ToolMenu.CreateToolCollection(
                    STRINGS.UI.TOOLS.EMPTY_PIPE.NAME,
                    "icon_action_empty_pipes",
                    Action.EmptyPipe,
                    AdvancedFiltrationAssets.ADVANCEDFILTERMENU_EMPTYPIPE_TOOLNAME,
                    STRINGS.UI.TOOLS.EMPTY_PIPE.TOOLTIP,
                    false
                );
            }
        }

        [HarmonyPatch(typeof(ToolMenu), "OnPrefabInit")]
        public static class ToolMenu_OnPrefabInit {
            public static void Postfix() {
                MultiToolParameterMenu.CreateInstance();
            }
        }

        [HarmonyPatch(typeof(ToolMenu), "CreateBasicTools")]
        public static class ToolMenu_CreateBasicTools {
            public static void Postfix(ToolMenu __instance) {
                for (int i = 0; i < __instance.basicTools.Count; ++i) {
                    if (__instance.basicTools[i].text == "Cancel") {
                        __instance.basicTools[i] = AdvancedFiltrationAssets.ADVANCEDFILTERMENU_CANCEL_TOOLCOLLECTION;
                    }

                    if (__instance.basicTools[i].text == "Deconstruct") {
                        __instance.basicTools[i] = AdvancedFiltrationAssets.ADVANCEDFILTERMENU_DECONSTRUCT_TOOLCOLLECTION;
                    }

                    if (__instance.basicTools[i].text == "Priority") {
                        __instance.basicTools[i] = AdvancedFiltrationAssets.ADVANCEDFILTERMENU_PRIORITIZE_TOOLCOLLECTION;
                    }

                    if (__instance.basicTools[i].text == "Empty Pipe") {
                        __instance.basicTools[i] = AdvancedFiltrationAssets.ADVANCEDFILTERMENU_EMPTYPIPE_TOOLCOLLECTION;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Game), "DestroyInstances")]
        public static class Game_DestroyInstances {
            public static void Postfix() {
                AdvancedFilterMenuCancelTool.DestroyInstance();
                AdvancedFilterMenuDeconstructTool.DestroyInstance();
                AdvancedFilterMenuPrioritizeTool.DestroyInstance();
                AdvancedFilterMenuEmptyPipeTool.DestroyInstance();
            }
        }
    }
}
