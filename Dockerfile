# Get the base image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
#EXPOSE 80/tcp
WORKDIR /app

COPY ./entrypoint.sh /
RUN chmod +x /entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]

