﻿namespace Shared.BuildingBlocks.Result;

public class ResultError
{
    public static readonly ResultError None = new(string.Empty, string.Empty);

    public string Code { get; }
    public string Message { get; }

    public ResultError(string code, string message)
    {
        Code = code;
        Message = message;
    }
}
