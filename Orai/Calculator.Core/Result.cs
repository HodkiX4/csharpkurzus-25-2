namespace Calculator.Core;

public class Result<TSucces, TError>
{
    private readonly TSucces? _success;

    private readonly TError? _error;

    public bool IsSuccess { get; }

    public bool IsError => !IsSuccess;

    public Result(TSucces success)
    {
        _success = success;
        IsSuccess = true;
    }

    public Result(TError error)
    {
        _error = error;
        IsSuccess = false;
    }

    public void Visit(Action<TSucces> succes, Action<TError> error)
    {
        if (IsSuccess && _success is not null)
        {
            succes.Invoke(_success);
            return;
        }
        else if (_error is not null)
        {
            error.Invoke(_error);
            return;
        }
        throw new InvalidOperationException("Result is in an invalid state.");
    }

    public override string ToString()
    {
        if (IsSuccess && _success is not null)
        {
            return $"{_success}";
        }
        else if (_error is not null)
        {
            return $"{_error}";
        }
        throw new InvalidOperationException("Result is in an invalid state.");
    }
}
