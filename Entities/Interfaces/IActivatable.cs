using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Interfaces
{
    internal interface IActivatable
    {
        public bool Active { get; }
        public IActivator? Activator { get; }
        public void SetActive(bool active);
        public void SetActivator(IActivator activator);
    }
}
