namespace TestApp.src
{
    class SecondTable
    {
        private readonly int _ID;
        private readonly double _Xs;
        private readonly double _Ys;
        private readonly double _Xe;
        private readonly double _Ye;
        public SecondTable(string id, string xs, string ys, string xe, string ye)
        {
            if (!int.TryParse(id, out _ID)) _ID = 0;
            if (!double.TryParse(xs, out _Xs)) _Xs = 0;
            if (!double.TryParse(ys, out _Ys)) _Ys = 0;
            if (!double.TryParse(xe, out _Xe)) _Xe = 0;
            if (!double.TryParse(ye, out _Ye)) _Ye = 0;
        }
        public int ID { get { return _ID; } }
        public double Xs { get { return _Xs; } }
        public double Ys { get { return _Ys; } }
        public double Xe { get { return _Xe; } }
        public double Ye { get { return _Ye; } }
    }
}
