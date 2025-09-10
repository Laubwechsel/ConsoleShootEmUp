using ConsoleShootEmUp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Movers
{
    internal class InBoundsActivator : Entity, IActivator
    {
        public List<IActivatable> Activatable { get; private set; } = new();
        public bool Active { get; private set; } = false;
        public bool SingleUse { get; set; } = false;
        private bool _used = false;
        public InBoundsActivator(Engine engine) : base(engine)
        {
        }
        public override void Update(double deltaTime)
        {
            if (Activatable.Count == 0)
                Engine.RemoveEntity(this);

            if (SingleUse && _used)
                return;

            bool lastActive = Active;
            Active = Engine.Display.BoundsCheck(GlobalPosition);
            if (lastActive != Active)
            {
                if (Active)
                    foreach (IActivatable toActivate in Activatable)
                        toActivate.SetActive(true);
                else
                    foreach (IActivatable toActivate in Activatable)
                        toActivate.SetActive(true);
                _used = true;
            }
        }
        public void AddToActivate(IActivatable activatable, bool initialActive = false)
        {
            Activatable.Add(activatable);
            activatable.SetActivator(this);
        }

        public void RemoveFromActivate(IActivatable activatable)
        {
            Activatable.Remove(activatable);
        }
    }
}
