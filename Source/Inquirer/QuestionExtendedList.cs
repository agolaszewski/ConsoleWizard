﻿using System;
using System.Collections.Generic;

namespace InquirerCS
{
    public class QuestionExtendedList<TDictionary, TResult> : QuestionDictionaryListBase<TDictionary, TResult> where TDictionary : Dictionary<ConsoleKey, TResult>, new()
    {
        internal QuestionExtendedList(string question) : base(question)
        {
        }

        internal override TResult Prompt()
        {
            bool tryAgain = true;
            TResult answer = DefaultValue;

            while (tryAgain)
            {
                DisplayQuestion();

                Console.WriteLine();
                Console.WriteLine();

                foreach (var item in Choices)
                {
                    ConsoleHelper.WriteLine(DisplayChoice(item.Key));
                }

                Console.WriteLine();
                ConsoleHelper.Write("Answer: ");

                bool isCanceled = false;
                var key = ConsoleHelper.ReadKey(out isCanceled);
                if (isCanceled)
                {
                    IsCanceled = isCanceled;
                    return default(TResult);
                }

                if (key == ConsoleKey.Enter && HasDefaultValue)
                {
                    tryAgain = Confirm(ConvertToStringFn(answer));
                }
                else if (Validate(key))
                {
                    answer = ParseFn(key);
                    tryAgain = Confirm(ConvertToStringFn(answer));
                }
            }

            Console.WriteLine();
            return answer;
        }

        private string DisplayChoice(ConsoleKey key)
        {
            return $"[{key}] {ConvertToStringFn(Choices[key])}";
        }
    }
}