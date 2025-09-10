using ConsoleShootEmUp.Entities.Walls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Enemies;

internal class EnemyShot : Entity
{
    private double _speed = 4d;
    private Vector2D _direction = new(-1, 0);
    public EnemyShot(Engine engine) : base(engine)
    {
        DisplayChar = '-';
        DisplayColor = ConsoleColor.Red;
        BitMask = BitMasks.ENEMY_PROJECTILE;
        Z = 3;
    }
    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);

        Move(_speed * deltaTime * _direction);
        if (!Engine.Display.BoundsCheck(GlobalPosition))
            Engine.RemoveEntity(this);

    }
    public override void OnCollisionEnter(Entity other)
    {

        if (other is Wall)
        {
            Engine.RemoveEntity(this);
            return;
        }

        if (other is not Player) return;
        Player player = (Player)other;
        player.TakeDamage(1);
        Engine.RemoveEntity(this);
    }
    public void SetDirection(double x, double y)
    {
        SetDirection(new Vector2D(x, y));
    }
    public void SetDirection(Vector2D direction)
    {
        _direction = direction.Normalized();
        if (Math.Abs(_direction.X) > Math.Abs(_direction.Y))
            DisplayChar = '-';
        else
            DisplayChar = '|';

    }
    public void SetDirection(Vector2 direction)
    {
        SetDirection(new Vector2D(direction));
    }
    public void SetSpeed(double speed)
    {
        _speed = speed;
    }
}
