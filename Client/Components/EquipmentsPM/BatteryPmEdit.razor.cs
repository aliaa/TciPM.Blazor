using EventAggregator.Blazor;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using TciPM.Blazor.Client.Events;
using TciPM.Blazor.Shared.Models.Equipments.PM;

namespace TciPM.Blazor.Client.Components.EquipmentsPM
{
    public partial class BatteryPmEdit : ComponentBase, IHandle<RectifierPmShutdownInPmChanged>
    {
        [Parameter]
        public BatteryPM Pm { get; set; }

        [Inject]
        private IEventAggregator EventAggregator { get; set; }

        private bool RectifierHasShutDown { get; set; }

        protected override void OnInitialized()
        {
            EventAggregator.Subscribe(this);
            foreach (var serie in Pm.Source.Batteries)
            {
                Pm.Series.Add(new BatteryPM.BatterySeriesPM(serie.CellsCountInt));
            }
        }

        public Task HandleAsync(RectifierPmShutdownInPmChanged message)
        {
            if (message.SourceId == message.SourceId)
                RectifierHasShutDown = message.ShutDownInPm;
            return Task.CompletedTask;
        }
    }
}
