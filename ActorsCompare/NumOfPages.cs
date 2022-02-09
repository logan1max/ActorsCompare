using System;
using System.Collections.Generic;
using System.Text;

namespace ActorsCompare
{
    public abstract class NumOfPages
    {
        public abstract int GetNumOfPages();
    }

    public class ZeroPages : NumOfPages
    {
        public override int GetNumOfPages(int num < 0 ? 0:1)
        {

        }
    }
}
