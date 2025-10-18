FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

RUN dotnet publish src/BritInsurance.Api/BritInsurance.Api.csproj -c Release -o /published


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /published .

EXPOSE 80
ENTRYPOINT ["dotnet", "BritInsurance.Api.dll"]