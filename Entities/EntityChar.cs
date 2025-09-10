using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities
{
    internal class EntityChar : Entity
    {
        public EntityChar(Engine engine) : base(engine)
        {
            DisplayColor = ConsoleColor.Gray;
        }
        public void SetChar(char c)
        {
            DisplayChar = c;
        }
        public void SetColor(ConsoleColor cc)
        {
            DisplayColor = cc;
        }
    }
}
