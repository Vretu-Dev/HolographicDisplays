using Exiled.API.Enums;
using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HolographicDisplays
{
    public class WarheadStatusName
    {
        public WarheadStatus Status { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }

    public class Translations : ITranslation
    {
        public List<WarheadStatusName> WarheadStatuses { get; set; } = new List<WarheadStatusName>()
        {
            new WarheadStatusName { Status = WarheadStatus.Armed,      Name = "Armed",      Color = "red" },
            new WarheadStatusName { Status = WarheadStatus.NotArmed,   Name = "Not Armed",  Color = "green" },
            new WarheadStatusName { Status = WarheadStatus.InProgress, Name = "In Progress",Color = "orange" },
            new WarheadStatusName { Status = WarheadStatus.Detonated,  Name = "Detonated",  Color = "#8B0000" },
        };

        public string GetWarheadStatusName(WarheadStatus status) => WarheadStatuses.FirstOrDefault(w => w.Status == status)?.Name ?? status.ToString();
        public string GetWarheadStatusColor(WarheadStatus status) => WarheadStatuses.FirstOrDefault(w => w.Status == status)?.Color ?? "white";
    }
}