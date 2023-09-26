using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Contracts;
public interface IBackendModule
{
    Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);

    Task ExecuteCommandAsync(ICommand command);

    Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}
