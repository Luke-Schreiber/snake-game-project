﻿using System.Collections;
using System.Xml.Linq;

namespace SnakeGame

{
    public static class World
    {
        static ArrayList walls = new ArrayList();
        static ArrayList powerups = new ArrayList();
        static ArrayList snakes = new ArrayList();
        static double worldSize = 2000;

        public static void setSize(double size)
        {
            worldSize = size;
        }

        public static void addWall(Wall wall)
        {
            walls.Add(wall);
        }
        public static void addPowerup(Powerup powerup)
        {
            powerups.Add(powerup);
        }
        public static void addSnake(Snake snake)
        {
            snakes.Add(snakes);
        }
    }
}