using ConsoleShootEmUp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities;

internal class PlayerShot : Entity
{
    private double speed = 8d;
    public Vector2D Direction { get; private set; } = new(1, 0);
    private static int totalCounter = 0;
    public PlayerShot(Engine engine) : base(engine)
    {
        DisplayChar = '-';
        DisplayColor = ConsoleColor.Yellow;
        BitMask = BitMasks.PLAYER_PROJECTLE;
        Z = -1;
    }
    public override void Update(double deltaTime)
    {
        Move(Direction * (deltaTime * speed));
        if (!Engine.Display.BoundsCheck(GlobalPosition))
            Engine.RemoveEntity(this, this);
    }
    public override void OnCollisionEnter(Entity other)
    {
        if (other is IDamageable damageable && (other.BitMask & BitMasks.PLAYER) == 0)
        {
            damageable.TakeDamage(1);
            Engine.RemoveEntity(this, this);
        }
    }
    public void SetDirection(Vector2 direction)
    {
        if(direction==Vector2.Down||direction==Vector2.Up)
            DisplayChar='|';
        Direction = new Vector2D(direction);
    }
    public override void EnterEngine()
    {
        base.EnterEngine();
    }
    public override void ExitEngine()
    {
        base.ExitEngine();
    }

}
