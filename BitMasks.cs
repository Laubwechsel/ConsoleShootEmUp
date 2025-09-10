using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp;

internal class BitMasks
{
    public const int PLAYER = 0b_0000_0000_0000_0001;
    public const int PLAYER_PROJECTLE = 0b_0000_0000_0000_0010;
    public const int ENEMY = 0b_0000_0000_0001_0000;
    public const int ENEMY_PROJECTILE = 0b_0000_0000_0010_0000;
}
