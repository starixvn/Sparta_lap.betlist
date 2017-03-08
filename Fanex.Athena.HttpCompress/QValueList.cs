namespace Fanex.Athena.HttpCompress
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    [DebuggerDisplay("QValue[{Count}, {AcceptWildcard}]")]
    internal sealed class QValueList : List<QValue>
    {
        private static char[] delimiters = { ',' };

        #region Fields

        private bool _acceptWildcard;
        private bool _autoSort;

        #endregion Fields

        #region Constructors

        public QValueList(bool autoSort)
        {
            _autoSort = autoSort;
        }

        public QValueList(string values) : this(null == values ? new string[0] : values.Split(delimiters, StringSplitOptions.RemoveEmptyEntries))
        {
        }

        public QValueList(string[] values)
        {
            var ordinal = -1;
            foreach (string value in values)
            {
                var qvalue = QValue.Parse(value.Trim(), ++ordinal);
                Add(qvalue);
            }

            DefaultSort();
            _autoSort = true;
        }

        #endregion Constructors

        #region Properties

        public bool AcceptWildcard
        {
            get { return _acceptWildcard; }
        }

        public bool AutoSort
        {
            get { return _autoSort; }
            set { _autoSort = value; }
        }

        public QValue this[params string[] candidates]
        {
            get { return FindPreferred(candidates); }
        }

        #endregion Properties

        #region Add

        public new void Add(QValue item)
        {
            if (item.Name.Equals("*"))
            {
                _acceptWildcard = item.CanAccept;
            }

            base.Add(item);
            ApplyAutoSort();
        }

        #endregion Add

        #region AddRange

        public new void AddRange(IEnumerable<QValue> collection)
        {
            _acceptWildcard = collection.Any(i => i.Name.Equals("*"));

            base.AddRange(collection);

            ApplyAutoSort();
        }

        #endregion AddRange

        #region DefaultSort

        public void DefaultSort()
        {
            Sort(QValue.CompareByWeightDesc);
        }

        #endregion DefaultSort

        #region Find

        public QValue Find(string name)
        {
            Predicate<QValue> criteria = delegate(QValue item) { return item.Name.Equals(name, StringComparison.OrdinalIgnoreCase); };
            return Find(criteria);
        }

        #endregion Find

        #region FindHighestWeight

        public QValue FindHighestWeight(params string[] candidates)
        {
            Predicate<QValue> criteria = delegate(QValue item)
            {
                return IsCandidate(item.Name, candidates);
            };
            return Find(criteria);
        }

        #endregion FindHighestWeight

        #region FindPreferred

        public QValue FindPreferred(params string[] candidates)
        {
            Predicate<QValue> criteria = delegate(QValue item)
            {
                return IsCandidate(item.Name, candidates) && item.CanAccept;
            };

            return Find(criteria);
        }

        #endregion FindPreferred

        #region isCandidate

        private static bool IsCandidate(string item, params string[] candidates)
        {
            return candidates.Any(i => i.Equals(item, StringComparison.OrdinalIgnoreCase));
        }

        #endregion isCandidate

        #region applyAutoSort

        private void ApplyAutoSort()
        {
            if (_autoSort)
            {
                DefaultSort();
            }
        }

        #endregion applyAutoSort
    }
}