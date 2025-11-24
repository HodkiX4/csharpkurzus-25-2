using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Calculator.HTTP;

public abstract class JsonRequestHandler<TResponse> : IRequestHandler
{
    private readonly string _serverPath;

    public JsonRequestHandler(string serverPath)
    {
        _serverPath = serverPath;
    }

    public abstract Task<(TResponse response, HttpStatusCode code)> Handle(HttpRequest httpRequest);

    public async Task<bool> HandlerRequest(HttpRequest request, NetworkStream responseStream, CancellationToken token)
    {
        if (request.Path.LocalPath != _serverPath)
            return false;

        var result = await Handle(request);
        var content = JsonSerializer.Serialize<TResponse>(result.response, JsonSerializerOptions.Default);

        var encoding = new UTF8Encoding(false);

        string response = $"""
            HTTP/1.1 {(int)result.code} {result.code.ToString()}
            Date: {DateTime.UtcNow:R}
            Content-Type: application/json; charset=utf-8
            Content-Length: {encoding.GetByteCount(content)}

            {content}
            """;

        using var writer = new StreamWriter(responseStream, encoding, leaveOpen: true);
        writer.Write(response);


        return true;
    }
}
