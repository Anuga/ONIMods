using System.Collections.Generic;

namespace Blueprints {
    public class MultiFilteredDragTool : DragTool {
        protected Dictionary<string, ToolParameterMenu.ToggleState> defaultParameters = new Dictionary<string, ToolParameterMenu.ToggleState>();

        protected override void OnActivateTool() {
            base.OnActivateTool();

            defaultParameters = GetDefaultFilters();

            MultiToolParameterMenu.Instance.PopulateMenu(defaultParameters);
            MultiToolParameterMenu.Instance.ShowMenu();
        }

        protected override void OnDeactivateTool(InterfaceTool newTool) {
            base.OnDeactivateTool(newTool);

            MultiToolParameterMenu.Instance.ClearMenu();
            MultiToolParameterMenu.Instance.HideMenu();
        }

        protected virtual Dictionary<string, ToolParameterMenu.ToggleState> GetDefaultFilters() {
            return new Dictionary<string, ToolParameterMenu.ToggleState> {
                { ToolParameterMenu.FILTERLAYERS.WIRES, ToolParameterMenu.ToggleState.On },
                { ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.On },
                { ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.On },
                { ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.On },
                { ToolParameterMenu.FILTERLAYERS.BUILDINGS, ToolParameterMenu.ToggleState.On },
                { ToolParameterMenu.FILTERLAYERS.LOGIC, ToolParameterMenu.ToggleState.On },
                { ToolParameterMenu.FILTERLAYERS.BACKWALL, ToolParameterMenu.ToggleState.On }
            };
        }
    }
}
