using ConsoleShootEmUp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp;

internal class Display
{
    public int Width = 30;
    public int Height = 11;
    public const double VISUAL_HEIGHT_MULTIPLIER = 2.2;
    public const char DEFAULT_EMPTY_SPACE = '.';
    public const ConsoleColor DEFAULT_EMPTY_COLOR = ConsoleColor.DarkGray;
    private char[,] _buffer;
    private ConsoleColor[,] _cBuffer;
    private int[,] _zBuffer;
    private object _bufferLock = new();

    private bool _clearOnNextDraw = false;

    private Dictionary<string, UIElement> _upperUI = new();
    private Dictionary<string, UIElement> _lowerUI = new();
    public Display()
    {
        ResizeDisplay(Width, Height);
    }
    [MemberNotNull(nameof(_buffer))]
    [MemberNotNull(nameof(_cBuffer))]
    [MemberNotNull(nameof(_zBuffer))]
    public void ResizeDisplay(int width, int height)
    {
        lock (_bufferLock)
        {
            Width = width;
            Height = height;

            _buffer = new char[Width, Height];
            _cBuffer = new ConsoleColor[Width, Height];
            _zBuffer = new int[Width, Height];
            _clearOnNextDraw = true;
            ResetBuffer();
            ResetCBuffer();
            ResetZBuffer();
        }
    }
    public void SetUpperUIElement(UIElement element)
    {
        int sizeBefore = _upperUI.Count;
        lock (_upperUI)
            _upperUI[element.Name] = element;
        int sizeAfter = _upperUI.Count;
        if (sizeBefore != sizeAfter)
            _clearOnNextDraw = true;
    }
    public void RemoveUpperUIElement(string element)
    {
        lock (_upperUI)
            if (_upperUI.ContainsKey(element))
            {
                _upperUI.Remove(element);
                _clearOnNextDraw = true;
            }
    }
    public void SetLowerUIElement(UIElement element)
    {
        int sizeBefore = _lowerUI.Count;
        lock (_lowerUI)
            _lowerUI[element.Name] = element;
        int sizeAfter = _lowerUI.Count;
        if (sizeBefore != sizeAfter)
            _clearOnNextDraw = true;
    }
    public void RemoveLowerUIElement(UIElement element)
    {
        RemoveLowerUIElement(element.Name);
    }
    public void RemoveLowerUIElement(string element)
    {
        lock (_lowerUI)
            if (_lowerUI.ContainsKey(element))
            {
                _lowerUI.Remove(element);
                _clearOnNextDraw = true;
            }
    }


    public void ResetBuffer()
    {
        lock (_bufferLock)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int o = 0; o < Height; o++)
                {
                    _buffer[i, o] = DEFAULT_EMPTY_SPACE;
                }
            }
        }
    }
    public void ResetZBuffer()
    {
        lock (_bufferLock)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int o = 0; o < Height; o++)
                {
                    _zBuffer[i, o] = int.MinValue;
                }
            }
        }
    }
    public void ResetCBuffer()
    {
        lock (_bufferLock)
        {

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _cBuffer[x, y] = DEFAULT_EMPTY_COLOR;
                }
            }
        }
    }
    public void DrawOnPlayArea(int x, int y, int z, char c, ConsoleColor cc = ConsoleColor.Gray)
    {
        lock (_bufferLock)
        {
            if (x >= Width || x < 0 || y >= Height || y < 0)
                return;
            if (_zBuffer[x, y] <= z)
            {
                _zBuffer[x, y] = z;
                _buffer[x, y] = c;
                _cBuffer[x, y] = cc;
            }
        }
    }
    public void DrawEntityOnPlayArea(Entity entity)
    {
        Vector2 position = new(entity.GlobalPosition);
        DrawOnPlayArea(position.X, position.Y, entity.Z, entity.DisplayChar, entity.DisplayColor);
    }
    public string GetBuffer()
    {
        StringBuilder sb = new();

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                sb.Append(_buffer[x, y]);
            }
            sb.Append('\n');
        }
        return sb.ToString();
    }
    public ConsoleColor[] GetColors()
    {
        ConsoleColor[] colors = new ConsoleColor[(Width * Height) + Height];
        int counter = 0;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (_buffer[x, y] == ' ' && counter > 0)
                    //colors[counter] = ConsoleColor.Gray;
                    colors[counter] = colors[counter - 1];
                else
                    //colors[counter] = ConsoleColor.Gray;
                    colors[counter] = _cBuffer[x, y];
                counter++;
            }
            //colors[counter] = ConsoleColor.Gray;
            colors[counter] = colors[counter - 1];
            counter++;
        }
        return colors;
    }
    public void WriteToConsol(bool inColor = false)
    {
        lock (_bufferLock)
        {
            Console.CursorVisible = false;
            if (_clearOnNextDraw)
            {
                Console.Clear();
                _clearOnNextDraw = false;
            }
            StringBuilder sb = new StringBuilder();
            List<UIElement> upperUI;
            lock (_upperUI)
                upperUI = _upperUI.Values.OrderBy(x => x.Order).ToList();
            List<UIElement> lowerUI;
            lock (_lowerUI)
                lowerUI = _lowerUI.Values.OrderBy(x => x.Order).ToList();

            if (!inColor)
            {
                foreach (UIElement element in upperUI)
                {
                    sb.Append(element.Name);
                    sb.Append(element.Value);
                    sb.Append('\n');
                }
                string buffer = GetBuffer();
                sb.Append(buffer);
                sb.Append('\n');
                foreach (UIElement element in lowerUI)
                {
                    sb.Append(element.Name);
                    sb.Append(element.Value);
                    sb.Append('\n');
                }

            }
            else
            {
                string text = GetBuffer();
                ConsoleColor[] colors = GetColors();
                ConsoleColor lastColor = Console.ForegroundColor;
                ConsoleColor currentColor;
                foreach (UIElement element in upperUI)
                {
                    currentColor = element.NameColor;
                    if (lastColor != currentColor)
                    {
                        Console.Write(sb.ToString());
                        sb.Clear();
                        Console.ForegroundColor = currentColor;
                        lastColor = currentColor;
                    }
                    sb.Append(element.Name);

                    currentColor = element.ValueColor;
                    if (lastColor != currentColor)
                    {
                        Console.Write(sb.ToString());
                        sb.Clear();
                        Console.ForegroundColor = currentColor;
                        lastColor = currentColor;
                    }
                    sb.Append(element.Value);
                    sb.Append('\n');
                }


                for (int i = 0; i < text.Length; i++)
                {
                    currentColor = colors[i];
                    char currentChar = text[i];
                    if (lastColor != currentColor)
                    {
                        Console.Write(sb.ToString());

                        sb.Clear();
                        Console.ForegroundColor = currentColor;
                        lastColor = currentColor;
                    }
                    sb.Append(currentChar);
                }

                foreach (UIElement element in lowerUI)
                {
                    currentColor = element.NameColor;
                    if (lastColor != currentColor)
                    {
                        Console.Write(sb.ToString());

                        sb.Clear();
                        Console.ForegroundColor = currentColor;
                        lastColor = currentColor;
                    }
                    sb.Append(element.Name);

                    currentColor = element.ValueColor;
                    if (lastColor != currentColor)
                    {
                        Console.Write(sb.ToString());

                        sb.Clear();
                        Console.ForegroundColor = currentColor;
                        lastColor = currentColor;
                    }
                    sb.Append(element.Value);
                    sb.Append('\n');
                }

            }
            Console.Write(sb);

        }
    }
    public enum Trim { None, Front, Back }
    public static string FormatNumber(int number, int length, Trim trim = Trim.None)
    {
        string ts = number.ToString();
        if (ts.Length > length)
            switch (trim)
            {
                case Trim.Front:
                    ts = ts.Substring(ts.Length - length, length);
                    break;
                case Trim.Back:
                    ts = ts.Substring(0, length);
                    break;
                case Trim.None:
                default:
                    break;
            }
        StringBuilder sb = new StringBuilder();
        for (int i = ts.Length; i < length; i++)
        {
            sb.Append('0');
        }
        sb.Append(ts);
        return sb.ToString();
    }
    public bool BoundsCheck(Vector2 position)
    {
        return !(position.X >= Width || position.X < 0 || position.Y >= Height || position.Y < 0);
    }
    public bool BoundsCheck(Vector2D position)
    {
        return BoundsCheck(new Vector2(position));
    }

}
