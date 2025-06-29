using System;

namespace Dto_s
{
    [Serializable]
    public class ObjectCreateDto
    {
        public int scaleX;
        public int scaleY;
        public int positionX;
        public int positionY;
        public int rotation;
        public string shape;
        public string environmentId;
    }
}