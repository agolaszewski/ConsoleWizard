﻿using System;
using System.Collections.Generic;
using System.Linq;
using InquirerCS.Components;
using InquirerCS.Interfaces;
using InquirerCS.Questions;
using InquirerCS.Traits;

namespace InquirerCS.Builders
{
    public class CheckboxBuilder<TResult>
        : IConfirmTrait<List<TResult>>,
        IConvertToStringTrait<TResult>,
        IDefaultTrait<List<TResult>>,
        IValidateResultTrait<List<TResult>>,
        IRenderQuestionTrait,
        IRenderChoicesTrait<TResult>,
        IDisplayErrorTrait,
        IWaitForInputTrait<StringOrKey>,
        IParseTrait<List<Selectable<TResult>>, List<TResult>>,
        IOnKeyTrait where TResult : IComparable
    {
        private List<Selectable<TResult>> _choices;

        private IConsole _console;

        public CheckboxBuilder(IConsole console)
        {
            _console = console;

            this.Confirm(this, _console);
            this.ConvertToString();
            this.Default();
            this.ResultValidate();
            this.Input(_console, ConsoleKey.Spacebar, ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.Enter);
            this.OnKey();
        }

        public CheckboxBuilder(string message, IEnumerable<TResult> choices, IConsole console) : this(console)
        {
            _choices = choices.Select(item => new Selectable<TResult>(false, item)).ToList();

            this.RenderQuestion(message, this, this, _console);
            this.RenderChoices(_choices, this, _console);
            this.Parse(_choices);
        }

        public IConfirmComponent<List<TResult>> Confirm { get; set; }

        public IConvertToStringComponent<TResult> Convert { get; set; }

        public IDefaultValueComponent<List<TResult>> Default { get; set; }

        public IDisplayErrorComponent DisplayError { get; set; }

        public IWaitForInputComponent<StringOrKey> Input { get; set; }

        public IOnKey OnKey { get; set; }

        public IParseComponent<List<Selectable<TResult>>, List<TResult>> Parse { get; set; }

        public IRenderChoices<TResult> RenderChoices { get; set; }

        public IRenderQuestionComponent RenderQuestion { get; set; }

        public IValidateComponent<List<TResult>> ResultValidators { get; set; }

        public Checkbox<List<TResult>, TResult> Build()
        {
            return new Checkbox<List<TResult>, TResult>(_choices, Confirm, RenderQuestion, Input, Parse, RenderChoices, ResultValidators, DisplayError, OnKey);
        }

        public virtual CheckboxBuilder<TResult> WithConfirmation()
        {
            this.Confirm(this, _console);
            return this;
        }

        public virtual CheckboxBuilder<TResult> WithConvertToString(Func<TResult, string> fn)
        {
            this.ConvertToString(fn);
            return this;
        }

        public virtual CheckboxBuilder<TResult> WithDefaultValue(List<TResult> defaultValues)
        {
            this.Default(_choices, defaultValues);
            return this;
        }

        public virtual CheckboxBuilder<TResult> WithDefaultValue(TResult defaultValue)
        {
            this.Default(_choices, new List<TResult>() { defaultValue });
            return this;
        }

        public CheckboxBuilder<TResult> WithValidation(Func<List<TResult>, bool> fn, Func<List<TResult>, string> errorMessageFn)
        {
            ResultValidators.Add(fn, errorMessageFn);
            return this;
        }

        public CheckboxBuilder<TResult> WithValidation(Func<List<TResult>, bool> fn, string errorMessage)
        {
            ResultValidators.Add(fn, errorMessage);
            return this;
        }
    }
}