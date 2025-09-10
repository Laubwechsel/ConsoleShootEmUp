using ConsoleShootEmUp.Entities;
using ConsoleShootEmUp.Entities.Enemies;
using ConsoleShootEmUp.Entities.Walls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp
{
    internal class CompoundEntityBuilder
    {
        Engine _engine;
        public CompoundEntityBuilder(Engine engine)
        {
            _engine = engine;
        }
        public Enemy ShieldedEnemy()
        {
            Enemy e = new Enemy(_engine);
            FragileWall w2 = new FragileWall(_engine);
            e.AddChild(w2);
            w2.SetLocalPosition(-1, 0);

            return e;
        }
        public Bomber ShieldedBomber()
        {
            Bomber b = new Bomber(_engine);
            FragileWall fw = new FragileWall(_engine);
            b.AddChild(fw);
            fw.SetLocalPosition(-1, 0);

            return b;
        }

        public Enemy ReflectorEnemy()
        {
            Enemy e = new Enemy(_engine);
            Reflector w2 = new Reflector(_engine);
            e.AddChild(w2);
            w2.SetLocalPosition(-1, 0);

            return e;
        }
        public Entity ReflectorBomber()
        {
            Bomber b = new Bomber(_engine);
            Reflector w2 = new Reflector(_engine);
            b.AddChild(w2);
            w2.SetLocalPosition(-1, 0);
            return b;
        }
        public Boss BossEnemy()
        {
            Boss boss = new Boss(_engine);
            List<Wall> walls = new();
            for (int i = 0; i < 8 + 8 + 8 + 10 + 8 + 8 + 8; i++)
            {
                Wall wall = new Wall(_engine);
                walls.Add(wall);
                boss.AddChild(wall);
            }
            int count = 0;
            int length = 0;
            length = 2; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(0, 0), false); count += length;
            length = 2; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(0, 5), false); count += length;
            length = 2; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(1, 0), false); count += length;
            length = 2; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(1, 5), false); count += length;
            length = 7; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(2, 0), false); count += length;
            length = 3; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(3, 2), false); count += length;
            length = 7; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(4, 0), false); count += length;
            length = 1; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(5, 3), false); count += length;
            length = 3; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(6, 2), false); count += length;
            length = 3; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(7, 2), false); count += length;
            length = 1; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(8, 3), false); count += length;
            length = 7; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(9, 0), false); count += length;
            length = 3; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(10, 2), false); count += length;
            length = 7; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(11, 0), false); count += length;
            length = 2; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(12, 0), false); count += length;
            length = 2; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(12, 5), false); count += length;
            length = 2; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(13, 0), false); count += length;
            length = 2; LevelBuilder.LineUpEntities(walls.GetRange(count, length), new(13, 5), false); count += length;

            foreach (Wall wall in walls)
            {
                wall.SetBlocker();
            }
            List<FragileWall> fws = new List<FragileWall>();
            for (int i = 0; i < 18; i++)
            {
                FragileWall fr = new(_engine);
                fws.Add(fr);
                boss.AddChild(fr);
            }
            fws[0].SetLocalPosition(3, 0);
            fws[1].SetLocalPosition(5, 0);
            fws[2].SetLocalPosition(6, 0);
            fws[3].SetLocalPosition(7, 0);
            fws[4].SetLocalPosition(8, 0);
            fws[5].SetLocalPosition(10, 0);
            fws[0].SetHorizontal(true);
            fws[1].SetHorizontal(true);
            fws[2].SetHorizontal(true);
            fws[3].SetHorizontal(true);
            fws[4].SetHorizontal(true);
            fws[5].SetHorizontal(true);
            fws[6].SetLocalPosition(0, 2);
            fws[7].SetLocalPosition(0, 3);
            fws[8].SetLocalPosition(0, 4);
            fws[6].SetHorizontal(false);
            fws[7].SetHorizontal(false);
            fws[8].SetHorizontal(false);
            fws[9].SetLocalPosition(13, 2);
            fws[10].SetLocalPosition(13, 3);
            fws[11].SetLocalPosition(13, 4);
            fws[9].SetHorizontal(false);
            fws[10].SetHorizontal(false);
            fws[11].SetHorizontal(false);
            fws[12].SetLocalPosition(3, 6);
            fws[13].SetLocalPosition(5, 6);
            fws[14].SetLocalPosition(6, 6);
            fws[15].SetLocalPosition(7, 6);
            fws[16].SetLocalPosition(8, 6);
            fws[17].SetLocalPosition(10, 6);
            fws[12].SetHorizontal(true);
            fws[13].SetHorizontal(true);
            fws[14].SetHorizontal(true);
            fws[15].SetHorizontal(true);
            fws[16].SetHorizontal(true);
            fws[17].SetHorizontal(true);

            List<Turret> turrets = new();
            for (int i = 0; i < 18; i++)
            {
                Turret turret = new(_engine);
                turrets.Add(turret);
                boss.AddChild(turret);
                boss.AddComponent(turret);
            }

            turrets[0].SetLocalPosition(1, 2);
            turrets[1].SetLocalPosition(1, 3);
            turrets[2].SetLocalPosition(1, 4);
            turrets[3].SetLocalPosition(3, 1);
            turrets[4].SetLocalPosition(6, 1);
            turrets[5].SetLocalPosition(7, 1);
            turrets[6].SetLocalPosition(10, 1);
            turrets[7].SetLocalPosition(5, 2);
            turrets[8].SetLocalPosition(8, 2);
            turrets[9].SetLocalPosition(3, 5);
            turrets[10].SetLocalPosition(6, 5);
            turrets[11].SetLocalPosition(7, 5);
            turrets[12].SetLocalPosition(10, 5);
            turrets[13].SetLocalPosition(5, 4);
            turrets[14].SetLocalPosition(8, 4);
            turrets[15].SetLocalPosition(12, 2);
            turrets[16].SetLocalPosition(12, 3);
            turrets[17].SetLocalPosition(12, 4);
            turrets[0].SetShotDirection(Vector2.Left);
            turrets[1].SetShotDirection(Vector2.Left);
            turrets[2].SetShotDirection(Vector2.Left);
            turrets[3].SetShotDirection(Vector2.Up);
            turrets[4].SetShotDirection(Vector2.Up);
            turrets[5].SetShotDirection(Vector2.Up);
            turrets[6].SetShotDirection(Vector2.Up);
            turrets[7].SetShotDirection(Vector2.Up);
            turrets[8].SetShotDirection(Vector2.Up);
            turrets[9].SetShotDirection(Vector2.Down);
            turrets[10].SetShotDirection(Vector2.Down);
            turrets[11].SetShotDirection(Vector2.Down);
            turrets[12].SetShotDirection(Vector2.Down);
            turrets[13].SetShotDirection(Vector2.Down);
            turrets[14].SetShotDirection(Vector2.Down);
            turrets[15].SetShotDirection(Vector2.Right);
            turrets[16].SetShotDirection(Vector2.Right);
            turrets[17].SetShotDirection(Vector2.Right);

            return boss;
        }
    }
}
