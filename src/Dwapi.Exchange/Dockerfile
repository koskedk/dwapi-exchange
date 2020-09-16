#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Dwapi.Exchange/Dwapi.Exchange.csproj", "src/Dwapi.Exchange/"]
COPY ["src/Dwapi.Exchange.Infrastructure/Dwapi.Exchange.Infrastructure.csproj", "src/Dwapi.Exchange.Infrastructure/"]
COPY ["src/Dwapi.Exchange.Core/Dwapi.Exchange.Core.csproj", "src/Dwapi.Exchange.Core/"]
COPY ["src/Dwapi.Exchange.SharedKernel/Dwapi.Exchange.SharedKernel.csproj", "src/Dwapi.Exchange.SharedKernel/"]
COPY ["src/Dwapi.Exchange.SharedKernel.Infrastructure/Dwapi.Exchange.SharedKernel.Infrastructure.csproj", "src/Dwapi.Exchange.SharedKernel.Infrastructure/"]
RUN dotnet restore "src/Dwapi.Exchange/Dwapi.Exchange.csproj"
COPY . .
WORKDIR "/src/src/Dwapi.Exchange"
RUN dotnet build "Dwapi.Exchange.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dwapi.Exchange.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dwapi.Exchange.dll"]
