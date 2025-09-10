using ConsoleShootEmUp.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp;

internal class Engine
{
    private Dictionary<Vector2, List<Entity>> _grid;
    private List<Entity> _allEntities;
    private ConcurrentQueue<Entity> _toRemove;
    private ConcurrentQueue<Entity> _toAdd;
    public Display Display { get; private set; }
    public event Action<ConsoleKeyInfo>? Input;
    private Thread? _inputThread;
    private Thread? _rederThread;
    private Thread? _engineThread;

    private const int AVRG_WINDOW = 10;
    private int[] _frameTimes = new int[AVRG_WINDOW];
    private int _frameTimeCounter = 0;
    private int[] _updateTimes = new int[AVRG_WINDOW];
    private int _updateTimeCounter = 0;


    private bool _engineFinished = false;
    public Engine()
    {
        Display = new();
        _allEntities = new List<Entity>();
        _toRemove = new ConcurrentQueue<Entity>();
        _toAdd = new ConcurrentQueue<Entity>();
        _grid = new Dictionary<Vector2, List<Entity>>();
    }
    public void Start()
    {
        Program.Input += HandleInput;
        Console.CursorVisible = false;

        _engineThread = new(EngineLoop);
        _engineThread.IsBackground = true;
        _engineThread.Start();

        _rederThread = new(Render);
        _rederThread.IsBackground = true;
        _rederThread.Start();
    }
    private void EngineLoop()
    {
        Stopwatch updateTimer = new();
        Stopwatch totalTimer = new();
        double deltaTime = 0.033d;
        while (!_engineFinished)
        {
            totalTimer.Restart();
            updateTimer.Restart();
            Update(deltaTime);
            updateTimer.Stop();
            Thread.Sleep(Math.Clamp(33 - (int)updateTimer.ElapsedMilliseconds, 0, int.MaxValue));
            if (SaveFile.Debug)
                AddUpdateTime((int)updateTimer.ElapsedMilliseconds);
            totalTimer.Stop();
            deltaTime = totalTimer.ElapsedMilliseconds / 1000d;
        }
    }
    private void Update(double deltaTime)
    {
        lock (_grid)
        {
            lock (_allEntities)
            {
                ProcessEntitiesToAdd();
                foreach (Entity entity in _allEntities)
                {
                    if (entity.NotInEngine) continue;
                    entity.Update(deltaTime);
                }
                ProcessEntitiesToRemove();
            }
            for (int x = 0; x < Display.Width; x++)
            {
                for (int y = 0; y < Display.Height; y++)
                {
                    List<Entity> list = GetEntitiesOnGridUnsafe(new(x, y));
                    CollisionStayDetection(list);
                }
            }
        }
    }
    private void ProcessEntitiesToAdd()
    {
        while (_toAdd.TryDequeue(out Entity? entity))
        {
            if (_allEntities.Contains(entity)) continue;
            _allEntities.Add(entity);
            Vector2 addPosition = new(entity.GlobalPosition);
            List<Entity> onGrid = GetEntitiesOnGridUnsafe(addPosition);
            onGrid.Add(entity);
            entity.NotInEngine = false;
            entity.EnterEngine();
            CollisionEnterDetection(entity, onGrid);
        }

    }
    private void ProcessEntitiesToRemove()
    {

        while (_toRemove.TryDequeue(out Entity? entity))
        {
            if (!_allEntities.Contains(entity)) continue;
            _allEntities.Remove(entity);
            Vector2 position = new(entity.GlobalPosition);
            List<Entity> onGrid = GetEntitiesOnGridUnsafe(position);
            onGrid.Remove(entity);
            entity.ExitEngine();
        }
    }
    public void AddEntity(Entity entity)
    {
        _toAdd.Enqueue(entity);
        foreach (Entity child in entity.Children)
        {
            AddEntity(child);
        }
    }
    public void RemoveEntity(Entity entity, Entity? remover = null)
    {
        entity.NotInEngine = true;
        _toRemove.Enqueue(entity);
    }
    public void MoveEntity(Entity entity, Vector2 from, Vector2 to)
    {
        lock (_grid)
        {
            if (entity.NotInEngine) return;
            if (from == to) return;
            List<Entity> fromList = GetEntitiesOnGridUnsafe(from);
            CollisionExitDetection(entity, fromList);
            if (entity.NotInEngine)
                return;

            fromList.Remove(entity);
            List<Entity> toList = GetEntitiesOnGridUnsafe(to);
            toList.Add(entity);
            CollisionEnterDetection(entity, toList);
        }
    }
    public List<Entity> GetEntitiesOnGrid(Vector2 position)
    {
        if (_grid.TryGetValue(position, out List<Entity>? list))
            return new(list);
        list = new();
        _grid.Add(position, list);
        return new(list);
    }
    private List<Entity> GetEntitiesOnGridUnsafe(Vector2 position)
    {
        if (_grid.TryGetValue(position, out List<Entity>? list))
            return list;
        list = new();
        _grid.Add(position, list);
        return list;
    }
    private void CollisionEnterDetection(Entity entity, List<Entity> list)
    {
        if (!list.Contains(entity)) return;
        foreach (Entity listEntity in new List<Entity>(list))
        {
            if (entity.NotInEngine) return;
            if (listEntity == entity || listEntity.NotInEngine)
                continue;
            listEntity.OnCollisionEnter(entity);
            entity.OnCollisionEnter(listEntity);
        }
    }
    private void CollisionExitDetection(Entity entity, List<Entity> list)
    {
        if (!list.Contains(entity)) return;
        foreach (Entity listEntity in new List<Entity>(list))
        {
            if (entity.NotInEngine) return;
            if (listEntity == entity || listEntity.NotInEngine) continue;

            listEntity.OnCollisionExit(entity);
            entity.OnCollisionExit(listEntity);
        }
    }
    private void CollisionStayDetection(List<Entity> list)
    {

        List<Entity> listCopy = new List<Entity>(list);
        for (int i = 0; i < listCopy.Count; i++)
        {
            Entity outer = listCopy[i];
            for (int o = i; o < listCopy.Count; o++)
            {
                Entity inner = listCopy[o];
                if (outer.NotInEngine) break;
                if (inner.NotInEngine) continue;

                outer.OnCollisionStay(inner);
                inner.OnCollisionStay(outer);
            }
        }
    }
    private void Render()
    {
        Stopwatch timer = new Stopwatch();
        while (!_engineFinished)
        {
            timer.Restart();

            Display.ResetBuffer();
            Display.ResetZBuffer();
            Display.ResetCBuffer();
            lock (_allEntities)
            {

                foreach (Entity entity in _allEntities)
                {
                    if (entity.NotInEngine) continue;
                    Display.DrawEntityOnPlayArea(entity);
                }
            }
            Console.SetCursorPosition(0, 0);
            Display.WriteToConsol(true);

            timer.Stop();
            if (SaveFile.Debug)
                AddFrameTime((int)timer.ElapsedMilliseconds);
            int time = Math.Clamp(33 - (int)timer.ElapsedMilliseconds, 0, int.MaxValue);
            Thread.Sleep(time);

        }
    }
    public void AddFrameTime(int time)
    {
        _frameTimes[_frameTimeCounter] = time;
        _frameTimeCounter++;
        _frameTimeCounter %= AVRG_WINDOW;
        Display.SetLowerUIElement(new UIElement(1000, "FT:", Display.FormatNumber(time, 5)));
        Display.SetLowerUIElement(new UIElement(1001, "FTA:", Display.FormatNumber(_frameTimes.Sum() / AVRG_WINDOW, 5)));
    }
    public void AddUpdateTime(int time)
    {
        _updateTimes[_updateTimeCounter] = time;
        _updateTimeCounter++;
        _updateTimeCounter %= AVRG_WINDOW;
        Display.SetLowerUIElement(new UIElement(1002, "UT:", Display.FormatNumber(time, 5)));
        Display.SetLowerUIElement(new UIElement(1003, "UTA:", Display.FormatNumber(_updateTimes.Sum() / AVRG_WINDOW, 5)));

    }

    private void HandleInput(ConsoleKeyInfo keyInfo)
    {
        if (keyInfo.Key == ConsoleKey.F1)
            if (SaveFile.Debug)
            {
                SaveFile.Debug = false;
                Display.RemoveLowerUIElement("FT:");
                Display.RemoveLowerUIElement("FTA:");
                Display.RemoveLowerUIElement("UT:");
                Display.RemoveLowerUIElement("UTA:");
            }
            else
                SaveFile.Debug = true;
        Input?.Invoke(keyInfo);
    }
}
