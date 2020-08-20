using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoMonad
{
    public interface IDUnion
    {
        object CoproductValue { get; }
    }
}
