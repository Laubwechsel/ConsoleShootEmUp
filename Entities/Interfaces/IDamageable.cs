using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Interfaces
{
    internal interface IDamageable
    {
        public int Health { get; }
        public void TakeDamage(int amount);
    }
}
