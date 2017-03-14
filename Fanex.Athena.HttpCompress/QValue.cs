namespace Fanex.Athena.HttpCompress
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    [DebuggerDisplay("QValue[{Name}, {Weight}]")]
    internal struct QValue : IComparable<QValue>
    {
        private const double DefaultWeight = 1;

        private static char[] delimiters = { ';', '=' };

        private string _name;
        private double _weight;
        private int _ordinal;

        public QValue(string value) : this(value, 0)
        {
        }

        public QValue(string value, int ordinal)
        {
            _name = null;
            _weight = 0;
            _ordinal = ordinal;

            ParseInternal(ref this, value);
        }

        public string Name
        {
            get { return _name; }
        }

        public double Weight
        {
            get { return _weight; }
        }

        public bool CanAccept
        {
            get { return _weight > 0; }
        }

        public bool IsEmpty
        {
            get { return string.IsNullOrEmpty(_name); }
        }

        public static int CompareByWeightAsc(QValue x, QValue y)
        {
            return x.CompareTo(y);
        }

        public static int CompareByWeightDesc(QValue x, QValue y)
        {
            return -x.CompareTo(y);
        }

        public static QValue Parse(string value)
        {
            var item = new QValue();
            ParseInternal(ref item, value);
            return item;
        }

        public static QValue Parse(string value, int ordinal)
        {
            var item = Parse(value);
            item._ordinal = ordinal;
            return item;
        }

        public static QValue Parse(string value, int ordinal, double weight)
        {
            var item = Parse(value, ordinal);
            item._weight = weight;
            return item;
        }

        public int CompareTo(QValue other)
        {
            var value = _weight.CompareTo(other._weight);
            if (value == 0)
            {
                var ord = -_ordinal;
                value = ord.CompareTo(-other._ordinal);
            }

            return value;
        }

        private static void ParseInternal(ref QValue target, string value)
        {
            var parts = value.Split(delimiters, 3);
            if (parts.Length > 0)
            {
                target._name = parts[0].Trim();
                target._weight = DefaultWeight;
            }

            if (parts.Length == 3)
            {
                double.TryParse(parts[2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture.NumberFormat, out target._weight);
            }
        }
    }
}