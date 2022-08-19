using System;
using Assets.Blockly.Scripts.Block.Interface;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Blockly.Scripts
{
    public class Calculator : MonoBehaviour
    {
        private Executor executor;
        private void Start()
        {
            this.executor = gameObject.GetComponent<Executor>();
        }
        public void UpdateVariableByBlock((string Name, string OperationTag, GameObject Value) updateValue)
        {
            void FloatOperation()
            {
                string FieldIsKey(string field)
                {
                    var varValue = executor.IsInDictionary(field);
                    if (varValue == null) return field;

                    //se è una variabile booleana
                    if (IBlock.IsBool(varValue))
                        throw new InvalidOperationException("cannot do operation between boolean");
                    return varValue;
                }

                void Calculator(string op, float f1, float f2)
                {
                    switch (op)
                    {
                        case "+":
                            executor.UpdateVariable((updateValue.Name, (f1 + f2).ToString()));
                            break;
                        case "-":
                            executor.UpdateVariable((updateValue.Name, (f1 - f2).ToString()));
                            break;
                        case "*":
                            executor.UpdateVariable((updateValue.Name, (f1 * f2).ToString()));
                            break;
                        case "/":
                            executor.UpdateVariable((updateValue.Name, (f1 / f2).ToString()));
                            break;
                        default:
                            throw new InvalidOperatorException("operator not valid", typeof(float));
                    }
                }

                var field1 = updateValue.Value.transform.Find("var1").Find("Text").GetComponent<Text>().text;
                var field2 = updateValue.Value.transform.Find("var2").Find("Text").GetComponent<Text>().text;
                // var op = updateValue.Value.transform.Find("Dropdown").Find("Text").GetComponent<Text>().text;
                var opDropdown = updateValue.Value.transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
                var op = opDropdown.options[opDropdown.value].text;

                field1 = FieldIsKey(field1);
                field2 = FieldIsKey(field2);

                var float1 = float.Parse(field1);
                var float2 = float.Parse(field2);

                Calculator(op, float1, float2);
            }
            void GetVarValue()
            {
                Transform valueField1 = updateValue.Value.transform.Find("InputField");
                string valueName = valueField1.transform.Find("Text").GetComponent<Text>().text;

                //if the name is valid
                ErrorCode err = IBlock.IsValidName(valueName);
                if (err != ErrorCode.NoError)
                    throw new ArgumentException("Errore: " + err);

                //if the variable exist
                if (executor.IsInDictionary(valueName) == null)
                    throw new ArgumentException("variable named " + valueName + " not found");

                executor.UpdateVariable((updateValue.Name, executor.IsInDictionary(valueName)));
            }
            void GetSimpleValue()
            {
                var transform1 = updateValue.Value.transform.Find("InputField");
                string value = transform1.transform.Find("Text").GetComponent<Text>().text;
                executor.UpdateVariable((updateValue.Name, value));
            }

            switch (updateValue.OperationTag)
            {
                //se è una variabile
                case "getValue":
                    GetVarValue();
                    break;

                //se è un valore
                case "value":
                    GetSimpleValue();
                    break;

                case "floatOpBlock":
                    FloatOperation();
                    break;

                default:
                    throw new Exception("questo blocco non restituisce una variabile");
            }
        }

        public void CheckCondition((GameObject ifBlock, string OperationTag, GameObject Value) condition)
        {
            void GetVarValue()
            {
                Transform valueField1 = condition.Value.transform.Find("InputField");
                string valueName = valueField1.transform.Find("Text").GetComponent<Text>().text;

                //if the name is valid
                ErrorCode err = IBlock.IsValidName(valueName);
                if (err != ErrorCode.NoError)
                    throw new ArgumentException("Errore: " + err);

                //if the variable exist
                var varValue = executor.IsInDictionary(valueName);
                if (varValue == null)
                    throw new ArgumentException("variabili named " + valueName + " not found");

                //if the variable is bool
                if (!IBlock.IsBool(varValue))
                    throw new TypeLoadException("variabili named " + valueName + " does not contain a bool value");

                //return the result to ifBlock
                condition.ifBlock.SendMessage("GetConditionResult", varValue);
            }
            void GetSimpleValue()
            {
                Transform transform1 = condition.Value.transform.Find("InputField");
                string value = transform1.transform.Find("Text").GetComponent<Text>().text;

                //if the value is bool
                if (!IBlock.IsBool(value))
                    throw new TypeLoadException(value + " is not a bool value");

                //return the result to ifBlock
                condition.ifBlock.SendMessage("GetConditionResult", value);
            }
            void BoolOperation()
            {
                void BooleanVariable(string op, bool b1, bool b2, GameObject ifBlock)
                {
                    switch (op)
                    {
                        case "==":
                            ifBlock.SendMessage("GetConditionResult", (b1 == b2).ToString());
                            break;
                        case "!=":
                            ifBlock.SendMessage("GetConditionResult", (b1 != b2).ToString());
                            break;
                        case "and" or "&&":
                            ifBlock.SendMessage("GetConditionResult", (b1 && b2).ToString());
                            break;
                        case "or" or "||":
                            ifBlock.SendMessage("GetConditionResult", (b1 || b2).ToString());
                            break;
                        default:
                            throw new InvalidOperatorException("operator not valid", typeof(bool));
                    }
                }
                void FloatVariable(string op, float f1, float f2, GameObject ifBlock)
                {
                    switch (op)
                    {
                        case "==":
                            ifBlock.SendMessage("GetConditionResult", (f1 == f2).ToString());
                            break;
                        case "!=":
                            ifBlock.SendMessage("GetConditionResult", (f1 != f2).ToString());
                            break;
                        case "<":
                            ifBlock.SendMessage("GetConditionResult", (f1 < f2).ToString());
                            break;
                        case "<=":
                            ifBlock.SendMessage("GetConditionResult", (f1 <= f2).ToString());
                            break;
                        case ">":
                            ifBlock.SendMessage("GetConditionResult", (f1 > f2).ToString());
                            break;
                        case ">=":
                            ifBlock.SendMessage("GetConditionResult", (f1 >= f2).ToString());
                            break;
                        default:
                            throw new InvalidOperatorException("operator not valid", typeof(bool));
                    }
                }

                float float1 = 0, float2 = 0;
                bool bool1 = false, bool2 = false, areBool = false;

                var field1 = condition.Value.transform.Find("var1").Find("Text").GetComponent<Text>().text;
                var field2 = condition.Value.transform.Find("var2").Find("Text").GetComponent<Text>().text;
                var opDropdown = condition.Value.transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
                var op = opDropdown.options[opDropdown.value].text;

                void HandleField1()
                {
                    var varValue = executor.IsInDictionary(field1);
                    if (varValue != null)
                    {
                        //se è una variabile booleana
                        if (IBlock.IsBool(varValue))
                        {
                            bool1 = bool.Parse(varValue);
                            areBool = true;
                        }
                        else float1 = float.Parse(varValue);
                    }
                    else if (IBlock.IsBool(field1))
                    {
                        bool1 = bool.Parse(field1);
                        areBool = true;
                    }
                    else float1 = float.Parse(field1);
                }
                void HandleField2()
                {
                    var varValue = executor.IsInDictionary(field2);
                    if (varValue != null)
                    {
                        switch (areBool)
                        {
                            //se entrambe sono variabili booleane
                            case true when IBlock.IsBool(varValue):
                                bool2 = bool.Parse(varValue);
                                break;
                            case false when IBlock.IsFloat(varValue):
                                float2 = float.Parse(varValue);
                                break;
                            default:
                                throw new InvalidOperationException("invalid type operation");
                        }
                    }
                    else
                        switch (areBool)
                        {
                            case true when IBlock.IsBool(field2):
                                bool2 = bool.Parse(field2);
                                break;
                            case false when IBlock.IsFloat(field2):
                                float2 = float.Parse(field2);
                                break;
                            default:
                                throw new InvalidOperationException("invalid type operation");
                        }
                }

                HandleField1();
                HandleField2();

                if (areBool)
                    BooleanVariable(op, bool1, bool2, condition.ifBlock);
                else
                    FloatVariable(op, float1, float2, condition.ifBlock);
            }

            switch (condition.OperationTag)
            {
                //se è una variabile
                case "getVarValue":
                    GetVarValue();
                    break;

                //se è un valore
                case "value":
                    GetSimpleValue();
                    break;

                //se è un'operazione
                case "boolOpBlock":
                    BoolOperation();
                    break;

                default:
                    Debug.Log("blocco non valido");
                    throw new Exception("questo blocco non restituisce una variabile");
            }
        }
    }
}
