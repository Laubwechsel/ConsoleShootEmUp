using ConsoleShootEmUp.Entities;
using ConsoleShootEmUp.Entities.Enemies;
using ConsoleShootEmUp.Entities.Interfaces;
using ConsoleShootEmUp.Entities.Movers;
using ConsoleShootEmUp.Entities.Walls;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace ConsoleShootEmUp;
using BasicGroup = LevelBuilder.ActivatorGroup<InBoundsActivator, Enemy, Entity>;
internal class LevelBuilder
{
    private Engine _engine;
    private CompoundEntityBuilder _compoundEntityBuilder;
    public LevelBuilder(Engine engine)
    {
        _engine = engine;
        _compoundEntityBuilder = new(engine);
    }
    public Level BuildLevel00()
    {
        Level level = new Level(_engine);

        Player player = new Player(_engine);
        player.SetParent(level);
        player.SetLocalPosition(1, 4);

        Tutorial tut = new(_engine);
        level.AddChild(tut);

        LevelWin lw = new(_engine);
        level.AddChild(lw);
        lw.SetLocalPosition(10, 10);

        Reflector reflector = new Reflector(_engine);
        level.AddChild(reflector);
        reflector.SetLocalPosition(20, 5);

        FragileWall fw = new FragileWall(_engine);
        level.AddChild(fw);
        fw.SetLocalPosition(20, 7);

        for (int i = 0; i < 3; i++)
        {
            Wall wall = new Wall(_engine);
            level.AddChild(wall);
            wall.SetLocalPosition(20, i + 1);
            if (i == 1)
                wall.SetHorizontal(true);
            if (i == 2)
                wall.SetBlocker();
        }

        Turret turret = new(_engine);
        level.AddChild(turret);
        turret.SetShotDirection(Vector2.Left);
        turret.SetLocalPosition(17, 5);

        FinishLevel(level);
        return level;
    }
    public Level BuildLevel01()
    {
        Level level = new Level(_engine);

        Player player = new Player(_engine);
        level.AddChild(player);
        player.SetLocalPosition(1, _engine.Display.Height / 2);

        Mover mover = new Mover(_engine);
        level.AddChild(mover);
        mover.SetLocalPosition(new Vector2D(_engine.Display.Width + 4, 0));

        List<Enemy> enemies = new List<Enemy>();
        for (int i = 0; i < 20; i++)
        {
            Enemy enemy = new Enemy(_engine);
            enemies.Add(enemy);
            mover.AddChild(enemy);
        }

        InBoundsActivator activator1 = new(_engine);
        activator1.SingleUse = true;
        mover.AddChild(activator1);
        activator1.SetLocalPosition(new Vector2D(0, 0));
        enemies[0].SetLocalPosition(new Vector2D(0, 5));
        activator1.AddToActivate(enemies[0]);

        InBoundsActivator activator2 = new(_engine);
        activator2.SingleUse = true;
        mover.AddChild(activator2);
        activator2.SetLocalPosition(new Vector2D(3, 0));
        enemies[1].SetLocalPosition(new Vector2D(3, 4));
        enemies[2].SetLocalPosition(new Vector2D(3, 5));
        enemies[3].SetLocalPosition(new Vector2D(3, 6));
        activator2.AddToActivate(enemies[1]);
        activator2.AddToActivate(enemies[2]);
        activator2.AddToActivate(enemies[3]);

        InBoundsActivator activator3 = new(_engine);
        activator3.SingleUse = true;
        mover.AddChild(activator3);
        activator3.SetLocalPosition(new Vector2D(8, 0));
        enemies[4].SetLocalPosition(new Vector2D(8, 1));
        enemies[5].SetLocalPosition(new Vector2D(9, 2));
        enemies[6].SetLocalPosition(new Vector2D(10, 3));
        enemies[7].SetLocalPosition(new Vector2D(8, 9));
        enemies[8].SetLocalPosition(new Vector2D(9, 8));
        enemies[9].SetLocalPosition(new Vector2D(10, 7));
        activator3.AddToActivate(enemies[4]);
        activator3.AddToActivate(enemies[5]);
        activator3.AddToActivate(enemies[6]);
        activator3.AddToActivate(enemies[7]);
        activator3.AddToActivate(enemies[8]);
        activator3.AddToActivate(enemies[9]);

        InBoundsActivator activator4 = new(_engine);
        mover.AddChild(activator4);
        activator4.SetLocalPosition(new Vector2D(16, 0));
        enemies[10].SetLocalPosition(new Vector2D(16, 1));
        enemies[12].SetLocalPosition(new Vector2D(16, 2));
        enemies[14].SetLocalPosition(new Vector2D(16, 5));
        enemies[18].SetLocalPosition(new Vector2D(16, 8));
        enemies[16].SetLocalPosition(new Vector2D(16, 9));
        enemies[11].SetLocalPosition(new Vector2D(17, 1));
        enemies[13].SetLocalPosition(new Vector2D(17, 2));
        enemies[15].SetLocalPosition(new Vector2D(17, 5));
        enemies[17].SetLocalPosition(new Vector2D(17, 9));
        enemies[19].SetLocalPosition(new Vector2D(17, 8));
        activator4.AddToActivate(enemies[10]);
        activator4.AddToActivate(enemies[11]);
        activator4.AddToActivate(enemies[12]);
        activator4.AddToActivate(enemies[13]);
        activator4.AddToActivate(enemies[14]);
        activator4.AddToActivate(enemies[15]);
        activator4.AddToActivate(enemies[16]);
        activator4.AddToActivate(enemies[17]);
        activator4.AddToActivate(enemies[18]);
        activator4.AddToActivate(enemies[19]);

        Entity wallGroup = new Entity(_engine);
        mover.AddChild(wallGroup);
        wallGroup.SetLocalPosition(19, 0);

        InBoundsMoverStoper moverStoperWall = new InBoundsMoverStoper(_engine);
        wallGroup.AddChild(moverStoperWall);
        moverStoperWall.SetLocalPosition(3, 0);

        InBoundsActivator inBoundsActivatorWall = new InBoundsActivator(_engine);
        inBoundsActivatorWall.SingleUse = true;
        wallGroup.AddChild(inBoundsActivatorWall);
        inBoundsActivatorWall.SetLocalPosition(3, 0);

        for (int i = 0; i < _engine.Display.Height; i++)
        {
            if (i == (_engine.Display.Height) / 2)
            {
                FragileWall fwall = new FragileWall(_engine);
                moverStoperWall.AddChild(fwall);
                fwall.SetLocalPosition(-3, i);
            }
            else
            {
                Wall wall = new Wall(_engine);
                wallGroup.AddChild(wall);
                wall.SetLocalPosition(0, i);
            }
        }

        DistributedSpawner enemySpawner1 = new DistributedSpawner(_engine, () => new Enemy(_engine), Vector2D.Left);
        enemySpawner1.SetSpawnOffset(new(-4, 0));
        moverStoperWall.AddChild(enemySpawner1);
        enemySpawner1.SetLocalPosition(-1, 0);
        inBoundsActivatorWall.AddToActivate(enemySpawner1);

        DistributedSpawner enemySpawner2 = new DistributedSpawner(_engine, () => new Enemy(_engine), Vector2D.Left);
        enemySpawner2.SetSpawnOffset(new(-4, 0));
        moverStoperWall.AddChild(enemySpawner2);
        enemySpawner2.SetLocalPosition(-1, _engine.Display.Height / 2);
        inBoundsActivatorWall.AddToActivate(enemySpawner2);

        DistributedSpawner enemySpawner3 = new DistributedSpawner(_engine, () => new Enemy(_engine), Vector2D.Left);
        enemySpawner3.SetSpawnOffset(new(-4, 0));
        moverStoperWall.AddChild(enemySpawner3);
        enemySpawner3.SetLocalPosition(-1, _engine.Display.Height - 1);
        inBoundsActivatorWall.AddToActivate(enemySpawner3);

        LevelWin levelWin = new LevelWin(_engine);
        mover.AddChild(levelWin);
        levelWin.SetLocalPosition(new Vector2D(23, _engine.Display.Height / 2));

        InBoundsMoverStoper levelWinStopper = new(_engine);
        mover.AddChild(levelWinStopper);
        levelWinStopper.SetLocalPosition(27, 0);

        FinishLevel(level);
        return level;
    }
    public Level BuildLevel02()
    {
        Level level = new Level(_engine);
        Mover mainMover = new Mover(_engine);
        mainMover.SetParent(level);
        mainMover.SetLocalPosition(_engine.Display.Width, 0);

        Player player = new Player(_engine);
        level.AddChild(player);
        player.SetLocalPosition(1, _engine.Display.Height / 2);

        #region First
        BasicGroup first = SpawnActivatedGroup<InBoundsActivator, Enemy, Entity>(10, _compoundEntityBuilder.ShieldedEnemy);
        mainMover.AddChild(first.Parent);
        first.Parent.SetLocalPosition(2, 0);
        first.Activator.SingleUse = true;
        for (int i = 0; i < 5; i++)
        {
            first.Group[i].SetLocalPosition(1 + i, 5 + 1 + i);
            first.Group[9 - i].SetLocalPosition(1 + i, 5 - 1 - i);
        }

        Enemy reflector = _compoundEntityBuilder.ReflectorEnemy();
        first.Activator.AddToActivate(reflector);
        first.Parent.AddChild(reflector);
        reflector.SetLocalPosition(2, 5);

        Enemy[] firstNormals = new Enemy[5];
        for (int i = 0; i < 5; i++)
        {
            firstNormals[i] = new Enemy(_engine);
            first.Add(firstNormals[i]);
        }
        firstNormals[0].SetLocalPosition(4, 4);
        firstNormals[1].SetLocalPosition(5, 2);
        firstNormals[2].SetLocalPosition(4, 6);
        firstNormals[3].SetLocalPosition(5, 8);
        firstNormals[4].SetLocalPosition(0, 5);
        #endregion

        #region Second
        ActivatorGroup<InBoundsActivator, Entity, Entity> second = new(new InBoundsActivator(_engine), new(), new(_engine));
        mainMover.AddChild(second.Parent);
        second.Parent.SetLocalPosition(11, 0);
        //second.Parent.SetLocalPosition(0, 0);
        second.Activator.SingleUse = true;
        for (int i = 0; i < 3; i++)
        {
            Bomber b = new(_engine);
            second.Add(b);
            b.SetLocalPosition(1 + i, 0);
        }

        second.Add(_compoundEntityBuilder.ReflectorEnemy());
        second.Group.Last().SetLocalPosition(0, 0);

        second.Add(_compoundEntityBuilder.ShieldedEnemy());
        second.Group.Last().SetLocalPosition(1, 8);
        second.Add(_compoundEntityBuilder.ShieldedEnemy());
        second.Group.Last().SetLocalPosition(2, 9);
        second.Add(_compoundEntityBuilder.ShieldedEnemy());
        second.Group.Last().SetLocalPosition(1, 10);

        second.Add(new Enemy(_engine));
        second.Group.Last().SetLocalPosition(3, 8);
        second.Add(new Enemy(_engine));
        second.Group.Last().SetLocalPosition(3, 10);
        #endregion

        #region Third
        EntityGroup<Entity, Entity> third = new(new(_engine), new());
        mainMover.AddChild(third.Parent);
        third.Parent.SetLocalPosition(19, 0);
        //third.Parent.SetLocalPosition(0, 0);

        for (int i = 0; i < 3; i++)
        {
            ActivatorGroup<InBoundsActivator, Enemy, Mover> down = new(new(_engine), new(), new(_engine));
            third.Add(down.Parent);
            down.Parent.SetLocalPosition(i, 0);
            down.Parent.SetSpeed(0.75);
            down.Activator.SingleUse = true;
            down.Activator.AddToActivate(down.Parent);
            down.Parent.SetDirection(Vector2D.Down);
            for (int o = 0; o < 2; o++)
            {
                down.Add(new Enemy(_engine));
                down.Last().SetLocalPosition(0, o);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            ActivatorGroup<InBoundsActivator, Enemy, Mover> up = new(new(_engine), new(), new(_engine));
            third.Add(up.Parent);
            up.Parent.SetLocalPosition(i + 0.5, 9d);
            up.Parent.SetSpeed(0.75);
            up.Activator.SingleUse = true;
            up.Activator.AddToActivate(up.Parent);
            up.Parent.SetDirection(Vector2D.Up);
            for (int o = 0; o < 2; o++)
            {
                up.Add(new Enemy(_engine));
                up.Last().SetLocalPosition(0, o);
            }
        }

        BasicGroup back = SpawnBasicGroup(12);
        third.Add(back.Parent);
        back.Activator.SingleUse = true;
        back.Parent.SetLocalPosition(2, 0);

        for (int i = 0; i < 3; i++)
        {
            back[i].SetLocalPosition(0, i + 1);
            back[i + 3].SetLocalPosition(1, i + 1);
            back[i + 6].SetLocalPosition(0, i + 7);
            back[i + 9].SetLocalPosition(1, i + 7);
        }
        #endregion

        #region Fourth
        BasicGroup fourth = SpawnBasicGroup(25);
        mainMover.AddChild(fourth.Parent);
        fourth.Parent.SetLocalPosition(26, 0);
        fourth.Activator.SingleUse = true;
        for (int i = 1; i < 11; i++)
        {
            fourth.Group[i - 1].SetLocalPosition(i, i % 2);
        }
        fourth.Group[10].SetLocalPosition(2, 1);
        fourth.Group[11].SetLocalPosition(3, 2);
        fourth.Group[12].SetLocalPosition(5, 3);
        fourth.Group[13].SetLocalPosition(4, 4);
        fourth.Group[14].SetLocalPosition(2, 5);
        fourth.Group[15].SetLocalPosition(5, 5);
        fourth.Group[16].SetLocalPosition(3, 6);
        fourth.Group[17].SetLocalPosition(4, 7);
        fourth.Group[18].SetLocalPosition(2, 8);
        fourth.Group[19].SetLocalPosition(5, 8);
        fourth.Group[20].SetLocalPosition(3, 9);
        fourth.Group[21].SetLocalPosition(2, 10);
        fourth.Group[22].SetLocalPosition(3, 10);
        fourth.Group[23].SetLocalPosition(5, 10);
        #endregion

        #region Fifth
        BasicGroup fifth = new(new InBoundsActivator(_engine), new(), new Entity(_engine));
        mainMover.AddChild(fifth.Parent);
        fifth.Parent.SetLocalPosition(38, 0);
        //fifth.Parent.SetLocalPosition(0, 0);

        for (int i = 0; i < 11; i++)
        {
            if (i % 2 == 0)
            {
                fifth.Add(_compoundEntityBuilder.ShieldedEnemy());
                fifth.Last().SetLocalPosition(0, i);
                fifth.Add(new Enemy(_engine));
                fifth.Last().SetLocalPosition(1, i);
            }
            else
            {
                fifth.Add(_compoundEntityBuilder.ReflectorEnemy());
                fifth.Last().SetLocalPosition(0, i);
            }
        }
        #endregion

        #region Sixth
        EntityGroup<Entity, Entity> sixth = new(new(_engine), new());
        //mainMover.AddChild(sixth.Parent);
        third.Parent.SetLocalPosition(48, 0);
        //sixth.Parent.SetLocalPosition(0, 0);

        for (int u = 0; u < 3; u++)
        {
            for (int i = 0; i < 3; i++)
            {
                ActivatorGroup<InBoundsActivator, Enemy, Mover> up = new(new(_engine), new(), new(_engine));
                sixth.Add(up.Parent);
                up.Parent.SetLocalPosition(i + (4 * u), 9);
                up.Activator.SingleUse = true;
                up.Activator.AddToActivate(up.Parent);
                up.Parent.SetDirection(Vector2D.Up);
                for (int o = 0; o < 2; o++)
                {
                    up.Add(new Enemy(_engine));
                    up.Last().SetLocalPosition(0, o);
                }
            }
        }
        for (int i = 0; i < 7; i++)
        {
            ActivatorGroup<InBoundsActivator, Bomber, Entity> bombers = SpawnActivatedGroup<InBoundsActivator, Bomber, Entity>(2, _compoundEntityBuilder.ShieldedBomber);
            sixth.Add(bombers.Parent);
            bombers.Activator.SingleUse = true;
            bombers.Parent.SetLocalPosition(i * 2, 0);
            bombers[0].SetLocalPosition(0, 0);
            bombers[1].SetLocalPosition(0, 1);
        }
        #endregion

        LevelWin win = new LevelWin(_engine);
        mainMover.AddChild(win);
        win.SetLocalPosition(56, 5);

        InBoundsActivator inBoundsActivator = new(_engine);
        mainMover.AddChild(inBoundsActivator);
        inBoundsActivator.SetLocalPosition(55, 5);
        Enemy lastEnemy = new(_engine);
        mainMover.AddChild(lastEnemy);
        lastEnemy.SetLocalPosition(55, 5);
        inBoundsActivator.AddToActivate(lastEnemy);
        InBoundsMoverStoper inBoundsMoverStoper = new(_engine);
        mainMover.AddChild(inBoundsMoverStoper);
        inBoundsMoverStoper.SetLocalPosition(60, 0);

        FinishLevel(level);
        return level;
    }
    public Level BuildLevel03()
    {
        Level level = new Level(_engine);
        Mover mainMover = new Mover(_engine);
        mainMover.SetParent(level);
        mainMover.SetLocalPosition(_engine.Display.Width - 4, 0);
        mainMover.SetSpeed(0.5);

        Player player = new Player(_engine);
        level.AddChild(player);
        player.SetLocalPosition(1, _engine.Display.Height / 2);

        EntityGroup<Entity, Wall> walls = SpawnGroup<Entity, Wall>(211);
        walls.Parent.SetParent(mainMover);
        walls.Parent.SetLocalPosition(0, 0);
        int count = 0;
        int length = 0;
        length = 6; LineUpEntities(walls.Group.GetRange(count, length), new(0, 0), true); count += length;
        length = 4; LineUpEntities(walls.Group.GetRange(count, length), new(0, 2), true); count += length;
        length = 9; LineUpEntities(walls.Group.GetRange(count, length), new(4, 2), false); count += length;
        length = 3; LineUpEntities(walls.Group.GetRange(count, length), new(5, 10), true); count += length;
        length = 5; LineUpEntities(walls.Group.GetRange(count, length), new(6, 0), false); count += length;
        length = 3; LineUpEntities(walls.Group.GetRange(count, length), new(6, 6), false); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(7, 0), true); count += length;
        length = 5; LineUpEntities(walls.Group.GetRange(count, length), new(8, 0), false); count += length;
        length = 5; LineUpEntities(walls.Group.GetRange(count, length), new(8, 6), false); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(9, 7), true); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(9, 3), true); count += length;
        length = 5; LineUpEntities(walls.Group.GetRange(count, length), new(10, 2), true); count += length;
        length = 12; LineUpEntities(walls.Group.GetRange(count, length), new(10, 8), true); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(11, 4), false); count += length;
        length = 7; LineUpEntities(walls.Group.GetRange(count, length), new(12, 4), true); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(18, 4), false); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(11, 6), false); count += length;
        length = 7; LineUpEntities(walls.Group.GetRange(count, length), new(12, 6), true); count += length;
        length = 3; LineUpEntities(walls.Group.GetRange(count, length), new(15, 0), false); count += length;
        length = 3; LineUpEntities(walls.Group.GetRange(count, length), new(16, 0), true); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(18, 6), false); count += length;
        length = 3; LineUpEntities(walls.Group.GetRange(count, length), new(19, 0), false); count += length;
        length = 2; LineUpEntities(walls.Group.GetRange(count, length), new(20, 2), true); count += length;
        length = 3; LineUpEntities(walls.Group.GetRange(count, length), new(22, 0), false); count += length;
        length = 3; LineUpEntities(walls.Group.GetRange(count, length), new(22, 8), false); count += length;
        length = 3; LineUpEntities(walls.Group.GetRange(count, length), new(23, 10), true); count += length;
        length = 3; LineUpEntities(walls.Group.GetRange(count, length), new(23, 0), true); count += length;
        length = 3; LineUpEntities(walls.Group.GetRange(count, length), new(26, 0), false); count += length;
        length = 3; LineUpEntities(walls.Group.GetRange(count, length), new(27, 0), true); count += length;
        length = 7; LineUpEntities(walls.Group.GetRange(count, length), new(26, 4), false); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(27, 4), true); count += length;
        length = 5; LineUpEntities(walls.Group.GetRange(count, length), new(27, 8), true); count += length;
        length = 3; LineUpEntities(walls.Group.GetRange(count, length), new(28, 2), false); count += length;
        length = 2; LineUpEntities(walls.Group.GetRange(count, length), new(28, 6), true); count += length;
        length = 7; LineUpEntities(walls.Group.GetRange(count, length), new(30, 0), false); count += length;
        length = 5; LineUpEntities(walls.Group.GetRange(count, length), new(31, 0), true); count += length;
        length = 5; LineUpEntities(walls.Group.GetRange(count, length), new(32, 2), false); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(32, 8), true); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(33, 8), false); count += length;
        length = 4; LineUpEntities(walls.Group.GetRange(count, length), new(34, 2), false); count += length;
        length = 2; LineUpEntities(walls.Group.GetRange(count, length), new(35, 4), true); count += length;
        length = 2; LineUpEntities(walls.Group.GetRange(count, length), new(34, 8), false); count += length;
        length = 5; LineUpEntities(walls.Group.GetRange(count, length), new(35, 8), true); count += length;
        length = 3; LineUpEntities(walls.Group.GetRange(count, length), new(36, 0), false); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(37, 2), false); count += length;
        length = 8; LineUpEntities(walls.Group.GetRange(count, length), new(37, 0), true); count += length;
        length = 5; LineUpEntities(walls.Group.GetRange(count, length), new(38, 2), false); count += length;
        length = 7; LineUpEntities(walls.Group.GetRange(count, length), new(40, 2), false); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(41, 2), true); count += length;
        length = 7; LineUpEntities(walls.Group.GetRange(count, length), new(42, 2), false); count += length;
        length = 2; LineUpEntities(walls.Group.GetRange(count, length), new(43, 8), true); count += length;
        length = 4; LineUpEntities(walls.Group.GetRange(count, length), new(45, 0), false); count += length;
        length = 2; LineUpEntities(walls.Group.GetRange(count, length), new(45, 7), false); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(46, 3), true); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(46, 7), true); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(47, 3), false); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(47, 2), true); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(47, 7), false); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(47, 8), true); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(48, 2), false); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(48, 1), true); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(48, 8), false); count += length;
        length = 1; LineUpEntities(walls.Group.GetRange(count, length), new(48, 9), true); count += length;
        length = 2; LineUpEntities(walls.Group.GetRange(count, length), new(49, 0), false); count += length;
        length = 2; LineUpEntities(walls.Group.GetRange(count, length), new(49, 9), false); count += length;



        EntityGroup<Entity, Turret> turrets1 = SpawnGroup<Entity, Turret>(22);
        mainMover.AddChild(turrets1.Parent);
        turrets1.Parent.SetLocalPosition(0, 0);
        Vector2[] positions = { new(5, 9), new(7, 1), new(7, 9), new(16, 1), new(17, 1), new(18, 1), new(23, 1), new(24, 1), new(25, 1), new(23, 9), new(24, 9), new(25, 9), new(27, 3), new(27, 7), new(31, 1), new(33, 6), new(33, 7), new(37, 7), new(37, 1), new(38, 1), new(43, 7), new(48, 7) };
        Vector2[] directions = { Vector2.Up, Vector2.Down, Vector2.Up, Vector2.Down, Vector2.Down, Vector2.Down, Vector2.Down, Vector2.Down, Vector2.Down, Vector2.Up, Vector2.Up, Vector2.Up, Vector2.Left, Vector2.Up, Vector2.Down, Vector2.Up, Vector2.Left, Vector2.Up, Vector2.Right, Vector2.Right, Vector2.Up, Vector2.Up };

        for (int i = 0; i < turrets1.Group.Count; i++)
        {
            turrets1.Group[i].SetLocalPosition(positions[i]);
            turrets1.Group[i].SetShotDirection(directions[i]);
        }


        EntityGroup<Entity, FragileWall> fragileWalls = SpawnGroup<Entity, FragileWall>(2);
        mainMover.AddChild(fragileWalls.Parent);
        fragileWalls.Parent.SetLocalPosition(0, 0);
        fragileWalls.Group[0].SetLocalPosition(6, 5);
        fragileWalls.Group[1].SetLocalPosition(34, 6);

        EntityGroup<Entity, Reflector> reflectors = SpawnGroup<Entity, Reflector>(3);
        mainMover.AddChild(reflectors.Parent);
        reflectors.Parent.SetLocalPosition(0, 0);
        reflectors.Group[0].SetLocalPosition(7, 8);
        reflectors.Group[0].SetHorizontal(true);
        reflectors.Group[1].SetLocalPosition(32, 7);
        reflectors.Group[2].SetLocalPosition(33, 5);
        reflectors.Group[2].SetHorizontal(true);

        //ExtraLife el = new(_engine);
        //mainMover.AddChild(el);
        //el.SetLocalPosition(15, 5);

        LevelWin levelWin = new LevelWin(_engine);
        mainMover.AddChild(levelWin);
        levelWin.SetLocalPosition(54, 5);

        InBoundsMoverStoper inBoundsMoverStoper = new(_engine);
        mainMover.AddChild(inBoundsMoverStoper);
        mainMover.AddChild(levelWin);
        inBoundsMoverStoper.SetLocalPosition(56, 0);

        FinishLevel(level);
        return level;

    }
    public Level BuildLevel04()
    {
        Level level = new(_engine);
        Player player = new(_engine);
        level.AddChild(player);
        player.SetLocalPosition(1, _engine.Display.Height / 2);

        Boss boss = _compoundEntityBuilder.BossEnemy();
        level.AddChild(boss);
        boss.SetLocalPosition((_engine.Display.Width / 2) - 7, (_engine.Display.Height / 2) - 3);

        FinishLevel(level);
        return level;
    }
    public Level BuildTestLevel()
    {
        Level level = new(_engine);


        Mover mainmover = new(_engine);
        level.AddChild(mainmover);
        mainmover.SetLocalPosition(_engine.Display.Width, 0);

        Wall w = new(_engine);
        mainmover.AddChild(w);
        w.SetLocalPosition(0, 0);

        InBoundsActivator a = new(_engine);
        mainmover.AddChild(a);
        a.SetLocalPosition(60, 0);
        Enemy e = new(_engine);
        mainmover.AddChild(e);
        e.SetLocalPosition(60, 0);
        a.AddToActivate(e);


        FinishLevel(level);
        return level;
    }
    public static void LineUpEntities<T>(List<T> entities, Vector2 initialPosition, bool horizontal) where T : Entity
    {
        for (int i = 0; i < entities.Count; i++)
        {
            Vector2 position = initialPosition;
            if (horizontal)
                position.X += i;
            else
                position.Y += i;
            entities[i].SetLocalPosition(position);
            if (entities[i] is IDirectionalDisplay directional)
                directional.SetHorizontal(horizontal);
        }
    }
    private void AddColumn(int offset, Entity parent, Func<Enemy>? spawnFunc = null)
    {
        BasicGroup g1 = SpawnBasicGroup(_engine.Display.Height, spawnFunc);
        g1.Parent.SetParent(parent);
        g1.Parent.SetLocalPosition(offset, 0);
        for (int i = 0; i < g1.Group.Count; i++)
        {
            g1.Group[i].SetLocalPosition(0, i);
        }
    }

    private void FinishLevel(Level level)
    {
        Queue<Entity> toDisable = new();
        toDisable.Enqueue(level);

        while (toDisable.TryDequeue(out Entity? entity))
        {
            foreach (Entity child in entity.Children)
            {
                toDisable.Enqueue(child);
            }
            if (entity is IActivator activator)
                foreach (IActivatable activatable in activator.Activatable)
                    activatable.SetActive(false);
        }
        _engine.AddEntity(level);
    }
    private ActivatorGroup<A, G, P> SpawnActivatedGroup<A, G, P>(int size, Func<G>? spawnFunc = null) where G : Entity, IActivatable where A : Entity, IActivator where P : Entity
    {
        P parent = (P)Activator.CreateInstance(typeof(P), _engine)!;
        A activator = (A)Activator.CreateInstance(typeof(A), _engine)!;
        List<G> group = new List<G>(size);

        for (int i = 0; i < size; i++)
        {
            if (spawnFunc != null)
                group.Add(spawnFunc());
            else
                group.Add((G)Activator.CreateInstance(typeof(G), _engine)!);
            activator.AddToActivate(group[i]);
            parent.AddChild(group[i]);
        }

        return new(activator, group, parent);
    }
    private EntityGroup<P, G> SpawnGroup<P, G>(int size, Func<G>? spawnFunc = null) where P : Entity where G : Entity
    {
        P parent = (P)Activator.CreateInstance(typeof(P), _engine)!;
        List<G> group = new List<G>(size);
        for (int i = 0; i < size; i++)
        {
            if (spawnFunc != null)
                group.Add(spawnFunc());
            else
                group.Add((G)Activator.CreateInstance(typeof(G), _engine)!);
            parent.AddChild(group[i]);
        }
        if (typeof(P) == typeof(Entity))
            parent.Z = int.MinValue;
        return new EntityGroup<P, G>(parent, group);
    }
    private BasicGroup SpawnBasicGroup(int size, Func<Enemy>? spawnFunc = null)
    {
        return SpawnActivatedGroup<InBoundsActivator, Enemy, Entity>(size, spawnFunc);
    }
    private class EntityGroup<P, G> where P : Entity where G : Entity
    {
        public P Parent;
        public List<G> Group;
        public EntityGroup(P parent, List<G> group)
        {
            Parent = parent;
            Group = group;
        }
        public void Add(G g)
        {
            Group.Add(g);
            Parent.AddChild(g);
        }
        public G this[int i]
        {
            get => Group[i];
        }
        public G Last()
        {
            return Group.Last();
        }

    }
    public class ActivatorGroup<A, G, P> where G : Entity where A : Entity, IActivator where P : Entity
    {
        public P Parent;
        public A Activator;
        public List<G> Group;
        public ActivatorGroup(A activator, List<G> group, P parent)
        {
            Parent = parent;
            Activator = activator;
            parent.AddChild(activator);
            Group = group;
        }
        public void Add<T>(T g) where T : IActivatable, G
        {
            Parent.AddChild(g);
            Activator.AddToActivate(g);
            Group.Add(g);
        }

        public G this[int i]
        {
            get => Group[i];
        }
        public G Last()
        {
            return Group.Last();
        }
    }
}
