FROM microsoft/dotnet:2.1-sdk-alpine AS build

LABEL maintainer="lukaszkepa"

WORKDIR /src
COPY Training.Docker.API/*.csproj Training.Docker.API/
COPY Training.Docker.Models/*.csproj Training.Docker.Models/
COPY Training.Docker.CommonLibs/*.csproj Training.Docker.CommonLibs/
RUN dotnet restore Training.Docker.API/Training.Docker.API.csproj

COPY . .

WORKDIR /src/Training.Docker.API
RUN dotnet publish -c Release -o /app --no-restore

FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine
WORKDIR /app
EXPOSE 80
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Training.Docker.API.dll"]