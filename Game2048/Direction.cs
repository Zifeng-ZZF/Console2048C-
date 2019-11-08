using System;
namespace Game2048
{
    /// <summary>
    /// "struct" cannot have non-parameter constructor
    /// because it has a default one
    /// </summary>
    public struct Direction
    {
        //自动属性的字段
        //struct不能赋处值，但是static静态变量可以，const常量也可以
        public int RIndex { get; set; }
        public int CIndex { get; set; }

        //自动属性的字段一定要调用无参构造函数进行赋值 ":this()"
        public Direction(int rIndex, int cIndex):this()
        {
            this.RIndex = rIndex;
            this.CIndex = cIndex;
        }

        public static Direction Up
        {
            get
            {
                return new Direction(-1, 0);
            }
        }

        public static Direction Down
        {
            get
            {
                return new Direction(1, 0);
            }
        }

        public static Direction Left
        {
            get
            {
                return new Direction(0, -1);
            }
        }

        public static Direction Right
        {
            get
            {
                return new Direction(0, 1);
            }
        }
    }
}
