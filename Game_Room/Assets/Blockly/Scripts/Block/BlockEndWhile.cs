using UnityEngine;

namespace UEBlockly
{
    public class BlockEndWhile : MonoBehaviour, IEndStatement
    {

        public bool TimeToRepeat = false;

        private void Update()
        {
            if (!TimeToRepeat) return;
            Restart();
        }
        public void Execute()
        {
            Debug.Log("Block " + this.name + ": while cycle ended");
            TimeToRepeat = true;
        }
        public void Restart()
        {
            transform.parent.parent.GetComponent<BlockWhile>().Execute();
        }

    }
}