using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm
{
    internal class Animal: ModelBase
    {
        private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(nameof(Name)); } }

        private int? _milk;
        public int? Milk { get { return _milk; } set { _milk = value; OnPropertyChanged(nameof(Milk)); } }

        private int? _num;
        public int? Num { get { return _num; } set { _num = value; OnPropertyChanged(nameof(Num)); } }

    }
}
