﻿using System;
using System.Collections.Generic;
using ConsoleWizard.Components;

namespace ConsoleWizard
{
    public class QuestionExtendedList<TDictionary, T> : QuestionDictionaryListBase<TDictionary, T>, IConvertToResult<ConsoleKey, T>, IValidation<ConsoleKey> where TDictionary : Dictionary<ConsoleKey, T>, new()
    {
        private DisplayQuestionSingleChoiceComponent<QuestionExtendedList<TDictionary, T>, T> _displayQuestionComponent;

        internal QuestionExtendedList(string question) : base(question)
        {
            _displayQuestionComponent = new DisplayQuestionSingleChoiceComponent<QuestionExtendedList<TDictionary, T>, T>(this);
        }

        public Func<ConsoleKey, T> ParseFn { get; set; } = v => { return default(T); };

        public Func<ConsoleKey, bool> ValidatationFn { get; set; } = v => { return true; };

        internal override T Prompt()
        {
            bool tryAgain = true;
            T answer = DefaultValue;

            while (tryAgain)
            {
                _displayQuestionComponent.DisplayQuestion();

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
                    return default(T);
                }

                if (key == ConsoleKey.Enter && HasDefaultValue)
                {
                    tryAgain = Confirm(ToStringFn(answer));
                }
                else if (ValidatationFn(key))
                {
                    answer = ParseFn(key);
                    tryAgain = Confirm(ToStringFn(answer));
                }
            }

            Console.WriteLine();
            return answer;
        }

        private string DisplayChoice(ConsoleKey key)
        {
            return $"[{key}] {Choices[key]}";
        }
    }
}
