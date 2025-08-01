﻿using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Plant_Project.API.Abstraction
{
    public class Result
    {
        public Result(bool isSuccess, Error error)
        {
            if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
                throw new InvalidOperationException("Invalid Result state.");

            IsSuccess = isSuccess;
            this.error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error error { get; }

        public static Result Success() => new Result(true, Error.None);
        public static Result Failure(Error error) => new Result(false, error);

        public static Result<TValue> Success<TValue>(TValue value) => new Result<TValue>(value, true, Error.None);
        public static Result<TValue> Failure<TValue>(Error error) => new Result<TValue>(default, false, error);
    }
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value =>
            IsSuccess
                ? _value!
                : throw new InvalidOperationException("Failure result: Value cannot be accessed.");
    }
}
