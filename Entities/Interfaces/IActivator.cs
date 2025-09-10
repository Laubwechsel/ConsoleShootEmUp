using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Interfaces
{
    internal interface IActivator
    {
        public bool Active { get; }
        public List<IActivatable> Activatable { get; }
        public void AddToActivate(IActivatable activatable, bool initialActive = false);
        public void RemoveFromActivate(IActivatable activatable);
    }
}
