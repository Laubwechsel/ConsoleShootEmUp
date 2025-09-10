using ConsoleShootEmUp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Enemies;

internal class EntitySpawnPackage : Entity
{
    private Vector2D _targetPosition;
    private double _speed = 5d;
    private Vector2D _entityDirection;
    private Func<Entity> _spawnFunc;
    public EntitySpawnPackage(Engine engine, Vector2D targetPosition,Vector2D entityDirection,Func<Entity> spawnFunc) : base(engine)
    {
        DisplayChar = 'e';
        DisplayColor = ConsoleColor.Magenta;
        _targetPosition = targetPosition;
        _entityDirection = entityDirection;
        _spawnFunc = spawnFunc;
    }
    public override void Update(double deltaTime)
    {
        Vector2D delta = _targetPosition - GlobalPosition;
        if (delta.Magnitude() < 0.1f)
        {
            Entity entity = _spawnFunc();
            entity.SetGlobalPosition(GlobalPosition);
            entity.SetParent(Parent);
            Engine.AddEntity(entity);
            if (entity is IDirectionalMovement directional)
                directional.SetDirection(_entityDirection);

            Engine.RemoveEntity(this);
            return;
        }
        Move(delta * (1d / delta.Magnitude() * _speed * deltaTime));
    }
}
