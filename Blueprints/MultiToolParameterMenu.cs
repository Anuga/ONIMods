using PeterHan.PLib.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blueprints {
    [AddComponentMenu("KMonoBehaviour/scripts/ToolParameterMenu")]
    public sealed class MultiToolParameterMenu : KMonoBehaviour {
        public static MultiToolParameterMenu Instance;

        private readonly Dictionary<string, GameObject> widgets = new Dictionary<string, GameObject>();
        private GameObject content;
        private GameObject widgetContainer;
        private Dictionary<string, ToolParameterMenu.ToggleState> parameters;            

        protected override void OnPrefabInit() {
            base.OnPrefabInit();

            GameObject baseContent = ToolMenu.Instance.toolParameterMenu.content;
            GameObject baseWidgetContainer = ToolMenu.Instance.toolParameterMenu.widgetContainer;

            content = Util.KInstantiateUI(baseContent, baseContent.transform.parent.gameObject, false);
            content.transform.GetChild(1).gameObject.SetActive(false);

            PRelativePanel buttonsPanel = new PRelativePanel();

            PButton allButton = new PButton {
                Text = Strings.Get(BlueprintsStrings.STRING_BLUEPRINTS_MULTIFILTER_ALL)
            };

            allButton.OnClick += (GameObject source) => {
                Instance.SetAll(ToolParameterMenu.ToggleState.On);
            };

            PButton noneButton = new PButton {
                Text = Strings.Get(BlueprintsStrings.STRING_BLUEPRINTS_MULTIFILTER_NONE)
            };

            noneButton.OnClick += (GameObject source) => {
                Instance.SetAll(ToolParameterMenu.ToggleState.Off);
            };

            buttonsPanel.AddChild(allButton);
            buttonsPanel.SetLeftEdge(allButton, 0)
                .SetRightEdge(allButton, 0.5F);

            buttonsPanel.AddChild(noneButton);
            buttonsPanel.SetLeftEdge(noneButton, 0.5F)
                .SetRightEdge(noneButton, 1);

            buttonsPanel.Build();

            widgetContainer = Util.KInstantiateUI(baseWidgetContainer, content, true);
            buttonsPanel.AddTo(content, 3);

            content.SetActive(false);
        }

        public void PopulateMenu(Dictionary<string, ToolParameterMenu.ToggleState> parameters) {
            ClearMenu();
            this.parameters = new Dictionary<string, ToolParameterMenu.ToggleState>(parameters);

            foreach (KeyValuePair<string, ToolParameterMenu.ToggleState> parameter in parameters) {
                GameObject widetPrefab = Util.KInstantiateUI(ToolMenu.Instance.toolParameterMenu.widgetPrefab, widgetContainer, true);
                widetPrefab.GetComponentInChildren<LocText>().text = Strings.Get("STRINGS.UI.TOOLS.FILTERLAYERS." + parameter.Key);

                MultiToggle toggle = widetPrefab.GetComponentInChildren<MultiToggle>();
                switch (parameter.Value) {
                    case ToolParameterMenu.ToggleState.On:
                        toggle.ChangeState(1);
                        break;

                    case ToolParameterMenu.ToggleState.Disabled:
                        toggle.ChangeState(2);
                        break;

                    default:
                        toggle.ChangeState(0);
                        break;
                }

                toggle.onClick += () => {
                    foreach (KeyValuePair<string, GameObject> widget in this.widgets) {
                        if (widget.Value == toggle.transform.parent.gameObject) {
                            if (this.parameters[widget.Key] == ToolParameterMenu.ToggleState.Disabled) {
                                break;
                            }
                            
                            if (this.parameters[widget.Key] == ToolParameterMenu.ToggleState.On) {
                                this.parameters[widget.Key] = ToolParameterMenu.ToggleState.Off;
                            }

                            else {
                                this.parameters[widget.Key] = ToolParameterMenu.ToggleState.On;
                            }

                            OnChange();
                            break;
                        }
                    }
                };

                widgets.Add(parameter.Key, widetPrefab);
            }

            content.SetActive(true);
        }

        public bool IsActiveLayer(string layer) {
            return parameters.ContainsKey(layer.ToUpper()) && parameters[layer.ToUpper()] == ToolParameterMenu.ToggleState.On;
        }

        public void SetAll(ToolParameterMenu.ToggleState toggleState) {
            foreach (string key in parameters.Keys.ToList()) {
                parameters[key] = toggleState;
            }

            OnChange();
        }

        public static string GetFilterLayerFromGameObject(GameObject gameObject) {
            BuildingComplete buildingComplete = gameObject.GetComponent<BuildingComplete>();
            if (buildingComplete != null) {
                return GetFilterLayerFromObjectLayer(buildingComplete.Def.ObjectLayer);
            }

            BuildingUnderConstruction buildingUnderConstruction = gameObject.GetComponent<BuildingUnderConstruction>();
            if (buildingUnderConstruction != null) {
                return GetFilterLayerFromObjectLayer(buildingUnderConstruction.Def.ObjectLayer);
            }

            if (gameObject.GetComponent<Clearable>() != null || gameObject.GetComponent<Moppable>() != null) {
                return "CleanAndClear";
            }
                
            return gameObject.GetComponent<Diggable>() != null ? "DigPlacer" : "Default";
        }

        public static string GetFilterLayerFromObjectLayer(ObjectLayer objectLayer) {
            switch (objectLayer) {
                case ObjectLayer.Building:
                    return "Buildings";

                case ObjectLayer.Backwall:
                    return "BackWall";

                case ObjectLayer.FoundationTile:
                    return "Tiles";

                case ObjectLayer.GasConduit:
                case ObjectLayer.GasConduitConnection:
                    return "GasPipes";

                case ObjectLayer.LiquidConduit:
                case ObjectLayer.LiquidConduitConnection:
                    return "LiquidPipes";

                case ObjectLayer.SolidConduit:
                case ObjectLayer.SolidConduitConnection:
                    return "SolidConduits";

                case ObjectLayer.Wire:
                case ObjectLayer.WireConnectors:
                    return "Wires";

                case ObjectLayer.LogicGate:
                case ObjectLayer.LogicWire:
                    return "Logic";
            }

            return "Default";
        }

        private void OnChange() {
            foreach (KeyValuePair<string, GameObject> widget in widgets) {
                switch (parameters[widget.Key]) {
                    case ToolParameterMenu.ToggleState.On:
                        widget.Value.GetComponentInChildren<MultiToggle>().ChangeState(1);
                        continue;

                    case ToolParameterMenu.ToggleState.Off:
                        widget.Value.GetComponentInChildren<MultiToggle>().ChangeState(0);
                        continue;

                    case ToolParameterMenu.ToggleState.Disabled:
                        widget.Value.GetComponentInChildren<MultiToggle>().ChangeState(2);
                        continue;
                }
            }
        }

        public void ClearMenu() {
            content.SetActive(false);

            foreach (KeyValuePair<string, GameObject> widget in widgets) {
                Util.KDestroyGameObject(widget.Value);
            }
                
            widgets.Clear();
        }

        public void ShowMenu() {
            content.SetActive(true);
        }

        public void HideMenu() {
            content.SetActive(false);
        }

        public static void CreateInstance() {
            GameObject parameterMenu = new GameObject("", typeof(MultiToolParameterMenu));
            parameterMenu.transform.SetParent(ToolMenu.Instance.toolParameterMenu.transform.parent);
            parameterMenu.gameObject.SetActive(true);
            parameterMenu.gameObject.SetActive(false);

            Instance = parameterMenu.GetComponent<MultiToolParameterMenu>();
        }

        public static void DestroyInstance() {
            Instance = null;
        }
    }
}
