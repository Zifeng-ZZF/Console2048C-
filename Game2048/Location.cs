using System;
namespace Game2048
{
    public struct Location
    {
        public int RIndex { get; set; }
        public int CIndex { get; set; }
             

        public Location(int rIndex, int cIndex)
        {
            this.RIndex = rIndex;
            this.CIndex = cIndex;
        }
    }
}
