using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Interfaces
{
    internal interface IDirectionalMovement
    {
        public void SetDirection(Vector2D direction);
    }
}
