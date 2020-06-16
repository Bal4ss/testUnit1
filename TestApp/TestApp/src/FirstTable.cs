namespace TestApp.src
{
    class FirstTable
    {
        private readonly int _ID;
        private readonly double _X;
        private readonly double _Y;
        public FirstTable(string id, string x, string y)
        {
            if (!int.TryParse(id, out _ID)) _ID = 0;
            if (!double.TryParse(x, out _X)) _X = 0;
            if (!double.TryParse(y, out _Y)) _Y = 0;
        }
        public int ID { get { return _ID; } }
        public double X { get { return _X; } }
        public double Y { get { return _Y; } }
    }
}
