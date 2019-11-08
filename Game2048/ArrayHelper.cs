using System;
namespace Game2048
{
    public class ArrayHelper
    {
        /// <summary>
        /// Get the elements according to the start point and number of elements
        /// </summary>
        /// <param name="map">the two-dimension array</param>
        /// <param name="rIndex">row index</param>
        /// <param name="cIndex">colume index</param>
        /// <param name="dir">Direction</param>
        /// <param name="count">Number of element</param>
        /// <returns>the array of querying elements</returns>
        public static int[] GetElement(int[,] map, int rIndex, int cIndex, Direction dir, int count)
        {
            int[] ans = new int[count];
            for (int i = 0; i < count; i++)
            {
                if(rIndex < map.GetLength(0) && rIndex >= 0 && cIndex < map.GetLength(1) && cIndex >= 0)
                {
                    ans[i] = map[rIndex, cIndex];
                    rIndex += dir.RIndex;
                    cIndex += dir.CIndex;
                }
            }

            return ans;
        }
    }
}
