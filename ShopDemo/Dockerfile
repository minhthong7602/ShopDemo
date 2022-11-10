#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ShopDemo/ShopDemo.Web.csproj", "ShopDemo/"]
COPY ["ShopDemo.Domain/ShopDemo.Domain.csproj", "ShopDemo.Domain/"]
COPY ["ShopDemo.Infrastructure/ShopDemo.Infrastructure.csproj", "ShopDemo.Infrastructure/"]
RUN dotnet restore "ShopDemo/ShopDemo.Web.csproj"
COPY . .
WORKDIR "/src/ShopDemo"
RUN dotnet build "ShopDemo.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShopDemo.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopDemo.Web.dll"]