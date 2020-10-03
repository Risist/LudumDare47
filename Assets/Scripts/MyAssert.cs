using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace Assets.Scripts
{
    public static class MyAssert
    {
        public static void Fail( string message)
        {
            throw new AssertionException("MyAssert.Fail: ",message);
        }
    }
}
