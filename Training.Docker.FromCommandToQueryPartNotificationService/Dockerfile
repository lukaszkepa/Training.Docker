FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build

LABEL maintainer="adamszweida"

WORKDIR /src
COPY Training.Docker.FromCommandToQueryPartNotificationService/*.csproj Training.Docker.FromCommandToQueryPartNotificationService/
COPY Training.Docker.Models/*.csproj Training.Docker.Models/
COPY Training.Docker.CommonLibs/*.csproj Training.Docker.CommonLibs/
RUN dotnet restore Training.Docker.FromCommandToQueryPartNotificationService/Training.Docker.FromCommandToQueryPartNotificationService.csproj

COPY . .

WORKDIR /src/Training.Docker.FromCommandToQueryPartNotificationService
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/core/runtime:2.2-alpine
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Training.Docker.FromCommandToQueryPartNotificationService.dll"]