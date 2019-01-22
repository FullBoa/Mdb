FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine3.8

WORKDIR /app
COPY ./Mdb/app /app/

ENTRYPOINT ["dotnet", "Mdb.dll"]