using Assets.Blockly.Scripts.Block.Interface;
using Assets.Blockly.Scripts.DragAndDrop;
using UnityEngine;

namespace Assets.Blockly.Scripts.Block
{
    public class StartBlock : MonoBehaviour
    {
        public void Execute()
        {
            var next = GetComponentInChildren<DropPosition>().droppedGameObject;
            if (next != null)
            {
                next.GetComponent<IBlock>().Execute();
            }
        }
    }
}
