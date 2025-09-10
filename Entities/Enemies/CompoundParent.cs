using ConsoleShootEmUp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Enemies;

internal class CompoundParent : Entity, IDirectionalMovement
{
    public CompoundParent(Engine engine) : base(engine)
    {
    }


    public override void Update(double deltaTime)
    {
        if (Children.Count == 0)
            Engine.RemoveEntity(this);
    }
    public void SetDirection(Vector2D direction)
    {
        foreach (var child in Children)
            if (child is IDirectionalMovement directional)
                directional.SetDirection(direction);
    }
}
