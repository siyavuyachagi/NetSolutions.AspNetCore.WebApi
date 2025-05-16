namespace Application.Helpers
{
    public class Result
    {
        public bool Succeeded { get; }
        public string[] Errors { get; }

        // Change the constructor to public
        public Result(bool succeeded, params string[] errors)
        {
            Succeeded = succeeded;
            Errors = errors ?? Array.Empty<string>();
        }

        public static Result Success()
        {
            return new Result(true);
        }

        public static Result Failure(params string[] errors)
        {
            return new Result(false, errors);
        }

        // Generic Success Method
        public static Result<T> Success<T>(T response)
        {
            return new Result<T>(true, response);
        }

        // Generic Failure Method
        public static Result<T> Failure<T>(params string[] errors)
        {
            return new Result<T>(false, default, errors);
        }
    }

    // Generic Result<T> Class for Typed Responses
    public class Result<T> : Result
    {
        public T Response { get; }

        internal Result(bool succeeded, T response, params string[] errors)
            : base(succeeded, errors)
        {
            Response = response;
        }
    }
}

