#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS runtime
EXPOSE 80
WORKDIR /app
COPY PortalApiBin .
CMD ["/bin/bash"]