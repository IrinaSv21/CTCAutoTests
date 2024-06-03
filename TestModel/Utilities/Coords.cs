namespace CTCAutoTests.Utilities
{
    public class Coords
    {
        public int Left = int.MaxValue;
        public int Right = int.MinValue;
        public int Top = int.MaxValue;
        public int Bottom = int.MinValue;

        public Coords() { }

        public Coords(int fill)
        {
            this.Left = fill;
            this.Right = fill;
            this.Top = fill;
            this.Bottom = fill;
        }

        /// <summary>
        /// Устанавливает положение и размер прямоугольника.
        /// </summary>
        /// <param name="left">Левый край прямоугольника.</param>
        /// <param name="right">Правый край прямоугольника.</param>
        /// <param name="top">Верхний край прямоугольника.</param>
        /// <param name="bottom">Нижний край прямоугольника.</param>
        public void SetBounds(int left, int right, int top, int bottom)
        {
            this.Left = left;
            this.Right = right;
            this.Top = top;
            this.Bottom = bottom;
        }

        public static bool operator ==(Coords c1, Coords c2)
        {
            return c1 is null ? c2 is null : c1.Equals(c2);
        }

        public static bool operator !=(Coords c1, Coords c2)
        {
            return !(c1 == c2);
        }

        public override bool Equals(object obj)
        {
            return obj is Coords c2
                ? Left == c2.Left && Right == c2.Right && Top == c2.Top && Right == c2.Right
                : false;
        }

        public override int GetHashCode()
        {
            return (Left, Right, Top, Bottom).GetHashCode();
        }
    }
}
