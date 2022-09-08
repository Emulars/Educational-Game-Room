using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UEBlockly
{
    public class Executor : MonoBehaviour
    {
        [SerializeField] private GameObject startPoint;
        [SerializeField] private string scriptName;

        public static Dictionary<string, string> variabili = new Dictionary<string, string>();

        public string isInDictionary(string variableName)
        {
            return variabili.ContainsKey(variableName) ? variabili[variableName] : null;
        }

        public void CreateVariable(string variableName)
        {
            variabili.TryAdd(variableName, "");
            Debug.Log("CREATE VARIABLE: " + variableName);
        }

        public void UpdateVariable((string Name, string Value) updateValue)
        {
            if(variabili.ContainsKey(updateValue.Name))
            {
                variabili[updateValue.Name] = updateValue.Value;
                Debug.Log("VARIABLE: " + updateValue.Name + " HAS VALUE " + variabili[updateValue.Name]);
            }
            else
            {
                throw new ArgumentException("variabili named " + updateValue.Name + " not found");
            }
        }

        public void CatchError(ErrorCode e)
        {
            Debug.Log("Error: " + e);
            throw new ArgumentNullException();
        }

        public void IsFinish(string s)
        {
            Debug.Log(s+": Termine Esecuzione");
            //gameObject.SendMessageUpwards("BlockGameUpdate", variabili);
        }
    }
}
