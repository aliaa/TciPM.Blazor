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

        protected override void OnInitialized()
        {
            EventAggregator.Subscribe(this);
        }

        public async Task HandleAsync(RectifierPmShutdownInPmChanged message)
        {
            if (message.SourceId == message.SourceId)
                Console.WriteLine("message retrieved: " + message.ShutDownInPm);
        }
    }
}
