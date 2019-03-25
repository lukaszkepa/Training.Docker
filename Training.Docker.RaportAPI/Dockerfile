FROM microsoft/dotnet:2.1-sdk AS build

LABEL maintainer="lukaszkepa"

WORKDIR /src
COPY Training.Docker.RaportAPI/ Training.Docker.RaportAPI/
RUN dotnet restore Training.Docker.RaportAPI/Training.Docker.RaportAPI.csproj

WORKDIR /src/Training.Docker.RaportAPI
RUN dotnet build -c Release -o /app --no-restore
RUN dotnet publish -c Release -o /app --no-restore

FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine
WORKDIR /app
EXPOSE 80
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Training.Docker.RaportAPI.dll"]