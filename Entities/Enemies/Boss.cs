using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Enemies;

internal class Boss : Entity
{
    private List<Entity> _components = new();
    public Boss(Engine engine) : base(engine)
    {

    }
    public override void Update(double deltaTime)
    {
        bool allComponentsDestroyed = true;
        for (int i = 0; i < _components.Count; i++)
        {
            Entity? component = _components[i];
            if (!component.NotInEngine)
            {
                allComponentsDestroyed = false;
                break;
            }
        }
        if (allComponentsDestroyed)
            Death();
    }
    public void AddComponent(Entity component)
    {
        _components.Add(component);
        if (component is Turret t)
        {
            t.SetWeaponCooldown(1.2);
            t.RandomizeTimer();
        }
    }
    private void Death()
    {
        foreach (Entity child in Children)
        {
            AnimatedChar ac = new(Engine);
            Engine.AddEntity(ac);
            Parent?.AddChild(ac);
            ac.AddState('X', ConsoleColor.DarkYellow, 0.3);
            ac.AddState('+', ConsoleColor.Yellow, 0.3);
            ac.SetState(0);
            ac.SetLifeDuration(2.5d);
            ac.SetGlobalPosition(child.GlobalPosition);
        }
        FindInParent<Game>()?.AddScore(100_000);
        Engine.RemoveEntity(this);
        LevelWin levelWin = new(Engine);
        Engine.AddEntity(levelWin);
        if (Parent != null)
            Parent.AddChild(levelWin);
        levelWin.SetLocalPosition(Engine.Display.Width / 2, Engine.Display.Height / 2);
    }
}
