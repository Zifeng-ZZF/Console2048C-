using System;

namespace Game2048
{
    class MainClass
    {
        private static GameCore core;
        private static bool isSuccess = false;

        public static void Main(string[] args)
        {
            core = new GameCore();
            //初始先生成两个随机数
            core.RandomGenerate();
            core.RandomGenerate();
            //画出游戏矩阵
            DrawMap();

            while (true)
            {
                Move();
                DrawMap();
                core.findAllEmpty();

                //成功退出
                if (isSuccess)
                {
                    Console.WriteLine("Congrats!! You have won!!! or not?");
                    break;
                }

                //失败（填满）退出
                if (core.EmptyLocationList.Count == 0)
                {
                    Console.WriteLine("Fail!!!!!!! hahahahha!!!!");
                    break;
                }
            }

        }

        /// <summary>
        /// 画地图
        /// </summary>
        private static void DrawMap()
        {
            int[,] map = core.Map;
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    Console.Write(map[r, c] + " ");
                    if (map[r, c] == 2048)
                        isSuccess = true;
                }
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// 监听控制台输入决定移动
        /// </summary>
        private static void Move()
        {
            switch (Console.ReadLine())
            {
                case "w":
                    core.Move(MoveDirection.Up);
                    break;
                case "s":
                    core.Move(MoveDirection.Down);
                    break;
                case "a":
                    core.Move(MoveDirection.Left);
                    break;
                case "d":
                    core.Move(MoveDirection.Right);
                    break;
            }
        }
    }
}
