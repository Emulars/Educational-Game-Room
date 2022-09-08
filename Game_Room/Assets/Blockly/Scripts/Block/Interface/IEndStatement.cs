using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UEBlockly
{
    public interface IEndStatement
    {   //used in end statement block (if and while)
        public void Execute();
    }
}
