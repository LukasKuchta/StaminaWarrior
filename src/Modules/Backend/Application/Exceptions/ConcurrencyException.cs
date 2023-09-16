﻿namespace Backend.Application.Exceptions;

public sealed class ConcurrencyException : Exception
{
    public ConcurrencyException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public ConcurrencyException()
    {
    }

    public ConcurrencyException(string message)
        : base(message)
    {
    }
}
