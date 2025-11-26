# ============================
# 1. BUILD (.NET 10 SDK)
# ============================
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY TaskFlow.Api.csproj ./
COPY global.json ./

RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/publish

# ============================
# 2. RUNTIME (.NET 10 ASPNET)
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app

RUN mkdir /data

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5208
EXPOSE 5208

ENTRYPOINT ["dotnet", "TaskFlow.Api.dll"]
