using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities;

internal class Player : Entity
{
    public int Health
    {
        get => _health;
        private set
        {
            _health = value;
            if (_health > _highestHealth)
                _highestHealth = _health;
            string healthString = "";
            for (int i = 0; i < _highestHealth; i++)
            {
                if (i < _health)
                    healthString += 'O';
                else
                    healthString += ' ';
            }
            Engine.Display.SetUpperUIElement(new(11, c_uiHealthString, healthString, ConsoleColor.Gray, ConsoleColor.Green));
        }
    }
    private int _health = 3;
    private int _highestHealth = 3;
    private const string c_uiHealthString = "Health:";

    private double _repulsorTimer = 0.0;
    private double _repulsorCoolDown = 2d;

    private bool _invincible = false;
    private const ConsoleColor c_normalColor = ConsoleColor.Green;
    private const ConsoleColor c_invincibleColor = ConsoleColor.Blue;



    public Player(Engine engine) : base(engine)
    {
        DisplayChar = 'P';
        DisplayColor = c_normalColor;
        Z = 10;
        BitMask = BitMasks.PLAYER;
        Engine.Input += HandleInput;
        Health = _health;
    }

    private void HandleInput(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.LeftArrow:
            case ConsoleKey.A:
                Move(-1, 0);
                break;
            case ConsoleKey.UpArrow:
            case ConsoleKey.W:
                Move(0, -1);
                break;
            case ConsoleKey.RightArrow:
            case ConsoleKey.D:
                Move(1, 0);
                break;
            case ConsoleKey.DownArrow:
            case ConsoleKey.S:
                Move(0, 1);
                break;
            case ConsoleKey.NumPad0:
            case ConsoleKey.Spacebar:
            case ConsoleKey.NumPad5:
                SetInvincible(true);
                return;
            case ConsoleKey.NumPad1:
            case ConsoleKey.B:
                break;
            case ConsoleKey.NumPad2:
            case ConsoleKey.K:
                FireBlaster(Vector2.Down);
                break;
            case ConsoleKey.NumPad4:
            case ConsoleKey.J:
                FireBlaster(Vector2.Left);
                break;
            case ConsoleKey.NumPad6:
            case ConsoleKey.L:
                FireBlaster(Vector2.Right);
                break;
            case ConsoleKey.NumPad8:
            case ConsoleKey.I:
                FireBlaster(Vector2.Up);
                break;
            case ConsoleKey.D0:
                break;
            case ConsoleKey.D1:
                break;
            case ConsoleKey.D2:
                break;
            case ConsoleKey.D3:
                break;
            case ConsoleKey.D4:
                break;
            case ConsoleKey.D5:
                break;
            case ConsoleKey.D6:
                break;
            case ConsoleKey.D7:
                break;
            case ConsoleKey.D8:
                break;
            case ConsoleKey.D9:
                break;

        }
        SetInvincible(false);
    }
    public override void Update(double deltaTime)
    {
        if (_repulsorTimer < _repulsorCoolDown)
        {
            _repulsorTimer += deltaTime;
        }

    }
    public override void ExitEngine()
    {
        base.ExitEngine();
        Engine.Display.RemoveUpperUIElement(c_uiHealthString);
        Engine.Input -= HandleInput;
    }
    public override void Move(Vector2D delta)
    {
        Vector2D pos = GlobalPosition;

        Vector2 to = new(pos + delta);
        if (!Engine.Display.BoundsCheck(to))
            return;
        base.Move(delta);
    }
    private void FireBlaster(Vector2 direction)
    {
        PlayerShot newShot = new PlayerShot(Engine);
        Vector2D shotPosition = GlobalPosition + direction;
        newShot.SetParent(Parent);
        newShot.SetGlobalPosition(shotPosition);
        newShot.SetDirection(direction);
        Engine.AddEntity(newShot);
    }
    private void FireRepulsor()
    {
        if (_repulsorTimer < _repulsorCoolDown) return;
        _repulsorTimer = 0;
        Vector2D position = GlobalPosition;
        position.X -= 2;
        for (int i = 0; i < 5; i++)
        {
            PlayerRepulsor a = new(Engine, 1);
            a.SetGlobalPosition(position);
            a.SetParent(Parent);
            PlayerRepulsor b = new(Engine, -1);
            b.SetGlobalPosition(position);
            b.SetParent(Parent);
            Engine.AddEntity(a);
            Engine.AddEntity(b);
            position.X += 1;
        }
    }
    private void SetInvincible(bool invincible)
    {
        _invincible = invincible;
        if (invincible)
            DisplayColor = c_invincibleColor;
        else
            DisplayColor = c_normalColor;
    }
    public void TakeDamage(int amount)
    {
        if (_invincible) return;
        Health -= amount;
        Math.Clamp(Health, 0, int.MaxValue);
        if (Health <= 0)
            FindInParent<Game>()?.FinishLevel(false);
    }
    public void Heal(int amount)
    {
        Health += amount;
    }
}
