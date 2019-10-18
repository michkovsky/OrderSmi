FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY OrderSmi/OrderSmi.csproj OrderSmi/
RUN dotnet restore OrderSmi/OrderSmi.csproj
COPY . .
WORKDIR /src/OrderSmi
RUN dotnet build OrderSmi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish OrderSmi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OrderSmi.dll"]
