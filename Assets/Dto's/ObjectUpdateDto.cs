using System;

namespace Dto_s
{
    [Serializable]
    public class ObjectUpdateDto
    {
        public string objectId;
        public int scaleX;
        public int scaleY;
        public int positionX;
        public int positionY;
        public int rotation;
        public string shape;
    }
}