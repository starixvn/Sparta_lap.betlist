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

        private bool _acceptWildcard;
        private bool _autoSort;

        public QValueList(bool autoSort)
        {
            _autoSort = autoSort;
        }

        public bool AcceptWildcard
        {
            get { return _acceptWildcard; }
        }

        public new void Add(QValue item)
        {
            if (item.Name.Equals("*"))
            {
                _acceptWildcard = item.CanAccept;
            }

            base.Add(item);
            ApplyAutoSort();
        }

        public void DefaultSort()
        {
            Sort(QValue.CompareByWeightDesc);
        }

        public QValue Find(string name)
        {
            Predicate<QValue> criteria = delegate(QValue item) { return item.Name.Equals(name, StringComparison.OrdinalIgnoreCase); };

            return Find(criteria);
        }

        public QValue FindPreferred(params string[] candidates)
        {
            Predicate<QValue> criteria = delegate(QValue item) { return IsCandidate(item.Name, candidates) && item.CanAccept; };

            return Find(criteria);
        }

        private static bool IsCandidate(string item, params string[] candidates)
        {
            return candidates.Any(i => i.Equals(item, StringComparison.OrdinalIgnoreCase));
        }

        private void ApplyAutoSort()
        {
            if (_autoSort)
            {
                DefaultSort();
            }
        }
    }
}