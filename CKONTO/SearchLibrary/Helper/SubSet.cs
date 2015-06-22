using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary.Helper
{
    public class SubSet<T>
    {
        private List<T> _list;
        private Int64 _length;
        private Int64 _max;
        private Int64 _count;

        public SubSet(List<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("lista");
            _list = list;
            _length = _list.Count;
            _count = 0;
            _max = (int)Math.Pow(2, _length);
        }


        public List<T> Next()
        {
            if (_count == _max)
            {
                return null;
            }
            Int64 rs = 0;

            List<T> l = new List<T>();

            while (rs < _length)
            {
                if ((_count & (1u << (int)rs)) > 0)
                {
                    l.Add(_list[(int)rs]);
                }
                rs++;
            }
            _count++;
            return l;
        }
    }
}
