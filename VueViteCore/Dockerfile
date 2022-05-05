FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

ENV NODE_VERSION=17.6.0
ENV NODE_DOWNLOAD_SHA=de9596fda9cc88451d03146278806687e954c03413e8aa0ee98ad46442d6cb1c

RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
    && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs

# set environment variable
ENV ASPNETCORE_ENVIRONMENT="Production"
# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./

# build net core app - this will also build vue app via vite
RUN dotnet publish -c Release -o ./publish

# Build runtime image
#FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine3.9
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "VueViteCore.dll"]
# Heroku
# CMD ASPNETCORE_URLS=http://*:$PORT dotnet ASPNETReact.dll
