using System;
using System.Collections.Generic;
using System.Text;

namespace AndroidCalc
{
    static class Logic
    {
        #region Поля
        private enum PartType { Number, Sign, Parenthesis };

        private struct Part
        {
            public string Value;
            public PartType Type;
            public int Position;
        }

        private static Stack<double> ArgStack;

        private struct Operation
        {
            public char Sign;
            public int Precedence;
        }

        private static Stack<Operation> OpStack;

        private static List<Part> Parts;

        private static string Input;
        #endregion

        public static double Calculate(string Input)
        { 
            try
            {
                SplitCheck(Input);
            }
            catch (Exception E)
            {
                throw new Exception("Ошибка осмысления строки:\n" + E.Message);
            }
            //int g = 1;
            //foreach (Part CurrentPart in Parts)
            //{
            //    string TypeString = "";
            //    switch (CurrentPart.Type)
            //    {
            //        case PartType.Sign:
            //            TypeString = "Знак";
            //            break;
            //        case PartType.Parenthesis:
            //            TypeString = "Скобка";
            //            break;
            //        case PartType.Number:
            //            TypeString = "Число";
            //            break;
            //    }
            //    g++;
            //}
            double Result = 0;
            try
            {
                Result = Evaluate();
            }
            catch (Exception E)
            {
                throw new Exception("Ошибка вычисления:\n " + E.Message);
            }
            return Result;
        }

        public static void SplitCheck(string S)
        {
            if (S == null || S.Length < 1) throw new Exception("Пустая строка");
            if (S.Length > 100) throw new Exception("Слишком длинная строка");            
            S = S + " ";
            int u = 0;
            for (int i = 0; i < S.Length; i++)
            {
                if (S[i] == ' ') u++;
            }
            if (u == S.Length) throw new Exception("Строка состоит из одних пробелов");
            Parts = new List<Part>();
            Part NewPart;
            int Index = 0;
            while (Index < S.Length)
            {
                switch (S[Index])
                {
                    case ' ':
                        Index++;
                        break;
                    case '+':
                    case '—':
                    case '*':
                    case '/':
                    case '^':
                    case '√':
                        NewPart.Value = S[Index].ToString();
                        NewPart.Type = PartType.Sign;
                        NewPart.Position = Index + 1;
                        Parts.Add(NewPart);
                        Index++;
                        break;
                    case '(':
                    case ')':
                        NewPart.Value = S[Index].ToString();
                        NewPart.Type = PartType.Parenthesis;
                        NewPart.Position = Index + 1;
                        Parts.Add(NewPart);
                        Index++;
                        break;
                    default:
                        if (char.IsNumber(S[Index]))
                        {
                            string C = S[Index].ToString();
                            if (Index != S.Length - 1)
                            {
                                while (char.IsNumber(S[Index + 1]))
                                {
                                    Index++;
                                    C = C + S[Index];
                                    if (Index == S.Length - 1) break;
                                }
                            }
                            if (S[Index + 1] == '.')
                            {
                                Index++;
                                C = C + S[Index];
                                if (Index != S.Length - 1)
                                {
                                    while (char.IsNumber(S[Index + 1]))
                                    {
                                        Index++;
                                        C = C + S[Index];
                                    }
                                }
                            }
                            NewPart.Value = C;
                            NewPart.Type = PartType.Number;
                            NewPart.Position = Index - C.Length + 2;
                            Parts.Add(NewPart);
                            Index++;
                            break;
                        }
                        else
                        {
                            throw new Exception("Недопустимый символ\"" + S[Index] +"\", позиция " + (Index + 1).ToString()
                                + (S[Index] == '√' ? String.Format("\nПопробуйте заменить конструкцию вида\"√x{0}\" на \"(√x{0})\"", (char)862) : "")
                                + (S[Index] == '—' ? "\nПопробуйте заменить конструкцию вида\"—x\" на \"—x)\"" : ""));
                        }
                }
            }
        }

        public static double Evaluate()
        {
            int Depth = 0;
            bool NeedValue = true;
            ArgStack = new Stack<double>();
            OpStack = new Stack<Operation>();
            int PartIndex = 0;

            //Семантический анализ;
            foreach (Part CurrentPart in Parts)
            {
                switch (CurrentPart.Type)
                {
                    case PartType.Number:
                        if (!NeedValue)
                            throw new Exception("Неуместное число (" + CurrentPart.Value + "): позиция " + CurrentPart.Position.ToString());
                        ArgStack.Push(double.Parse(CurrentPart.Value));
                        NeedValue = false;
                        break;
                    case PartType.Parenthesis:
                        if (CurrentPart.Value[0] == '(')
                            if (!NeedValue)
                                throw new Exception("Неуместная открывающая скобка: позиция " + CurrentPart.Position.ToString());
                            else Depth++;
                        else
                            if (Depth == 0 || NeedValue)
                            throw new Exception("Неуместная закрывающая скобка: позиция " + CurrentPart.Position.ToString());
                        else Depth--;
                        break;
                    case PartType.Sign:
                        Operation CurrentOperation;
                        if (NeedValue)
                            if (CurrentPart.Value[0] == '—')
                            {
                                CurrentOperation.Sign = '~';
                                CurrentOperation.Precedence = Depth * 10 + 1;
                                if (OpStack.Count != 0) Reduce(CurrentOperation.Precedence);
                                OpStack.Push(CurrentOperation);
                            }
                            else if (CurrentPart.Value[0] == '√')
                            {
                                CurrentOperation.Sign = '√';
                                CurrentOperation.Precedence = Depth * 10 + 3;
                                if (OpStack.Count != 0) Reduce(CurrentOperation.Precedence);
                                OpStack.Push(CurrentOperation);
                            }
                            else
                                throw new Exception("Неуместный знак (" + CurrentPart.Value + "): позиция " + CurrentPart.Position.ToString());
                        else
                        {
                            CurrentOperation.Sign = CurrentPart.Value[0];
                            CurrentOperation.Precedence = 0;
                            switch (CurrentPart.Value[0])
                            {
                                case '+':
                                case '—':
                                    CurrentOperation.Precedence = Depth * 10 + 1;
                                    break;
                                case '*':
                                case '/':
                                    CurrentOperation.Precedence = Depth * 10 + 2;
                                    break;
                                case '√':
                                case '^':
                                    CurrentOperation.Precedence = Depth * 10 + 3;
                                    break;
                            }
                            if (OpStack.Count != 0) Reduce(CurrentOperation.Precedence);
                            OpStack.Push(CurrentOperation);
                            NeedValue = true;
                        }
                        break;
                }
                PartIndex++;
            }
            if (NeedValue)
                throw new Exception("Нет значения в конце строки");
            if (Depth > 0)
                throw new Exception("Не закрыты скобки в конце строки");
            Reduce(0);
            return ArgStack.Peek();
        }

        public static void Reduce(int MinPrecedence)
        {
            if (OpStack.Count == 0)
                return;
            Operation CurrentOperation = OpStack.Peek();
            if (CurrentOperation.Precedence >= MinPrecedence)
            {
                double Arg1, Arg2, Res = 0.0;
                CurrentOperation = OpStack.Pop();
                switch (CurrentOperation.Sign)
                {
                    case '+':
                        Arg1 = ArgStack.Pop();
                        Arg2 = ArgStack.Pop();
                        Res = Arg2 + Arg1;
                        ArgStack.Push(Res);
                        break;
                    case '*':
                        Arg1 = ArgStack.Pop();
                        Arg2 = ArgStack.Pop();
                        Res = Arg2 * Arg1;
                        ArgStack.Push(Res);
                        break;
                    case '—':
                        Arg1 = ArgStack.Pop();
                        Arg2 = ArgStack.Pop();
                        Res = Arg2 - Arg1;
                        ArgStack.Push(Res);
                        break;
                    case '/':
                        Arg1 = ArgStack.Pop();
                        Arg2 = ArgStack.Pop();
                        Res = Arg2 / Arg1;
                        ArgStack.Push(Res);
                        break;
                    case '^':
                        Arg2 = ArgStack.Pop();
                        Arg1 = ArgStack.Pop();
                        Res = Math.Pow(Arg1, Arg2);
                        ArgStack.Push(Res);
                        break;
                    case '~':
                        Arg1 = ArgStack.Pop();
                        Res = Arg1 * -1;
                        ArgStack.Push(Res);
                        break;
                    case '√':
                        Arg1 = ArgStack.Pop();
                        Res = Math.Sqrt(Arg1);
                        ArgStack.Push(Res);
                        break;
                }
                if (double.IsNaN(Res))
                    throw new Exception("Неверный результат операции (результат не является действительным числом)");
                else
                if (double.IsInfinity(Res))
                    throw new Exception("Переполнение (в результате вычислений в качестве промежуточного или" +
                        " окончательного результата получено значение '" + (char)8734 + "')");
                if (OpStack.Count > 0) if (OpStack.Peek().Precedence >= MinPrecedence) Reduce(MinPrecedence);
            }
        }
    }
}
