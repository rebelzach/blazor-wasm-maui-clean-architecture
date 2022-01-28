using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Thing.Designer.Core.Contracts.Models;

public interface IContractResult
{
    object? Value { get; }
    ResultStatus Status { get; }
    IEnumerable<string> Errors { get; }
    List<string> ValidationErrors { get; }
}

[ProtoContract]
[DataContract]
public class ContractResult<T> : IContractResult
{
    [DataMember(Order = 1, IsRequired = true)]
    public T? Value { get; set; }
    object? IContractResult.Value => Value;

    [DataMember(Order = 2, IsRequired = true)]
    public ResultStatus Status { get; set; }

    [DataMember(Order = 3, IsRequired = true)]
    public IEnumerable<string> Errors {get; set; } = Enumerable.Empty<string>();

    [DataMember(Order = 4, IsRequired = true)]
    public List<string> ValidationErrors { get; set; } = new List<string>();
}

public static class ContractResultExtensions
{
    public static ContractResult<T> AsContractResult<T>(this Result<T> r)
    {
        return new ContractResult<T>()
        {
            Value = r.Value,
            Status = r.Status,
            Errors = r.Errors,
            ValidationErrors = r.ValidationErrors.Select(e => e.ErrorMessage).ToList()
        };
    }

    public static bool IsSuccess(this IContractResult contractResult)
    {
        return contractResult.Status == ResultStatus.Ok;
    }
}
