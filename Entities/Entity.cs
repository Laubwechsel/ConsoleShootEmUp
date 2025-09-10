using ConsoleShootEmUp.Entities.Movers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities
{
    internal class Entity
    {
        public uint ID { get; private set; }
        private static uint s_idCounter = 0;
        private static object s_idLock = new();
        public int Z { get; set; } = 0;
        public Vector2D GlobalPosition
        {
            get
            {
                if (Parent != null)
                    return LocalPosition + Parent.GlobalPosition;
                return LocalPosition;
            }
            set
            {
                if (Parent != null)
                    LocalPosition = value - Parent.GlobalPosition;
                else
                    LocalPosition = value;
            }
        }
        public Vector2D LocalPosition
        {
            get => _localPosition;
            set
            {
                Vector2 oldPos = new(GlobalPosition);
                _localPosition = value;
                Vector2 newPos = new(GlobalPosition);
                Engine.MoveEntity(this, oldPos, newPos);
            }
        }
        private Vector2D _localPosition = new();
        public Entity? Parent { get; private set; }
        public ReadOnlyCollection<Entity> Children { get => _children.AsReadOnly(); }
        private List<Entity> _children = new();
        public char DisplayChar { get; protected set; } = Display.DEFAULT_EMPTY_SPACE;
        public ConsoleColor DisplayColor { get; set; } = Display.DEFAULT_EMPTY_COLOR;
        public enum CI
        {
            NoInterpolation,
            Interpolation
        }

        public CI CollisonInterpolation { get; set; } = CI.NoInterpolation;
        public int BitMask { get; protected set; } = 0;
        public int MovementBlockMask { get; protected set; } = 0;
        public Engine Engine { get; protected set; }
        public bool NotInEngine
        {
            get => _notInEngine;
            set
            {
                _notInEngine = value;
                if (!value) return;
                foreach (Entity child in _children)
                {
                    child.NotInEngine = value;
                }
            }
        }
        private bool _notInEngine = true;
        public Entity(Engine engine)
        {
            Engine = engine;
            lock (s_idLock)
            {
                ID = s_idCounter;
                s_idCounter++;
            }
        }
        public void SetGlobalPosition(Vector2D position)
        {
            GlobalPosition = position;
        }
        public void SetGlobalPosition(Vector2 position)
        {
            GlobalPosition = new Vector2D(position);
        }
        public void SetGlobalPosition(double x, double y)
        {
            GlobalPosition = new Vector2D(x, y);
        }
        public void SetLocalPosition(Vector2D position)
        {
            _localPosition = position;
        }
        public void SetLocalPosition(Vector2 position)
        {
            _localPosition = new Vector2D(position);
        }
        public void SetLocalPosition(double x, double y)
        {
            _localPosition = new Vector2D(x, y);
        }
        public void SetParent(Entity? parent)
        {
            if (parent == Parent)
                return;
            if (parent == null)
            {
                RemoveParent();
                return;
            }
            Vector2D position = GlobalPosition;
            Entity? oldParent = Parent;
            Parent = parent;
            if (oldParent != null)
                oldParent.RemoveChild(this);
            Parent.AddChild(this);
            position -= Parent.GlobalPosition;
            _localPosition = position;
        }
        public void RemoveParent()
        {
            Vector2D position = GlobalPosition;
            Entity? oldParent = Parent;
            Parent = null;
            if (oldParent != null)
                oldParent.RemoveChild(this);
            _localPosition = position;
        }
        public void AddChild(Entity child)
        {
            if (_children.Contains(child))
                return;
            _children.Add(child);
            if (child.Parent != this)
                child.SetParent(this);
        }
        public void RemoveChild(Entity child)
        {
            if (!_children.Contains(child))
                return;
            _children.Remove(child);
            if (child.Parent == this)
                child.RemoveParent();
        }
        public virtual void Update(double deltaTime)
        {

        }
        public virtual void OnCollisionEnter(Entity other)
        {

        }
        public virtual void OnCollisionStay(Entity other)
        {

        }
        public virtual void OnCollisionExit(Entity other)
        {

        }
        public void Move(double x, double y)
        {
            Move(new Vector2D(x, y));
        }
        public void Move(Vector2 delta)
        {
            Move(new Vector2D(delta));
        }
        public virtual void Move(Vector2D delta)
        {
            Vector2D from = GlobalPosition;
            Vector2D to = new(from.X + delta.X, from.Y + delta.Y);
            switch (CollisonInterpolation)
            {
                case CI.NoInterpolation:
                    MoveNotInterpolated(from, to);
                    break;
                case CI.Interpolation:
                    MoveInterpolated(from, to);
                    break;
                default:
                    break;
            }

        }
        private bool MoveNotInterpolated(Vector2D from, Vector2D to)
        {
            if (!BlockerCheck(from, to))
                return false;

            if (Parent == null)
                _localPosition = to;
            else
                _localPosition = to - Parent.GlobalPosition;

            Engine.MoveEntity(this, new(from), new(to));

            foreach (var child in _children)
                child.ParentMoved(from, to);

            return true;
        }
        private Vector2D MoveInterpolated(Vector2D from, Vector2D to)
        {
            List<Vector2D> path = Vector2D.GetPath(from, to);
            for (int i = 1; i < path.Count; i++)
            {
                if (!MoveNotInterpolated(path[i - 1], path[i]))
                    return path[i - 1];
            }
            return to;
        }
        private void ParentMoved(Vector2D fromParent, Vector2D toParent)
        {
            Vector2D fromGlobal = fromParent + _localPosition;
            Vector2D toGlobal = toParent + _localPosition;
            if (!BlockerCheck(fromGlobal, toGlobal))
            {
                _localPosition -= toParent - fromParent;
                return;
            }

            Engine.MoveEntity(this, new(fromGlobal), new(toGlobal));
            foreach (var child in _children)
                child.ParentMoved(fromGlobal, toGlobal);
        }
        private bool BlockerCheck(Vector2D from, Vector2D to)
        {
            List<Entity> atDestination = Engine.GetEntitiesOnGrid(new(to));
            if (new Vector2(from) == new Vector2(to))
                return true;

            foreach (Entity other in atDestination)
            {
                if ((other.MovementBlockMask & BitMask) > 0)
                {
                    OnBlockedMovement(other, from, to);
                    other.OnBlockOtherMovement(this, from, to);
                    return false;
                }
                if ((MovementBlockMask & other.BitMask) > 0)
                {
                    other.OnBlockedMovement(this, from, to);
                    OnBlockOtherMovement(other, from, to);
                }
            }
            return true;
        }
        protected virtual void OnBlockedMovement(Entity blockedBy, Vector2D from, Vector2D to)
        {

        }
        protected virtual void OnBlockOtherMovement(Entity other, Vector2D from, Vector2D to)
        {

        }
        public virtual void ExitEngine()
        {
            RemoveParent();
            foreach (Entity child in _children)
            {
                Engine.RemoveEntity(child);
            }
        }
        public virtual void EnterEngine()
        {

        }
        public T? FindInParent<T>() where T : class
        {
            Entity? parent = Parent;
            while (parent is not T)
            {
                if (parent == null)
                    return null;
                parent = parent.Parent;
            }
            return parent as T;
        }
        public List<T> FindInChildren<T>() where T : class
        {
            List<T> list = new();
            Queue<Entity> queue = new Queue<Entity>(Children);
            while (queue.TryDequeue(out Entity? e))
            {
                if (e is T t)
                    list.Add(t);
                foreach (Entity child in e.Children)
                    queue.Enqueue(child);
            }
            return list;
        }
    }
}
