using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Movers
{
    internal class InBoundsMoverStoper : Entity
    {
        private Mover? _mover;
        public InBoundsMoverStoper(Engine engine) : base(engine)
        {
            Z = int.MinValue;
        }
        public override void Update(double deltaTime)
        {
            if (Engine.Display.BoundsCheck(GlobalPosition))
            {
                if (Parent != null)
                    _mover = FindInParent<Mover>();
                _mover?.StopMoving();
                if (Children.Count == 0)
                    Engine.RemoveEntity(this);
            }

        }
        public override void ExitEngine()
        {
            _mover?.StartMoving();
            base.ExitEngine();
        }
    }
}
