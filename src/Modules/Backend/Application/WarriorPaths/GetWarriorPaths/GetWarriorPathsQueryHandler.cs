using System.Data;
using Backend.Application.Abstractions.Queries;
using Backend.Application.Contracts;
using BuildingBlocks.Application.Data;
using Dapper;

namespace Backend.Application.WarriorPaths.GetWarriorPaths;

public sealed record GetWarriorPathsQuery(Guid WarriorId) : IQuery<IEnumerable<WarriorPathResponse>>;

internal sealed class GetWarriorPathsQueryHandler : IQueryHandler<GetWarriorPathsQuery, IEnumerable<WarriorPathResponse>>
{
    private readonly ISqlConnectionFactory _sqlDatabaseConnectionFactroy;

    public GetWarriorPathsQueryHandler(ISqlConnectionFactory sqlDatabaseConnectionFactroy)
    {
        _sqlDatabaseConnectionFactroy = sqlDatabaseConnectionFactroy;
    }

    public async Task<IEnumerable<WarriorPathResponse>> Handle(GetWarriorPathsQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = _sqlDatabaseConnectionFactroy.CreateConnection();

        string sql = @"SELECT *
                       wp.WarriorPathId as WarriorPathId,
                       wp.WarriorId as WarriorId,
                       wpp.Lattitude as Lattitude,
                       wpp.Longitudal as Longitudal,
                       FROM WarriorPaths wp
                       LEFT JOIN WarriorPathPoints wpp on wp.WarriorPathId = wpp.WarriorPathid 
                       WHERE wp.WarriorId = @id";

        var warriorPathsDictionary = new Dictionary<Guid, WarriorPathResponse>();
        IEnumerable<WarriorPathResponse> data = await connection.QueryAsync<WarriorPathResponse, Waypoint, WarriorPathResponse>(
            sql,
            (path, waypoint) =>
            {
                if (!warriorPathsDictionary.TryGetValue(path.WarriorPathId, out WarriorPathResponse? response))
                {
                    response = path;
                    warriorPathsDictionary.Add(response.WarriorPathId, response);
                }

                response.Waypoints.Add(waypoint);
                return response;
            },
            new { request.WarriorId },
            splitOn: "Lattitude").ConfigureAwait(false);

        return data.ToList();
    }
}
