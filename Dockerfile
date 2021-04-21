# Get the base image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
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

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS release
COPY --from=builder /publish /dist
WORKDIR /dist

COPY ./entrypoint.release.sh /
RUN chmod +x /entrypoint.release.sh
ENTRYPOINT ["/entrypoint.release.sh"]
