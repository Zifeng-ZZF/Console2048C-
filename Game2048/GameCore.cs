using System;
using System.Collections.Generic;
namespace Game2048
{
    /// <summary>
    /// 2048，项目核心算法，无关界面
    /// </summary>
    public class GameCore
    {
        private int[,] map;
        private int[] rowColBuffer;
        private int[] removeZeroTemp;
        private List<Location> emptyLocationList;
        private Random random;
        private int[,] originalMap;
        private bool IsChange { get; set; }

        public int[,] Map
        {
            get { return this.map; }
        }

        public List<Location> EmptyLocationList
        {
            get { return this.emptyLocationList; }
        }

        public GameCore()
        {
            map = new int[4, 4];
            rowColBuffer = new int[4];
            removeZeroTemp = new int[4];
            emptyLocationList = new List<Location>();
            random = new Random();
            originalMap = new int[4, 4];
        }

        /// <summary>
        /// 移除数组中所有的0
        /// </summary>
        private void RemoveZero()
        {
            //每次去0操作的数组都会保留上次结果，需要清理为空
            Array.Clear(removeZeroTemp, 0, 4);
            int i = 0;
            foreach(int a in rowColBuffer)
            {
                if(a != 0)
                {
                    removeZeroTemp[i] = a;
                    i++;
                }
            }
            removeZeroTemp.CopyTo(rowColBuffer, 0);
        }

        /// <summary>
        /// 合并相邻两个一样的数，合并前后把0去掉
        /// </summary>
        private void Merge()
        {
            RemoveZero();

            for(int i = 0; i < rowColBuffer.Length - 1; i++)
            {
                if(rowColBuffer[i] != 0 && rowColBuffer[i] == rowColBuffer[i + 1])
                {
                    rowColBuffer[i] += rowColBuffer[i + 1];
                    rowColBuffer[i + 1] = 0;
                }
            }

            RemoveZero();
        }

        /// <summary>
        /// 往上滑的过程：首先获得被操作的游戏图，执行以下操作
        /// 1.空的位置被补全（去零）
        /// 2.从上往下数，从下往上合并：相邻相同的数字合并
        /// 3.空的位置被补全
        /// 4.空白位置随机产生新的数字
        /// </summary>
        private void MoveUp ()
        {
            for(int c = 0; c < map.GetLength(1); c++)
            {
                //获得一列
                for(int r = 0; r < map.GetLength(0); r++)
                {
                    rowColBuffer[r] = map[r, c];
                }

                Merge();

                //更新到游戏矩阵中
                for (int r = 0; r < map.GetLength(0); r++)
                {
                    map[r, c] = rowColBuffer[r];
                }
            }
        }

        /// <summary>
        /// 往下滑的过程，从下往上数，从上往下合并
        /// </summary>
        private void MoveDown()
        {
            for (int c = 0; c < map.GetLength(1); c++)
            {
                //获得一列
                for (int r = map.GetLength(0) - 1; r >= 0; r--)
                {
                    rowColBuffer[map.GetLength(0) - 1 - r] = map[r, c];
                }

                Merge();

                //更新到游戏矩阵中
                for (int r = map.GetLength(0) - 1; r >= 0; r--)
                {
                    map[r, c] = rowColBuffer[map.GetLength(0) - 1 - r];
                }
            }
        }

        /// <summary>
        /// 往左滑，拿出一行进行操作
        /// 从左往右数，从右往左合并
        /// </summary>
        private void MoveLeft()
        {
            for (int r = 0; r < map.GetLength(0); r++)
            {
                //获得一行
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    rowColBuffer[c] = map[r, c];
                }

                Merge();

                //更新到游戏矩阵中
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    map[r, c] = rowColBuffer[c];
                }
            }
        }

        /// <summary>
        /// 向右滑，从右往左数，从左往右合并
        /// </summary>
        private void MoveRight()
        {
            for (int r = 0; r < map.GetLength(0); r++)
            {
                //获得一行
                for (int c = map.GetLength(1) - 1; c >= 0; c--)
                {
                    rowColBuffer[map.GetLength(1) - 1 - c] = map[r, c];
                }

                Merge();

                //更新到游戏矩阵中
                for (int c = map.GetLength(1) - 1; c >= 0; c--)
                {
                    map[r, c] = rowColBuffer[map.GetLength(1) - 1 - c];
                }
            }
        }

        /// <summary>
        /// 根据枚举来判断当前移动，并调用适当的移动方法
        /// 该方法需要公开，应为要获得输入
        /// </summary>
        /// <param name="dir">移动方向</param>
        public void Move(MoveDirection dir)
        {
            Array.Copy(map, originalMap, map.Length);
            switch (dir)
            {
                case MoveDirection.Up:
                    MoveUp();
                    break;
                case MoveDirection.Down:
                    MoveDown();
                    break;
                case MoveDirection.Left:
                    MoveLeft();
                    break;
                case MoveDirection.Right:
                    MoveRight();
                    break;
            }

            //判断地图是否变化，无变换就不会产生随机数
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for(int c = 0; c < map.GetLength(1); c++)
                {
                    if (map[r, c] != originalMap[r, c])
                    {
                        RandomGenerate();
                        return;
                    }
               }
            }
        }

        /// <summary>
        /// 找到所有空白的位置，并放在一个List中
        /// </summary>
        public void findAllEmpty()
        {
            emptyLocationList.Clear();
            for (int r = 0; r < map.GetLength(0); r ++)
            {
                for (int c = 0; c < map.GetLength(1); c ++)
                {
                    if(map[r, c] == 0)
                    {
                        emptyLocationList.Add(new Location(r, c));
                    }
                }
            }
        }

        /// <summary>
        /// 在空白位置随机产生一个2或一个4，产生2的概率为90%，产生4的概率为10%
        /// </summary>
        public void RandomGenerate()
        {
            findAllEmpty();
            int randomIndex = random.Next(0, emptyLocationList.Count);
            Location loc = emptyLocationList[randomIndex];
            //按概率返回4、2:在0到10中拿一个数的概率是百分之十，如果是则返回4，否则2
            map[loc.RIndex, loc.CIndex] = random.Next(0, 10) == 1 ? 4 : 2;
        }
    }
}
