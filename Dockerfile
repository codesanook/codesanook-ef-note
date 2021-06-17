# https://hub.docker.com/_/microsoft-dotnet-sdk
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS base
RUN dotnet tool install --global dotnet-ef

# /app is mounted to local app folder in docker-compose configuration 
# We can't copy files to a working directory
WORKDIR /app 

COPY ./entrypoint.sh /
RUN chmod +x /entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]

FROM base as builder
# Copy the source code
COPY ./app /workspace
WORKDIR /workspace
RUN mkdir /publish

# Restore all of the nugets
RUN dotnet restore
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS release
ENV ASPNETCORE_URLS http://*:8000
EXPOSE 8000

COPY --from=builder /publish /dist

# Copy .NET Global tool
RUN mkdir -p /root/.dotnet/tools/
COPY --from=builder /root/.dotnet/tools/ /root/.dotnet/tools/

RUN apk add tree

WORKDIR /dist

COPY ./entrypoint-release.sh /
RUN chmod +x /entrypoint-release.sh
ENTRYPOINT ["/bin/sh", "-c" , "sleep 10 && dotnet Codesanook.EFNote.dll"]
